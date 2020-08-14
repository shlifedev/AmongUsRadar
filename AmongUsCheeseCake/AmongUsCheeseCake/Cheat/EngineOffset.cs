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
            /// Get PlayerControl Class 
            /// </summary>
            public static string PlayerControl = "F8 84 D7 0F ?? ?? ?? ??"; 

            /// <summary>
            /// Get PlayerControl.Get_Data();
            /// </summary>
            public static string PlayerControl_GetData = "55 8B EC 80 3D 58 06 ??";
        } 
    }
}