using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AmongUsCheeseCake;
using AmongUsCheeseCake.Cheat;
using AmongUsCheeseCake.Game;
using GameOverlay.Drawing;
using GameOverlay.Windows;

public class RadarOverlay : IDisposable
{ 
    public enum Map
    {
        Skeld, Unknown, Polus
    }

    public static RadarOverlay Instance
    {
        get
        {
            if (instance == null) instance = new RadarOverlay();
            return instance;
        }
    }

    Image polus_img = null;

    public Map map;
    private static RadarOverlay instance;
    private readonly GraphicsWindow _window; 
    private readonly Dictionary<string, SolidBrush> _brushes;
    private readonly Dictionary<string, Font> _fonts;
    private readonly Dictionary<string, Image> _images;
    public bool drawDisable = false; 
    private readonly Dictionary<byte, Vector2> _diedPlayersMap;
    /// <summary>
    /// 오버레이 사이즈
    /// </summary>
    public  float overlaySize   = 350;
    /// <summary>
    /// 맵사이즈
    /// </summary>
    public  float map_size      = 50; 
    public  float center        = 0; 
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
        polus_img = gfx.CreateImage(System.IO.File.ReadAllBytes("polus.png"));




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




    public Point Vec2ToOverlayPos(Vector2 pos)
    {
        var overlayX = (overlaySize/2) + (overlaySize * ((pos.x +center) / map_size));
        var overlayY = (overlaySize/2) - (overlaySize * ((pos.y -center) / map_size));
        return new Point(overlayX, overlayY);
    }





    public void Init()
    {
        foreach (var data in CheatBase.Instance.RealPlayerInstance)
        { 
            data.onDie += OnDiePlayer;
            Console.WriteLine("Add Die Event");
        }

        CheatBase.Instance.onInit += OnCheatInit; 
        if(map == Map.Polus)
        {
            this.center = -20;
            this.map_size = 100;
        }
    
    }


    public void OnCheatInit()
    {
        Console.WriteLine("OnCheatInit");
         _diedPlayersMap.Clear();

    
    }

    public void RenderPlayer(Graphics gfx, CachedPlayerControllInfo player)
    {
        var pInfo = player.PlayerInfo.Value;
        if (pInfo.IsDead == 0)
        {
            SolidBrush playerInstanceBrush =  _brushes[$"player_color_{pInfo.ColorId}"];
            var overlayPosition = Vec2ToOverlayPos(player.Position);
            gfx.FillCircle(playerInstanceBrush, overlayPosition.X, overlayPosition.Y, 4);

            if (pInfo.IsImpostor == 1) 
                gfx.DrawText(_fonts["arial_small"], _brushes["red"], new Point(overlayPosition.X, overlayPosition.Y - 5), "Imposter"); 
        } 
    }
    public void RenderDeadBody(Graphics gfx, CachedPlayerControllInfo player)
    {
        var pInfo = player.PlayerInfo.Value;
        Vector2 diePos = Vector2.Zero;
        var b = _diedPlayersMap.TryGetValue(pInfo.ColorId, out diePos);
        player.ObserveState();
        if(b)
        {
            SolidBrush circleBrush = _brushes[$"player_color_{pInfo.ColorId}_dead"];
            var overlayPoint = Vec2ToOverlayPos(diePos);
            gfx.FillCircle(circleBrush, overlayPoint.X, overlayPoint.Y, 5);
            gfx.DrawText(_fonts["arial_small"], _brushes["red"], new Point(overlayPoint.X, overlayPoint.Y + 5), $"DIE");
        }  
    }

    public void DrawCreatorInfo(Graphics gfx)
    {
        gfx.FillRectangle(_brushes["black 50%"], new Rectangle(0, 0, overlaySize, 15));
        gfx.DrawText(_fonts["consolas-mid"], _brushes["green"], new Point(0, 0), " 2D Radar | Coder by : 에비츄(shlifedev)");
    }

    public void DrawPlayerCount(Graphics gfx)
    {
        gfx.DrawText(_fonts["consolas"], _brushes["white"], new Point(0, overlaySize - 36), " Player Count : " + CheatBase.Instance.RealPlayerInstance.Count);

        string imposters = null; 
        foreach (var playerInstance in CheatBase.Instance.RealPlayerInstance)
        {
            PlayerInfo value = playerInstance.PlayerInfo.Value;
            if (value.IsImpostor == 1)
            {
                imposters += ((ColorID)value.ColorId).ToString() + "  ";
            }
        }

       gfx.DrawText(_fonts["consolas"], _brushes["green"], new Point(0, overlaySize - 18), $" Imposter : {imposters}"); 
    }
    private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
    {
      
        var gfx = e.Graphics;
        gfx.ClearScene(_brushes["black 50%"]);  
        DrawCreatorInfo(gfx);
        DrawPlayerCount(gfx);
        gfx.DrawImage(polus_img, new Point(0, 0), 1f);

        foreach (var playerInstance in CheatBase.Instance.RealPlayerInstance)
        {
            playerInstance.ReadMemory();
            Vector2 playerPosition = playerInstance.Position;
            SolidBrush playerInstanceBrush = _brushes["black"];  
            var overlayPosition = Vec2ToOverlayPos(playerPosition);
            RenderPlayer(gfx, playerInstance);
            RenderDeadBody(gfx, playerInstance);

            //debug
            gfx.DrawText(_fonts["arial_small"], _brushes["white"], new Point(overlayPosition.X, overlayPosition.Y - 15), $"{playerPosition.x.ToString("0.0")},{playerPosition.y.ToString("0.0")}");
        }
     
        if(drawDisable)
        {
            gfx.ClearScene();
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