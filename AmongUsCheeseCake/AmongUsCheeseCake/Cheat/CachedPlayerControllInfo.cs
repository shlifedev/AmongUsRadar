
using AmongUsCheeseCake.Game;
using Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace AmongUsCheeseCake.Cheat
{
    public class CachedPlayerControllInfo
    {
        public string offset;
        public IntPtr offset_ptr;
        public S_PlayerControll Instance; 
        public bool isOther = false;
        public bool isMine;
        public bool isImposter = false; 
        public Vector2 __updateSyncPosition = Vector2.Zero;  
        public void ReadMemory()
        {
            
            Instance = S_PlayerControll.FromBytes(CheatBase.Memory.ReadBytes(offset, S_PlayerControll.SizeOf()));
            if(Instance.inVent == 1) 
                isImposter = true;
        }

        #region


        static IntPtr __getDataAddress = new IntPtr(0x54615DF0); 
        public IntPtr __getData()
        { 
            return CheatBase.MemorySharp.Assembly.Execute<IntPtr>(__getDataAddress, Binarysharp.MemoryManagement.Assembly.CallingConvention.CallingConventions.Thiscall, offset_ptr);
        }
        static IntPtr __getVisibleAddress = new IntPtr(0x54615E90);
        public bool __getVisible()
        {
            return CheatBase.MemorySharp.Assembly.Execute<bool>(__getVisibleAddress, Binarysharp.MemoryManagement.Assembly.CallingConvention.CallingConventions.Fastcall, offset_ptr);
        }
    

        public void __setKillTimer()
        {

        }


        #endregion
    }
}