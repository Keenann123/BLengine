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

namespace RenderingEngine
{
    public class Window : GameWindow
    {
        ImGuiController _controller;
        Mesh mesh1;
        Texture tex;

        public Window(GraphicsMode gMode) : base(1280, 720, gMode,
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
            mesh1 = new Mesh();
            tex = new Texture("Textures/test.png");
            _controller = new ImGuiController(Width, Height);            
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Update the opengl viewport
            GL.Viewport(0, 0, Width, Height);

            // Tell ImGui of the new size
            _controller.WindowResized(Width, Height);
        }

    
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Util.TotalTime += (float)e.Time;  //TotalTime += deltaTime

            base.OnRenderFrame(e);

            _controller.Update(this, (float)e.Time);

            GL.ClearColor(new Color4(0.5f, 0.5f, 1f, 1.0f)); //pretty colors :^)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

  
            ImGui.Begin("Renderer");
            ImGui.SetWindowSize(new System.Numerics.Vector2(500, 500));
            if(ImGui.Button("Quit", new System.Numerics.Vector2(100,100)))
            {
                base.Exit();
            }
            ImGui.End();

            tex.UseTexture();
            mesh1.Render();

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
