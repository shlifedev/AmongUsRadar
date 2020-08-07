﻿using System;
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

        
                        //플레이어의 netTransform 주소값
                        var netTransform = ((int)info.NetTransform + 80).ToString("X"); 
                        var vec2Data= m.ReadBytes($"{netTransform}", 8); // 주소로부터 8바이트 읽는다
                        S_Vector2 vec2 = S_Vector2.FromBytes(vec2Data);
                        Console.WriteLine($"\t\t\tㄴ{netTransform} {vec2.x},{vec2.y}");
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
