
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
        public PlayerControll Instance; 
        public bool isOther = false;
        public bool isMine; 
        public Vector2 __updateSyncPosition = Vector2.Zero;  


    
        private string playerInfoOffset = null;
        public PlayerInfo? PlayerInfo
        {
            get
            {
                if(m_pInfo == null)
                {
                    var scanTask = CheatBase.Memory.AoBScan(EngineOffset.Pattern.PlayerControl_GetData);
                        scanTask.Wait();

                    Console.WriteLine("PlayerControl.GetData() Scan Count => " + scanTask.Result.Count());

                    if(scanTask.Result.Count() == 1)
                    { 
                        var ptr = (IntPtr)scanTask.Result.First();
                        var playerInfoAddress = CheatBase.ProcessMemory.CallFunction(ptr, this.offset_ptr).GetAddress();
                            playerInfoOffset = playerInfoAddress;
                        PlayerInfo pInfo = Game.PlayerInfo.FromBytes(CheatBase.Memory.ReadBytes(playerInfoAddress, Game.PlayerInfo.SizeOf()));
                        this.m_pInfo = pInfo;
                        Console.WriteLine("PlayerControl.GetData() Scan Complete");
                    }
                }
                else
                {
                    PlayerInfo pInfo = Game.PlayerInfo.FromBytes(CheatBase.Memory.ReadBytes(playerInfoOffset, Game.PlayerInfo.SizeOf()));
                    this.m_pInfo = pInfo; 
                } 
                return m_pInfo;
            }
        }

        private PlayerInfo? m_pInfo = null;
        public void ReadMemory()
        { 
            Instance = PlayerControll.FromBytes(CheatBase.Memory.ReadBytes(offset, PlayerControll.SizeOf())); 
        }

        #region


    

        public void __setKillTimer()
        {

        }


        #endregion
    }
}