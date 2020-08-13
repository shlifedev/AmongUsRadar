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
    public struct PlayerInfo
    {
        public static PlayerInfo FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (PlayerInfo)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(PlayerInfo));
            gcHandle.Free();
            return data;
        } 

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(PlayerInfo)); 
            Console.WriteLine(size);
            return size;
        }
        public IntPtr test,test2;
        public byte PlayerId;
        public UIntPtr PlayerName;
        public byte ColorId;
        public uint HatId;
        public uint PetId;
        public uint SkinId;
        public bool Disconnected;
        public UIntPtr Tasks;
        public bool IsImpostor;
        public bool IsDead;
        private UIntPtr _object;
    }
}