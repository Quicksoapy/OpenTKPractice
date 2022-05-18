using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace OpenTKtest;

// This is where all OpenGL code will be written.
// OpenToolkit allows for several functions to be overriden to extend functionality; this is how we'll be writing code.
public class Window : GameWindow
{
    private readonly float[] _vertices =
    {
        0.5f,  0.5f, 0.0f, // top right
        0.5f, -0.5f, 0.0f, // bottom right
        -0.5f, -0.5f, 0.0f, // bottom left
        -0.5f,  0.5f, 0.0f, // top left
    };

    private readonly uint[] _indices =
    {
        0, 1, 3,
        1, 2, 3
    };  
     
    // What these objects are will be explained in OnLoad.
    private int _vertexBufferObject;

    private int _vertexArrayObject;
    
    // This class is a wrapper around a shader, which helps us manage it.
    // The shader class's code is in the Common project.
    // What shaders are and what they're used for will be explained later in this tutorial.
    private Shader _shader;
    private int _elementBufferObject;
    // A simple constructor to let us set properties like window size, title, FPS, etc. on the window.
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

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        
        _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        
        _shader.Use();

    }
    
    // This function runs on every update frame.
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        _shader.Use();
        
        GL.BindVertexArray(_vertexArrayObject);
        
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

        // When the window gets resized, we have to call GL.Viewport to resize OpenGL's viewport to match the new size.
        // If we don't, the NDC will no longer be correct.
        GL.Viewport(0, 0, Size.X, Size.Y);
    }
}