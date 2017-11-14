using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory {
    static class MemoryUtils {

        static public uint GetDwordFromBlock(byte[] data, uint offset) {
            return BitConverter.ToUInt32(data, (int)offset);
        }

        static public float GetFloatFromBlock(byte[] data, uint offset) {
            return BitConverter.ToSingle(data, (int)offset);
        }

    }
}
