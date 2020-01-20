using OpenTK;

namespace PongGame
{
    interface IScene
    {
        void Render(FrameEventArgs e);
        void Update(FrameEventArgs e);
    }
}
