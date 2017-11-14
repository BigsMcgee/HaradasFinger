using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Memory {
    class MemoryInterop {

        [DllImport("Kernel32.dll")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, IntPtr lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("Kernel32.dll")]
        public static extern uint GetLastError();

    }
}
