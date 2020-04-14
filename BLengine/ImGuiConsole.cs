using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
namespace RenderingEngine
{

    class ImGuiConsole_Methods
    {
        
        void Test()
        {

        }
    }

    class ImGuiConsole
    {
        public List<string> entries = new List<string>();
        public void Setup()
        {

        }
 
        public void RenderConsole()
        {
            ImGui.Begin("Console");

            foreach(var entry in entries)
            {
                ImGui.Text(entry);
            }

            ImGui.End();
        }
        public void Print(string print)
        {
            entries.Add(print);
        }
    }
}
