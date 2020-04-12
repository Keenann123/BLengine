using System;
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
    class MeshComponent
    {
        Matrix4 ModelMatrix;
        Vector3 Translation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 Rotation = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 Scale = new Vector3(1.0f, 1.0f, 1.0f);
        float[] vertices =
                           {
                            //Position          Texture coordinates              Normals 
                            0.5f,  0.5f, 0.0f,  1.0f, 1.0f, 0.0f, 0.0f, 1.0f, // top right
                            0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f, 0.0f, 1.0f, // bottom right
                           -0.5f, -0.5f, 0.0f,  0.0f, 0.0f, 0.0f, 0.0f, 1.0f, // bottom left
                           -0.5f,  0.5f, 0.0f,  0.0f, 1.0f, 0.0f, 0.0f, 1.0f,  // top left
                           };

        uint[] indices =
                         {
                          0, 1, 3,
                          1, 2, 3,
                         };

        int VertexBufferObject;
        int VertexArrayObject;
        int ElementBufferObject;
        public Material mat;

        public MeshComponent()
        {
            Update();

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();
            mat = new Material("Textures/test.png", "Textures/testnormal.png", this);
            MeshManager.AddMesh(this);
            Initialise();
        }
        void Initialise()
        {
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            int texCoordLocation = mat.shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            int normalLocation = mat.shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);

            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));
        }

        public void Render(ShaderType_BL type)
        {
            // add material stuff here
            mat.RenderMaterial();
            mat.shader = ShaderManager.get(type, mat.flags);
            mat.shader.BindMatrix4("model", GetModelMatrix());
            mat.shader.BindMatrix4("view", CameraManager.GetActiveCamera().GetViewMatrix());
            mat.shader.BindMatrix4("projection", CameraManager.GetActiveCamera().GetProjectionMatrix());
            mat.shader.BindVector3("viewPos", CameraManager.GetActiveCamera().Position);
            mat.shader.BindFloat("FogEndDistance", RenderingParameters.FogEndDistance);


            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        public void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(ElementBufferObject);
            MeshManager.Meshes.Remove(this);
        }

        public void Update()
        {
            Matrix4 scale = Matrix4.CreateScale(Scale.X, Scale.Y, Scale.Z);
            Matrix4 rotation =  Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Rotation.X)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Rotation.Y)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation.Z));
            Matrix4 translation = Matrix4.CreateTranslation(Translation);
            ModelMatrix = scale * rotation * translation;
        }

        public Matrix4 GetModelMatrix()
        {
            return ModelMatrix;
        }

        public void SetScale(float scale)
        {
            Scale = new Vector3(scale, scale, scale);
            Update();
        }

        public void SetScale(Vector3 scale)
        {
            Scale = scale;
            Update();
        }

        public void SetRotation(float x, float y, float z)
        {
            Rotation = new Vector3(x, y, z);
            Update();
        }

        public void SetRotation(Vector3 rotation)
        {
            Rotation = rotation;
            Update();
        }

        public void SetTranslation(float x, float y, float z)
        {
            Translation = new Vector3(x, y, z);
            Update();
        }

        public void SetTranslation(Vector3 translation)
        {
            Translation = translation;
            Update();
        }
    }
}
