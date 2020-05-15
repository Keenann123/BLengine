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
          //  mesh1 = new MeshComponent();
            mesh2 = new MeshComponent();
            
            for(int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    Meshes[i + j] = new MeshComponent();
                    Meshes[i + j].SetTranslation(i * 10, j * 10, 0);
                }
            }
            Q = new FullscreenQuad(ShaderType_BL.DebugGBuffer);
            mesh2.SetTranslation(0.0f, 0.0f, 0.0f);
            mesh2.SetRotation(0.0f, 0.0f, 0.0f);
            tex = new Texture("Textures/test.png");
            tex2 = new Texture("Textures/testnormal.png");
            _controller = new ImGuiController(Width, Height);
            _controller.CreateSkin();
            player = new Player();
            DeferredRenderer.SetupGBuffer();
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
          //  mesh1.mat.DiffuseColour = new Vector3((float)Math.Sin(Util.TotalTime), (float)Math.Cos(Util.TotalTime), 0.5f);
        //    mesh1.SetTranslation((float)Math.Sin(Util.TotalTime) * 3, (float)Math.Cos(Util.TotalTime) * 3, (float)Math.Sin(Util.TotalTime) * 2);
        //    mesh1.SetScale(100.0f);
        //    mesh1.SetRotation(0, 0, Util.TotalTime * 10);

            _controller.Update(this, (float)e.Time);

            GL.ClearColor(new Color4(0.1f, 0.07f, 0.13f, 1.0f)); //pretty colors :^)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            #region Render Debug ImGUI
            ImGui.Begin("Render Debug");
            ImGui.SetWindowSize(new System.Numerics.Vector2(450, 365), ImGuiCond.Once);
            ImGui.Text("Shaders: " + ShaderManager.GetShaderCount());

            ImGui.Text("Camera Pos: " + player.GetCamera().Position);
            ImGui.Text("Shader Flags: " + mesh2.mat.flags.ToString());


            if (ImGui.Button("Change Shader LIT", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.LIT | ShaderFlags.USE_DIFFUSE_TEXTURE | ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change Shader DIFFUSE", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.USE_DIFFUSE_TEXTURE;
            }
            if (ImGui.Button("Change Shader NORMAL", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change shader DEBUG_LIGHTING", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.DEBUG_LIGHTING | ShaderFlags.USE_NORMAL_TEXTURE;
            }
            if (ImGui.Button("Change Shader DEBUG_WORLDPOSITION", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.DEBUG_WORLDPOSITION;
            }
            if (ImGui.Button("Change Shader DEBUG_VIEWPOS", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.DEBUG_VIEWPOS;
            }
            if (ImGui.Button("Change Shader DEBUG_VIEWDIRECTION", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.DEBUG_VIEWDIRECTION;
            }
            if (ImGui.Button("Change Shader DEBUG_FOG", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = ShaderFlags.DEBUG_FOG;
            }
            if (ImGui.Button("Change Shader USE_NONE", new System.Numerics.Vector2(400, 32)))
            {
                foreach (var mesh in MeshManager.Meshes)
                    mesh.mat.flags = 0;
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
                    Shader shader = new Shader(mesh2.mat.shader.SourceCode_frag, mesh2.mat.shader.SourceCode_vert);
                    ShaderManager.put(ShaderType_BL.GBuffer, mesh.mat.flags, shader);
                }
            
            }
            if (ImGui.CollapsingHeader("Fragment")){
                ImGui.InputTextMultiline("Fragment Shader", ref mesh2.mat.shader.SourceCode_frag, 4096, new System.Numerics.Vector2(800, 600));
            }
            if (ImGui.CollapsingHeader("Vertex"))
            {
                ImGui.InputTextMultiline("Vertex Shader", ref mesh2.mat.shader.SourceCode_vert, 4096, new System.Numerics.Vector2(800, 600));
            }
          
           
            ImGui.End(); 
            #endregion

            tex.UseTexture(TextureUnit.Texture0);
            tex2.UseTexture(TextureUnit.Texture1);


            player.GetCamera().ProcessInput();
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            DeferredRenderer.Render();
            GL.Disable(EnableCap.CullFace);
           // MeshManager.Render(ShaderManager.ShaderType_BL.GBuffer);
     
            DeferredRenderer.BindGBufferTextures();
            Q.Render(); // uncomment for deferred debug :)

            _controller.Render();
            
            Util.CheckGLError("End of frame");

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
