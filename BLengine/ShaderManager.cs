using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderingEngine
{
    class ShaderManager
    {
        private static Dictionary<Tuple<ShaderFlags, ShaderType_BL>, Shader> Shaders = new Dictionary<Tuple<ShaderFlags, ShaderType_BL>, Shader>();
        public static Shader currentShader;

        public static int GetShaderCount() { return Shaders.Count; }

        public enum ShaderType_BL
        {
            Default,
            GBuffer,
            ImGui,
            DebugGBuffer,
            DeferredDirectional,
            DeferredSpot,
            DeferredPoint,
            DeferredArea,
            Lighting
        };

        [Flags]
        public enum ShaderFlags
        {
            NONE = 0,
            DEBUG_LIGHTING = 1,
            USE_DIFFUSE_TEXTURE = 2,
            USE_NORMAL_TEXTURE = 4,
            USE_SPECULAR_TEXTURE = 8,
            DEBUG_FOG = 16,
            DEBUG_VIEWPOS = 32,
            DEBUG_VIEWDIRECTION = 64,
            DEBUG_WORLDPOSITION = 128,
            LIT = 256
        };


        public static void put(ShaderType_BL type, ShaderFlags flags, Shader shader)
        {
            Shaders[Tuple.Create(flags, type)] = shader;
        }

        public static Shader get(ShaderType_BL type, ShaderFlags flags = 0)
        {
            if (Shaders.ContainsKey(Tuple.Create(flags, type)))
            {
                //Return existing shader
                return Shaders[Tuple.Create(flags, type)];
            }
            else
            {
                //Create shader
                Shader shader = new Shader(type, flags);
                Shaders.Add(Tuple.Create(flags, type), shader);
                return shader;
            }
        }

        public static void SetCurrentShader(Shader shader)
        {
            currentShader = shader;
        }

        public static Shader GetCurrentShader()
        {
            return currentShader;
        }
    }
}
