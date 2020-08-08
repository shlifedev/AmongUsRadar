using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AmongUsCheeseCake.Game
{
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class S_GameData : S_InnerNetObject
    {
        public static S_GameData FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (S_GameData)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(S_GameData));
            gcHandle.Free();
            return data;
        }

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(S_GameData)); ;
            return size;
        }
         
        public UIntPtr AllPlayers;  
        public int TotalTasks; 
        public int CompletedTasks; 
    }
}