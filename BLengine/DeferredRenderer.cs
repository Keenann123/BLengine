using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;

namespace RenderingEngine
{
    class DeferredRenderer
    {
        static int TextureWidth = 512;
        static int TextureHeight = 512;
        static uint AlbedoRT;
        static uint NormalRT;
        static uint SpecularRT;
        static uint DepthRT;
        static uint DeferredFBOHandle;
        static Rectangle viewport;
        static Rectangle oldViewport;
        static uint LightingRT;
        static uint LightingFBOHandle;

        public static void Render()
        {
            BeginRenderToGBuffer();
            GL.ClearColor(new Color4(0.1f, 0.07f, 0.13f, 1.0f)); //pretty colors :^)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            viewport.Width = TextureWidth;
            viewport.Height = TextureHeight;
            GL.Viewport(viewport);
            RenderToGBuffer();
            GL.Viewport(oldViewport);
            EndRenderToGBuffer();
        }

        public static void UpdateRenderViewport(int width, int height)
        {
            oldViewport.Width = width;
            oldViewport.Height = height;
        }

        public static void RenderToGBuffer()
        {
            MeshManager.Render(ShaderManager.ShaderType_BL.GBuffer);
        }

        public static void BeginRenderToGBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, DeferredFBOHandle);
        }
        public static void EndRenderToGBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
        }

        public static void BeginRenderToLightingBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, LightingFBOHandle);
        }

        public static void BindGBufferTextures()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, AlbedoRT);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, NormalRT);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, SpecularRT);
            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, DepthRT);
        }

        public static void SetupGBuffer()
        {
            GL.GenTextures(1, out AlbedoRT);
            GL.BindTexture(TextureTarget.Texture2D, AlbedoRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureWidth, TextureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, AlbedoRT, 0);

            GL.GenTextures(1, out NormalRT);
            GL.BindTexture(TextureTarget.Texture2D, NormalRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureWidth, TextureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment1, TextureTarget.Texture2D, NormalRT, 0);

            GL.GenTextures(1, out SpecularRT);
            GL.BindTexture(TextureTarget.Texture2D, SpecularRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureWidth, TextureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment2, TextureTarget.Texture2D, SpecularRT, 0);

            GL.GenTextures(1, out DepthRT);
            GL.BindTexture(TextureTarget.Texture2D, DepthRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.R32f, TextureWidth, TextureHeight, 0, PixelFormat.Red, PixelType.UnsignedInt, IntPtr.Zero);
            // things go horribly wrong if DepthComponent's Bitcount does not match the main Framebuffer's Depth
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment3, TextureTarget.Texture2D, DepthRT, 0);

            // Create internal depth buffer
            uint internalDepthBuffer;
            GL.GenTextures(1, out internalDepthBuffer);
            GL.BindTexture(TextureTarget.Texture2D, internalDepthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent32, TextureWidth, TextureHeight, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            // Create a FBO and attach the textures
            GL.GenFramebuffers(1, out DeferredFBOHandle);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, DeferredFBOHandle);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, AlbedoRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment1Ext, TextureTarget.Texture2D, NormalRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment2Ext, TextureTarget.Texture2D, SpecularRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment3Ext, TextureTarget.Texture2D, DepthRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, internalDepthBuffer, 0);

            // Create the lighting buffer
            GL.GenTextures(1, out LightingRT);
            GL.BindTexture(TextureTarget.Texture2D, LightingRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureWidth, TextureHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
          
            GL.GenFramebuffers(1, out LightingFBOHandle);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, LightingFBOHandle);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, LightingRT, 0);
            
            EndRenderToGBuffer();
        }
    }
}
