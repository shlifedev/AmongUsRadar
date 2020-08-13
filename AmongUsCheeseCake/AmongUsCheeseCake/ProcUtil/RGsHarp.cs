using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ProcessUtil;

namespace RGsHarp
{

    /// <summary>
    /// The renegade process class. Offers some static methods like opening/waiting for the renegade process
    /// and member methods like ScriptCommands and engine calls
    /// </summary>
    public class Renegade
    {
        // member variables
        protected Process m_RenegadeProcess;
        protected ProcessMemory m_RenegadeProcessMemory;

        // public attributes and get/set
        public ScriptCommands ScriptCommands;

        // static values
        static string[] DefaultModuleName = {"game","game2"};

        #region Constructors
        /// <summary>
        /// Creates a new "Renegade" instance by process
        /// </summary>
        /// <param name="renegade"></param>
        public Renegade(Process renegade)
        {
            if (renegade != null)
            {
                this.m_RenegadeProcess = renegade;
                this.m_RenegadeProcessMemory = new ProcessMemory(this.m_RenegadeProcess);
                this.m_RenegadeProcessMemory.Open(ProcessAccess.AllAccess);

                ScriptCommands = new ScriptCommands(this.m_RenegadeProcessMemory);

            }
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Tries to find the Renegade-process by its default name. Returns 'Process'-instance or NULL if not found.
        /// </summary>
        /// <returns></returns>
        public static Process GetRenegadeProcess()
        {
            foreach (string name in Renegade.DefaultModuleName)
            {
                Process[] p = Process.GetProcessesByName(name);
                if (p.Length > 0)
                    return p[0];
            }

            return null;
        }

        /// <summary>
        /// Returns a 'Renegade'-instance if running or NULL if not.
        /// </summary>
        /// <param name="Wait">If TRUE it blocks until Renegade is found</param>
        /// <returns></returns>
        public static Renegade GetRenegadeProcess(bool Wait)
        {
            Process RenegadeProcess = null;
            Renegade ret = null;

            do
            {
                RenegadeProcess = Renegade.GetRenegadeProcess();
                if (RenegadeProcess == null && Wait)
                    System.Threading.Thread.Sleep(100); // wait for iiiiiiiiiit (barney stinson)
            }
            while (RenegadeProcess == null && Wait);

            if (RenegadeProcess!=null)
                ret = new Renegade(RenegadeProcess);

            return ret;
        }
        #endregion

        #region RGsHarp API / Public methods

        // the public API. most recent methods on top (also see changelog.txt)

        /// <summary>
        /// Gets a playerobject (cPlayer) player id
        /// </summary>
        /// <param name="playerId">id of the player</param>
        public int FindPlayer(int playerId)
        {
            return this.m_RenegadeProcessMemory.CallFunction(FunctionPointer.FindPlayer, (IntPtr)playerId);
        }

        /// <summary>
        /// Gets a gameobject by player id. Returns IntPtr.Zero on error
        /// </summary>
        /// <param name="playerID">id of the player</param>
        /// <returns></returns>
        public IntPtr Get_GameObj(int playerID)
        {
            IntPtr ret = IntPtr.Zero;

            // get player by id
            IntPtr cPlayer = (IntPtr) this.FindPlayer(playerID);
            
            // player instance (cplayer) found
            if (cPlayer != IntPtr.Zero)
            {
                // get gameobject
                ret = this.ReadPointer(cPlayer, 0x14); // cplayer
                ret = this.ReadPointer(ret, 4);
            }
            return ret;
        }


        /// <summary>
        /// Gets the team id (player type) of the given gameobject
        /// </summary>
        /// <param name="GameObject">Pointer to a gameobject</param>
        public Team GetTeam(IntPtr GameObject)
        {
            return (Team)this.ScriptCommands.Get_Player_Type(GameObject);
        }

        /// <summary>
        /// Get current radarmode
        /// </summary>
        public RadarMode GetRadarMode()
        {
            RadarMode ret = RadarMode.Default;

            byte[] rm = this.ReadMemory(FunctionPointer.RadarMode, 4);
            if (rm != null)
            {
                ret = (RadarMode) BitConverter.ToInt32(rm, 0);

            }

            return ret;
        }

        /// <summary>
        /// Set radarmode (WARNING: Overwrites radarmode in memory. DETECTED)
        /// </summary>
        /// <param name="radarMode">The radarmode to set</param>
        public bool SetRadarMode(RadarMode radarMode)
        {
            bool ret = false;

            byte[] toWrite = BitConverter.GetBytes((int)radarMode);
            ret = this.WriteMemory(FunctionPointer.RadarMode, toWrite);

            return ret;
        }

        /// <summary>
        /// Get the current player count
        /// </summary>
        /// <returns></returns>
        public int GetPlayerCount()
        {
            return (int) this.m_RenegadeProcessMemory.CallFunction(FunctionPointer.GetPlayerCount, IntPtr.Zero);
        }

        /// <summary>
        /// Returns the player id if in-game
        /// </summary>
        /// <returns></returns>
        public int GetLocalPlayerId()
        {
            int playerID = 0;

            // read localplayer pointer
            playerID = (int) this.ReadPointer(MemoryPointer.LocalPlayer);
            if (playerID > 0)
                playerID = (int)this.ReadPointer((IntPtr)playerID);

            return playerID;
        }

        /// <summary>
        /// Gets an array of all players (Player[]) currently in game
        /// </summary>
        public Player[] GetPlayers()
        {
            System.Collections.ArrayList playerList = new System.Collections.ArrayList();
            
            IntPtr node = this.ReadPointer(MemoryPointer.PlayerList,4); //SList->Headnode

            while (node != IntPtr.Zero)
            {
                Player player = new Player();

                IntPtr nodeData = this.ReadPointer(node, 4); // GenericSLNode->NodeData (cPlayer)
                
                // get the gameobject for the player
                IntPtr go = this.ReadPointer(nodeData, 0x14);
                go = this.ReadPointer(go, 4);

                if (go!=IntPtr.Zero)
                {
                    // Set the GameObject
                    player.GameObject = go;
                    
                    // Gets the player name
                    IntPtr pName = this.ReadPointer(nodeData, 0x758);
                    // we dont know the length of the player name so we just read 64bytes (so 32chars after unicode conversion)
                    // should be enough?!
                    byte[] name = this.ReadMemory(pName, 64);
                    // wide char (unicode) to string
                    string s = Encoding.Unicode.GetString(name);
                    player.Name = s.Remove(s.IndexOf('\0'));

                    // Get and save the player id
                    player.Id = (int)this.ReadPointer(nodeData, 0x75c);

                    // add player to arraylist
                    playerList.Add(player);
                }

                node = this.ReadPointer(node); // GenericSLNode->NodeNext;
            }

            return (Player[])playerList.ToArray(typeof(Player));
        }

        /// <summary>
        /// Returns TRUE if currently in game. Simply checks if the local player id is > 0
        /// </summary>
        /// <returns></returns>
        public bool InGame() { return (this.GetLocalPlayerId() > 0); }

        #endregion

        #region Internal methods

        protected byte[] ReadMemory(IntPtr address, int size)
        {
            if (this.m_RenegadeProcessMemory != null
                && this.m_RenegadeProcessMemory.Opened)
            {
                return this.m_RenegadeProcessMemory.Read(address, size);
            }
            
            return null;
        }

        protected IntPtr ReadPointer(IntPtr address)
        {
            return this.ReadPointer(address, 0);
        }

        protected IntPtr ReadPointer(IntPtr address, int offset)
        {
            if (this.m_RenegadeProcessMemory != null
                && this.m_RenegadeProcessMemory.Opened)
            {
                return this.m_RenegadeProcessMemory.ReadPointer(address, offset);
            }

            return IntPtr.Zero;
        }

        protected bool WriteMemory(IntPtr address, byte[] data)
        {
            bool ret = false;

            if (this.m_RenegadeProcessMemory != null
                && this.m_RenegadeProcessMemory.Opened)
            {
                ret = this.m_RenegadeProcessMemory.Write(address, data);
            }
            
            return ret;
        }

        #endregion

    }
}
