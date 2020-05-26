using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace RenderingEngine
{
    class Player
    {
        Camera cam;
        public Player()
        { 
            cam = new Camera();
        }

        public Camera GetCamera()
        {
            return cam;
        }
    }
}
