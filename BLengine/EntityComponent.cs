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
    }
}
