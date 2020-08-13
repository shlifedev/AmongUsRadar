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


    private readonly Dictionary<byte, Vector2> _diedPlayersMap;
    public float map_size = 50;
    public float overlaySize = 350;
    public RadarOverlay()
    {
        _brushes = new Dictionary<string, SolidBrush>();
        _fonts = new Dictionary<string, Font>();
        _images = new Dictionary<string, Image>();
        _diedPlayersMap = new Dictionary<byte, Vector2>();
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





        _brushes["player_color_0"] = gfx.CreateSolidBrush(255, 0, 0);
        _brushes["player_color_1"] = gfx.CreateSolidBrush(0, 051, 204);
        _brushes["player_color_2"] = gfx.CreateSolidBrush(0, 102, 0);
        _brushes["player_color_3"] = gfx.CreateSolidBrush(255, 0, 204);
        _brushes["player_color_4"] = gfx.CreateSolidBrush(255, 102, 51);
        _brushes["player_color_5"] = gfx.CreateSolidBrush(255, 255, 051);
        _brushes["player_color_6"] = gfx.CreateSolidBrush(51, 51, 51);
        _brushes["player_color_7"] = gfx.CreateSolidBrush(255, 255, 255);
        _brushes["player_color_8"] = gfx.CreateSolidBrush(102, 0, 153);
        _brushes["player_color_9"] = gfx.CreateSolidBrush(102, 51, 0);
        _brushes["player_color_10"] = gfx.CreateSolidBrush(51, 255, 255);
        _brushes["player_color_11"] = gfx.CreateSolidBrush(0, 255, 0);



        _brushes["player_color_0_dead"] = gfx.CreateSolidBrush(255, 0, 0, 150);
        _brushes["player_color_1_dead"] = gfx.CreateSolidBrush(0, 051, 204, 150);
        _brushes["player_color_2_dead"] = gfx.CreateSolidBrush(0, 102, 0, 150);
        _brushes["player_color_3_dead"] = gfx.CreateSolidBrush(255, 0, 204, 150);
        _brushes["player_color_4_dead"] = gfx.CreateSolidBrush(255, 102, 51, 150);
        _brushes["player_color_5_dead"] = gfx.CreateSolidBrush(255, 255, 051, 150);
        _brushes["player_color_6_dead"] = gfx.CreateSolidBrush(51, 51, 51, 150);
        _brushes["player_color_7_dead"] = gfx.CreateSolidBrush(255, 255, 255, 150);
        _brushes["player_color_8_dead"] = gfx.CreateSolidBrush(102, 0, 153, 150);
        _brushes["player_color_9_dead"] = gfx.CreateSolidBrush(102, 51, 0, 150);
        _brushes["player_color_10_dead"] = gfx.CreateSolidBrush(51, 255, 255, 150);
        _brushes["player_color_11_dead"] = gfx.CreateSolidBrush(0, 255, 0, 150);


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


    public Point GetRadarPosition(Vector2 pos)
    {
        var overlayX = (overlaySize/2) + (overlaySize * ((pos.x +center) / map_size));
        var overlayY = (overlaySize/2) - (overlaySize * ((pos.y -center) / map_size));
        return new Point(overlayX, overlayY);
    }



    bool _diePlayerFlag = false;
    bool _fullInit = false;

    private void Init()
    {
        if (_diePlayerFlag == false)
        {
            foreach (var data in cb.RealPlayerInstance)
            {
                _diePlayerFlag = true;
                data.onDie += OnDiePlayer;
                Console.WriteLine("Add Die Event");
            }
        }

        if(_diePlayerFlag == true)
        {
            _fullInit = true;
        }
    }
    private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
    {
        if (_fullInit == false)
        {
            Init();
        }
        var gfx = e.Graphics;
        gfx.ClearScene(_brushes["black 50%"]);
        gfx.FillRectangle(_brushes["black 50%"], new Rectangle(0, 0, overlaySize, 15));
        gfx.DrawText(_fonts["consolas-mid"], _brushes["green"], new Point(0, 0), " 2D Radar | Coder by : 에비츄(shlifedev)");
        gfx.DrawText(_fonts["consolas"], _brushes["white"], new Point(0, overlaySize - 18), " 플레이어 수 : " + cb.RealPlayerInstance.Count);
        foreach (var x in cb.RealPlayerInstance)
        {
            var pos = Vector2.Zero;
            var playerBrush = _brushes["black"];



            if (x.isOther && x.isMine == false)
                pos = x.Instance.GetSyncPosition();
            else if (x.isMine)
                pos = x.Instance.GetMyPosition();


             

            var overlayX = (overlaySize/2) + (overlaySize * ((pos.x +center) / map_size));
            var overlayY = (overlaySize/2) - (overlaySize * ((pos.y -center) / map_size));

            x.ReadMemory();
            x.ObserveState();
            if (x.PlayerInfo.Value.IsImpostor == 1)
            {
                gfx.DrawText(_fonts["arial_small"], _brushes["red"], new Point(overlayX, overlayY - 5), "임포스터");
            }
            if (x.PlayerInfo.Value.IsDead == 1)
            {
                playerBrush = _brushes[$"player_color_{x.PlayerInfo.Value.ColorId}_dead"];
                Vector2 diePos = Vector2.Zero;
                _diedPlayersMap.TryGetValue(x.PlayerInfo.Value.ColorId, out diePos);
                if (diePos.IsZero() == false)
                {
                    var overlayPoint = GetRadarPosition(diePos);
                    gfx.FillCircle(playerBrush, overlayPoint.X, overlayPoint.Y, 5);
                    gfx.DrawText(_fonts["arial_small"], _brushes["red"], new Point(overlayPoint.X, overlayPoint.Y + 5), $"죽음");
                }
            }
            else
            {
                playerBrush = _brushes[$"player_color_{x.PlayerInfo.Value.ColorId}"];
                gfx.FillCircle(playerBrush, overlayX, overlayY, 4);
            }
            gfx.DrawText(_fonts["arial_small"], _brushes["white"], new Point(overlayX, overlayY - 15), $"{pos.x.ToString("0.0")},{pos.y.ToString("0.0")}");
      
        }
    } 



    public void OnDiePlayer(Vector2 pos, byte colorID)
    {
        _diedPlayersMap.Add(colorID, pos);
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