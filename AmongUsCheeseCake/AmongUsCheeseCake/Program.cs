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
                    var x = m.AoBScan("?? ?? ?? 0A ?? ?? 0F");
                    x.Wait();
                    var result = x.Result;

                    foreach(var m in result)
                    {
                        Console.WriteLine(result.Count());
                    }
                    Console.WriteLine(x);
                }
            }
        }
    }
}
