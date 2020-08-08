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
        public static List<UIntPtr> datas = new List<UIntPtr>() {
            new UIntPtr(0x06854960),
            new UIntPtr(0x06854A00),
            new UIntPtr(0x06854AA0),
            new UIntPtr(0x06854B40),
            new UIntPtr(0x06854BE0)
        };

        class playerData
        {
            public S_PlayerControll _instance;
            public S_Vector2 vec2;
            public string hex;
            public int size = 0;
            public void ReadMemory()
            {  
                var data          = m.ReadBytes($"{hex}", size); 
                var instance = S_PlayerControll.FromBytes(data);
                this._instance = instance;
            }
            public void ReadPos()
            {
                int _offset_vec2_position = 80;
                int _offset_vec2_sizeOf = 8;

                var netTransform = ((int)_instance.NetTransform + _offset_vec2_position).ToString("X");
                var vec2Data= m.ReadBytes($"{netTransform}",_offset_vec2_sizeOf); // 주소로부터 8바이트 읽는다   
                this.vec2 = S_Vector2.FromBytes(vec2Data);
            }
        } 
        static List<playerData> playerControllList = new List<playerData>(); 
        static void log(string tag, Object log)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[{tag}] ");
            Console.Write($"{log}\n");
             
        }
   
        static void readPlayerList(string base_offset, int player_count)
        {
            var size = S_PlayerControll.SizeOf() + 4;
            log("player struct size", $"{size} byte ");
            for (int i = 0; i < player_count; i++)
            {
                var nextOffset    = int.Parse(base_offset, System.Globalization.NumberStyles.HexNumber);
                nextOffset = nextOffset + (size * i);
                var nextOffsetHex = nextOffset.ToString("X");
                var pd = new playerData();
                pd.hex = nextOffsetHex;
                pd.size = size;
                pd.ReadMemory();
                playerControllList.Add(pd);
            }
        }

   
        static void updatePos()
        {
           
            string log = null;
            foreach(var player in playerControllList)
            { 
                player.ReadMemory(); 
                player.ReadPos();
                log += $"Player{player._instance.netId} => x{player.vec2.x} y{player.vec2.y}\n";
            } 
            Console.WriteLine(log);
        }
        
        static void ReadTest()
        {
            if (m.OpenProcess("Among Us"))
            {
                Console.WriteLine(S_PlayerControll.SizeOf());
                readPlayerList("073D4D20", 1);
                while (true)
                {
                    updatePos();
                    Thread.Sleep(20);
                    Console.Clear();
                }

            }
        }
         static void Readtest2(string dummyInstanceOffset)
        {
            Console.WriteLine(S_GameData.SizeOf());

            // 쓰레기 힙에 더미로 생성된 S_GameData정보
            var dummyInstanceByte = m.ReadBytes(dummyInstanceOffset, S_GameData.SizeOf());
            var data = S_GameData.FromBytes(dummyInstanceByte); 
            var singletonHex = data.Instance_Dummy.ToString("X"); 
          
            log("GameData Singleton Hex", singletonHex); 
           
            // GameData의 실제 싱글톤
            var singletonHexDummy = int.Parse(singletonHex, System.Globalization.NumberStyles.HexNumber) - 8;
            var realInstanceHex = singletonHexDummy.ToString("X");
            log("GameData Singleton realInstanceHex", realInstanceHex); 

            var realInstanceByte = m.ReadBytes(realInstanceHex, S_GameData.SizeOf());

        }
        static void Main(string[] args)
        { 
            var c =  m.OpenProcess("Among us");
            //ReadTest();
            Readtest2("044B6860"); 
            System.Threading.Thread.Sleep(1000000);
        }
    }
}
