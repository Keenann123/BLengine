using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;

namespace RenderingEngine
{
    class Camera
    {
        Matrix4 ProjectionMatrix;
        Matrix4 ViewMatrix;
        public Vector3 Position;
        Vector3 LookTarget;
        Vector3 Direction;


        protected const float m_pitchLimit = 1.8f;
        protected const float m_speed = 0.125f;
        protected const float m_mouseSpeedX = 0.0035f;
        protected const float m_mouseSpeedY = 0.0035f;
        protected Vector3 m_up = Vector3.UnitZ;

        public Camera()
        {
            
            Position = new Vector3(0.1f, -0.14f, 11f);
            LookTarget = new Vector3(0f, 0f, 90f);

            Direction = Vector3.Normalize(Position - LookTarget);

            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1280f / 720f, 1f, 10000); //fix aspect for resize
            ViewMatrix = CreateLookAt();
            CameraManager.AddCamera(this);
            CameraManager.SetActiveCamera(this);
        }

        protected MouseState m_prevMouse;

        public void UpdateCameraMatrix(float width, float height)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, width / height, 1f, 10000); //fix aspect for resize
        }
        public void ProcessInput()
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            if (mouse.IsButtonDown(MouseButton.Right))
            {

                // Move camera with WASD keys
                if (keyboard.IsKeyDown(Key.W))
                {
                    // Move forward and backwards by adding m_position and m_direction vectors
                    Position += Direction * m_speed;
                }

                if (keyboard.IsKeyDown(Key.S))
                {
                    Position -= Direction * m_speed;
                }

                if (keyboard.IsKeyDown(Key.A))
                {
                    // Strafe by adding a cross product of m_up and m_direction vectors
                    Position += Vector3.Cross(m_up, Direction) * m_speed;
                }

                if (keyboard.IsKeyDown(Key.D))
                {
                    Position -= Vector3.Cross(m_up, Direction) * m_speed;
                }
                if (keyboard.IsKeyDown(Key.Space))
                {
                    Position += m_up * m_speed;
                }
                if (keyboard.IsKeyDown(Key.ControlLeft) || keyboard.IsKeyDown(Key.X))
                {
                    Position -= m_up * m_speed;
                }

                // Calculate yaw to look around with a mouse
                Direction = Vector3.Transform(Direction, Matrix3.CreateFromAxisAngle(m_up, -m_mouseSpeedX * (mouse.X - m_prevMouse.X)));

                // Pitch is limited to m_pitchLimit
                float angle = m_mouseSpeedY * (mouse.Y - m_prevMouse.Y);
                if ((Pitch < m_pitchLimit || angle > 0) && (Pitch > -m_pitchLimit || angle < 0))
                {
                    Direction = Vector3.Transform(Direction,
                        Matrix3.CreateFromAxisAngle(Vector3.Cross(m_up, Direction), angle)
                    );
                }

               
                ViewMatrix = CreateLookAt();

            }

            if (keyboard.IsKeyDown(Key.Z))
            {
                //Change Render Mode
            }

            m_prevMouse = mouse;
        }


        protected Matrix4 CreateLookAt()
        {
            return Matrix4.LookAt(Position, Position + Direction, m_up);
        }

        public double Pitch
        {
            get { return Math.Asin(Direction.Y); }
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
