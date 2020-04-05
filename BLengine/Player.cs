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
        CameraComponent cam;
        public Player() : base(new Vector3(0, 0, 0), new Quaternion(new Vector3(0, 0, 0)), new Vector3(1, 1, 1))
        {
            cam = new CameraComponent(this);
            cam.SetLocalPosition(new Vector3(0, 0, 0));
            cam.SetLocalRotation(new Vector3(0, 0, 0));
            cam.SetLocalScale(new Vector3(0.5f, 0.5f, 0.5f));
        }

        public CameraComponent GetCamera()
        {
            return cam;
        }
    }
}
