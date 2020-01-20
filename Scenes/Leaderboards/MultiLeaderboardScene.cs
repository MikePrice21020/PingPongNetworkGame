using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace PongGame
{
    class MultiLeaderboardScene : Scene, IScene
    {
        static string ipAddress = "127.0.0.1";
        static int Port = 43;
        string[] playerScores = new string[10];
        public string inputName;
        public int currentScore = 0;
        bool ready = false;
        bool exit = false;
        bool checkedScore = false;
        bool permissionToSubmit = false;
        public MultiLeaderboardScene(SceneManager sceneManager, bool check, int playerScore) : base(sceneManager)
        {
            if (exit == false)
            {
                // Set the title of the window
                sceneManager.Title = "Pong - Multiplayer leaderBoards";
                if (checkedScore == false && check == true)
                {
                    ServerAccess(false, false, check, playerScore);
                    ServerAccess(false, true, false, 0);
                }
                else
                {
                    ServerAccess(false, true, false, 0);
                }
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public void ServerAccess(bool update, bool retreive, bool check, int playerScore)
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(ipAddress, Port);
                //Timeout Code
                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                //UPDATE
                if (check == true)
                {
                    sw.WriteLine("@multi\r\n@check\r\n{0}", playerScore);
                    sw.Flush();
                    string line = string.Empty;
                    line = sr.ReadToEnd();
                    string[] Client_msg = Regex.Split(line, "\r\n");
                    if (Client_msg[0] == "@checkis" && Client_msg[1] == "True")
                    {
                        currentScore = playerScore;
                        permissionToSubmit = true;
                    }
                    else
                    {
                        permissionToSubmit = false;
                        currentScore = -1;
                    }
                    checkedScore = true;
                }
                //UPDATE
                if (update == true)
                {
                    sw.WriteLine("@multi\r\n@update\r\n{0}\r\n{1}", inputName, currentScore);
                    sw.Flush();
                }
                //RETRIEVE
                if (retreive == true)
                {
                    sw.WriteLine("@multi\r\n@retrieve");
                    sw.Flush();
                    string line = string.Empty;
                    line = sr.ReadToEnd();
                    string[] Client_msg = Regex.Split(line, "\r\n");
                    for (int i = 0; i < 10; i++)
                    {
                        playerScores[i] = Client_msg[i];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // This alows Wave UI to see what state the WaveSpawner is in
        // So set a public get
        //string[,] playerScores = new string[5, 2] { { "Michael", "420" }, { "Kevin", "320" }, { "Hombzy", "240" }, { "Cherry", "69" }, { "Sonba", "49" } };
        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (exit == false)
            {
                KeyboardState KeyStates = Keyboard.GetState();
                Console.WriteLine("{0}", e.Key);
                if (permissionToSubmit == true)
                {
                    if (inputName == null && e.Key.ToString().Length == 1)
                    {
                        inputName += e.Key;
                    }
                    else if (inputName != null && inputName.Length < 10 && e.Key.ToString().Length == 1 && ready == false)
                    {
                        inputName += e.Key;
                    }
                    if (KeyStates.IsKeyDown(Key.BackSpace) && inputName != "" && inputName != null)
                    {
                        inputName = inputName.Substring(0, (inputName.Length - 1));
                    }
                    if (KeyStates.IsKeyDown(Key.Enter) && inputName != "" && inputName != null)
                    {
                        if (ready == true)
                        {
                            //submit name, confirmed
                            ServerAccess(true, false, false, currentScore);
                            ready = false;
                            permissionToSubmit = false;
                        }
                        else
                        {
                            //are you ready to submit?
                            ready = true;
                        }
                        ServerAccess(false, true, false, 0);
                    }
                }
                if (KeyStates.IsKeyDown(Key.Enter))
                {
                    ServerAccess(false, true, false, 0);
                }
                if (KeyStates.IsKeyDown(Key.Escape))
                {
                    if (ready == true)
                    {
                        ready = false;
                    }
                    else if (permissionToSubmit == false)
                    {
                        // Go to previous scene
                        exit = true;
                        // If player has come from playing a game, he will go straight to the Main menu
                        if (checkedScore == true)
                        {
                            sceneManager.StartMenu();
                        }
                        // If player has come the Mainmenu, he will go back to the leaderboard scene
                        else
                        {
                            sceneManager.OpenLeaderboardMenu();
                        }
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

            if (permissionToSubmit == true)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Multiplayer LeaderBoards: - Highscore!", (int)fontSize / 2, StringAlignment.Center, Color.DarkGreen);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Multiplayer LeaderBoards:", (int)fontSize / 2, StringAlignment.Center);
            }
            GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize / 1f)), "                                1. " + playerScores[0], (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize * 2.5f), (int)width, (int)(fontSize / 1f)), "                   " + playerScores[1], (int)fontSize / 2, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize * 3.5f), (int)width, (int)(fontSize / 1f)), "                                2. " + playerScores[2], (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize * 3.5f), (int)width, (int)(fontSize / 1f)), "                   " + playerScores[3], (int)fontSize / 2, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize / 1f)), "                                3. " + playerScores[4], (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize * 4.5f), (int)width, (int)(fontSize / 1f)), "                   " + playerScores[5], (int)fontSize / 2, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize * 5.5f), (int)width, (int)(fontSize / 1f)), "                                4. " + playerScores[6], (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize * 5.5f), (int)width, (int)(fontSize / 1f)), "                   " + playerScores[7], (int)fontSize / 2, StringAlignment.Center);
            GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize / 1f)), "                                5. " + playerScores[8], (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize * 6.5f), (int)width, (int)(fontSize / 1f)), "                   " + playerScores[9], (int)fontSize / 2, StringAlignment.Center);
            if(permissionToSubmit == true)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 8f), (int)width, (int)(fontSize / 1f)), "Enter Name: " + inputName, (int)fontSize / 2, StringAlignment.Center);
            }
            if(ready == true && permissionToSubmit == true)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 8.5f), (int)width, (int)(fontSize / 1f)), "Press 'Enter' to Submit (Press 'Esc' to go back)", (int)fontSize / 2, StringAlignment.Center, Color.Lime);
            }
            else if (permissionToSubmit == true)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize * 9f), (int)width, (int)(fontSize / 1f)), "Press 'Enter' to confirm name", (int)fontSize / 2, StringAlignment.Center, Color.Yellow);
            }

            GUI.Render();
        }
    }
}