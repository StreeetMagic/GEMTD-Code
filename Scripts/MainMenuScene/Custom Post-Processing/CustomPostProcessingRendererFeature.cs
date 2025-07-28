using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MirzaBeig.LightningVFX
{
    // Mostly copied from here, and modified:
    // https://github.com/Unity-Technologies/Graphics/blob/0ded1f1707c946faf6ccd35dfd4e10d4bcee024f/Packages/com.unity.render-pipelines.universal/Runtime/RendererFeatures/FullScreenPassRendererFeature.cs

    public class CustomPostProcessingRendererFeature : ScriptableRendererFeature
    {
        public enum InjectionPoint
        {
            BeforeRenderingTransparents = RenderPassEvent.BeforeRenderingTransparents,
            BeforeRenderingPostProcessing = RenderPassEvent.BeforeRenderingPostProcessing,
            AfterRenderingPostProcessing = RenderPassEvent.AfterRenderingPostProcessing
        }

        public InjectionPoint injectionPoint = InjectionPoint.AfterRenderingPostProcessing;
        public ScriptableRenderPassInput requirements = ScriptableRenderPassInput.Color;

        FullScreenRenderPass fullScreenPass;

        bool requiresColor;
        bool injectedBeforeTransparents;

        /// <inheritdoc/>
        public override void Create()
        {
            fullScreenPass = new FullScreenRenderPass();
            fullScreenPass.renderPassEvent = (RenderPassEvent)injectionPoint;

            // This copy of requirements is used as a parameter to configure input in order to avoid copy color pass.

            ScriptableRenderPassInput modifiedRequirements = requirements;

            requiresColor = (requirements & ScriptableRenderPassInput.Color) != 0;
            injectedBeforeTransparents = injectionPoint <= InjectionPoint.BeforeRenderingTransparents;

            if (requiresColor && !injectedBeforeTransparents)
            {
                // Removing Color flag in order to avoid unnecessary CopyColor pass.
                // Does not apply to before rendering transparents, due to how depth and color are being handled until that injection point.

                modifiedRequirements ^= ScriptableRenderPassInput.Color;
            }
            fullScreenPass.ConfigureInput(modifiedRequirements);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            fullScreenPass.Setup(requiresColor, injectedBeforeTransparents, renderingData);
            renderer.EnqueuePass(fullScreenPass);
        }

        protected override void Dispose(bool disposing)
        {
            fullScreenPass.Dispose();
        }

        class FullScreenRenderPass : ScriptableRenderPass
        {
            private bool m_RequiresColor;
            private bool m_IsBeforeTransparents; // Mirza: Unused, but keeping for reference.

            private RTHandle m_CopiedColor;

            private static readonly int m_BlitTextureShaderID = Shader.PropertyToID("_BlitTexture");

            public void Setup(bool requiresColor, bool isBeforeTransparents, in RenderingData renderingData)
            {
                m_RequiresColor = requiresColor;
                m_IsBeforeTransparents = isBeforeTransparents;

                var colorCopyDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                colorCopyDescriptor.depthBufferBits = (int)DepthBits.None;

                RenderingUtils.ReAllocateIfNeeded(ref m_CopiedColor, colorCopyDescriptor, name: "_FullscreenPassColorCopy");
            }

            public void Dispose()
            {
                m_CopiedColor?.Release();
            }

            readonly List<Material> fxMaterials = new List<Material>();

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                // If no custom post-processing layer, return.

                if (!CustomPostProcessingLayer.instance)
                {
                    return;
                }

                // FullScreenPass manages its own RenderTarget.
                // ResetTarget here so that ScriptableRenderer's active attachement can be invalidated when processing this ScriptableRenderPass.

                ResetTarget();

                // Get current post-processing FX list from instance.

                List<CustomPostProcessingInstance> fxList = CustomPostProcessingLayer.instance.fxList;

                // Loading material settings.

                // I've put this here for the sake of being clean.
                // I could also do this in the loop in Execute rather than another one here.

                for (int i = 0; i < fxList.Count; i++)
                {
                    CustomPostProcessingInstance fx = fxList[i];
                    CustomPostProcessingLayer.instance.GetDistanceToFX(fx, out float distance, out float normalizedInverseDistance);

                    if (normalizedInverseDistance != 0.0f)
                    {
                        fx.UpdateMaterial(distance, normalizedInverseDistance);
                        fxMaterials.Add(new Material(fx.materialInstance));
                    }
                }
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = CommandBufferPool.Get();

                ref var cameraData = ref renderingData.cameraData;
                var source = cameraData.renderer.cameraColorTargetHandle;

                for (int i = 0; i < fxMaterials.Count; i++)
                {
                    Material fxMaterial = fxMaterials[i];

                    if (cameraData.isPreviewCamera)
                    {
                        return;
                    }

                    if (m_RequiresColor)
                    {
                        fxMaterial.SetTexture(m_BlitTextureShaderID, m_CopiedColor);
                        Blitter.BlitCameraTexture(cmd, source, m_CopiedColor);
                    }

                    CoreUtils.SetRenderTarget(cmd, cameraData.renderer.cameraColorTargetHandle);
                    CoreUtils.DrawFullScreen(cmd, fxMaterial);
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                // Similar to when I populate this list (which could be at the start of the loop in Execute), I could put this at the end of Execute.

                fxMaterials.Clear();
            }
        }
    }
}
