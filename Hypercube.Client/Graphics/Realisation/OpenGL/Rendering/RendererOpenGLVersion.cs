﻿namespace Hypercube.Client.Graphics.Realisation.OpenGL.Rendering;

public enum RendererOpenGLVersion : byte
{
    Auto = default,
    GL33 = 1,
    GL31 = 2,
    GLES3 = 3,
    GLES2 = 4,
}