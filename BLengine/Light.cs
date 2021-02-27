using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace RenderingEngine
{
    class Light
    {
        FullscreenQuad Q;
        Vector3 position;
        Vector3 colour;
        LightType lightType;
        public enum LightType
        {
            LIGHT_DIRECTIONAL = 0,
            LIGHT_POINT = 1,
            LIGHT_SPOT = 2,
            LIGHT_AREA = 4
        }
        public Light(Vector3 pos, Vector3 col, LightType type)
        {
            if (type == LightType.LIGHT_DIRECTIONAL)
            {
                Q = new FullscreenQuad(ShaderManager.ShaderType_BL.DeferredDirectional, ShaderManager.ShaderFlags.LIT);
            }    

            position = pos;
            colour = col;

            DeferredRenderer.AddLightToRenderer(this);
        }

        public void Render()
        {
            
            Q.shader.UseShader();
            Q.shader.BindVector3("lightColour", colour);
            Q.shader.BindVector3("lightDirection", position);
            Q.shader.BindVector3("cameraPosition", CameraManager.GetActiveCamera().Position);
            Q.Render();
        }
        public virtual void RemoveLight()
        {
            DeferredRenderer.RemoveLightFromRenderer(this);
        }
    }
}
