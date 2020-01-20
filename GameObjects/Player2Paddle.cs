using System;

namespace PongGame
{
    class Player2Paddle : Paddle
    {
        public Player2Paddle(int x, int y, int r, int g, int b) : base(x, y, r, g, b)
        { 
        }

        public override void Update(float dt)
        {
        }

        public void Move(int dy)
        {
            position.Y += dy;
            if (position.Y < 0) position.Y = 0;
            else if (position.Y > SceneManager.WindowHeight) position.Y = SceneManager.WindowHeight;
        }
    }
}
