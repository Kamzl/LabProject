using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrabScreenFeature : ScriptableRendererFeature
{
    private CustomRenderPass customRenderPass;
    [SerializeField] private GraphicsFormat graphicsFormat;
    public override void Create()
    {
        customRenderPass = new CustomRenderPass(graphicsFormat);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass);
    }
}