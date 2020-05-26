using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class LightDirectional : Light
    {
        FullscreenQuad Q;
        Shader shader = ShaderManager.get(ShaderManager.ShaderType_BL.DebugGBuffer, ShaderManager.ShaderFlags.LIT);
        public LightDirectional(Vector3 pos, Vector3 col) : base(pos, col)
        {
            base.colour = col;
            base.position = pos;
            DeferredRenderer.AddLightToRenderer(this);
        }

        public override void Render()
        {
            base.Render();
            shader.UseShader();
            shader.BindVector3("LightColour", base.colour);
            shader.BindVector3("LightDirection", base.position);
            shader.BindVector3("cameraPosition", CameraManager.GetActiveCamera().Position);
            Q.shader = shader;
            Q.Render();
        }
    }
}
