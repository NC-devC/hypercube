﻿using Hypercube.Client.Graphics.Monitors;
using Hypercube.Client.Graphics.Rendering;
using Hypercube.Client.Input.Handler;
using Hypercube.Math.Vectors;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hypercube.Client.Graphics.Windows.Realisation.Glfw;

public sealed unsafe partial class GlfwWindowManager : IWindowManager
{
    [Dependency] private readonly IRenderer _renderer = default!;
    [Dependency] private readonly IInputHandler _inputHandler = default!;
    
    private readonly ILogger _logger =  LoggingManager.GetLogger("glfw");
    
    private bool _initialized;
    private bool _running;
    private bool _disposed;
    
    private int _nextWindowId = 1;
    
    public bool Init()
    {
        DependencyManager.Inject(this);

        // We don't let GC take our callbacks
        HandleCallbacks();
        
        if (!GlfwInit())
            return false;

        InitMonitors();
        return true;
    }

    public void Shutdown()
    {
        if (!_initialized)
            return;
        
        GLFW.Terminate();
    }

    public void EnterWindowLoop()
    {
        _logger.EngineInfo("Enter main loop");
        
        // Don't use GLFW.WindowShouldClose because we handle it use GLFW.SetWindowCloseCallback,
        // apply own callback, and close window on it
        _running = true;
        while (_running)
        {
            GLFW.WaitEvents();
        }
    }

    public void PollEvents()
    {
        GLFW.PollEvents();
    }

    public void Terminate()
    {
        _running = false;
        
        // Send an empty event to unfreeze the window
        // so that the lifecycle can be instantly terminated.
        // Otherwise, main app thread will wait for an event that will never come
        // GLFW.PostEmptyEvent();
        
        _logger.EngineInfo("Exit main loop");
    }
    
    public void MakeContextCurrent(WindowRegistration? window)
    {
        if (window is not null)
        {
            if (window.IsDisposed)
                return;

            var reg = (GlfwWindowRegistration)window;

            GLFW.MakeContextCurrent(reg.Pointer);
            return;
        }

        GLFW.MakeContextCurrent(null);
    }
    
    public nint GetProcAddress(string procName)
    {
        return GLFW.GetProcAddress(procName);
    }

    public void WindowSetMonitor(WindowRegistration window, MonitorRegistration monitor, Vector2Int vector)
    {
        if (monitor is not GlfwMonitorRegistration glfwMonitor)
            return;
        
        if (window is not GlfwWindowRegistration glfwWindow)
            return;
        
        GLFW.SetWindowMonitor(
            glfwWindow.Pointer, 
            glfwMonitor.Pointer, 
            vector.X, vector.Y, 
            monitor.Handle.Size.X, monitor.Handle.Size.Y, 
            monitor.Handle.RefreshRate);
    }
    
    public void Dispose()
    {
        Shutdown();
    }
}