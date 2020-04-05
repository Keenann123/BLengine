using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class CameraComponent : EntityComponent
    {
        Matrix4 cameraMatrix;
        public CameraComponent(Entity parent) : base(parent, new Vector3(0, 0, 0), new Quaternion(new Vector3(0, 0, 0)), new Vector3(1, 1, 1))
        {

        }

        public override void Update()
        {
            Matrix4 scale = Matrix4.CreateScale(0.5f);
            Matrix4 rotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(45f));
            cameraMatrix = scale * rotation;// * translation;
            // do matrix multiplying here with rotation and scale!!!
        }

        public Matrix4 GetMatrix()
        {
            return cameraMatrix;
        }
    }
}
