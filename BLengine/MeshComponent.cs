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
        ObjVolume vol;

        public Material mat;

        public bool shaderChanged = false;

        public MeshComponent(string filename)
        {
            Update();
            mat = new Material("Textures/test.png", "Textures/testnormal.png", "");
            vol = ObjVolume.get(filename, mat);
            MeshManager.AddMesh(this);
        }

        public void Render(ShaderType_BL type)
        {
            // add material stuff here
            mat.RenderMaterial();
            mat.SetShader(type);
            mat.UpdateWorldTransformMatrix(GetModelMatrix());
            mat.shader.BindMatrix4("view", CameraManager.GetActiveCamera().GetViewMatrix());
            mat.shader.BindMatrix4("projection", CameraManager.GetActiveCamera().GetProjectionMatrix());
            mat.shader.BindVector3("viewPos", CameraManager.GetActiveCamera().Position);
            mat.shader.BindFloat("FogEndDistance", RenderingParameters.FogEndDistance);
            
            vol.Render();
        }
        public void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            vol.OnUnload();
            MeshManager.Meshes.Remove(this);
        }

        public void Update()
        {
            Matrix4 scale = Matrix4.CreateScale(Scale.X / 100f, Scale.Y / 100f, Scale.Z / 100f);
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
