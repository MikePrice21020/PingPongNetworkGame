﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace PongGame
{
    class GameScene : Scene, IScene
    {
        Matrix4 projectionMatrix;

        //Create Instances
        PlayerPaddle paddlePlayer;
        AIPaddle paddleAI;
        Ball ball;
        SpeedPowerUp speedPowerUp;
        LengthPowerUp lengthPowerUp;

        //Length of game (s)
        int clock = 30;
        private int Timer = 60;
        bool GamerOver = false;

        //PowerUp Logic
        bool powerUpSpawned = false;
        static Random r = new Random();
        int rInt;
        int lInt;
        float lintF;

        //Player Logic
        bool Player1lastHit = false;

        bool player1SpeedEnabled = false;
        public bool player2SpeedEnabled = false;

        bool player1LengthEnabled = false;
        bool player2LengthEnabled = false;

        int scorePlayer = 0;
        int scoreAI = 0;
        //Paddle hitboxes
        float normalPaddle = 35.0f;
        float superPaddle = 45.0f;

        //Debug Mode
        bool DebugMode = false;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.Keyboard.KeyDown += Keyboard_KeyDown;

            ResetGame();

            GL.ClearColor(Color.LightSkyBlue);
        }

        private void ResetGame()
        {
            paddlePlayer = new PlayerPaddle(40, (int)(SceneManager.WindowHeight * 0.5), 0, 43, 255);
            paddlePlayer.Init(false);
            paddleAI = new AIPaddle(SceneManager.WindowWidth - 40, (int)(SceneManager.WindowHeight * 0.5), 255, 0, 0);
            paddleAI.Init(false);
            ball = new Ball((int)(SceneManager.WindowWidth * 0.5), (int)(SceneManager.WindowHeight * 0.5));
            ball.Init();
            speedPowerUp = new SpeedPowerUp(-180, -180, 255, 255, 0);
            speedPowerUp.Init();
            lengthPowerUp = new LengthPowerUp(-180, -180, 0, 255, 0);
            lengthPowerUp.Init();
            powerUpSpawned = false;
            player1SpeedEnabled = false;
            player2SpeedEnabled = false;
            player1LengthEnabled = false;
            player2LengthEnabled = false;
        }

        public void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    break;
                case Key.Down:
                    break;
            }
        }
        public void Update(FrameEventArgs e)
        {
            if (GamerOver == false)
            {

                Console.WriteLine("X: {0} Y: {1}", ball.Velocity.X, ball.Velocity.Y);
                Timer -= 1;

                if (Timer <= 0)
                {
                    Timer = 60;
                    clock--;
                    if (powerUpSpawned == false)
                    {
                        rInt = r.Next(0, 5); //for ints
                        lInt = r.Next(3, 9); //for ints
                        lintF = (float)lInt / 10;
                        if (rInt == 3)
                        {
                            powerUpSpawned = true;
                            speedPowerUp.Position = new Vector2((int)(SceneManager.WindowWidth * lintF), (int)(SceneManager.WindowHeight * lintF));
                        }
                        else if (rInt == 2)
                        {
                            powerUpSpawned = true;
                            lengthPowerUp.Position = new Vector2((int)(SceneManager.WindowWidth * lintF), (int)(SceneManager.WindowHeight * lintF));
                        }
                    }
                }
                if (clock <= 0)
                {
                    GamerOver = true;
                    clock = 3;
                    Timer = 60;
                }
                //Console.WriteLine("Timer:" + clock);

                // Set the title of the window
                sceneManager.Title = "Pong - Player Score: " + scorePlayer + " - AI Score: " + scoreAI;
                KeyboardState KeyStates = Keyboard.GetState();
                if (KeyStates.IsKeyDown(Key.Down))
                {
                    if (DebugMode == true)
                    {
                        ball.Position = new Vector2(ball.Position.X, ball.Position.Y - 5);
                    }
                    else
                    {
                        if (player1SpeedEnabled == true)
                        {
                            paddlePlayer.Move(-15);
                        }
                        else
                        {
                            paddlePlayer.Move(-8);
                        }
                    }
                }
                else if (KeyStates.IsKeyDown(Key.Up))
                {
                    if (DebugMode == true)
                    {
                        ball.Position = new Vector2(ball.Position.X, ball.Position.Y + 5);
                    }
                    else
                    {
                        if (player1SpeedEnabled == true)
                        {
                            paddlePlayer.Move(15);
                        }
                        else
                        {
                            paddlePlayer.Move(8);
                        }
                    }
                }
                if (DebugMode == true)
                {
                    if (KeyStates.IsKeyDown(Key.Left))
                    {
                        ball.Position = new Vector2(ball.Position.X - 5, ball.Position.Y);
                    }
                    else if (KeyStates.IsKeyDown(Key.Right))
                    {
                        ball.Position = new Vector2(ball.Position.X + 5, ball.Position.Y);
                    }
                }
                else
                {
                    paddlePlayer.Move(0);
                }
                paddleAI.Move(ball.Position, player2SpeedEnabled);

                if (DebugMode == false)
                {
                    ball.Update((float)e.Time);
                }
                paddleAI.Update((float)e.Time);

                CollisionDetection();
                if (GoalDetection())
                {
                    ResetGame();
                }
            }
            else
            {
                Timer -= 1;

                if (Timer <= 0)
                {
                    Timer = 60;
                    clock--;
                }
                if (clock <= 0)
                {
                    sceneManager.OpenSingleLeaderboard(true, scorePlayer);
                }
            }
        }

        private bool GoalDetection()
        {
            if (ball.Position.X < 0)
            {
                scoreAI++;
                return true;
            }
            else if (ball.Position.X > SceneManager.WindowWidth)
            {
                scorePlayer++;
                return true;
            }

            return false;
        }


        private void CollisionDetection()
        {
            // AI
            if (player2LengthEnabled == false)
            {
                if ((paddleAI.Position.X - ball.Position.X) < ball.Radius &&
               ball.Position.Y > (paddleAI.Position.Y - normalPaddle) && ball.Position.Y < (paddleAI.Position.Y + normalPaddle))
                {
                    ball.Position = new Vector2(paddleAI.Position.X - ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 2.0f;
                    Player1lastHit = false;
                }
            }
            else
            {
                if ((paddleAI.Position.X - ball.Position.X) < ball.Radius &&
                    ball.Position.Y > (paddleAI.Position.Y - superPaddle) && ball.Position.Y < (paddleAI.Position.Y + superPaddle))
                {
                    ball.Position = new Vector2(paddleAI.Position.X - ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 2.0f;
                    Player1lastHit = false;
                }
            }
            // Speed Powerup
            if ((speedPowerUp.Position.X - ball.Position.X) < (ball.Radius + 55.0f) && (ball.Radius - 55.0f) < (speedPowerUp.Position.X - ball.Position.X) &&
                ball.Position.Y > (speedPowerUp.Position.Y - 55.0f) && ball.Position.Y < (speedPowerUp.Position.Y + 55.0f))
            {
                speedPowerUp.Position = new Vector2(-180, -180);
                powerUpSpawned = false;
                if (Player1lastHit == true)
                {
                    player1SpeedEnabled = true;
                }
                else
                {
                    player2SpeedEnabled = true;
                }
            }
            // Length PowerUp
            if ((lengthPowerUp.Position.X - ball.Position.X) < (ball.Radius + 55.0f) && (ball.Radius - 55.0f) < (lengthPowerUp.Position.X - ball.Position.X) &&
                ball.Position.Y > (lengthPowerUp.Position.Y - 55.0f) && ball.Position.Y < (lengthPowerUp.Position.Y + 55.0f))
            {
                lengthPowerUp.Position = new Vector2(-180, -180);
                powerUpSpawned = false;
                if (Player1lastHit == true)
                {
                    player1LengthEnabled = true;
                    paddlePlayer.Init(true);
                }
                else
                {
                    player2LengthEnabled = true;
                    paddleAI.Init(true);
                }
            }
            // Player
            if (player1LengthEnabled == false)
            {
                if ((ball.Position.X - paddlePlayer.Position.X) < ball.Radius &&
                    ball.Position.Y > (paddlePlayer.Position.Y - normalPaddle) && ball.Position.Y < (paddlePlayer.Position.Y + normalPaddle))
                {
                    ball.Position = new Vector2(paddlePlayer.Position.X + ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 2.0f;
                    Player1lastHit = true;
                }
            }
            else
            {
                if ((ball.Position.X - paddlePlayer.Position.X) < ball.Radius &&
                    ball.Position.Y > (paddlePlayer.Position.Y - superPaddle) && ball.Position.Y < (paddlePlayer.Position.Y + superPaddle))
                {
                    ball.Position = new Vector2(paddlePlayer.Position.X + ball.Radius, ball.Position.Y);
                    ball.Velocity = new Vector2(ball.Velocity.X * -1.0f, ball.Velocity.Y) * 2.0f;
                    Player1lastHit = true;
                }
            }
        }

        public void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, sceneManager.Width, 0, sceneManager.Height, -1.0f, +1.0f);

            ball.Render(projectionMatrix);
            paddlePlayer.Render(projectionMatrix);
            paddleAI.Render(projectionMatrix);
            speedPowerUp.Render(projectionMatrix);
            lengthPowerUp.Render(projectionMatrix);

            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            if (GamerOver == false)
            {
                GUI.Label(new Rectangle(0, (int)(fontSize / 5f), (int)width, (int)(fontSize * 1f)), ("Time: " + clock + "." + Timer), (int)fontSize / 2, StringAlignment.Center);
            }
            else
            {
                GUI.Label(new Rectangle(0, (int)(fontSize / 5), (int)width, (int)(fontSize * 10f)), ("Game Over"), (int)fontSize * 2, StringAlignment.Center);
            }
            GUI.Label(new Rectangle(0, (int)(fontSize / 5f), (int)width, (int)(fontSize * 1f)), ("" + scorePlayer), (int)fontSize / 2, StringAlignment.Near);
            GUI.Label(new Rectangle(0, (int)(fontSize / 5f), (int)width, (int)(fontSize * 1f)), (scoreAI + ""), (int)fontSize / 2, StringAlignment.Far);
            //GUI.Label(new Rectangle(0, (int)(fontSize / 16f), (int)width, (int)(fontSize * 4f)), ("Player " + scorePlayer + "       AI: " + scoreAI), (int)fontSize, StringAlignment.Center);
            //GUI.Label(new Rectangle(0, (int)(fontSize / 16f), (int)width, (int)(fontSize * 4f)), ("X " + ball.Velocity.X + "       Y: " + ball.Velocity.Y), (int)fontSize, StringAlignment.Center);
            GUI.Render();
        }
    }
}

