using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        }

        static List<playerData> playerControllList = new List<playerData>();



        static void readPlayerList(string base_offset, int player_count)
        {
            var size = S_PlayerControll.SizeOf();
            for (int i = 0; i < player_count; i++)
            {
                var nextOffset = int.Parse(base_offset, System.Globalization.NumberStyles.HexNumber); 
                nextOffset = nextOffset + (i * size); 
                var nextOffsetHex = nextOffset.ToString("X"); 
                var data = m.ReadBytes($"{nextOffsetHex}", size); 
                var info = S_PlayerControll.FromBytes(data) ;

                if (info != null)
                    playerControllList.Add(new playerData() {
                        _instance = info
                    });
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("cannot add player");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        static void updatePos()
        {
            foreach(var player in playerControllList)
            {
                //플레이어의 netTransform 포인터가 가르키는 값의 80 = x,y값 (80~88범위)
                var netTransform = ((int)player._instance.NetTransform + 80).ToString("X");
                var vec2Data= m.ReadBytes($"{netTransform}", 8); // 주소로부터 8바이트 읽는다   
                player.vec2 = S_Vector2.FromBytes(vec2Data); 
                Console.WriteLine(player.vec2.x);
            }
        }


        static void Main(string[] args)
        { 
            string FIRST_PLAYER_OFFSET = "06A94D20"; 
            Console.WriteLine("----포인터 디버거---");
            Console.WriteLine("주소A\t주소B\t계산결과");
            for (int i = 0; i < datas.Count - 1; i++)
            {
                UIntPtr x = (UIntPtr)datas[i];
                UIntPtr y = (UIntPtr)datas[i+1];
                Console.WriteLine(x.ToString() + "\t" + y.ToString() + " offset distance " + ((int)y - (int)x));
            }
            Console.WriteLine("----포인터 디버거---");

   
            unsafe
            {
                m.OpenProcess("Among us");
                readPlayerList("06A94D20", 3);
                updatePos();

                Console.WriteLine("PID\tVent\tKillTimer\tNetTransform");
                while (true)
                {
                    if (m.OpenProcess("Among us"))
                    {
                    

                        //플레이어 정보 가져오기
                        var size = S_PlayerControll.SizeOf();
                        var data = m.ReadBytes($"{FIRST_PLAYER_OFFSET}", size);
                        var info = S_PlayerControll.FromBytes(data); 
                        Console.WriteLine($"{info.PlayerId}\t{info.inVent}\t{info.killTimer}\t{info.NetTransform}");

        
                        
                        //플레이어의 netTransform 포인터가 가르키는 값의 80 = x,y값 (80~88범위)
                        var netTransform = ((int)info.NetTransform + 80).ToString("X"); 
                        var vec2Data= m.ReadBytes($"{netTransform}", 8); // 주소로부터 8바이트 읽는다 

                        //읽어온 벡터정보 
                        S_Vector2 vec2 = S_Vector2.FromBytes(vec2Data);
                        Co nsole.WriteLine($"\t\t\tㄴptr:{netTransform} x:{vec2.x},y:{vec2.y}");
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
