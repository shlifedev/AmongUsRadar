﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AmongUsCheeseCake.Cheat;
using GameOverlay.Drawing;
using GameOverlay.Windows;

public class RadarOverlay : IDisposable
{
    public CheatBase cb;
    private readonly GraphicsWindow _window;

    private readonly Dictionary<string, SolidBrush> _brushes;
    private readonly Dictionary<string, Font> _fonts;
    private readonly Dictionary<string, Image> _images;
     
    private Random _random;
    private long _lastRandomSet;
    private List<Action<Graphics, float, float>> _randomFigures;

    public RadarOverlay()
    {
        _brushes = new Dictionary<string, SolidBrush>();
        _fonts = new Dictionary<string, Font>();
        _images = new Dictionary<string, Image>();

        var gfx = new Graphics()
        {
            MeasureFPS = true,
            PerPrimitiveAntiAliasing = true,
            TextAntiAliasing = true
        };

        _window = new GraphicsWindow(0, 0, 300, 300, gfx)
        {
            FPS = 60,
            IsTopmost = true,
            IsVisible = true
        };

        _window.DestroyGraphics += _window_DestroyGraphics;
        _window.DrawGraphics += _window_DrawGraphics;
        _window.SetupGraphics += _window_SetupGraphics;
    }

    private void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
    {
        var gfx = e.Graphics;

        if (e.RecreateResources)
        {
            foreach (var pair in _brushes) pair.Value.Dispose();
            foreach (var pair in _images) pair.Value.Dispose();
        }

        _brushes["black"] = gfx.CreateSolidBrush(0, 0, 0);
        _brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
        _brushes["red"] = gfx.CreateSolidBrush(255, 0, 0);
        _brushes["green"] = gfx.CreateSolidBrush(0, 255, 0);
        _brushes["blue"] = gfx.CreateSolidBrush(0, 0, 255);
        _brushes["background"] = gfx.CreateSolidBrush(0x33, 0x36, 0x3F);
        _brushes["grid"] = gfx.CreateSolidBrush(255, 255, 255, 0.2f);
        _brushes["random"] = gfx.CreateSolidBrush(0, 0, 0);
        _brushes["black 50%"] = gfx.CreateSolidBrush(0, 0, 0, 0.5f);

        if (e.RecreateResources) return;

        _fonts["arial"] = gfx.CreateFont("Arial", 12);
        _fonts["consolas"] = gfx.CreateFont("Consolas", 14); 
   
    }

    private void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
    {
        foreach (var pair in _brushes) pair.Value.Dispose();
        foreach (var pair in _fonts) pair.Value.Dispose();
        foreach (var pair in _images) pair.Value.Dispose();
    }



    public float map_size = 50;

    private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
    {
        var gfx = e.Graphics;  
        gfx.ClearScene(_brushes["black 50%"]);

        Console.WriteLine(cb.RealPlayerInstance.Count);
        foreach (var x in cb.RealPlayerInstance)
        {
            var pos = x.GetSyncPosition(); 
            float overlayXPer = pos.x / map_size;
            float overlayYPer = pos.y / map_size;  
            var overlayX = 150 + (300 * (pos.x / map_size));
            var overlayY = 150 - (300 * (pos.y / map_size)); 
            gfx.FillRectangle(_brushes["red"], overlayX - 2, overlayY - 2, overlayX + 2, overlayY + 2);
        }
    }
 

    public void Run()
    {
        _window.Create();
        _window.Join();
    }

    ~RadarOverlay()
    {
        Dispose(false);
    }

    #region IDisposable Support
    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            _window.Dispose();

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}