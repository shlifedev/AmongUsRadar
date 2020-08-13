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
    public class GameData : InnerNetObject
    {
        public static GameData FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (GameData)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(GameData));
            gcHandle.Free();
            return data;
        }

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(GameData)); ;
            return size;
        }
         
        public UIntPtr AllPlayers;  
        public int TotalTasks; 
        public int CompletedTasks; 
    }
}