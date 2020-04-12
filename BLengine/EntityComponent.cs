using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class EntityComponent
    {
        Vector3 localPosition;
        Quaternion localRotation;
        Vector3 localScale;
        Entity parent;

        public EntityComponent(Entity p, Vector3 localposition, Quaternion localrotation, Vector3 localscale)
        {
            localPosition = localposition;
            localRotation = localrotation;
            localScale = localscale;
            parent = p;
        }

        public virtual void Update()
        {

        }

        public void SetLocalPosition(Vector3 pos)
        {
            localPosition = pos;
            Update();
        }

        public Vector3 GetLocalPosition()
        {
            return localPosition;
        }

        public void SetLocalRotation(Vector3 rot)
        {
            localRotation = Quaternion.FromEulerAngles(rot);
            Update();
        }

        public void SetLocalRotation(Quaternion q)
        {
            localRotation = q;
            Update();
        }

        public Quaternion GetLocalRotation()
        {
            return localRotation;
        }

        public void SetLocalScale(float scl)
        {
            localScale = new Vector3(scl, scl, scl);
            Update();
        }

        public void SetLocalScale(Vector3 scl)
        {
            localScale = scl;
            Update();
        }
    }
}
