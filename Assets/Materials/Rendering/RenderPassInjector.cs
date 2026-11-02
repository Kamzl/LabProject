using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderPassInjector : MonoBehaviour
{
    // Declare a custom render pass
    private CustomRenderPass exampleRenderPass;

    private void OnEnable()
    {
        // Create the custom render pass
        exampleRenderPass = new CustomRenderPass(GraphicsFormat.R8G8B8A8_SRGB);
        RenderPipelineManager.beginCameraRendering += InjectRenderPass;
    }
    
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= InjectRenderPass;
    }
    
    private void InjectRenderPass(ScriptableRenderContext context, Camera cam)
    {
        // Use the EnqueuePass API to inject the custom render pass.
        cam.GetUniversalAdditionalCameraData().scriptableRenderer.EnqueuePass(exampleRenderPass);
    }
}

