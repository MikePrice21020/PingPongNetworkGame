using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace PongGame
{
    class AIScene : Scene, IScene
    {
        public AIScene(SceneManager sceneManager) : base(sceneManager)
        {
            if (state != menuState.NEXTMENU)
            {
                // Set the title of the window
                sceneManager.Title = "Pong - AI Difficulty Selection Screen";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public static int AIdifficulty;
        public enum menuState { EASY, MEDIUM, HARD, IMP, NEXTMENU };

        private menuState state = menuState.EASY;
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
                    if (state == menuState.EASY)
                    {
                        state = menuState.MEDIUM;
                    }
                    else if (state == menuState.MEDIUM)
                    {
                        state = menuState.HARD;
                    }
                    else if (state == menuState.HARD)
                    {
                        state = menuState.IMP;
                    }
                    else if (state == menuState.IMP)
                    {
                        state = menuState.EASY;
                    }
                }
                else if (KeyStates.IsKeyDown(Key.Up))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Up", state);
                    if (state == menuState.EASY)
                    {
                        state = menuState.IMP;
                    }
                    else if (state == menuState.MEDIUM)
                    {
                        state = menuState.EASY;
                    }
                    else if (state == menuState.HARD)
                    {
                        state = menuState.MEDIUM;
                    }
                    else if (state == menuState.IMP)
                    {
                        state = menuState.HARD;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Enter))
                {
                    Console.WriteLine("Debug: AIscene({0}) - Enter", state);
                    if (state == menuState.EASY)
                    {
                        AIdifficulty = 1;
                        sceneManager.StartNewGame();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.MEDIUM)
                    {
                        AIdifficulty = 2;
                        sceneManager.StartNewGame();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.HARD)
                    {
                        AIdifficulty = 3;
                        sceneManager.StartNewGame();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.IMP)
                    {
                        AIdifficulty = 4;
                        sceneManager.StartNewGame();
                        state = menuState.NEXTMENU;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Escape))
                {
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

            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "AI Difficulty:", (int)fontSize, StringAlignment.Center);

            if (state == menuState.EASY)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Easy", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize * 2f)), "Easy", (int)fontSize, StringAlignment.Center);
            }
            if (state == menuState.MEDIUM)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Medium", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize * 2f)), "Medium", (int)fontSize, StringAlignment.Center);
            }
            if (state == menuState.HARD)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize * 2f)), "Hard", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize * 2f)), "Hard", (int)fontSize, StringAlignment.Center);
            }
            if (state == menuState.IMP)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 8.5f), (int)width, (int)(fontSize * 2f)), "Impossible", (int)fontSize, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 8.5f), (int)width, (int)(fontSize / 1f)), "Impossible", (int)fontSize / 2, StringAlignment.Center, Color.Red);
            }

            GUI.Render();
        }
    }
}