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
using RenderingEngine.ShaderEditor;
using OpenTK.Input;

namespace RenderingEngine
{
    public class Window : GameWindow
    {
        ImGuiController _controller;
        public static int _Width = 1280;
        public static int _Height = 720;
        public static double fps;
        MeshComponent[] Meshes = new MeshComponent[10000];
        FullscreenQuad Q;
        Texture tex;
        Texture tex2;
        Player player;
        Light light;
        Light light2;
        Light light3;
        System.Numerics.Vector3 lightColour1 = new System.Numerics.Vector3(1.0f, 0.0f, 0.0f);
        System.Numerics.Vector3 lightColour2 = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
        System.Numerics.Vector3 lightColour3 = new System.Numerics.Vector3(0.0f, 0.0f, 0.0f);
        float light1Intensity = 1.0f, light2Intensity = 1.0f, light3Intensity = 1.0f;
        float lightRadius = 1000.0f;
        static FullscreenQuad q;
        ShaderEditorGraph graph = new ShaderEditorGraph();
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
            GL.DepthRange(0.0f, 1.0f);
            base.OnLoad(e);
            light = new Light(new Vector3(-1f, -1f, -1f), new Vector3(lightColour1.X, lightColour1.Y, lightColour1.Z), light1Intensity, lightRadius, Light.LightType.LIGHT_POINT);
            light2 = new Light(new Vector3(5f, 10f, 5f), new Vector3(lightColour2.X, lightColour2.Y, lightColour2.Z), light2Intensity, 1.0f, Light.LightType.LIGHT_DIRECTIONAL);
            light3 = new Light(new Vector3(1f, -1f, 5f), new Vector3(lightColour3.X, lightColour3.Y, lightColour3.Z), light3Intensity, 1.0f, Light.LightType.LIGHT_DIRECTIONAL);
            int num = 6;
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

            GL.ClearColor(new Color4(0.0f, 0.0f, 0.0f, 0.0f)); //pretty colors :^)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            #region Render Editor UI
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0.0f, 0.0f));
            ImGui.Begin("Editor UI");
            ImGui.SetWindowSize(new System.Numerics.Vector2(380, Window._Height));
            
            ImGui.ColorEdit3("Light 1 Colour", ref lightColour1);
            ImGui.DragFloat("Light 1 Intensity", ref light1Intensity);
            ImGui.DragFloat("Light 1 Radius", ref lightRadius);
            ImGui.ColorEdit3("Light 2 Colour", ref lightColour2);
            ImGui.DragFloat("Light 2 Intensity", ref light2Intensity);
            ImGui.ColorEdit3("Light 3 Colour", ref lightColour3);
            ImGui.DragFloat("Light 3 Intensity", ref light3Intensity);

            ImGui.End();
            #endregion

            // move this to editorlayer or something :)
            light.setColour(lightColour1);
            light2.setColour(lightColour2);
            light3.setColour(lightColour3);
            light.setIntensity(light1Intensity);
            light2.setIntensity(light2Intensity);
            light3.setIntensity(light3Intensity);
            light.setRadius(lightRadius);
            bool isOpenMenu = true;
            #region Render Debug ImGUI
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(Width - 450, 0.0f));
            ImGui.Begin("Render Debug", ref isOpenMenu, ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.None);
            ImGui.SetWindowSize(new System.Numerics.Vector2(450, 365), ImGuiCond.Once);
            ImGui.Text("Shaders: " + ShaderManager.GetShaderCount());

            ImGui.Text("Camera Pos: " + player.GetCamera().Position);
           
            ImGui.SliderFloat("Fog End Distance", ref RenderingParameters.FogEndDistance, 10.0f, 10000.0f);

            if (ImGui.Button("Quit", new System.Numerics.Vector2(100, 32)))
            {
                Exit();
            }
            ImGui.End();
            #endregion

            graph.RenderGraph((float)e.Time);
            #region Shader Live Coding
            ImGui.Begin("Shader Live Coding");
            ImGui.SetWindowSize(new System.Numerics.Vector2(530, 475), ImGuiCond.Once);
            if (ImGui.Button("Compile", new System.Numerics.Vector2(400, 32)))
            {
                foreach(var mesh in MeshManager.Meshes)
                {
                    Shader shader = new Shader(mesh.mat.shader.SourceCode_frag, mesh.mat.shader.SourceCode_vert);
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
                DeferredRenderer.mode = DeferredRenderer.RenderingMode.RENDER_WORLD_POSITION_ONLY;
            }

            if (keyboard.IsKeyDown(Key.F6))
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


            if (DeferredRenderer.mode == DeferredRenderer.RenderingMode.RENDER_WORLD_POSITION_ONLY)
            {
                DeferredRenderer.BindGBufferTextures();
                Q.shader = ShaderManager.get(ShaderType_BL.DebugGBuffer, ShaderFlags.DEBUG_WORLD_POSITION);
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
