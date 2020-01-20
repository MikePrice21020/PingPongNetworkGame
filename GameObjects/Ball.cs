using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PongGame
{
    class Ball : GameObject
    {
        Random rand = new Random();

        public Ball()
        {
            position.X = 0;
            position.Y = 0;
            velocity = RandomVelocity();
        }

        public Ball(int x, int y)
        {
            position.X = x;
            position.Y = y;
            velocity = RandomVelocity();
        }

        private Vector2 RandomVelocity()
        {
            Vector2 vel = (new Vector2(((float)rand.NextDouble() * 2.0f) - 1.0f, ((float)rand.NextDouble() * 2.0f) - 1.0f));
            vel.Normalize();
            vel *= 100.0f;
            return vel;
        }
        void SpeedUpBall()
        {
            // if on right side
            if (velocity.X < 0)
            {
                if (velocity.X > -60)
                {
                    velocity.X -= 20.0f;
                }
                else
                {
                    velocity.X -= 5.0f;
                }
            }
            else
            {
                if (velocity.X < 60)
                {
                    velocity.X += 20.0f;
                }
                else
                {
                    velocity.X += 5.0f;
                }
            }
        }
        int xLimit = 250;
        int yLimit = 350;
        public override void Update(float dt)
        {
            position += velocity * (dt * 2);
            if (velocity.X >= xLimit)
            {
                velocity.X = xLimit;
            }
            else if (velocity.X <= -xLimit)
            {
                velocity.X = -xLimit;
            }
            if (velocity.Y >= yLimit)
            {
                velocity.Y = yLimit;
            }
            else if (velocity.Y <= -yLimit)
            {
                velocity.Y = -yLimit;
            }
            if (position.Y < 0.0f)
            {
                position.Y = 1.0f;
                velocity.Y *= -1.0f;
                SpeedUpBall();
            }
            else if (position.Y > SceneManager.WindowHeight)
            {
                position.Y = SceneManager.WindowHeight - 1.0f;
                velocity.Y *= -1.0f;
                SpeedUpBall();
            }
        }

        public override void Render(Matrix4 projectionMatrix)
        {
            GL.UseProgram(pgmID);
            GL.BindVertexArray(vao_Handle);

            Matrix4 worldMatrix = Matrix4.CreateTranslation(position.X, position.Y, 0);
            Matrix4 worldViewProjection = worldMatrix * viewMatrix * projectionMatrix;
            GL.UniformMatrix4(uniform_mview, false, ref worldViewProjection);

            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        public int Radius
        {
            get { return 10; }
        }

        public void Init()
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            attribute_vpos = GL.GetAttribLocation(pgmID, "a_Position");
            attribute_vcol = GL.GetAttribLocation(pgmID, "a_Colour");
            uniform_mview = GL.GetUniformLocation(pgmID, "WorldViewProj");

            if (attribute_vpos == -1 || attribute_vcol == -1 || uniform_mview == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenVertexArrays(1, out vao_Handle);
            GL.BindVertexArray(vao_Handle);

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);

            vertdata = new Vector3[] {
                new Vector3(-10f, +10f, 0f),
                new Vector3(-10f, -10f, 0f),
                new Vector3(+10f, -10f, 0f),
                new Vector3(+10f, +10f, 0f) };

            coldata = new Vector3[] {
                new Vector3(1f, 1f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(1f, 1f, 1f),
                new Vector3(1f, 1f, 1f) };

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_position);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_color);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, 0, 0);

            GL.BindVertexArray(0);

            viewMatrix = Matrix4.Identity;
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
    }
}
