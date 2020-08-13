using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Principal;
using System.Runtime.InteropServices;

namespace ProcessUtil
{
    [Flags()]
    public enum ProcessAccess : int
    {
        /// <summary>Specifies all possible access flags for the process object.</summary>
        AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead | VMWrite | Synchronize,
        /// <summary>Enables usage of the process handle in the CreateRemoteThread function to create a thread in the process.</summary>
        CreateThread = 0x2,
        /// <summary>Enables usage of the process handle as either the source or target process in the DuplicateHandle function to duplicate a handle.</summary>
        DuplicateHandle = 0x40,
        /// <summary>Enables usage of the process handle in the GetExitCodeProcess and GetPriorityClass functions to read information from the process object.</summary>
        QueryInformation = 0x400,
        /// <summary>Enables usage of the process handle in the SetPriorityClass function to set the priority class of the process.</summary>
        SetInformation = 0x200,
        /// <summary>Enables usage of the process handle in the TerminateProcess function to terminate the process.</summary>
        Terminate = 0x1,
        /// <summary>Enables usage of the process handle in the VirtualProtectEx and WriteProcessMemory functions to modify the virtual memory of the process.</summary>
        VMOperation = 0x8,
        /// <summary>Enables usage of the process handle in the ReadProcessMemory function to' read from the virtual memory of the process.</summary>
        VMRead = 0x10,
        /// <summary>Enables usage of the process handle in the WriteProcessMemory function to write to the virtual memory of the process.</summary>
        VMWrite = 0x20,
        /// <summary>Enables usage of the process handle in any of the wait functions to wait for the process to terminate.</summary>
        Synchronize = 0x100000
    }

    public static class CurrentUser
    {
        public static WindowsIdentity Identity = WindowsIdentity.GetCurrent();

        /// <summary>
        /// Returns if the current user (or rather current process context) has administrative priviledges
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            bool isAdmin = false;
            
            WindowsPrincipal wp = new WindowsPrincipal(CurrentUser.Identity);
            isAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            return isAdmin;
        }
    }

    public class ProcessMemory
    {
        #region WINAPI definitions

        [Flags]
        public enum AllocationType
        {
             Commit = 0x1000,
             Reserve = 0x2000,
             Decommit = 0x4000,
             Release = 0x8000,
             Reset = 0x80000,
             Physical = 0x400000,
             TopDown = 0x100000,
             WriteWatch = 0x200000,
             LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
             Execute = 0x10,
             ExecuteRead = 0x20,
             ExecuteReadWrite = 0x40,
             ExecuteWriteCopy = 0x80,
             NoAccess = 0x01,
             ReadOnly = 0x02,
             ReadWrite = 0x04,
             WriteCopy = 0x08,
             GuardModifierflag = 0x100,
             NoCacheModifierflag = 0x200,
             WriteCombineModifierflag = 0x400
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int Type;
        }
        #endregion

        #region WINAPI DLL-IMPORTS
        /// <summary>
        /// Opens an existing local process object.
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the process object. This access right is checked against the security descriptor for the process. This parameter can be one or more of the ProcessAccess-Flags</param>
        /// <param name="bInheritHandle">If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.</param>
        /// <param name="dwProcessId">The identifier of the local process to be opened. </param>
        /// <returns>If the function succeeds, the return value is an open handle to the specified process. If the function fails, the return value is NULL.</returns>
        /// Source: MSDN ( http://msdn.microsoft.com/en-us/library/ms684320%28VS.85%29.aspx )
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
        [DllImport("kernel32.dll")]
        static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);
        
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);
        
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, AllocationType dwFreeType);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, uint lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

        #endregion

        #region CONSTRUCTOR
        public ProcessMemory()
        {
            this.m_Process = new Process();
            this.m_ProcessAccess = 0;
        }
        public ProcessMemory(string processName) : this()
        {
            this.m_Process = Process.GetProcessesByName(processName)[0];
        }
        public ProcessMemory(int processId)
            : this()
        {
            this.m_Process = Process.GetProcessById(processId);
        }
        public ProcessMemory(Process process)
            : this()
        {
            this.m_Process = process;
        }

        ~ProcessMemory()
        {
            this.Close();
            this.m_Process = null;
        }
        #endregion

        #region Get/Set
        public int Id { get { return this.m_Process.Id; } }
        public ProcessAccess AccessFlags { get { return this.m_ProcessAccess; } }
        public bool Opened { get { return (this.m_Handle != IntPtr.Zero); } }
        #endregion

        /* memalloc: tries to alloc memory in the opened process */
        public IntPtr MemAlloc(uint size)
        {
            return MemAlloc(IntPtr.Zero, size, AllocationType.Reserve | AllocationType.Commit , MemoryProtection.ExecuteReadWrite);
        }
        public IntPtr MemAlloc(IntPtr baseAddress, uint size, AllocationType allocationType, MemoryProtection memoryProtection)
        {
            return VirtualAllocEx(this.m_Handle, baseAddress, size, allocationType, memoryProtection);
        }

        public bool MemFree(IntPtr baseAddress)
        {
            return VirtualFreeEx(this.m_Handle, baseAddress, 0, AllocationType.Release);
        }

        /// <summary>
        /// Calls a function pointer in the remote process via CreateRemoteThread and waits for it to exit.
        /// Returns the thread exit code, which is EAX by default.
        /// (min-intrusive way to call the function of another process)
        /// </summary>
        /// <param name="functionPtr">Address of the remote function</param>
        /// <param name="param">First parameter for the function</param>
        /// <returns></returns>
        public int CallFunction(IntPtr functionPtr, IntPtr param)
        {
            uint threadID;
            uint returnValue = 0;

            IntPtr hThread = CreateRemoteThread(this.m_Handle, IntPtr.Zero, 0, functionPtr, param, 0, out threadID);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            GetExitCodeThread(hThread, out returnValue);

            return (int) returnValue;
        }

        /// <summary>
        /// Calls a function pointer in the remote process like 'CallFunction' but with SEH.
        /// If the calling function throws an exception, it will be catched and the function returns 0xffffffff 
        /// instead of crashing the whole remote process.
        /// its like try { return CallFunction(...); } catch { return 0xffffffff; }
        /// </summary>
        /// <param name="functionPtr">Address of the remote function</param>
        /// <param name="param">First parameter for the function</param>
        /// <returns></returns>
        public int SecureCallFunction(IntPtr functionPtr, IntPtr param)
        {
            // SEH (execute with try/catch)
            byte[] SEHBegin = {
                0x68, 0, 0, 0, 0,               // PUSH [ERRORHANDLER] - pointer to error handler
                0x64, 0xFF, 0x35, 0, 0, 0 ,0,   // PUSH DWORD PTR FS:[0]   - save old SE handler
                0x64, 0x89, 0x25, 0, 0, 0, 0    // MOV DWORD PTR FS:[0],ESP - set new seh record
            };

            // alloc mem and write small asm function
            byte[] inline = {
                0x68, 0, 0, 0, 0,           // PUSH [param]  - push the param onto the stack
                0xFF, 0x15, 0, 0, 0, 0,     // CALL DWORD PTR DS:[0] - call functionPtr
                0x59,                       // POP ECX - clean up stack
                0xEB, 0x09                  // JUMP SHORT +9 - - jump to no error
            };

            // End of SEH
            byte[] SEHEnd = {
                0x8B, 0x64, 0xE4, 0x08,         // MOV ESP,DWORD PTR SS:[ESP+8] - error handler here... restore esp
                0x31, 0xC0,                     // XOR EAX,EAX  - set eax
                0x83, 0xE8, 0x01,               // SUB EAX,1      to -1
                0x64, 0x8F, 0x5, 0, 0, 0, 0,    // POP DWORD PTR FS:[0] - jump here if no error
                0x83, 0xC4, 0x4,                // ADD ESP,4
                0xC3                            // RETN
            };

            // length of asm code
            int codeLen = SEHBegin.Length + inline.Length + SEHEnd.Length;

            // alloc memory for asm code + 4bytes for function pointer
            IntPtr destination = this.MemAlloc((uint) codeLen + 4);

            // Fill in PUSH [param]
            Buffer.BlockCopy(BitConverter.GetBytes((int)param), 0, inline, 1, 4);

            // Fill in error handler
            int SEHandler = (int)destination + SEHBegin.Length + inline.Length; // calc ptr of SEHEnd
            Buffer.BlockCopy(BitConverter.GetBytes(SEHandler), 0, SEHBegin, 1, 4);

            // Fill in functionPtr
            this.Write((IntPtr) ((int)destination + codeLen), BitConverter.GetBytes((int)functionPtr));
            // Fill in functionPtr call
            Buffer.BlockCopy(BitConverter.GetBytes((int)destination + codeLen), 0, inline, 7, 4);

            this.Write(destination, SEHBegin);
            this.Write((IntPtr)((int)destination + SEHBegin.Length), inline);
            this.Write((IntPtr)((int)destination + SEHBegin.Length + inline.Length), SEHEnd);

            // call the inline function
            int ret = this.CallFunction(destination, IntPtr.Zero);

            // dealloc (free) mem
            bool success = this.MemFree(destination);

            return ret;
        }

        /// <summary>
        /// Calls a function pointer like 'CallFunction' but returns a float value instead.
        /// </summary>
        /// <param name="functionPtr">Address of the remote function</param>
        /// <param name="param">First parameter for the function</param>
        /// <returns></returns>
        /*  Normally a function returns via EAX. This works for IntPtr or Int's but to get
            the return as float we have to inject some asm since we have to pop the float value
            from the FPU stack. Somehow tricky AND error-prone
        */
        public float CallFunctionFloat(IntPtr functionPtr, IntPtr param)
        {
            float ret = 0;
            /* SEH (execute with try/catch)
                68 00000000     PUSH [ERRORHANDLER] // pointer to handler
                64:FF35 0000000 PUSH DWORD PTR FS:[0]   // save old SE handler
                64:8925 0000000 MOV DWORD PTR FS:[0],ESP // set new seh record
                
                // error prone asm here...
              
                8B64E4 08       MOV ESP,DWORD PTR SS:[ESP+8] // error handler
                64:8F05 0000000 POP DWORD PTR FS:[0]
                83C4 04         ADD ESP,4
            */

            /* alloc mem and write small asm function
                68 [param]    PUSH [param]  // push the param onto the stack
                FF15 00000000 CALL DWORD PTR DS:[0] // call functionPtr
                D91D 00000000 FSTP DWORD PTR DS:[0] // FSTorePop (pop float from ST)
                A1 00000000   MOV EAX,DWORD PTR DS:[0] // mov float into eax (default "return")
                59            POP ECX // clean up stack
                C3            RETN  // return
             */

            byte[] inline = { 0x68,0,0,0,0,0xFF, 0x15, 0, 0, 0, 0, 0xD9, 0x1D, 0, 0, 0, 0, 0xA1, 0, 0, 0, 0,0x59, 0xC3,0,0,0,0,0,0,0,0 };

            // alloc mem
            IntPtr destination = this.MemAlloc((uint)inline.Length);

            // Fill in PUSH [param]
            Buffer.BlockCopy(BitConverter.GetBytes((int)param), 0, inline, 1, 4);

            // Fill in functionPtr to call
            Buffer.BlockCopy(BitConverter.GetBytes((int)functionPtr), 0, inline, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((int)destination + 24), 0, inline, 7, 4);

            // Fill in pointer to return value position
            byte[] pReturnValue = BitConverter.GetBytes((int)destination + 28);
            Buffer.BlockCopy(pReturnValue, 0, inline, 13, 4);
            Buffer.BlockCopy(pReturnValue, 0, inline, 18, 4);

            this.Write(destination, inline);

            // call the inline function
            int littleEndian = this.CallFunction(destination, IntPtr.Zero);
            
            // convert the return float
            ret = BitConverter.ToSingle(BitConverter.GetBytes(littleEndian).ToArray(), 0);

            // dealloc (free) mem
            bool success = this.MemFree(destination);

            return ret;
        }

        public bool Open(ProcessAccess desiredAccess)
        {
            bool ret = false;

            this.m_ProcessAccess = desiredAccess;

            this.m_Handle = OpenProcess(desiredAccess, false, this.m_Process.Id);
            if (this.m_Handle != null) ret = true;

            return ret;
        }
        public bool Open()
        {
            return this.Open(ProcessAccess.AllAccess);
        }

        public bool Close()
        {
            bool success = false;

            if (this.m_Handle != IntPtr.Zero)
            {
                success = CloseHandle(this.m_Handle);
                if (success)
                    this.m_Handle = IntPtr.Zero;
            }

            return success;
        }


        #region Read/Write
        public byte[] Read(int BaseAddress, int size)
        {
            return this.Read((IntPtr)BaseAddress, size);
        }
        public byte[] Read(IntPtr BaseAddress,int size)
        {
            int bytesRead = 0;
            byte[] buffer = new byte[size];

            bool success = ReadProcessMemory(this.m_Handle, BaseAddress, buffer, (UIntPtr) size, out bytesRead);

            if(!success)
                buffer = null;
            
            return buffer;
        }
        /// <summary>
        /// Returns address a pointer is pointing to :P
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public IntPtr ReadPointer(IntPtr address)
        {
            return this.ReadPointer(address, 0);
        }
        public IntPtr ReadPointer(IntPtr address, int offset)
        {
            IntPtr ret = IntPtr.Zero;

            if (offset > 0)
            {
                address = (IntPtr)((int)address + offset);
            }

            byte[] ptr = this.Read(address, 4);
            if (ptr != null)
            {
                ret = (IntPtr)BitConverter.ToInt32(ptr, 0);
            }

            return ret;
        }

        /// <summary>
        /// Writes stuff to the process' memory
        /// </summary>
        /// <param name="BaseAddress">Address to write to</param>
        /// <param name="data">Data to write</param>
        /// <returns></returns>
        public bool Write(IntPtr BaseAddress, byte[] data)
        {
            int bytesWritten = 0;
            bool success = WriteProcessMemory(this.m_Handle, BaseAddress, data, (uint)data.Length, out bytesWritten);
            return success;
        }


        #endregion


        public MEMORY_BASIC_INFORMATION[] MemoryRegions()
        {
            System.Collections.ArrayList ret = new System.Collections.ArrayList();
            uint address = 0;
            int result = 0;
            do
            {
                MEMORY_BASIC_INFORMATION m = new MEMORY_BASIC_INFORMATION();
                result = VirtualQueryEx(this.m_Handle, address, out m, Marshal.SizeOf(m));
                if (result != 0)
                    ret.Add(m);
                else
                    break;
                address = (uint)m.BaseAddress + (uint)m.RegionSize;
            } while (address!=0); //HARDCORE! :P

            return (MEMORY_BASIC_INFORMATION[]) ret.ToArray(typeof(MEMORY_BASIC_INFORMATION));
        }


        #region DECLARE MEMBER-VARS
        protected Process m_Process;
        protected IntPtr m_Handle;
        protected ProcessAccess m_ProcessAccess;
        #endregion
    }
}
