
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

        public System.Action onInit;


        public List<CachedPlayerControllInfo> SearchAllPlayers()
        {
            // create cached player data list
            List<CachedPlayerControllInfo> list = new List<CachedPlayerControllInfo>();

            // find player pointer
            byte[] playerAoB = Memory.ReadBytes(EngineOffset.Pattern.PlayerControl_Pointer, PlayerControll.SizeOf());
      
            
            int cnt = 0;
            // aob pattern
            string aobData = "";

            // read 4byte aob pattern.
            foreach (var _byte in playerAoB)
            { 
                if(_byte < 16) 
                    aobData += "0"+_byte.ToString("X"); 
                else 
                    aobData += _byte.ToString("X"); 
              
                if(cnt+1 != 4) 
                    aobData += " ";

                cnt++;
                if (cnt == 4)
                {
                    aobData += " ?? ?? ?? ??"; 
                    break;
                } 
            } 
            // get result 
            var result = Memory.AoBScan(aobData, true, true);
            result.Wait();
            var results =    result.Result;
            
            // player
            foreach (var x in results)
            {
                var bytes = Memory.ReadBytes(x.GetAddress(), PlayerControll.SizeOf());
                var playerControll = PlayerControll.FromBytes(bytes);
                // among us real instanced player ownerid is 257 :)
                if (playerControll.OwnerId == 257 && playerControll.netId != 0)
                {
                    Logger.Log("Add Player :: " + playerControll.PlayerId);
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


        /// <summary>
        /// init real player list.
        /// </summary>
        public void InitRealPlayerList()
        {
            RealPlayerInstance.Clear();
            foreach (var x in SearchedPlayerList) 
                RealPlayerInstance.Add(x); 
        } 

       

        public void Init()
        {
     
            var b = Memory.OpenProcess("Among Us");
            MemorySharp = new MemorySharp(Memory.GetProcIdFromName("Among Us")); 
            Process proc = Process.GetProcessesByName("Among Us")[0];
            ProcessMemory = new ProcessMemory(proc);
            ProcessMemory.Open(ProcessAccess.AllAccess);
            onInit?.Invoke(); 

            if (b)
            {
                // remove tick thread
                if (tickThread != null)
                {
                    tickThread.Abort();
                    tickThread = null;
                    Logger.Log("thread suspend..");
                }

                // create thread
                if (tickThread == null)
                    tickThread = new Thread(Tick);

                // create thread
                if (radarThread == null)
                    radarThread = new Thread(Radar);

                // clear list.
                this.RealPlayerInstance.Clear();
                this.SearchedPlayerList.Clear();

                 
                tickThread.Start();
                if (radarThread.ThreadState == System.Threading.ThreadState.Unstarted)
                    radarThread.Start();
            }
        }
         
        public void FilterPlayerOwner()
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
                    {
                        if (x.isMine == false)
                        {
                            x.isOther = true;
                        }
                    }
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
                    pc = RealPlayerInstance[i]; 
            } 
            if (cnt == RealPlayerInstance.Count-1) 
                pc.isMine = true;   
        }

 


        public void Tick()
        {
            Logger.Log("Start Tick Thread!"); 
            SearchedPlayerList = SearchAllPlayers();
            InitRealPlayerList();

            RadarOverlay.Instance.Init();

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


                FilterPlayerOwner();
                                        

                System.Threading.Thread.Sleep(10); 
            }
        }
        public void Radar()
        {
            Logger.Log("Start Radar Thread!");
            RadarOverlay.Instance.Run();
        }

         
    }
}