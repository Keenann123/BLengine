using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class Entity
    {
        Vector3 position;
        Quaternion rotation;
        Vector3 scale;
        List<EntityComponent> components;

        public Entity(Vector3 pos, Quaternion rot, Vector3 scl)
        {
            position = pos;
            rotation = rot;
            scale = scl;
            EntityManager.AddEntity(this);
        }

        public void SetPosition(Vector3 pos)
        {
            position = pos;
            UpdateComponents();
        }

        public void SetRotation(Vector3 rot)
        {
            rotation = Quaternion.FromEulerAngles(rot);
            UpdateComponents();
        }

        public void SetRotation(Quaternion rot)
        {
            rotation = rot;
            UpdateComponents();
        }

        public void SetScale(Vector3 scl)
        {
            scale = scl;
            UpdateComponents();
        }

        public void SetScale(float scl)
        {
            scale = new Vector3(scl, scl, scl);
            UpdateComponents();
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Quaternion GetRotation()
        {
            return rotation;
        }

        public Vector3 GetScale()
        {
            return scale;
        }

        public void UpdateComponents()
        {
            foreach (EntityComponent c in components)
            {
                c.Update();
            }
        }

        public void Update()
        {
            UpdateComponents();
        }

    }
}
