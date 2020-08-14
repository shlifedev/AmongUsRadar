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

    public class TX
    {
        private static Sdl2Window _sdlWindow;
        private static GraphicsDevice _graphicsDriver;
        private static ImGuiController _controller;
        private static CommandList _cl;
        private static Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);
        public void Run()
        {
            VeldridStartup.CreateWindowAndGraphicsDevice(
                   new WindowCreateInfo(50, 50, 1280, 720, WindowState.Normal, "ImGui.NET Sample Program"),
                   new GraphicsDeviceOptions(true, null, true),
                   out _sdlWindow,
                   out _graphicsDriver);
            _sdlWindow.Resized += () =>
            {
                _graphicsDriver.MainSwapchain.Resize((uint)_sdlWindow.Width, (uint)_sdlWindow.Height);
                _controller.WindowResized(_sdlWindow.Width, _sdlWindow.Height);
            };
            _cl = _graphicsDriver.ResourceFactory.CreateCommandList();
            _controller = new ImGuiController(_graphicsDriver, _graphicsDriver.MainSwapchain.Framebuffer.OutputDescription, _sdlWindow.Width, _sdlWindow.Height);
        
            while (_sdlWindow.Exists)
            {
                InputSnapshot snapshot = _sdlWindow.PumpEvents();
                if (!_sdlWindow.Exists) { break; }
                _controller.Update(1f / 60f, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.

                SubmitUI();

                _cl.Begin();
                _cl.SetFramebuffer(_graphicsDriver.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
                _controller.Render(_graphicsDriver, _cl);
                _cl.End();
                _graphicsDriver.SubmitCommands(_cl);
                _graphicsDriver.SwapBuffers(_graphicsDriver.MainSwapchain);
            }

            // Clean up Veldrid resources
            _graphicsDriver.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _graphicsDriver.Dispose();
        }

        private static unsafe void SubmitUI()
        {
            // Demo code adapted from the official Dear ImGui demo program:
            // https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx11/main.cpp#L172

            // 1. Show a simple window.
            // Tip: if we don't call ImGui.BeginWindow()/ImGui.EndWindow() the widgets automatically appears in a window called "Debug".
            {
                ImGui.Text("Hello, world!");
                ImGui.Text($"Mouse position: {ImGui.GetMousePos()}");
                if (ImGui.Button("Button"))                                         // Buttons return true when clicked (NB: most widgets return true when edited/activated)
                {

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

    static void Main(string[] args)
    {

        CheatBase.Instance.Init();
        TX t = new TX();
        t.Run();
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
