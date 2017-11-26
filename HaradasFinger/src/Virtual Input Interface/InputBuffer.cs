using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace Tekken7 {
    class InputBuffer {

        public InputBuffer(string mmfName) {
            _mappedFile = MemoryMappedFile.CreateOrOpen(mmfName, 128); //TODO: CONFIG
        }

        public void WriteToMap(uint value, int waitFrames = 1) {
            using (_accessor = _mappedFile.CreateViewAccessor(0, 128)) {
                _accessor.Write(0, value);
            }
            WaitFrames(waitFrames);
        }
         
        public void WriteToMap(uint value, double waitFrames = 1) {
            using (_accessor = _mappedFile.CreateViewAccessor(0, 128)) {
                _accessor.Write(0, value);
            }
            WaitFrames(waitFrames);
        }

        public static void WaitFrames(int waitFrames) {
            WaitFrames((double)waitFrames);
        }

        public static void WaitFrames(double waitFrames) {
            double time = (double)waitFrames * FRAMETIME;
            System.Threading.Thread.Sleep((int)System.Math.Ceiling(time));
        }

        public const double FRAMETIME = 16.66666667; //TODO: CONFIG and pass as constructor param
        //public const double FRAMETIME = 17;
        MemoryMappedFile _mappedFile;
        MemoryMappedViewAccessor _accessor;
    }
}
