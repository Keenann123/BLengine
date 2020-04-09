﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenTK.Graphics.OpenGL4;

namespace RenderingEngine
{
    class DeferredRenderer
    {
        int TextureSize = 2048;
        uint AlbedoRT;
        uint NormalRT;
        uint DepthRT;
        uint DeferredFBOHandle;

        uint LightingRT;
        uint LightingFBOHandle;

        void RenderToGBuffer()
        {
            // here we should render all objects
            //MeshManager.Render(ShaderManager.ShaderType_BL.Default);
        }

        void SetupGBuffer()
        {
            GL.GenTextures(1, out AlbedoRT);
            GL.BindTexture(TextureTarget.Texture2D, AlbedoRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureSize, TextureSize, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.GenTextures(1, out NormalRT);
            GL.BindTexture(TextureTarget.Texture2D, NormalRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, TextureSize, TextureSize, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            GL.GenTextures(1, out DepthRT);
            GL.BindTexture(TextureTarget.Texture2D, DepthRT);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.R32f, TextureSize, TextureSize, 0, PixelFormat.Red, PixelType.UnsignedInt, IntPtr.Zero);
            // things go horribly wrong if DepthComponent's Bitcount does not match the main Framebuffer's Depth
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            // Create internal depth buffer
            uint internalDepthBuffer;
            GL.GenTextures(1, out internalDepthBuffer);
            GL.BindTexture(TextureTarget.Texture2D, internalDepthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)All.DepthComponent32, TextureSize, TextureSize, 0, PixelFormat.DepthComponent, PixelType.UnsignedInt, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

            // Create a FBO and attach the textures
            GL.GenFramebuffers(1, out DeferredFBOHandle);
            GL.BindFramebuffer(FramebufferTarget.FramebufferExt, DeferredFBOHandle);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, AlbedoRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment1Ext, TextureTarget.Texture2D, NormalRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment2Ext, TextureTarget.Texture2D, DepthRT, 0);
            GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, internalDepthBuffer, 0);

        }
    }
}