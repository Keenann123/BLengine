using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using static RenderingEngine.ShaderManager;
using OpenTK.Input;

namespace RenderingEngine
{
    public class Window : GameWindow
    {
        ImGuiController _controller;
        public static int _Width = 1280;
        public static int _Height = 720;
        MeshComponent mesh1;
        MeshComponent mesh2;
        public static double fps;
        MeshComponent[] Meshes = new MeshComponent[10000];
        FullscreenQuad Q;
        Texture tex;
        Texture tex2;
        Player player;
        Light light;
        Light light2;
        Light light3;
        static FullscreenQuad q;

        List<MeshComponent> meshEntries = new List<MeshComponent>();

        public Window(GraphicsMode gMode) : base(_Width, _Height, gMode,
                                    "Legend286 and Boomer678's Rendering Engine",
                                    GameWindowFlags.Default,
                                    DisplayDevice.Default,
                                    4, 6, GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            light = new Light(new Vector3(-1f, -10f, 5f), new Vector3(1.0f, 0.0f, 0.0f), 1.0f, Light.LightType.LIGHT_DIRECTIONAL);
            light2 = new Light(new Vector3(5f, 10f, 5f), new Vector3(0.0f, 0.0f, 1.0f), 1.0f, Light.LightType.LIGHT_DIRECTIONAL);
            light3 = new Light(new Vector3(1f, -1f, 5f), new Vector3(0.0f, 1.0f, 0.0f), 1.0f, Light.LightType.LIGHT_DIRECTIONAL);
            int num = 2;
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    for (int k = 0; k < num; k++)
                    {
                        Meshes[i + j] = new MeshComponent("Meshes/sphere_lowres.obj");
                        Meshes[i + j].SetTranslation(i * 25, j * 25, k * 25);
                    }   
                }
            }
            
            Q = new FullscreenQuad(ShaderType_BL.DebugGBuffer, ShaderFlags.NONE);

            tex = new Texture("Textures/test.png");
            tex2 = new Texture("Textures/testnormal.png");
            _controller = new ImGuiController(Width, Height);
            _controller.CreateSkin();
            player = new Player();
            DeferredRenderer.SetupGBuffer();
            q = new FullscreenQuad(ShaderManager.ShaderType_BL.Default, ShaderManager.ShaderFlags.NONE);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Update the opengl viewport
            GL.Viewport(0, 0, Width, Height);
            DeferredRenderer.UpdateRenderViewport(Width, Height);
            // Tell ImGui of the new size
            _controller.WindowResized(Width, Height);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Util.TotalTime += (float)e.Time;  //TotalTime += deltaTime
            fps = 1.0 / e.Time;
            base.Title = "Legend286 and Boomer678's Rendering Engine" + ": OpenGL Version: " + GL.GetString(StringName.Version) + ": frametime: " + (int)fps;
            base.OnRenderFrame(e);

            _controller.Update(this, (float)e.Time);

            GL.ClearColor(new Color4(0.3f, 0.3f, 0.3f, 1.0f)); //pretty colors :^)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            #region Render Editor UI
            ImGui.Begin("Editor UI");
            ImGui.SetWindowSize(new System.Numerics.Vector2(256, Window._Height));
            ImGui.End();
            #endregion

            #region Render Debug ImGUI
            ImGui.Begin("Render Debug");
            ImGui.SetWindowSize(new System.Numerics.Vector2(450, 365), ImGuiCond.Once);
            ImGui.Text("Shaders: " + ShaderManager.GetShaderCount());

            ImGui.Text("Camera Pos: " + player.GetCamera().Position);
            


            if (ImGui.Button("Change Shader LIT", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.LIT | ShaderFlags.USE_DIFFUSE_TEXTURE | ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change Shader DIFFUSE", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.USE_DIFFUSE_TEXTURE;
            }
            if (ImGui.Button("Change Shader NORMAL", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change shader DEBUG_LIGHTING", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.DEBUG_LIGHTING | ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change Shader DEBUG_WORLDPOSITION", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.DEBUG_WORLDPOSITION;
            }
            if (ImGui.Button("Change Shader DEBUG_VIEWPOS", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.DEBUG_VIEWPOS;
            }
            if (ImGui.Button("Change Shader DEBUG_VIEWDIRECTION", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.DEBUG_VIEWDIRECTION;
            }
            if (ImGui.Button("Change Shader DEBUG_FOG", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = ShaderFlags.DEBUG_FOG;
            }
            if (ImGui.Button("Change Shader USE_NONE", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    Meshes[0].mat.flags = 0;
            }
            ImGui.SliderFloat("Fog End Distance", ref RenderingParameters.FogEndDistance, 10.0f, 10000.0f);

            if (ImGui.Button("Quit", new System.Numerics.Vector2(100, 32)))
            {
                Exit();
            }
            ImGui.End();
            #endregion


            #region Shader Live Coding
            ImGui.Begin("Shader Live Coding");
            ImGui.SetWindowSize(new System.Numerics.Vector2(530, 475), ImGuiCond.Once);
            if (ImGui.Button("Compile", new System.Numerics.Vector2(400, 32)))
            {
                foreach(var mesh in MeshManager.Meshes)
                {
                    Shader shader = new Shader(Meshes[0].mat.shader.SourceCode_frag, Meshes[0].mat.shader.SourceCode_vert);
                    ShaderManager.put(ShaderType_BL.GBuffer, mesh.mat.flags, shader);
                }
            
            }
            if (ImGui.CollapsingHeader("Fragment")){
                ImGui.InputTextMultiline("Fragment Shader", ref Meshes[0].mat.shader.SourceCode_frag, 4096, new System.Numerics.Vector2(800, 600));
            }
            if (ImGui.CollapsingHeader("Vertex"))
            {
                ImGui.InputTextMultiline("Vertex Shader", ref Meshes[0].mat.shader.SourceCode_vert, 4096, new System.Numerics.Vector2(800, 600));
            }
          
           
            ImGui.End(); 
            #endregion

            tex.UseTexture(TextureUnit.Texture0);
            tex2.UseTexture(TextureUnit.Texture1);


            player.GetCamera().ProcessInput();

            
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            DeferredRenderer.Render(); 
            

            if (keyboard.IsKeyDown(Key.F1))
            {
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_LIT;
            } 

            if (keyboard.IsKeyDown(Key.F2))
            {
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_NORMAL_ONLY;
            }

            if (keyboard.IsKeyDown(Key.F3))
            {
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_SPECULAR_ONLY;
            }

            if (keyboard.IsKeyDown(Key.F4))
            {
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_DEPTH_ONLY;
            }

            if (keyboard.IsKeyDown(Key.F5))
            {
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_DEBUG;
            }
            // render debug mode

            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_DIFFUSE_ONLY)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.DEBUG_DIFFUSE_ONLY);
                Q.Render();
            }

            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_NORMAL_ONLY)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.DEBUG_NORMAL_ONLY);
                Q.Render();
            }

            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_SPECULAR_ONLY)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.DEBUG_SPECULAR_ONLY);
                Q.Render();
            }

            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_DEPTH_ONLY)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.DEBUG_DEPTH_ONLY);
                Q.Render();
            }

            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_DEBUG)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.NONE);
                Q.Render();
            }


            // render light texture 
            if(DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_LIT)
            {
             //   DeferredRenderer.BindLightingTexture();
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.Lighting, ShaderFlags.LIT);
                Q.Render();
            }

            

            DeferredRenderer.UnbindGBufferTextures();
            _controller.Render();
            
            Util.CheckGLError("End of frame");
            q.Render();

            SwapBuffers();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            _controller.PressChar(e.KeyChar);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
