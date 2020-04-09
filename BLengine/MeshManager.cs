using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class MeshManager
    {
        List<MeshComponent> Meshes = new List<MeshComponent>();
        public void Render(ShaderManager.ShaderType_BL type)
        {
            // render all meshes here!
            foreach(MeshComponent mesh in Meshes)
            {
                mesh.Render();
            }
        }

        public void AddMesh(MeshComponent mesh)
        {
            Meshes.Add(mesh);
        }
    }
}
