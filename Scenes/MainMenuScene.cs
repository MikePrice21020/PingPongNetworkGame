using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;

namespace PongGame
{
    class MainMenuScene : Scene, IScene
    {
        public MainMenuScene(SceneManager sceneManager) : base(sceneManager)
        {
            if (state != menuState.NEXTMENU)
            {
                // Set the title of the window
                sceneManager.Title = "Pong - Main Menu";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public enum menuState{ HIGHSCORES, SINGLEPLAYER, MULTIPLAYER, NETWORK, EXIT, NEXTMENU };

        private menuState state = menuState.HIGHSCORES;
        //This alows Wave UI to see what state the WaveSpawner is in
        // So set a public get
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
                    Console.WriteLine("Debug: MainMenuScene({0}) - Key Down", state);
                    if (state == menuState.HIGHSCORES)
                    {
                        state = menuState.SINGLEPLAYER;
                    }
                    else if (state == menuState.SINGLEPLAYER)
                    {
                        state = menuState.MULTIPLAYER;
                    }
                    else if (state == menuState.MULTIPLAYER)
                    {
                        state = menuState.NETWORK;
                    }
                    else if (state == menuState.NETWORK)
                    {
                        state = menuState.EXIT;
                    }
                    else if (state == menuState.EXIT)
                    {
                        state = menuState.HIGHSCORES;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Up))
                {
                    Console.WriteLine("Debug: MainMenuScene({0}) - Key Up", state);
                    if (state == menuState.HIGHSCORES)
                    {
                        state = menuState.EXIT;
                    }
                    else if (state == menuState.SINGLEPLAYER)
                    {
                        state = menuState.HIGHSCORES;
                    }
                    else if (state == menuState.MULTIPLAYER)
                    {
                        state = menuState.SINGLEPLAYER;
                    }
                    else if (state == menuState.NETWORK)
                    {
                        state = menuState.MULTIPLAYER;
                    }
                    else if (state == menuState.EXIT)
                    {
                        state = menuState.NETWORK;
                    }
                }
                if (KeyStates.IsKeyDown(Key.Enter))
                {
                    Console.WriteLine("Debug: MainMenuScene({0}) - Enter", state);
                    if (state == menuState.HIGHSCORES)
                    {
                        sceneManager.OpenLeaderboardMenu();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.SINGLEPLAYER)
                    {
                        sceneManager.AIScene();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.MULTIPLAYER)
                    {
                        sceneManager.StartNewLocalGame();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.NETWORK)
                    {
                        sceneManager.ChooseNetwork();
                        state = menuState.NEXTMENU;
                    }
                    else if (state == menuState.EXIT)
                    {
                        Environment.Exit(0);
                    }
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

            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Pong", (int)fontSize, StringAlignment.Center);

            if (state == menuState.HIGHSCORES)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize)), "Highscores", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize)), "Highscores", (int)fontSize / 2, StringAlignment.Center);
            }
            if (state == menuState.SINGLEPLAYER){
                GUI.Label(new Rectangle(0, (int)(fontSize * 3.5f), (int)width, (int)(fontSize)), "Singleplayer", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 3.5f), (int)width, (int)(fontSize)), "Singleplayer", (int)fontSize / 2, StringAlignment.Center);
            }
            if (state == menuState.MULTIPLAYER)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize)), "Multiplayer", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize)), "Multiplayer", (int)fontSize / 2, StringAlignment.Center);
            }
            if (state == menuState.NETWORK)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 5.5f), (int)width, (int)(fontSize)), "Online Multiplayer", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 5.5f), (int)width, (int)(fontSize)), "Online Multiplayer", (int)fontSize / 2, StringAlignment.Center);
            }
            if (state == menuState.EXIT)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize)), "Exit", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize)), "Exit", (int)fontSize / 2, StringAlignment.Center);
            }

            GUI.Render();
        }
    }
}