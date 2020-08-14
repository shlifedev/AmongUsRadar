
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

        /// <summary>
        /// get_Data()의 method pointer
        /// </summary>
        public static IntPtr PlayerControl_GetData_Offset = IntPtr.Zero;

        /// <summary>
        /// 플레이어 Info의 Offset
        /// </summary>
        private string playerInfoOffset = null;
        /// <summary>
        /// 플레이어 Info의 Offset Pointer
        /// </summary>
        public IntPtr playerInfoOffset_ptr;

        /// <summary>
        /// 플레이어 인스턴스의 오프셋
        /// </summary>
        public string offset;
        /// <summary>
        /// 플레이어 인스턴스의 포인터
        /// </summary>
        public IntPtr offset_ptr;


        public PlayerControll Instance; 

        /// <summary>
        /// other 플레이어의 경우 true
        /// </summary>
        public bool isOther = false;
        /// <summary>
        /// 나 자신일 경우 true
        /// </summary>
        public bool isMine;  

        public Vector2 syncPos = Vector2.Zero; 

 
        /// <summary>
        /// PlayerInfo 가져오기 
        /// </summary>
        public PlayerInfo? PlayerInfo
        {
            get
            {
                if (m_pInfo == null)
                { 
                    if (PlayerControl_GetData_Offset == IntPtr.Zero)
                    {
                        var aobScan = CheatBase.Memory.AoBScan(EngineOffset.Pattern.PlayerControl_GetData);

                        aobScan.Wait();
                        if (aobScan.Result.Count() == 1)
                            PlayerControl_GetData_Offset = (IntPtr)aobScan.Result.First(); 
                    }

                    var scanTask = PlayerControl_GetData_Offset;


                    if (PlayerControl_GetData_Offset != IntPtr.Zero)
                    {
                        var ptr = PlayerControl_GetData_Offset;
                        var playerInfoAddress = CheatBase.ProcessMemory.CallFunction(ptr, this.offset_ptr);
                        playerInfoOffset = playerInfoAddress.GetAddress();
                        playerInfoOffset_ptr = (IntPtr)playerInfoAddress;


                        PlayerInfo pInfo = Game.PlayerInfo.FromBytes(CheatBase.Memory.ReadBytes(playerInfoOffset, Game.PlayerInfo.SizeOf()));
                        if (pInfo.IsDead == 1)
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

        /// <summary>
        /// 해당 플레이어 현재위치 가져오기
        /// </summary>
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


#region ObserveStates
        private bool observe_dieFlag = false;
#endregion
        /// <summary>
        /// State변경 감지
        /// </summary>
        public void ObserveState()
        {
            if (PlayerInfo.HasValue)
            {
                if (observe_dieFlag == false && PlayerInfo.Value.IsDead == 1)
                {
                    observe_dieFlag = true;
                    onDie?.Invoke(Position, PlayerInfo.Value.ColorId);
                }
            }
        } 

        public void WriteMemory_Imposter(byte value)
        {
            var targetPointer = playerInfoOffset_ptr.Sum(40);
            Console.WriteLine(" PlayerInfo.IsImposter Offset => " + targetPointer.GetAddress());
            CheatBase.Memory.WriteMemory(targetPointer.GetAddress(), "byte", value.ToString());
        }
        public void WriteMemory_IsDead(byte value)
        {
            var targetPointer = playerInfoOffset_ptr.Sum(41);
            Console.WriteLine(" PlayerInfo.IsDead Offset => " + targetPointer.GetAddress());
            CheatBase.Memory.WriteMemory(targetPointer.GetAddress(), "byte", value.ToString());
        } 
        /// <summary>
        /// 킬 쿨타임 초기화
        /// </summary>
        /// <param name="value"></param>
        public void WriteMemory_KillTimer(float value)
        {
            var targetPointer = offset_ptr.Sum(44);
            Console.WriteLine(" PlayerControl.KillTimer Offset => " + targetPointer.GetAddress());
            CheatBase.Memory.WriteMemory(targetPointer.GetAddress(), "float", value.ToString());
        }
        public void ReadMemory()
        {
            Instance = PlayerControll.FromBytes(CheatBase.Memory.ReadBytes(offset, PlayerControll.SizeOf()));
        } 
    }
}