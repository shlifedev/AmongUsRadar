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
    public struct Vector2
    {
        public float x,y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsZero()
        {
            if(this.x == 0 && this.y == 0) return true;
            else return false;
        }
        public static Vector2 Zero
        {
            get
            {
                return new Vector2(0,0);
            }
        }

        public static Vector2 FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned); 
            var data = (Vector2)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(Vector2));
            gcHandle.Free();
            return data;
        }

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(Vector2)); ;
            return size;
        }
    }
}