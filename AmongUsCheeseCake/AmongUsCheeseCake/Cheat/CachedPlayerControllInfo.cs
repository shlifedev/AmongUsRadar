
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
        /// <summary>
        /// Position, ColorID  
        /// </summary>
        public System.Action<Vector2, byte> onDie;


        private string playerInfoOffset = null;
        public IntPtr playerInfoOffset_ptr;

        public string offset;
        public IntPtr offset_ptr;
        public PlayerControll Instance; 
        public bool isOther = false;
        public bool isMine; 
        public Vector2 syncPos = Vector2.Zero; 
        private bool observe_dieFlag = false;

   

        public Vector2 Position
        {
            get
            {
                if (isMine) return Instance.GetMyPosition();
                else
                {
                    return Instance.GetSyncPosition();
                }
            }
        }


        public void ObserveState()
        {
            if(observe_dieFlag == false && PlayerInfo.Value.IsDead == 1)
            {
                observe_dieFlag = true;
                onDie?.Invoke(Position, PlayerInfo.Value.ColorId);
            }
        }


        public void WM_SetImposter(byte x)
        {
            var xxx = playerInfoOffset_ptr.ToInt32() + 40;
            Console.WriteLine(xxx.GetAddress());
            CheatBase.Memory.WriteMemory(xxx.GetAddress(), "byte", x.ToString());
        }
        public void WM_SetDead(byte x)
        {
            var xxx = playerInfoOffset_ptr.ToInt32() + 41;
            Console.WriteLine(xxx.GetAddress());
            CheatBase.Memory.WriteMemory(xxx.GetAddress(), "byte", x.ToString());
        }
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
                        var playerInfoAddress = CheatBase.ProcessMemory.CallFunction(ptr, this.offset_ptr);
                            playerInfoOffset = playerInfoAddress.GetAddress();
                            playerInfoOffset_ptr = (IntPtr)playerInfoAddress;


                        PlayerInfo pInfo = Game.PlayerInfo.FromBytes(CheatBase.Memory.ReadBytes(playerInfoOffset, Game.PlayerInfo.SizeOf()));
                        if(pInfo.IsDead == 1)
                            observe_dieFlag = true;


                        this.m_pInfo = pInfo;
                        Console.WriteLine("PlayerControl.GetData() Scan Complete  " + playerInfoOffset);
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