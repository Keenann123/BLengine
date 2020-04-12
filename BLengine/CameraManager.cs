using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class CameraManager
    {
        public static Camera ActiveCamera;
        public static List<Camera> Cameras = new List<Camera>();

        public static void SetActiveCamera(Camera cam)
        {
            ActiveCamera = cam;
        }

        public static Camera GetActiveCamera()
        {
            return ActiveCamera;
        }

        public static List<Camera> GetCameras()
        {
            return Cameras;
        }

        public static void AddCamera(Camera cam)
        {
            Cameras.Add(cam);
        }
    }
}
