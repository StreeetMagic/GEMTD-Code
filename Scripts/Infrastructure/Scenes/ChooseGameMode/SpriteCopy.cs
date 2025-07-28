using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Scenes.ChooseGameMode
{
    public class SpriteCopy : MonoBehaviour
    {
        public Image Image;
    
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>(); 
        }

        private void Update()
        {
            _image.sprite = Image.sprite; 
        }
    }
}