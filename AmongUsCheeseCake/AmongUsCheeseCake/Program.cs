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
    class Program
    {
        /*Memory Ref https://github.com/shlifedev/memory.dll*/
        static public Mem m = new Mem();  
        static void Main(string[] args)
        {
            unsafe
            {
                while (true)
                {
                    var size = Marshal.SizeOf(typeof(S_PlayerControll)); 
                    if (m.OpenProcess("Among us"))
                    {  
                        var data = m.ReadBytes("Among us.exe+06854D20", size);
                        var info = S_PlayerControll.FromBytes(data);

                        Console.WriteLine(info.killTimer);
                    }
                }
            }
        }
    }
}
