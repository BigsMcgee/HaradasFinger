using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Memory {
    abstract class MemoryStore {
        ///Store many frames of data as well as the ability to access it
        ///
        ///How many frames do I keep? Will I be able to read EVERY SINGLE frame?
        ///Maybe just capture each frame until the frame counter resets? Then figure out how to serialize this class out to log files/DB/whatever platform I wanna use to consume matches later on.
        public virtual bool AddFrame(MemoryFrame frame) {
            if (_frameList[(int)frame.FrameNum] == null) {
                _frameList[(int)frame.FrameNum] = frame;
                return true;
            } else {
                return false;
            }
        }

        public MemoryFrame GetFrameAt(uint frameNum) {
            return _frameList[(int)frameNum];
        }

        //private List<MemoryFrame> _listFrames;
        MemoryStore _instance;
        protected Dictionary<uint, MemoryFrame> _frameDict;
        //protected ArrayList _frameList;
        protected MemoryFrame[] _frameList;

    }
}
