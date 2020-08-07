using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
namespace AmongUsCheeseCake
{
    class Program
    {
        /*Memory Ref https://github.com/shlifedev/memory.dll*/
        static public Mem m = new Mem();  
        static void Main(string[] args)
        {
            while (true)
            {

                if (m.OpenProcess("Among us"))
                { 
                    var x = m.AoBScan("00 30 42 1A ?? ?? ?? ??");  
                    x.Wait();

                    foreach(var m in x.Result)
                    {
                        Console.WriteLine(m);
                    } 
                }
            }
        }
    }
}
