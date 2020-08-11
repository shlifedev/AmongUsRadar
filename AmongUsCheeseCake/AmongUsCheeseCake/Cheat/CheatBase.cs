
using AmongUsCheeseCake.Game;
using Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmongUsCheeseCake.Cheat
{
    public class CheatBase
    {
        static string PlayerControllPattern = "48 52 06 11 ?? ?? ?? ??";
        static string GameDataPattern = "A8 A4 B0 06 ?? ?? ?? ??";

        public static Mem Memory = new Mem();


        /// <summary>
        /// 플레이어 위치정보
        /// </summary>
        public List<CachedPlayerControllInfo> playersPositions = new List<CachedPlayerControllInfo>();  


        private string m_cached_gameDataOffset = null;

        private List<S_PlayerControll> S_PlayerControllList = new List<S_PlayerControll>();
        private Dictionary<int, Vector2> UpdatedVectorDictionary = new Dictionary<int, Vector2>();
        private Thread tickThread = null;
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


         
        void UpdatePlayerList()
        {
            int idx = 0;
            foreach (var x in S_PlayerControllList)
            {
                var vec2 = x.GetSyncPosition();
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
                        if (originalData.x != currentVec.x || originalData.y != currentVec.y)
                        {
                            Console.WriteLine("Player ID : " + x.PlayerId + "    X " + currentVec.x + ", Y " + currentVec.y + ",  " + x.NetTransform);
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
                tickThread = new Thread(Tick);
                this.UpdatedVectorDictionary.Clear();
                this.S_PlayerControllList.Clear();
                this.S_PlayerControllList = SearchPlayerInfoList();
                tickThread.Start();
            }
        }
        public void Tick()
        {
            
            while(true)
            {
                UpdatePlayerList();
                System.Threading.Thread.Sleep(10);
            }
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