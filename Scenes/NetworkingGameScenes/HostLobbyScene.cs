using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace PongGame
{
    class HostLobbyScene : Scene, IScene
    {
        bool exit = false;
        bool permissionToSubmit = false;
        bool ready = false;
        public string inputName;
        bool ranserver = false;

        static string ipAddress = "127.0.0.1";
        static int port = 43;

        static IPAddress address = IPAddress.Parse(ipAddress);

        public HostLobbyScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            if (state != menuState.NEXTMENU)
            {
                sceneManager.Title = "Pong - Choose to Host or Join";
                // Set the Render and Update delegates to the Update and Render methods of this class
                sceneManager.renderer = Render;
                sceneManager.updater = Update;

                sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;
                if (ranserver == false)
                {
                    ranserver = true;
                    try
                    {
                        runServer();
                    }
                    catch (IOException i)
                    {
                        Console.WriteLine(i);
                    }
                }
            }
        }
        public enum menuState { HOST, JOIN, NEXTMENU };

        private menuState state = menuState.HOST;
        public menuState State
        {
            get { return state; }
        }
        public static void runServer()
        {
            //Network
            TcpListener listener;
            Socket connection;
            try
            {
                //Intialise TcpListener
                listener = new TcpListener(address, port);
                listener.Start();
                while (true)
                {
                    connection = listener.AcceptSocket();
                    //Parameterized Thread - DoRequest
                    Thread r = new Thread(new ParameterizedThreadStart(doRequest));
                    r.Start(connection);
                    //doRequest(socketStream);

                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0}", e.ToString());
                Console.ResetColor();
            }
        }
        private static void doRequest(object ded)
        {
            //Network
            Socket connection = (Socket)ded;
            NetworkStream socketStream;
            socketStream = new NetworkStream(connection);

            //Variables used for reading sr.ReadLine
            string Full_Client_msg = null;

            //Stream Writer/Reader
            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);

            try
            {
                //Timeout code for read and write
                socketStream.ReadTimeout = 1000;
                socketStream.WriteTimeout = 1000;

                //Read Stream with sr.Peek
                while (sr.Peek() >= 0)
                {
                    Full_Client_msg += (char)sr.Read();
                }
                string[] Client_msg = Regex.Split(Full_Client_msg, "\r\n");
            }

            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: {0}", e.ToString());
                Console.ResetColor();
            }
        }
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

                    }
                    if (KeyStates.IsKeyDown(Key.Enter))
                    {

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

            GUI.Label(new Rectangle(0, (int)(fontSize * 8f), (int)width, (int)(fontSize / 1f)), "Enter IP: " + inputName, (int)fontSize / 2, StringAlignment.Center);

            GUI.Render();
        }
    }
}