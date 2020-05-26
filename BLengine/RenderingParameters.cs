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
        public static Vector3 SunDirection = new Vector3(0.0f, 1.0f, -1.0f);
        public static bool EnableDepthTest = true;
        public static float zNear = 1;
        public static float zFar = 10000;
    }
}
