using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace PongGame
{
    class ChooseNetworkScene : Scene, IScene
    {
        public ChooseNetworkScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            if (state != menuState.NEXTMENU)
            {
                sceneManager.Title = "Pong - Choose to Host or Join";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public enum menuState { HOST, JOIN, NEXTMENU };

        private menuState state = menuState.HOST;
        public menuState State
        {
            get { return state; }
        }
        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (state != menuState.NEXTMENU)
            {
                KeyboardState KeyStates = Keyboard.GetState();
                if (KeyStates.IsKeyDown(Key.Down))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Down", state);
                    if (state == menuState.HOST)
                    {
                        state = menuState.JOIN;
                    }
                    else if (state == menuState.JOIN)
                    {
                        state = menuState.HOST;
                    }
                }
                else if (KeyStates.IsKeyDown(Key.Up))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Up", state);
                    if (state == menuState.HOST)
                    {
                        state = menuState.JOIN;
                    }
                    else if (state == menuState.JOIN)
                    {
                        state = menuState.HOST;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Enter))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Enter", state);
                    if (state == menuState.HOST)
                    {
                        sceneManager.HostNetwork();
                        state = menuState.NEXTMENU;
                    }
                    if (state == menuState.JOIN)
                    {
                        sceneManager.JoinNetwork();
                        state = menuState.NEXTMENU;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Escape))
                {
                    //Go to previous scene
                    state = menuState.NEXTMENU;
                    sceneManager.StartMenu();
                }
            }
        }


        public void Update(FrameEventArgs e)
        {

        }

        public void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.CornflowerBlue;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            if (state == menuState.HOST)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Host", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Host", (int)fontSize, StringAlignment.Center);
            }
            if (state == menuState.JOIN)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Join", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Join", (int)fontSize, StringAlignment.Center);
            }

            GUI.Render();
        }
    }
}