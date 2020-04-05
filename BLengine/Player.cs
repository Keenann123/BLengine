using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class Player : Entity
    {
        Camera cam;
        public Player() : base(new Vector3(0, 0, 0), new Quaternion(new Vector3(0, 0, 0)), new Vector3(1, 1, 1))
        {
            cam = new Camera(this);
            cam.SetPosition(new Vector3(0, 0, 0));
        }

        public Camera GetCamera()
        {
            return cam;
        }
    }
}
