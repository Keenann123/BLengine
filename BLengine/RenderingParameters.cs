using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class RenderingParameters
    {
        public static float FogEndDistance = 100.0f;
        public static Vector3 FogColour = new Vector3(0.5f, 0.5f, 0.9f);
        public static Vector3 SunDirection = new Vector3(0.0f, 10.0f, 10.0f);
        public static bool EnableDepthTest = true;

    }
}
