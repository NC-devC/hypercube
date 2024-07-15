﻿using Hypercube.Client.Graphics.Texturing;
using Hypercube.Client.Graphics.Texturing.Settings;
using Hypercube.Shared.Dependency;
using Hypercube.Shared.Logging;
using Hypercube.Shared.Resources;
using Hypercube.Shared.Resources.Manager;
using StbImageSharp;

namespace Hypercube.Client.Graphics.Realisation.OpenGL.Texturing;

public sealed class TextureManager : ITextureManager
{
    [Dependency] private readonly IResourceManager _resourceManager = default!;
    
    private readonly Logger _logger = LoggingManager.GetLogger("texturing");

    public TextureManager()
    {
        StbImage.stbi_set_flip_vertically_on_load(1);
    }
    
    public ITextureHandle GetTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(path, settings);
    }
    
    public ITextureHandle GetTextureHandle(ResourcePath path)
    {
        return GetTextureHandleInternal(path, new Texture2DCreationSettings());
    }
    
    public ITexture GetTexture(ResourcePath path)
    {
        return GetTextureInternal(path);
    }

    public ITextureHandle GetTextureHandle(ITexture texture)
    {
        return GetTextureHandleInternal(texture.Path, new Texture2DCreationSettings());
    }

    public ITextureHandle GetTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return GetTextureHandleInternal(texture.Path, settings);
    }

    private ITexture GetTextureInternal(ResourcePath path)
    {
        var texture = CreateTexture(path);
        return texture;
    }

    private ITexture CreateTexture(ResourcePath path)
    {
        using var stream = _resourceManager.ReadFileContent(path);
        
        var result = ImageResult.FromStream(stream);
        var texture = new Texture(path, (result.Width, result.Height), result.Data);
        
        return texture;
    }

    private ITextureHandle GetTextureHandleInternal(ResourcePath path, ITextureCreationSettings settings)
    {
        return CreateTextureHandle(path, settings);
    }

    private ITextureHandle CreateTextureHandle(ResourcePath path, ITextureCreationSettings settings)
    {
        var texture = GetTexture(path);
        var handle = new TextureHandle(texture, settings);

        return handle;
    }
    
    private ITextureHandle CreateTextureHandle(ITexture texture, ITextureCreationSettings settings)
    {
        return new TextureHandle(texture, settings);
    }
}