using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace PongGame
{
    class ChooseLeaderboardScene : Scene, IScene
    {
        public ChooseLeaderboardScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            if (state != menuState.NEXTMENU)
            {
                sceneManager.Title = "Pong - Choose Leaderboard";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public enum menuState { SINGLE, MULTI, NEXTMENU };

        private menuState state = menuState.SINGLE;
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
                    if (state == menuState.SINGLE)
                    {
                        state = menuState.MULTI;
                    }
                    else if (state == menuState.MULTI)
                    {
                        state = menuState.SINGLE;
                    }
                }
                else if (KeyStates.IsKeyDown(Key.Up))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Up", state);
                    if (state == menuState.SINGLE)
                    {
                        state = menuState.MULTI;
                    }
                    else if (state == menuState.MULTI)
                    {
                        state = menuState.SINGLE;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Enter))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Enter", state);
                    if (state == menuState.SINGLE)
                    {
                        sceneManager.OpenSingleLeaderboard(false, -1);
                        state = menuState.NEXTMENU;
                    }
                    if (state == menuState.MULTI)
                    {
                        sceneManager.OpenMultiLeaderboard(false, -1);
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

            if (state == menuState.SINGLE)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Singleplayer", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Singleplayer", (int)fontSize, StringAlignment.Center);
            }
            if (state == menuState.MULTI)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Multiplayer", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Multiplayer", (int)fontSize, StringAlignment.Center);
            }

            GUI.Render();
        }
    }
}