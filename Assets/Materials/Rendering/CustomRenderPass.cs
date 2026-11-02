using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class CustomRenderPass : ScriptableRenderPass
{
    public class MyCustomData : ContextItem {
        public TextureHandle textureToTransfer;

        public override void Reset()
        {
            // textureToTransfer = TextureHandle.nullHandle;
        }
    }    

    class PassData
    {
        public TextureHandle copySourceTexture;
    }

    public CustomRenderPass(GraphicsFormat graphicsFormat)
    {
        _graphicsFormat = graphicsFormat;
        renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
    }
    
    private int globalTextureID = Shader.PropertyToID("_MyGlobalTexture");
    private GraphicsFormat _graphicsFormat;
    
    // This method adds and configures one or more render passes in the render graph.
    // This process includes declaring their inputs and outputs,
    // but does not include adding commands to command buffers.
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameContext)
    {
        string passName = "Copy To Debug Texture";
        var customData = frameContext.Create<MyCustomData>();

        // Add a raster render pass to the render graph. The PassData type parameter determines
        // the type of the passData output variable.
        using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName,
            out var passData))
        {
            // UniversalResourceData contains all the texture references used by URP,
            // including the active color and depth textures of the camera.
            UniversalResourceData frameData = frameContext.Get<UniversalResourceData>();

            // Populate passData with the data needed by the rendering function
            // of the render pass.
            // Use the camera's active color texture
            // as the source texture for the copy operation.
            passData.copySourceTexture = frameData.activeColorTexture;

            // Create a destination texture for the copy operation based on the settings,
            // such as dimensions, of the textures that the camera uses.
            // Set depthBufferBits to 0 to ensure that the CreateRenderGraphTexture method
            // creates a color texture and not a depth texture.
            TextureDesc desc = frameData.cameraDepthTexture.GetDescriptor(renderGraph);
            desc.depthBufferBits = 0;
            desc.format = _graphicsFormat;
            TextureHandle destination = renderGraph.CreateTexture(desc);

            customData.textureToTransfer = destination;
            // Declare that this render pass uses the source texture as a read-only input.
            builder.UseTexture(passData.copySourceTexture);

            // Declare that this render pass uses the temporary destination texture
            // as its color render target.
            // This is similar to cmd.SetRenderTarget prior to the RenderGraph API.
            builder.SetRenderAttachment(destination, 0);

            // Make sure the render graph system keeps the render pass, even if it's not used in the final frame.
            // Don't use this in production code, because it prevents the render graph system from removing the render pass if it's not needed. 
            builder.SetGlobalTextureAfterPass(destination, globalTextureID);
            builder.AllowPassCulling(false);
            

            // Set the ExecutePass method as the rendering function that render graph calls
            // for the render pass. 
            // This sample uses a lambda expression to avoid memory allocations.
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context)
                => ExecutePass(data, context));
        }
    }
    static void ExecutePass(PassData data, RasterGraphContext context)
    {
        // Records a rendering command to copy, or blit, the contents of the source texture
        // to the color render target of the render pass.
        Blitter.BlitTexture(context.cmd, data.copySourceTexture,
            new Vector4(1, 1, 0, 0), 0, false);
    }

}
