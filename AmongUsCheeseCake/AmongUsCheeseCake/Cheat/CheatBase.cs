
using AmongUsCheeseCake.Game;
using Binarysharp.MemoryManagement;
using Memory;
using ProcessUtil;
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
    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }


    public class CheatBase
    {
        #region externs
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        #endregion

        #region singleton
        public static CheatBase Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new CheatBase();
                }
                return m_instance;
            }
        }
        private static CheatBase m_instance;
        #endregion


        #region memory_tools
        public static Mem Memory = new Mem();
        public static MemorySharp MemorySharp;
        public static ProcessMemory ProcessMemory;
        #endregion


        #region readed_memorys
        private List<CachedPlayerControllInfo> SearchedPlayerList = new List<CachedPlayerControllInfo>();
        public List<CachedPlayerControllInfo> RealPlayerInstance = new List<CachedPlayerControllInfo>();
        #endregion


        #region threads
        private Thread tickThread = null;
        private Thread radarThread = null;
        #endregion


        public List<CachedPlayerControllInfo> SearchAllPlayers()
        {

            List<CachedPlayerControllInfo> list = new List<CachedPlayerControllInfo>();
            var result = Memory.AoBScan(EngineOffset.Pattern.PlayerControl, true, true);
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
            }
            return list;
        }


        public void FindAllRealPlayerInstance()
        {
            RealPlayerInstance.Clear();
            foreach (var x in SearchedPlayerList)
            {
                var vec2 = x.Instance.GetSyncPosition();
                RealPlayerInstance.Add(x);
            }
        } 


        public void Init()
        {
     
            var b = Memory.OpenProcess("Among Us");
            MemorySharp = new MemorySharp(Memory.GetProcIdFromName("Among Us")); 
            Process proc = Process.GetProcessesByName("Among Us")[0];
            ProcessMemory = new ProcessMemory(proc);
            ProcessMemory.Open(ProcessAccess.AllAccess);
   

             
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

                if (x.syncPos.x == 0 && x.syncPos.y == 0)
                {
                    x.syncPos = test; 
                    continue;
                }
                else
                {
                    if ((x.syncPos.x != test.x) || x.syncPos.y != test.y)
                        x.isOther = true;  
                }
            }
             
            CachedPlayerControllInfo pc = null;
            int cnt = 0;
            for(int i = 0; i< RealPlayerInstance.Count; i++ )
            {
                if (RealPlayerInstance[i].isOther)
                {
                    cnt++;
                    continue;
                }
                else
                {
                    pc = RealPlayerInstance[i];
                }
            } 
            if (cnt == RealPlayerInstance.Count-1)
            {
                pc.isMine = true; 
            }
        }

 
        public void Tick()
        {
            Console.WriteLine("Start Tick Thread!");
            SearchedPlayerList = SearchAllPlayers();
            FindAllRealPlayerInstance();

            var proc = Process.GetProcessesByName("Among Us");

            bool test_rect = true;
            if (proc != null)
            {

            }
            while (true)
            { 
                if (test_rect)
                { 
                    if (MemorySharp.Windows.MainWindow.IsActivated)
                    {
                        Process lol = proc[0];
                        IntPtr ptr = lol.MainWindowHandle;
                        Rect rect = new Rect();
                        GetWindowRect(ptr, ref rect);
                        RadarOverlay.Instance.SetWindowPos(rect.Left + 9, rect.Top + 31);
                        RadarOverlay.Instance.drawDisable = false;  
                    }
                    else
                    { 
                        RadarOverlay.Instance.drawDisable = true;
                    } 
                }


                UpdatePlayerPosition();
                foreach(var x in RealPlayerInstance)
                {
                    x.ObserveState();
                }


                System.Threading.Thread.Sleep(10); 
            }
        }
        public void Radar()
        {
            Console.WriteLine("Start Radar Thread!");
            RadarOverlay.Instance.Run();
        }

         
    }
}