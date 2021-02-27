using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp.Processing;

namespace RenderingEngine
{
    public class Light
    {
        FullscreenQuad Q;
        Vector3 lightPosition;
        Vector3 lightColour;
        float lightIntensity;
        LightType lightType;
        public enum LightType
        {
            LIGHT_DIRECTIONAL = 0,
            LIGHT_POINT = 1,
            LIGHT_SPOT = 2,
            LIGHT_AREA = 4
        }
        public Light(Vector3 pos, Vector3 col, float intensity, LightType type)
        {
            lightType = type;
            if (lightType == LightType.LIGHT_DIRECTIONAL)
            {
                Q = new FullscreenQuad(ShaderManager.ShaderType_BL.DeferredDirectional, ShaderManager.ShaderFlags.LIT);
            }    

            lightPosition = pos;
            lightColour = col;
            lightIntensity = intensity;


            DeferredRenderer.AddLightToRenderer(this);
        }

        public void Render()
        {

            Q.shader.UseShader();
            Q.shader.BindVector3("lightColour", lightColour);
            Q.shader.BindVector3("lightPosition", lightPosition);
            Q.shader.BindFloat("lightIntensity", lightIntensity);
            Q.shader.BindVector3("cameraPosition", CameraManager.GetActiveCamera().Position);
            Q.Render();
        }
        public virtual void RemoveLight()
        {
            DeferredRenderer.RemoveLightFromRenderer(this);
        }
    }
}
