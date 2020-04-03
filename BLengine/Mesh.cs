using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;


namespace RenderingEngine
{
    class Mesh
    {
        float[] vertices = {
                             -0.5f, -0.5f, 0.0f, //Bottom-left vertex
                              0.5f, -0.5f, 0.0f, //Bottom-right vertex
                              0.0f,  0.5f, 0.0f  //Top vertex
                            };
        int VBO;
        int VAO;
        Shader shader;

        public Mesh()
        {
            shader = new Shader("test", "../../../Shaders/default.vert" ,"../../../Shaders/default.frag");
            VBO = GL.GenBuffer();
            VAO = GL.GenVertexArray();
            Initialise();
        }
        void Initialise()
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

           
        }

        public void Render()
        {
            shader.UseShader();
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }
        void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
        }
    }
}
