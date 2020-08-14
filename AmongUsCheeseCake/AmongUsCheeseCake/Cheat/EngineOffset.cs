 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AmongUsCheeseCake.Cheat
{
    public class EngineOffset
    {
        public sealed class Pattern
        { 
            public static string PlayerControl_Pointer = "";  //GameAssembly.dll+E22AE8
            /// <summary>
            /// Get PlayerControl.Get_Data();
            /// </summary>
            public static string PlayerControl_GetData = "";
        } 
    }
}