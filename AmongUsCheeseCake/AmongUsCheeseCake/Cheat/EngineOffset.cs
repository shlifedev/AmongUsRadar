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
            /// <summary>
            /// Get PlayerControl
            /// </summary>
            public static string PlayerControl_Pointer = "GameAssembly.dll+E22AE8";  //GameAssembly.dll+E22AE8
            /// <summary>
            /// Get PlayerControl.Get_Data();
            /// </summary>
            public static string PlayerControl_GetData = "55 8B EC 80 3D 58 06 ??";
        } 
    }
}