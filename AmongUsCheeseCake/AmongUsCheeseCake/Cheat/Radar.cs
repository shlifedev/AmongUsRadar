using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AmongUsCheeseCake.Cheat;
using AmongUsCheeseCake.Game;
using GameOverlay.Drawing;
using GameOverlay.Windows;

public class RadarOverlay : IDisposable
{
    public CheatBase cb;
    private readonly GraphicsWindow _window;

    private readonly Dictionary<string, SolidBrush> _brushes;
    private readonly Dictionary<string, Font> _fonts;
    private readonly Dictionary<string, Image> _images;

    public float map_size = 50;
    public float overlaySize = 350;
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

        _window = new GraphicsWindow(0, 0, (int)overlaySize, (int)overlaySize, gfx)
        {
            FPS = 60,
            IsTopmost = true,
            IsVisible = true
        };

        _window.DestroyGraphics += _window_DestroyGraphics;
        _window.DrawGraphics += _window_DrawGraphics;
        _window.SetupGraphics += _window_SetupGraphics;
    }
    public void SetWindowSize(int x, int y)
    {
        _window.Resize(x, y);
    }
    public void SetWindowPos(int x, int y)
    {
        _window.Move(x, y);
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

        if (e.RecreateResources)
            return;

        _fonts["arial"] = gfx.CreateFont("Arial", 12);
        _fonts["arial_small"] = gfx.CreateFont("Arial", 10);
        _fonts["consolas"] = gfx.CreateFont("Consolas", 13); 
        _fonts["consolas-mid"] = gfx.CreateFont("Consolas", 11);

    }

    private void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
    {
        foreach (var pair in _brushes) pair.Value.Dispose();
        foreach (var pair in _fonts) pair.Value.Dispose();
        foreach (var pair in _images) pair.Value.Dispose();
    }


    public float center = 0;
     
    private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
    { 
        var gfx = e.Graphics;  
        gfx.ClearScene(_brushes["black 50%"]);
        gfx.FillRectangle(_brushes["black 50%"], new Rectangle(0, 0, overlaySize, 15));  
        gfx.DrawText(_fonts["consolas-mid"], _brushes["green"], new Point(0, 0), " 2D Radar | Coder by : 에비츄(shlifedev)"); 
        gfx.DrawText(_fonts["consolas"], _brushes["white"], new Point(0, overlaySize-18), " 플레이어 수 : " + cb.RealPlayerInstance.Count);
        foreach (var x in cb.RealPlayerInstance)
        {
            var pos = Vector2.Zero;
            var playerBrush = _brushes["green"];   
                pos = x.Instance.GetSyncPosition();
                    

            if (x.isOther)
            {
                playerBrush = _brushes["red"];
            }
            else
            {
                pos = x.Instance.GetMyPosition(); 
            }
            float overlayXPer = (pos.x +center) / map_size; 
            float overlayYPer = (pos.y +center) / map_size;  
            var overlayX = (overlaySize/2) + (overlaySize * ((pos.x +center) / map_size));
            var overlayY = (overlaySize/2) - (overlaySize * ((pos.y -center) / map_size));  

            x.ReadMemory();
            if(x.isOther && x.isImposter)
            {
                playerBrush = _brushes["blue"]; 
            }

            if(x.Instance.inVent == 0) 
                gfx.DrawText(_fonts["arial_small"], _brushes["white"], new Point(overlayX, overlayY - 5), "벤트");




            gfx.DrawText(_fonts["arial_small"], _brushes["white"], new Point(overlayX, overlayY - 15), $"{pos.x.ToString("0.0")},{pos.y.ToString("0.0")}");
            gfx.FillCircle(playerBrush, overlayX - 2, overlayY - 2, 2);
            gfx.DrawText(_fonts["arial_small"], _brushes["white"], new Point(overlayX, overlayY), x.Instance.PlayerId.ToString());  
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