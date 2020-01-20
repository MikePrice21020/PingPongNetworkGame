

namespace PongGame
{
    public class Start
    {
        public static void Main(string[] args)
        {
            using (SceneManager sceneManager = new SceneManager())
            {
                sceneManager.Run(60.0f);
            }
        }
    };
}
