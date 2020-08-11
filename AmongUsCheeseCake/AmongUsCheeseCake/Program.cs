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

        static string PlayerControllPattern = "08 47 6E 07 ?? ?? ?? ??";
        static string GameDataPattern = "88 FB 3B 07 ?? ?? ?? ??"; 
        static List<S_PlayerControll> SearchPlayerInfoList()
        {
            List<S_PlayerControll> list = new List<S_PlayerControll>();
            var result = m.AoBScan(PlayerControllPattern, true, true);
            result.Wait();
            var results =    result.Result;

            Console.WriteLine("instanced S_PlayerInfo offset count : => " + results.Count());
            foreach (var x in results)
            {
                var bytes = m.ReadBytes(GetAddress(x), S_PlayerControll.SizeOf()); 
                var playerControll = S_PlayerControll.FromBytes(bytes);   
                list.Add(playerControll); 
            }
            return list;
        }

        static List<S_PlayerControll> S_PlayerControllList = null;
        static Dictionary<int, S_Vector2> UpdatedVectorDictionary = new Dictionary<int, S_Vector2>();
        static void UpdatePlayerList()
        {
            int _offset_vec2_position = 80;
            int _offset_vec2_sizeOf = 8;
            int idx = 0;
            foreach (var x in S_PlayerControllList)
            {
                var netTransform = ((int)x.NetTransform + _offset_vec2_position).ToString("X"); 
                var vec2Data= m.ReadBytes($"{netTransform}",_offset_vec2_sizeOf); // 주소로부터 8바이트 읽는다   
                if (vec2Data != null && vec2Data.Length != 0)
                {
                    var vec2 = S_Vector2.FromBytes(vec2Data);  
                    if(UpdatedVectorDictionary.ContainsKey(idx) == false)
                    {  
                            UpdatedVectorDictionary.Add(idx, vec2);  
                    }
                    else
                    {
                        var originalData = UpdatedVectorDictionary[idx]; 
                        var currentVec = vec2;
                        if (originalData.x != currentVec.x)
                        {
                            Console.WriteLine(x.PlayerId + "    " + currentVec.x + "," + currentVec.y);
                        } 
                    } 
                    idx++;
                }
            }
        }
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
            Console.WriteLine("instanced S_GameData offset count : => " + results.Count());
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
            S_PlayerControllList = SearchPlayerInfoList();
            if (open)
            {
                while (true)
                {
              
                    UpdatePlayerList();
                    var gameDataOffset = FindGameDataInstance();
                    var bytes = m.ReadBytes(gameDataOffset, S_GameData.SizeOf());
                    var gameData = S_GameData.FromBytes(bytes);
                    Console.WriteLine(gameData.TotalTasks + "개의 미션중 " + gameData.CompletedTasks + " 개 완료함");
                     
              


                }
            }

            System.Threading.Thread.Sleep(100000000);
        }

    }
}
