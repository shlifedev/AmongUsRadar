
using AmongUsCheeseCake.Game;
using Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmongUsCheeseCake.Cheat
{
    public class CheatBase
    {
        static string PlayerControllPattern = "C0 35 69 10 ?? ?? ?? ??";
        static string GameDataPattern = "A8 A4 B0 06 ?? ?? ?? ??";

        public static Mem Memory = new Mem();




        private string m_cached_gameDataOffset = null;

        private List<S_PlayerControll> SearchedPlayerList = new List<S_PlayerControll>();
        public List<S_PlayerControll> RealPlayerInstance = new List<S_PlayerControll>();
        private Dictionary<int, S_PlayerControll> RealPlayerInstancePID = new Dictionary<int, S_PlayerControll>();
        private Dictionary<int, Vector2> UpdatedVectorDictionary = new Dictionary<int, Vector2>();
        public int localPID = -999;

        private Thread tickThread = null;
        private Thread radarThread = null;




        List<S_PlayerControll> SearchPlayerInfoList()
        {
            List<S_PlayerControll> list = new List<S_PlayerControll>();
            var result = Memory.AoBScan(PlayerControllPattern, true, true);
            result.Wait();
            var results =    result.Result;
            foreach (var x in results)
            {
                var bytes = Memory.ReadBytes(x.GetAddress(), S_PlayerControll.SizeOf());
                var playerControll = S_PlayerControll.FromBytes(bytes);
                list.Add(playerControll);
            }
            return list;
        }


        /// <summary>
        /// 2회이상 실행해야 찾을 수 있음
        /// </summary>
        void UpdatePlayerList()
        {
            int idx = 0;
            foreach (var x in SearchedPlayerList)
            {  
                var vec2 = x.GetSyncPosition();
                if ((vec2.x <= 300 && vec2.y <= 300))
                { 
                    if (vec2.IsZero() == false && vec2.IsGarbage() == false)
                    {
                        if (UpdatedVectorDictionary.ContainsKey(idx) == false)
                        {
                            UpdatedVectorDictionary.Add(idx, vec2);
                        }
                        else
                        {
                            var originalData = UpdatedVectorDictionary[idx];
                            var currentVec = vec2;
                            if (originalData.x != currentVec.x || originalData.y != currentVec.y)
                            {
                                if (x.OwnerId == 257)
                                {
                                    if (RealPlayerInstancePID.ContainsKey(x.PlayerId) == false)
                                    {
                                        RealPlayerInstance.Add(x);
                                        RealPlayerInstancePID.Add(x.PlayerId, x);
                                    }
                                }
                            }
                        }
                    }  
                }
                idx++;
            }
        }

        /// <summary>
        /// 2회이상 실행해야 찾을 수 있음
        /// </summary>
        void UpdateMyPlayer()
        {
            int idx = 2000;
            foreach (var x in SearchedPlayerList)
            {
                var vec2 = x.GetMyPosition();  
                if (vec2.IsZero() == false)
                {
                    if (UpdatedVectorDictionary.ContainsKey(idx) == false)
                    { 
                        UpdatedVectorDictionary.Add(idx, vec2);
                    }
                    else
                    {
                        var originalData = UpdatedVectorDictionary[idx];
                        var currentVec = vec2;
                        if (originalData.x != currentVec.x)
                        {
                            if (RealPlayerInstancePID.ContainsKey(x.PlayerId) == false)
                            { 
                                localPID = x.PlayerId;
                                RealPlayerInstance.Add(x);
                                RealPlayerInstancePID.Add(x.PlayerId, x); 
                            } 
                        }
                    }
                }
                idx++;
            }
        }




        /// <summary>
        /// 게임데이터 오프셋을 새로고침함
        /// </summary>
        public void RefreshGameDataOffset()
        {
            string offset = null;
            var result = Memory.AoBScan(GameDataPattern, true, true);
            result.Wait();
            var results = result.Result;

            foreach (var x in results)
            {
                var bytes = Memory.ReadBytes(x.GetAddress(), S_GameData.SizeOf());
                var gameData = S_GameData.FromBytes(bytes);
                // OWNER ID가 -2이고, NetId가 4294967295가 아닌 객체는 실제 인스턴스이다.
                // 4294967295(uint의 max값)은, 이미 인스턴스가 해제된 가비지값을 가리킴
                if (gameData.OwnerId == -2 && gameData.NetId != 4294967295)
                    offset = x.GetAddress();
            }
            this.m_cached_gameDataOffset = offset;
        }

        public void Init()
        { 
            var b = Memory.OpenProcess("Among Us");
            if (b)
            {
                if (tickThread != null)
                {
                    tickThread.Interrupt();
                    tickThread = null;
                }
                if (radarThread != null)
                {
                    radarThread.Interrupt();
                    radarThread = null;
                }




                localPID = -999;
                tickThread = new Thread(Tick);
                radarThread = new Thread(Radar);
                this.UpdatedVectorDictionary.Clear();
                this.RealPlayerInstance.Clear();
                this.RealPlayerInstancePID.Clear();
                this.SearchedPlayerList.Clear();
                this.SearchedPlayerList = SearchPlayerInfoList(); 




                tickThread.Start();
                radarThread.Start();
            }
        }

        public void UpdatePlayerPosition()
        {
            foreach (var x in RealPlayerInstance)
            { 
                if (localPID == x.PlayerId)
                {
                    var currentVec = x.GetMyPosition();
                    Console.WriteLine("My Player ID : " + x.PlayerId + "    X " + currentVec.x + ", Y " + currentVec.y + ",  " + x.NetTransform);
                } 
                else
                {
                    var currentVec = x.GetSyncPosition();
                    Console.WriteLine($"Player ID : {x.PlayerId}  Net ID : {x.netId}  Owner ID : {x.OwnerId}    ({currentVec.x.ToString("0.0")},{currentVec.y.ToString("0.0")})");
                }
             
            }
        }


        
        public void ShowRadar()
        {

        }
        public void Tick()
        {
            Console.WriteLine("Tick Thread!");
            while (true)
            {
                UpdatePlayerList(); 
                UpdateMyPlayer();
                UpdatePlayerPosition(); 
                System.Threading.Thread.Sleep(10);
            }
        }
        public void Radar()
        {
            Console.WriteLine("Radar Thread!");
            RadarOverlay rd = new RadarOverlay();
            rd.cb = this;
            rd.Run();
        }


        public S_GameData ReadGameData()
        {
            try
            {
                return S_GameData.FromBytes(Memory.ReadBytes(m_cached_gameDataOffset, S_GameData.SizeOf()));
            }
            catch
            {
                return null;
            }
        }
    }
}