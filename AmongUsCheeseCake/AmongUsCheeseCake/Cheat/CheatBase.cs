
using AmongUsCheeseCake.Game;
using Binarysharp.MemoryManagement;
using Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmongUsCheeseCake.Cheat
{
    public class CheatBase
    {
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        static string PlayerControllPattern = "D8 03 A5 06 ?? ?? ?? ??";
        static string GameDataPattern = "A8 A4 B0 06 ?? ?? ?? ??";

        public static Mem Memory = new Mem();
        public static MemorySharp MemorySharp;

        private string m_cached_gameDataOffset = null;

        private List<CachedPlayerControllInfo> SearchedPlayerList = new List<CachedPlayerControllInfo>();
        public List<CachedPlayerControllInfo> RealPlayerInstance = new List<CachedPlayerControllInfo>();
        public int localNetworkID = 483328960;
        public RadarOverlay radar;
        private Thread tickThread = null;
        private Thread radarThread = null;




        List<CachedPlayerControllInfo> SearchPlayersWithoutMine()
        {

            List<CachedPlayerControllInfo> list = new List<CachedPlayerControllInfo>();
            var result = Memory.AoBScan(PlayerControllPattern, true, true);
            result.Wait();
            var results =    result.Result;
            foreach (var x in results)
            {
                var bytes = Memory.ReadBytes(x.GetAddress(), PlayerControll.SizeOf());
                var playerControll = PlayerControll.FromBytes(bytes);
                // 모든 플레이어의 공통 owner id
                if (playerControll.OwnerId == 257 && playerControll.netId != 0)
                {
                    Console.WriteLine("add Players :: " + playerControll.PlayerId);
                    list.Add(new CachedPlayerControllInfo()
                    {
                        Instance = playerControll,
                        offset = x.GetAddress(),
                        offset_ptr = new IntPtr((int)x)
                    });
                }
                else
                {

                }
            }
            return list;
        }


        List<PlayerControll> SearchAllPlayerInstance()
        {

            List<PlayerControll> list = new List<PlayerControll>();
            var result = Memory.AoBScan(PlayerControllPattern, true, true);
            result.Wait();
            var results =    result.Result;
            foreach (var x in results)
            {
                var bytes = Memory.ReadBytes(x.GetAddress(), PlayerControll.SizeOf());
                var playerControll = PlayerControll.FromBytes(bytes);
                list.Add(playerControll);
            }
            return list;
        }


        void FindAllRealPlayerInstance()
        {
            RealPlayerInstance.Clear();
            foreach (var x in SearchedPlayerList)
            {
                var vec2 = x.Instance.GetSyncPosition();
                RealPlayerInstance.Add(x);
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
                var bytes = Memory.ReadBytes(x.GetAddress(), GameData.SizeOf());
                var gameData = GameData.FromBytes(bytes);
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
            MemorySharp = new MemorySharp(Memory.GetProcIdFromName("Among Us")); 
            if (b)
            {
                if (tickThread != null)
                {
                    tickThread.Abort();
                    tickThread = null;
                    Console.WriteLine("thread suspend..");
                }


                if (tickThread == null)
                    tickThread = new Thread(Tick);

                if (radarThread == null)
                    radarThread = new Thread(Radar);
                this.RealPlayerInstance.Clear();
                this.SearchedPlayerList.Clear();



                tickThread.Start();
                if (radarThread.ThreadState == System.Threading.ThreadState.Unstarted)
                    radarThread.Start();
            }
        }

        /// <summary>
        /// 로깅 테스트
        /// </summary>
        public void UpdatePlayerPosition()
        {
            foreach (var x in RealPlayerInstance)
            {
                var test = x.Instance.GetSyncPosition();

                if (x.__updateSyncPosition.x == 0 && x.__updateSyncPosition.y == 0)
                {
                    x.__updateSyncPosition = test; 
                    continue;
                }
                else
                {
                    if ((x.__updateSyncPosition.x != test.x) || x.__updateSyncPosition.y != test.y)
                        x.isOther = true;  
                }
            }
        }



        public void ShowRadar()
        {

        }
        public void Tick()
        {
            Console.WriteLine("Start Tick Thread!");
            SearchedPlayerList = SearchPlayersWithoutMine();
            FindAllRealPlayerInstance();

            var proc = Process.GetProcessesByName("Among Us");
            bool test_rect = false;
            if (proc != null)
            {

            }
            while (true)
            {
                if (test_rect)
                {
                    Process lol = proc[0];
                    IntPtr ptr = lol.MainWindowHandle;
                    Rect rect = new Rect();
                    GetWindowRect(ptr, ref rect);
                    radar.SetWindowPos(rect.Left + 10, rect.Top + 30);
                }
                UpdatePlayerPosition();
                System.Threading.Thread.Sleep(10);
            }
        }
        public void Radar()
        {
            Console.WriteLine("Start Radar Thread!");
            radar = new RadarOverlay();
            radar.cb = this;
            radar.Run();
        }


        public GameData ReadGameData()
        {
            try
            {
                return GameData.FromBytes(Memory.ReadBytes(m_cached_gameDataOffset, GameData.SizeOf()));
            }
            catch
            {
                return null;
            }
        }
    }
}