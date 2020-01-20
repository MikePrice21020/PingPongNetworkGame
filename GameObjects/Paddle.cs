using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PongGame
{
    abstract class Paddle : GameObject
    {
        public Paddle()
        {
            position.X = 0;
            position.Y = 0;
        }

        public Paddle(int x, int y, int r, int g, int b)
        {
            position.X = x;
            position.Y = y;

            coldata = new Vector3[] {
                new Vector3(r, g, b),
                new Vector3(r, g, b),
                new Vector3(r, g, b),
                new Vector3(r, g, b) };
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

        public void Init(bool lengthPowerUp)
        {
            // Create and load shader program
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

            // Store geometry in vertex buffer 
            GL.GenVertexArrays(1, out vao_Handle);
            GL.BindVertexArray(vao_Handle);

            GL.GenBuffers(1, out vbo_position);
            GL.GenBuffers(1, out vbo_color);
            if (lengthPowerUp == false)
            {
                vertdata = new Vector3[] {
                new Vector3(-10f, +30f, 0f),
                new Vector3(-10f, -30f, 0f),
                new Vector3(+10f, -30f, 0f),
                new Vector3(+10f, +30f, 0f) };
            }
            else
            {
                vertdata = new Vector3[] {
                new Vector3(-10f, +45f, 0f),
                new Vector3(-10f, -45f, 0f),
                new Vector3(+10f, -45f, 0f),
                new Vector3(+10f, +45f, 0f) };
            }

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

        private void LoadShader(String filename, ShaderType type, int program, out int address)
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
