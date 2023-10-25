using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

public class Program
{
    public static void Main()
    {
        var nativeWindowSettings = new NativeWindowSettings
        {
            Size = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "Lab1 Serezhkin"
        };

        using (var game = new Game(GameWindowSettings.Default, nativeWindowSettings))
        {
            game.Run();
        }
    }
}

public class Game : GameWindow
{
    private int numbers_vao;
    private int numbers_ebo;
    private int indices_count;
    Shader shader;
    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

        MakeOne();

        base.OnLoad();
    }

    protected override void OnUnload()
    {
        shader.Dispose();
        base.OnUnload();
    }

    protected override void OnRenderFrame(OpenTK.Windowing.Common.FrameEventArgs e)
    {
        base.OnRenderFrame(e);


        GL.Clear(ClearBufferMask.ColorBufferBit);


        shader.Use();


        GL.BindVertexArray(numbers_vao);

        GL.DrawElements(PrimitiveType.Triangles, indices_count, DrawElementsType.UnsignedInt, 0);


        SwapBuffers();
    }

    private void MakeOne()
    {
        // Илюстрация происходящего в файле
        float[] numbers_vert = {
            // x y z
            // ЧИСЛО 1
            // 1 прямая
            -0.20f, -0.5f, 0.0f, // b-r 0
            -0.25f, -0.5f, 0.0f, // b-l 1
            -0.20f, 0.5f, 0.0f,  // t-r 2
            -0.25f, 0.5f, 0.0f,  // t-l 3
            // 2 наклонная
            -0.5f, 0.0f, 0.0f,   // m-l 4
            -0.45f, 0.0f, 0.0f,  // m-r 5
            // ЧИСЛО 2
            // 1 прямая
            0.0f, 0.5f, 0.0f,   // t-l 6
            0.05f, 0.5f, 0.0f,  // t-r 7
            0.0f, 0.25f, 0.0f,  // b-l 8 
            0.05f, 0.25f, 0.0f, // b-r 9
            // 2 прямая
            // юзает 6 как         t-l
            0.5f, 0.5f, 0.0f,  //  t-r 10
            0.0f, 0.45f, 0.0f, //  b-l 11
            0.5f, 0.45f, 0.0f, //  b-r 12
            // 3 наклонная
            // юзает 12 как         t-r
            0.45f, 0.45f, 0.0f, // t-l 13
            0.05f, -0.45f, 0.0f,// b-r 14
            0.0f, -0.45f, 0.0f, // b-l 15
            // 4 прямая
            // юзает 15 как         t-l
            0.5f, -0.45f, 0.0f, // t-r 16
            0.0f, -0.5f, 0.0f,  // b-l 17
            0.5f, -0.5f, 0.0f,  // b-r 18
        };

        uint[] numbers_indices = {
            // ЧИСЛО 1
            0, 1, 2,
            2, 3, 1,
            2, 3, 5,
            4, 5, 3,

            // ЧИСЛО 2
            6, 7, 9,
            8, 9, 6, 
            6, 10, 12,
            11, 12, 6,
            12, 13, 14,
            14, 15, 13,
            15, 16, 17,
            17, 18, 16
        };

        indices_count = numbers_indices.Length;

        int numbers_vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, numbers_vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, numbers_vert.Length * sizeof(float), numbers_vert, BufferUsageHint.StaticDraw);

        shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

        numbers_vao = GL.GenVertexArray();
        GL.BindVertexArray(numbers_vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindBuffer(BufferTarget.ArrayBuffer, numbers_vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, numbers_vert.Length * sizeof(float), numbers_vert, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        numbers_ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, numbers_ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, numbers_indices.Length * sizeof(uint), numbers_indices, BufferUsageHint.StaticDraw);
    }
}