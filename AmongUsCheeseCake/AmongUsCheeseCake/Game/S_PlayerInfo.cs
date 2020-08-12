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
    public struct S_PlayerInfo
    {
        public static S_PlayerInfo FromBytes(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (S_PlayerInfo)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(S_PlayerInfo));
            gcHandle.Free();
            return data;
        }
        public static List<S_PlayerInfo> FromBytesList(byte[] bytes)
        {
            GCHandle gcHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var data = (List<S_PlayerInfo>)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(List<S_PlayerInfo>));
            gcHandle.Free();
            return data;
        }

        public static int SizeOf()
        {
            var size = Marshal.SizeOf(typeof(S_PlayerInfo)); ;
            return size;
        } 
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