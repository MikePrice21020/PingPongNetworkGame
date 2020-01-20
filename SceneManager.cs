using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace PongGame
{
    class SceneManager : GameWindow
    {
        Scene scene;
        static int width = 0;
        static int height = 0;

        public delegate void SceneDelegate(FrameEventArgs e);
        public SceneDelegate renderer;
        public SceneDelegate updater;

        public SceneManager()
        {
            Mouse.ButtonDown += Mouse_ButtonDown;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);

            base.Width = 1024;
            base.Height = 512;
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Load the GUI
            GUI.SetUpGUI(Width, Height);

            scene = new MainMenuScene(this);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            updater(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            renderer(e);

            GL.Flush();
            SwapBuffers();
        }
        //disable this later
        private void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButton.Left:
                    StartNewGame();
                    break;
                case MouseButton.Right:
                    StartMenu();
                    break;
            }
        }

        public void AIScene()
        {
            scene = new AIScene(this);
        }
        //Singleplayer
        public void StartNewGame()
        {
            scene = new GameScene(this);
        }
        // Local Multiplayer
        public void StartNewLocalGame()
        {
            scene = new LocalGameScene(this);
        }
        // Menu to choose whether to Host or Join
        public void ChooseNetwork()
        {
            scene = new ChooseNetworkScene(this);
        }
        // Client scene to insert ip
        public void JoinNetwork()
        {
            scene = new ClientLobbyScene(this);
        }
        // Host Lobby scene to wait for a client to connect
        public void HostNetwork()
        {
            scene = new HostLobbyScene(this);
        }
        // MainMenu
        public void StartMenu()
        {
            scene = new MainMenuScene(this);
        }
        // Open Multiplayer Leaderboards
        public void OpenMultiLeaderboard(bool check, int playerscore)
        {
            scene = new MultiLeaderboardScene(this, check, playerscore);
        }
        // Menu for selecting Singleplayer or multiplayer leaderboards
        public void OpenLeaderboardMenu()
        {
            scene = new ChooseLeaderboardScene(this);
        }
        // Open Singleplayer leaderboards
        public void OpenSingleLeaderboard(bool check, int playerscore)
        {
            scene = new SingleLeaderboardScene(this, check, playerscore);
        }

        public static int WindowWidth
        {
            get { return width; }
        }

        public static int WindowHeight
        {
            get { return height; }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            SceneManager.width = Width;
            SceneManager.height = Height;

            //Load the GUI
            GUI.SetUpGUI(Width, Height);
        }
    }

}

