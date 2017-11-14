using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory {
    class MemoryFrame {
        ///This class will be the outline for a single frame of data as read from the game's memory. Will not be in charge of reading
        ///Base class will mainly be empty since the contents rely almost entirely on implementation
        protected MemoryFrame(uint frameNumber) {
            _frameNumber = frameNumber;
        }

        public uint FrameNum {
            get { return _frameNumber; }
        }
        
        private uint _frameNumber;


    }
}
