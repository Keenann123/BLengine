using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class ShaderManager
    {
        private static Dictionary<ShaderFlags, Shader> Shaders = new Dictionary<ShaderFlags, Shader>();

        public static int GetShaderCount() { return Shaders.Count; }

        public enum ShaderType_BL
        {
            Default,
            ImGui
        };

        [Flags]
        public enum ShaderFlags
        {
            NONE = 0,
            USE_TEST1 = 1,
            USE_TEST2 = 2,
            USE_TEST3 = 4
        };


        public static Shader get(ShaderType_BL type, ShaderFlags flags = 0)
        {
            if (Shaders.ContainsKey(flags))
            {
                //Return existing shader
                return Shaders[flags];
            }
            else
            {
                //Create shader
                Shader shader = new Shader(type, flags);
                Shaders.Add(flags, shader);
                shader.UseShader();
                return shader;
            }
        }
    }
}
