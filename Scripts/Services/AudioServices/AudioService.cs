using System;
using System.Collections.Generic;
using System.Linq;
using Game.Towers;
using Services.AudioServices.AudioMixers;
using Services.AudioServices.Sounds;
using Services.AudioServices.Sounds.Configs;
using Services.Cameras;
using Services.StaticData;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Object = UnityEngine.Object;

namespace Services.AudioServices
{
    public class AudioService : ITickable, IInitializable
    {
        private const float SliderMutedLoudness = -40;
        private const float MutedLoudness = -80;

        private const string Music = nameof(Music);
        private const string SoundEffects = nameof(SoundEffects);

        // ReSharper disable once NotAccessedField.Local
        private readonly SoundPlayer _soundPlayer;
        private readonly AudioMixer _audioMixer;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly Dictionary<AudioMixerGroupEnum, AudioMixerGroupWrapper> _audioMixerGroupWrappers = new();

        private readonly IStaticDataService _staticDataService;

        private readonly Transform _container;
        private readonly AudioListener _audioListener;
        private readonly ICameraProvider _cameraProvider;

        public AudioService(AudioMixer audioMixer,
            IStaticDataService staticDataService,
            AudioSourceContainer audioSourceContainer,
            AudioListener audioListener,
            ICameraProvider cameraProvider)
        {
            _audioMixer = audioMixer;
            _staticDataService = staticDataService;
            _audioListener = audioListener;
            _cameraProvider = cameraProvider;
            _container = audioSourceContainer.transform;

            _soundPlayer = new SoundPlayer();
        }

        public bool IsWorking { get; private set; } = true;

        public float MusicLoudness { get; private set; }

        public float SoundEffectsLoudness { get; private set; }

        public bool IsMusicOn { get; private set; }

        public void Initialize()
        {
            CreateAudioMixerGroupWrappers();
        }
        
        public void Tick()
        {
            foreach (AudioMixerGroupWrapper audioMixerGroupWrapper in _audioMixerGroupWrappers.Values)
                audioMixerGroupWrapper.Tick();

            MoveAudioListener();
        }

        public void Play(SoundEnum @enum, bool isNeedRandomPitch = true, Vector3 at = default)
        {
            if (IsWorking == false)
                return;

            if (@enum == SoundEnum.Unknown)
                throw new Exception("Unknown sound id");

            var soundSetup = _staticDataService.GetSoundConfig(@enum);

            var groupId = _staticDataService.GetAudioMixerGroupEnum(soundSetup);
            var groupSetup = _staticDataService.GetAudioMixerGroupSetup(groupId);

            var audioSourceWrapper = _audioMixerGroupWrappers[groupId].GetOrNull();

            if (audioSourceWrapper == null)
                return;

            audioSourceWrapper.IsActive = true;

            var source = audioSourceWrapper.AudioSource;
            
            if(isNeedRandomPitch)
                source.pitch = UnityEngine.Random.Range(0.95f, 1.1f);

            if (soundSetup.AudioClipsWithChance.Count == 0)
            {
                Debug.LogWarning($"Sound {soundSetup.Id} has no clips");
                return;
            }

            var clip = GetWeightedRandomClip(soundSetup.AudioClipsWithChance);

            _soundPlayer.Play(clip, source, soundSetup.Volume, at, groupSetup.Loop);
        }

        public void PlayMusic(SoundEnum @enum)
        {
            IsMusicOn = true;

            if (IsMusicOn)
                Play(@enum, false);
        }

        public void PlayByTowerEnum(TowerEnum towerEnum, Vector3 at = default)
        {
            Play(_staticDataService.GetSoundByTowerEnum(towerEnum),true, at);
        }

        public void StopAll()
        {
            foreach (AudioMixerGroupWrapper audioMixerGroupWrapper in _audioMixerGroupWrappers.Values)
                audioMixerGroupWrapper.StopAll();
        }

        public void SetMusicLoudness(float value)
        {
            if (value <= SliderMutedLoudness)
                value = MutedLoudness;

            MusicLoudness = value;

            _audioMixer.SetFloat(Music, MusicLoudness);
        }

        public void SetSoundEffectsLoudness(float value)
        {
            if (value <= SliderMutedLoudness)
                value = MutedLoudness;

            SoundEffectsLoudness = value;

            _audioMixer.SetFloat(SoundEffects, SoundEffectsLoudness);
        }

        public void Mute()
        {
            foreach (AudioMixerGroupWrapper audioMixerGroupWrapper in _audioMixerGroupWrappers.Values)
                audioMixerGroupWrapper.Mute();
        }

        public void UnMute()
        {
            foreach (AudioMixerGroupWrapper audioMixerGroupWrapper in _audioMixerGroupWrappers.Values)
                audioMixerGroupWrapper.UnMute();
        }

        private void CreateAudioMixerGroupWrappers()
        {
            AudioMixerGroupEnum[] groupIds = _staticDataService.GetAudioMixerGroupEnums();

            foreach (var id in groupIds)
            {
                var setup = _staticDataService.GetAudioMixerGroupSetup(id);

                float cooldown = setup.Cooldown;
                int count = setup.Count;

                var gameObject = CreateAudioMixerGroudWrapper(id, cooldown);

                for (int i = 0; i < count; i++)
                    CreateAudioSourceWrapper(gameObject, i, id);
            }
        }

        private GameObject CreateAudioMixerGroudWrapper(AudioMixerGroupEnum id, float cooldown)
        {
            var setup = _staticDataService.GetAudioMixerGroupSetup(id);
            var group = setup.AudioMixerGroup;
            var gameObject = new GameObject(group.name + "Group");
            gameObject.transform.SetParent(_container.transform);
            var audioMixerGroupWrapper = new AudioMixerGroupWrapper(group, cooldown);
            _audioMixerGroupWrappers.Add(id, audioMixerGroupWrapper);
            return gameObject;
        }

        private void CreateAudioSourceWrapper(GameObject groupGameObject, int count, AudioMixerGroupEnum id)
        {
            var group = _staticDataService.GetAudioMixerGroupSetup(id).AudioMixerGroup;
            var gameObject = Object.Instantiate(_staticDataService.ProjectConfig.AudioSourcePrefab,
                groupGameObject.transform);
            gameObject.name = groupGameObject.name + count;
            var audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = group;

            _audioMixerGroupWrappers[id].Add(new AudioSourceWrapper(audioSource));
        }

        private void MoveAudioListener()
        {
            if (_cameraProvider.Camera)
            {
                _audioListener.transform.position = _cameraProvider.Camera.transform.position;
            }
            else
            {
                if (_audioListener.transform.position != Vector3.zero)
                {
                    _audioListener.transform.position = Vector3.zero;
                }
            }
        }
        
        private static AudioClip GetWeightedRandomClip(List<AudioClipWithChance> clips)
        {
            switch (clips.Count)
            {
                case 0:
                    Debug.LogWarning("No audio clips to choose from");
                    return null;
                case 1:
                    return clips[0].AudioClip;
            }

            int totalWeight = clips.Sum(clip => clip.Chance);

            if (totalWeight == 0)
                return clips[UnityEngine.Random.Range(0, clips.Count)].AudioClip;

            int randomPoint = UnityEngine.Random.Range(0, totalWeight);

            int currentWeight = 0;
            
            foreach (var clip in clips)
            {
                currentWeight += clip.Chance;
                
                if (randomPoint < currentWeight)
                    return clip.AudioClip;
            }

            return clips[^1].AudioClip;
        }
    }
}