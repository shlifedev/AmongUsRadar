/*
 * EnginePointer.cs
 * All engine pointers here, please! :)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGsHarp
{
    /// <summary>
    /// Static renegade function pointers as of Renegade v1.037
    /// </summary>
    sealed class FunctionPointer
    {
        public static IntPtr GameType = (IntPtr)0x00856518;
        public static IntPtr RadarMode = (IntPtr)0x811218;
        public static IntPtr GetPlayerCount = (IntPtr)0x00417040;
        public static IntPtr FindPlayer = (IntPtr)0x004157E0;

    }

    /// <summary>
    /// Static renegade memory pointers as of Renegade v1.037
    /// </summary>
    static class MemoryPointer
    {
        public static IntPtr LocalPlayer = (IntPtr)0x00820D98;
        public static IntPtr PlayerList = (IntPtr)0x0081CFE8;
    }
}
