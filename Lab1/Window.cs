﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;




public class Window : GameWindow
{
    // We modify the vertex array to include vertices for our number 0.
    // private readonly float[] _vertices =
    // {
    //       // x      y     z
    //         -0.5f, -0.5f, 0.0f,
    //         -0.2f, -0.4f, 0.0f,
    //          0.3f, -0.5f, 0.0f,
    //          0.2f, -0.4f, 0.0f,
    //          0.3f,  0.5f, 0.0f,
    //          0.2f,  0.4f, 0.0f,
    //         -0.2f,  0.4f, 0.0f,
    //         -0.3f,  0.5f, 0.0f,
    //     };

        private readonly float[] _vertices =
    {
          // x      y     z
            -0.5f, -0.5f, 0.0f,
            0.5f, 0.5f, 0.0f,
             1f, 1f, 0.0f,
        };

    // Then, we create a new array: indices.
    // This array controls how the EBO will use those vertices to create triangles
    private readonly uint[] _indices =
    {
            // Note that indices start at 0
            0, 1, 2,
            2, 1, 0,
            0, 1, 2,
        };

    private int _vertexBufferObject;

    private int _vertexArrayObject;

    private Shader? _shader = null;

    // Add a handle for the EBO
    private int _elementBufferObject;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // We create/bind the Element Buffer Object EBO the same way as the VBO, except there is a major difference here which can be REALLY confusing.
        // The binding spot for ElementArrayBuffer is not actually a global binding spot like ArrayBuffer is. 
        // Instead it's actually a property of the currently bound VertexArrayObject, and binding an EBO with no VAO is undefined behaviour.
        // This also means that if you bind another VAO, the current ElementArrayBuffer is going to change with it.
        // Another sneaky part is that you don't need to unbind the buffer in ElementArrayBuffer as unbinding the VAO is going to do this,
        // and unbinding the EBO will remove it from the VAO instead of unbinding it like you would for VBOs or VAOs.
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        // We also upload data to the EBO the same way as we did with VBOs.
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        _shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _shader?.Use();

        // Because ElementArrayObject is a property of the currently bound VAO,
        // the buffer you will find in the ElementArrayBuffer will change with the currently bound VAO.
        GL.BindVertexArray(_vertexArrayObject);

        // Then replace your call to DrawTriangles with one to DrawElements
        // Arguments:
        //   Primitive type to draw. Triangles in this case.
        //   How many indices should be drawn. Six in this case.
        //   Data type of the indices. The indices are an unsigned int, so we want that here too.
        //   Offset in the EBO. Set this to 0 because we want to draw the whole thing.
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}
