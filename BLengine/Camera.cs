using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class Camera
    {
        Matrix4 ProjectionMatrix;
        Matrix4 ViewMatrix;
        Vector3 Position;
        Vector3 LookTarget;
        Vector3 Direction;

        public Camera(Entity parent)
        {
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            LookTarget = new Vector3(0.0f, 0.0f, -1.0f);
            Direction = Vector3.Normalize(Position - LookTarget);
            Update();
        }

        public void Update()
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)Window._Width / (float)Window._Height, 0.1f, 1000.0f);
            

            Vector3 Up = Vector3.UnitY;
            Vector3 CameraRight = Vector3.Normalize(Vector3.Cross(Up, Direction));
            Vector3 CameraUp = Vector3.Cross(Direction, CameraRight);
            ViewMatrix = Matrix4.LookAt(Position, LookTarget, CameraUp);
        }

        public void SetPosition(Vector3 pos)
        {
            Position = pos;
            Update();
        }

        public void SetLookTarget(Vector3 pos)
        {
            LookTarget = pos;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return ProjectionMatrix;
        }

        public Matrix4 GetViewMatrix()
        {
            return ViewMatrix;
        }
    }
}
