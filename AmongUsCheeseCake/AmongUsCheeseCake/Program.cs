﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmongUsCheeseCake.Cheat;
using AmongUsCheeseCake.Game;
using Binarysharp.MemoryManagement.Assembly.CallingConvention;
using Memory;
using ProcessUtil;

namespace AmongUsCheeseCake
{
    /// <summary>
    /// 해당 치트는 플레이어의 첫번째 인스턴스 배열을 찾으면 된다.
    /// 가이드 : 킬타이머 offset찾은이후 offset -44 = base Position => monodissert에서 pBase찾기
    /// </summary>
    public class Program
    { 

        static void Main(string[] args)
        {
            
            CheatBase cb = new CheatBase();
            cb.Init();

            while (true)
            {
                var command = Console.ReadLine();
                if(command.ToLower().Contains("reset"))
                {
                    cb.Init();
                }
                if (command.ToLower().Contains("mapsize"))
                {
                    var x = command.Split(' ');
                    var size = int.Parse(x[1]);
                    cb.radar.map_size = size;
                }
                if (command.ToLower().Contains("overlaysize"))
                {
                    var x = command.Split(' ');
                    var size = int.Parse(x[1]);
                    cb.radar.SetWindowSize(size, size);
                    cb.radar.overlaySize = size;
                }
                if (command.ToLower().Contains("center"))
                {
                    var x = command.Split(' ');
                    var size = int.Parse(x[1]);
                    cb.radar.center = size;
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
                    foreach (var m in cb.RealPlayerInstance)
                    {
                        if (m.isMine)
                        {
                            m.WM_SetImposter(1);
                        }
                    }

                }
                if (command.ToLower().Contains("revive"))
                {
                    foreach (var m in cb.RealPlayerInstance)
                    {
                        if (m.isMine)
                        {
                            m.WM_SetDead(0);
                        }
                    }

                }
                if (command.ToLower().Contains("dead"))
                {
                    foreach (var m in cb.RealPlayerInstance)
                    {
                        if (m.isMine)
                        {
                            m.WM_SetDead(1);
                        }
                    }

                }
            }

            System.Threading.Thread.Sleep(100000000);
        }

    }
}
