using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmongUsCheeseCake.Cheat;
using AmongUsCheeseCake.Game;
using Memory;
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

            while(true)
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
            }

            System.Threading.Thread.Sleep(100000000);
        }

    }
}
