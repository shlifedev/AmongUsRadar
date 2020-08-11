using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmongUsCheeseCake.Game;
using Memory;
namespace AmongUsCheeseCake
{
    /// <summary>
    /// 해당 치트는 플레이어의 첫번째 인스턴스 배열을 찾으면 된다.
    /// 가이드 : 킬타이머 offset찾은이후 offset -44 = base Position => monodissert에서 pBase찾기
    /// </summary>
    class Program
    {
        /*Memory Ref https://github.com/shlifedev/memory.dll*/
        static public Mem m = new Mem();

        static string GetAddress(long value) { return value.ToString("X"); }
        static string GetAddress(int value) { return value.ToString("X"); }
        static string GetAddress(uint value) { return value.ToString("X"); }

        static string GameDataPattern = "08 77 CA 06 ?? ?? ?? ??";


        /// <summary>
        /// 데이터를 찾는 비용이 크므로 1회성 실행한다.
        /// </summary>
        /// <returns></returns>
        static String FindGameDataInstance()
        {
            string offset = null;
            var result = m.AoBScan(GameDataPattern, true, true);
            result.Wait();
            var results =    result.Result;
            Console.WriteLine("instanced gamedata offset count : => " + results.Count());
            foreach (var x in results)
            {
                var bytes = m.ReadBytes(GetAddress(x), S_GameData.SizeOf());
                var gameData = S_GameData.FromBytes(bytes);
                // OWNER ID가 -2이고, NetId가 4294967295가 아닌 객체는 실제 인스턴스이다.
                // 4294967295(uint의 max값)은, 이미 인스턴스가 해제된 가비지값을 가리킴
                if (gameData.OwnerId == -2 && gameData.NetId != 4294967295)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Found Real Gamedata Instance!" + GetAddress(x));
                    Console.ForegroundColor = ConsoleColor.White;
                    offset = GetAddress(x);
                }
            }
            return offset;
        }


        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var open =  m.OpenProcess("Among us");
            if (open)
            {
                while (true)
                {

                    var gameDataOffset = FindGameDataInstance();
                    var bytes = m.ReadBytes(gameDataOffset, S_GameData.SizeOf());
                    var gameData = S_GameData.FromBytes(bytes);

                    Console.WriteLine(gameData.TotalTasks + "개의 미션중 " + gameData.CompletedTasks + " 개 완료함"); 
                    var ppp = m.ReadBytes(gameData.AllPlayers.ToUInt32().ToString("X"), S_PlayerInfo.SizeOf()); 
                }
            }

            System.Threading.Thread.Sleep(100000000);
        }

    }
}
