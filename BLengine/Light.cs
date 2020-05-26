using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Diagnostics;

namespace RenderingEngine
{
    class Light
    {
        public Vector3 position;
        public Vector3 colour;

        public Light(Vector3 pos, Vector3 col)
        {
            DeferredRenderer.AddLightToRenderer(this);
            position = pos;
            colour = col;
        }

        public virtual void RemoveLight()
        {
            DeferredRenderer.RemoveLightFromRenderer(this);
        }
        public virtual void Render()
        {
            Debug.Print("Rendering light");
        }

    }

}
