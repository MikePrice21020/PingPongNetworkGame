using System;
using OpenTK;

namespace PongGame
{
    class AIPaddle : Paddle
    {
        public AIPaddle(int x, int y, int r, int g, int b) : base(x, y, r, g, b)
        {  
        }

        public override void Update(float dt)
        {
            //Only "Medium" and "Hard" Difficulty use velocity in their calculations
            if (AIScene.AIdifficulty == 2 || AIScene.AIdifficulty == 3)
            {
                position += velocity;
            }
        }
        public void Move(Vector2 ballPosition, bool Lengthincreased)
        {
            // EASY Difficulty
            if (AIScene.AIdifficulty == 1)
            {
                position.Y += (((ballPosition.Y - (ballPosition.Y/20))) - position.Y) * 0.15f;
            }
            // Medium Difficulty
            else if (AIScene.AIdifficulty == 2)
            {
                //Limit the velocity the AI can gain from the start to end
                if (velocity.Y >= 1)
                {
                    velocity.Y = 1;
                }
                //If AI has the extra length powerup, to make it fair he will be more unaccurate
                if (Lengthincreased == false)
                {
                    velocity.Y += ((ballPosition.Y - 10) - position.Y) * 0.1f;
                }
                else
                {
                    velocity.Y += ((ballPosition.Y - 15) - position.Y) * 0.09f;
                }
            }
            // HARD Difficulty
            else if (AIScene.AIdifficulty == 3)
            {
                //Limit the velocity the AI can gain from the start to end
                if (velocity.Y >= 2)
                {
                    velocity.Y = 2;
                }
                //If AI has the extra length powerup, to make it fair he will be more unaccurate
                if (Lengthincreased == false)
                {
                    velocity.Y += ((ballPosition.Y - 0.01f) - position.Y) * 0.25f;
                }
                else
                {
                    velocity.Y += ((ballPosition.Y - 1f) - position.Y) * 0.05f;
                }
            }
            // Impossible Difficulty
            else if (AIScene.AIdifficulty == 4)
            {
                // its just.... well unbeatable
                position.Y = ballPosition.Y;
            }
        }
    }
}
