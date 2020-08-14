using System;
using System.Numerics;
using AmongUsCheeseCake;
using AmongUsCheeseCake.Cheat;
using Binarysharp.MemoryManagement.Assembly.CallingConvention;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace AmongUsCheeseCake
{

    public class CheatWindow
    {
        public static CheatWindow Instance
        {
            get
            {
                if(m_Instance == null) 
                    m_Instance = new CheatWindow(); 
                return m_Instance;
            }
        }

        public  Sdl2Window SdlWindow { get => _sdlWindow; set => _sdlWindow = value; }

        private static CheatWindow m_Instance;
        private Sdl2Window _sdlWindow;
        private GraphicsDevice _graphicsDriver;
        private ImGuiController _controller;
        private CommandList _cl;
        private Vector3 _clearColor = new Vector3(0.2f, 0.2f, 0.2f);
        public void Run()
        {
        
            SdlWindow = new Sdl2Window("",50,50,200,400, SDL_WindowFlags.AlwaysOnTop, true);
            _graphicsDriver = VeldridStartup.CreateDefaultD3D11GraphicsDevice(new GraphicsDeviceOptions() { 
            
            }, SdlWindow);
            SdlWindow.Resized += () =>
            {
                _graphicsDriver.MainSwapchain.Resize((uint)SdlWindow.Width, (uint)SdlWindow.Height);
                _controller.WindowResized(SdlWindow.Width, SdlWindow.Height);
            }; 
            SdlWindow.BorderVisible = false; 
            _cl = _graphicsDriver.ResourceFactory.CreateCommandList();
            _controller = new ImGuiController(_graphicsDriver, _graphicsDriver.MainSwapchain.Framebuffer.OutputDescription, SdlWindow.Width, SdlWindow.Height);
            SdlWindow.Opacity = 0.85f;
            while (SdlWindow.Exists)
            {
                InputSnapshot snapshot = SdlWindow.PumpEvents();
                if (!SdlWindow.Exists) { break; }
                _controller.Update(1f / 60f, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.

                SubmitUI();

                _cl.Begin();
                _cl.SetFramebuffer(_graphicsDriver.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 0.5f));
                _controller.Render(_graphicsDriver, _cl);
                _cl.End();
                _graphicsDriver.SubmitCommands(_cl);
                _graphicsDriver.SwapBuffers(_graphicsDriver.MainSwapchain);
            }
             
            _graphicsDriver.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _graphicsDriver.Dispose();
        }

        private static unsafe void SubmitUI()
        {
            {
                ImGui.SetNextWindowSize(new Vector2(200, 400), ImGuiCond.Appearing);
                ImGui.Begin("", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove);
                ImGui.SetWindowPos(new Vector2(0,0)); 
                ImGui.Text("by shlifedev@gmail.com"); 
              
                var xx = ImGui.IsKeyDown('A');
                
                if (ImGui.Button("Change Imposter State"))
                {

                    foreach (var m in CheatBase.Instance.RealPlayerInstance)
                    {
                        if (m.isMine)
                        {
                            if(m.PlayerInfo.Value.IsImpostor == 0)
                            {
                                m.WriteMemory_Imposter(1);
                            }
                            else
                            {
                                m.WriteMemory_Imposter(0);
                            }
                       
                        }
                    }
                } 
                if (ImGui.Button("Change Dead State"))
                {
                    foreach (var m in CheatBase.Instance.RealPlayerInstance)
                    {
                        if (m.isMine)
                        {
                            if (m.PlayerInfo.Value.IsDead == 0)
                            {
                                m.WriteMemory_IsDead(1);
                            }
                            else
                            {
                                m.WriteMemory_IsDead(0);
                            }
                        }
                    }
                }
                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
            }
        }
    }


}
/// <summary>
/// 해당 치트는 플레이어의 첫번째 인스턴스 배열을 찾으면 된다.
/// 가이드 : 킬타이머 offset찾은이후 offset -44 = base Position => monodissert에서 pBase찾기
/// </summary>
public class Program
{ 
    [STAThread]
    static void Main(string[] args)
    {

       
        CheatBase.Instance.Init();
        
      
        while (true)
        {
            var command = Console.ReadLine();
            if (command.ToLower().Contains("reset"))
            {
                CheatBase.Instance.Init();
            }
            if (command.ToLower().Contains("mapsize"))
            {
                var x = command.Split(' ');
                var size = int.Parse(x[1]);
                RadarOverlay.Instance.map_size = size;
            }
            if (command.ToLower().Contains("overlaysize"))
            {
                var x = command.Split(' ');
                var size = int.Parse(x[1]);
                RadarOverlay.Instance.SetWindowSize(size, size);
                RadarOverlay.Instance.overlaySize = size;
            }
            if (command.ToLower().Contains("center"))
            {
                var x = command.Split(' ');
                var size = int.Parse(x[1]);
                RadarOverlay.Instance.center = size;
            }
            if (command.ToLower().Contains("soundmanager"))
            {
                var x = CheatBase.MemorySharp.Assembly.Execute(new IntPtr(0x5161EED0), CallingConventions.Stdcall);
                Console.WriteLine("SoundManager PTR => " + x);
                CheatBase.MemorySharp.Assembly.Execute(x, CallingConventions.Thiscall, new IntPtr(0x5161D760), 0);
            }
            if (command.ToLower().Contains("inject"))
            {
                CheatBase.MemorySharp.Modules.Inject(@"C:\Users\shlif\OneDrive\Documents\GitHub\AmongUsCheat\AmongUsCheeseCake\Release\MethodDLL.dll");
            }
            if (command.ToLower().Contains("eject"))
            {
                CheatBase.MemorySharp.Modules.Eject("MethodDLL");
                Console.WriteLine("method dll eject!");
            }
            if (command.ToLower().Contains("test"))
            {
                CheatBase.MemorySharp["MethodDLL"]["Test"].Execute(CallingConventions.Stdcall);

            }
            if (command.ToLower().Contains("imposter"))
            {
                foreach (var m in CheatBase.Instance.RealPlayerInstance)
                {
                    if (m.isMine)
                    {
                        m.WriteMemory_Imposter(1);
                    }
                }

            }
            if (command.ToLower().Contains("revive"))
            {
                foreach (var m in CheatBase.Instance.RealPlayerInstance)
                {
                    if (m.isMine)
                    {
                        m.WriteMemory_IsDead(0);
                    }
                }

            }
            if (command.ToLower().Contains("dead"))
            {
                foreach (var m in CheatBase.Instance.RealPlayerInstance)
                {
                    if (m.isMine)
                    {
                        m.WriteMemory_IsDead(1);
                    }
                }

            }
        }

        System.Threading.Thread.Sleep(100000000);
    }

}
