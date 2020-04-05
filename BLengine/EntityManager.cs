using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class EntityManager
    {
        static List<Entity> entityList = new List<Entity>();

        public static void AddEntity(Entity ent)
        {
            entityList.Add(ent);
        }

        public static void UpdateEntities()
        {
            foreach(Entity ent in entityList)
            {
                ent.Update();
            }
        }
    }
}
