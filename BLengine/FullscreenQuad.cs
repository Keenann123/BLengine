﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using static RenderingEngine.ShaderManager;

namespace RenderingEngine
{
    class FullscreenQuad
    {
        Shader shader;
        float[] vertices =
                           {
                            //Position          Texture coordinates              Normals 
                            1.0f,  1.0f, 0.0f,  1.0f, 1.0f, // top right
                            1.0f, -1.0f, 0.0f,  1.0f, 0.0f, // bottom right
                           -1.0f, -1.0f, 0.0f,  0.0f, 0.0f, // bottom left
                           -1.0f,  1.0f, 0.0f,  0.0f, 1.0f  // top left
                           };

        uint[] indices =
                         {
                          0, 1, 3,
                          1, 2, 3,
                         };

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;

        public FullscreenQuad(ShaderType_BL type)
        {
            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();
            Initialise(type);
        }
        void Initialise(ShaderType_BL type)
        {
            shader = ShaderManager.get(type, ShaderFlags.NONE);
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            int texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);

            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        }

        public void Render()
        {
            shader.UseShader();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        public void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(ElementBufferObject);
        }
    }
}
