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
    public struct S_Vector2
    {
        public float x,y;
        public static S_Vector2 FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (S_Vector2)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(S_Vector2));
            gcHandle.Free();
            return data;
        }
    }
}