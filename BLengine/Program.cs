using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace RenderingEngine 
{
    class Program
    {
        public static Window wnd;
        static void Main(string[] args)
        {
            GraphicsMode mode = new GraphicsMode(new ColorFormat(24), 32, 8, 4, new ColorFormat(32), 2, false);
            wnd = new Window(mode);
            wnd.Run(144.0, 144.0);
        }
    }
}
