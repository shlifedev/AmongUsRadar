
using AmongUsCheeseCake.Game;
using Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AmongUsCheeseCake.Cheat
{
    public class CachedPlayerControllInfo
    {
        public string offset;
        public S_PlayerControll Instance; 
        public bool isOther = false;
        public bool isMine;
        public bool isImposter = false; 
        public Vector2 __updateSyncPosition = Vector2.Zero;  
        public void ReadMemory()
        {
            Instance = S_PlayerControll.FromBytes(CheatBase.Memory.ReadBytes(offset, S_PlayerControll.SizeOf()));
            if(Instance.inVent == 0)
                isImposter = true;
        } 
    }
}