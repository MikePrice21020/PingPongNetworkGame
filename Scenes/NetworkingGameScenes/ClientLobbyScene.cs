using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

namespace PongGame
{
    class ClientLobbyScene : Scene, IScene
    {
        bool exit = false;
        bool permissionToSubmit = false;
        public string inputName;
        static string ipAddress = "127.0.0.1";
        static int Port = 43;

        public ClientLobbyScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            if (exit != true)
            {
                sceneManager.Title = "Pong - Choose to Host or Join";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
            }
        }
        public void ServerAccess()
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
                //CHECK
                    sw.WriteLine("@single\r\n@check\r\n");
                    sw.Flush();
                    string line = string.Empty;
                    line = sr.ReadToEnd();
                    string[] Client_msg = Regex.Split(line, "\r\n");
                    //If host accepts player can join then v
                    if (Client_msg[0] == "@checkis" && Client_msg[1] == "True")
                    {
                        permissionToSubmit = true;
                    }
                    // else host rejected client or no response
                    else
                    {
                        permissionToSubmit = false;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            string FilterNum;
            if (exit == false)
            {
                KeyboardState KeyStates = Keyboard.GetState();
                Console.WriteLine("{0}", e.Key);
                Console.WriteLine("{0}", inputName);
                if (permissionToSubmit == false)
                {
                    if (inputName == null && e.Key.ToString().Contains("Number"))
                    {
                        FilterNum = e.Key.ToString().Substring(6);
                        inputName += FilterNum;
                    }
                    else if (inputName != null && inputName.Length < 15 && e.Key.ToString().Contains("Number"))
                    {
                        FilterNum = e.Key.ToString().Substring(6);
                        inputName += FilterNum;
                    }
                    else if (inputName != null && inputName.Length < 15 && e.Key.ToString() == "Period")
                    {
                        FilterNum = ".";
                        inputName += FilterNum;
                    }
                    if (KeyStates.IsKeyDown(Key.BackSpace) && inputName != "" && inputName != null)
                    {
                        inputName = inputName.Substring(0, (inputName.Length - 1));
                    }
                    if (KeyStates.IsKeyDown(Key.Enter) && inputName != "" && inputName != null)
                    {
                        ipAddress = inputName;
                        ServerAccess();
                        // send ip
                    }
                    if (KeyStates.IsKeyDown(Key.Escape))
                    {
                        sceneManager.ChooseNetwork();
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

            GUI.Label(new Rectangle(0, (int)(fontSize * 8f), (int)width, (int)(fontSize / 1f)), "Enter IP: " + inputName, (int)fontSize / 2, StringAlignment.Center);

            GUI.Render();
        }
    }
}