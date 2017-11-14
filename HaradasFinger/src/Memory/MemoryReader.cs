﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Memory {
    class MemoryReader {
        

        public MemoryReader(string processName) {
            _readBuffer = new byte[4];
            _process = new Process[1];
            _process = Process.GetProcessesByName(processName);
            try {
                _baseAddress = _process[0].MainModule.BaseAddress;
                _hProcess = _process[0].Handle;
            } catch {
                Console.WriteLine("Could not find process...");
                throw;
            }
            

            _hOpenProc = MemoryInterop.OpenProcess(PROCESS_VM_READ, 0, _process[0].Id);
        }

        public long ReadValueAtAddress(UInt64 readAddress) { //for reading a single value
            byte[] temp = Read((IntPtr)readAddress, 4);
            long lTemp = BitConverter.ToInt64(temp, 0);
            return lTemp;
        }

        public byte[] ReadBlockAtAddress(UInt64 readAddress, uint blockSize) {
            return Read((IntPtr)readAddress, blockSize);
        }

        private byte[] Read(IntPtr readAddress, uint size) {
            IntPtr lpNumberOfBytesRead = IntPtr.Zero;
            int dwResult = 0;
            uint resVal = 0;
            uint errorVal;
            byte[] readBuf = new byte[size + 4];

            try {
                dwResult = MemoryInterop.ReadProcessMemory(_hProcess, readAddress, readBuf, size, lpNumberOfBytesRead);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            if (dwResult <= 0) {
                errorVal = MemoryInterop.GetLastError();
                Console.WriteLine("ReadProcessMemory failed with error {0}", errorVal);
            } else {
                //turn the bytes into a valid address
                Byte indexer = 0;
                
                foreach(byte b in _readBuffer) {
                    resVal |= (uint)(b << indexer);
                    indexer+=8;
                }
                //Console.WriteLine("ReadProcessMemory success: {0}, {1}", resVal, lpNumberOfBytesRead);
            }

            return readBuf;
        }
        
        protected IntPtr _baseAddress;
        private byte[] _readBuffer;
        private Process[] _process;
        private IntPtr _hProcess;
        private IntPtr _hOpenProc;
        private const uint PROCESS_VM_READ = 0x0010;
    }
}
