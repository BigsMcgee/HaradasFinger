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
            if (frame == null)
                return false;
            else {
                if (!_frameList.Contains(frame)) {
                    _frameList[frame.FrameNum % 3600] = frame; //only store 3600
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Safe way to access objects in the frame list. Returns default value if no matching object found
        /// </summary>
        /// <returns></returns>
        public virtual MemoryFrame GetFrame(uint frameNum) {
            MemoryFrame frame;
            try {
                frame = _frameList[frameNum % 3600];
            } catch (Exception ex) {
                frame = null;
                return null;
            }

            if (frame?.FrameNum == frameNum) {
                return frame;
            }

            return null;
        }

        /// <summary>
        /// Retrieves the frame from the specified index in the frame buffer. If no frame is found, returns null;
        /// </summary>
        /// <param name="index"></param>
        /// <returns>MemoryFrame object found at the given index, null if not found</returns>
        public MemoryFrame GetIndex(uint index) {
            MemoryFrame retFrame;
            if (index > _frameList.Length) {
                return null;
            }

            try {
                retFrame = _frameList[index];
            } catch {
                retFrame = null;
            }

            return retFrame;
        }

        /// <summary>
        /// Gets the first object found in the buffer
        /// </summary>
        /// <returns>First MemoryFrame object found in the buffer, null if no object is found</returns>
        public MemoryFrame GetFirst() {
            return _frameList.FirstOrDefault(x => x != null);
        }

        public MemoryFrame[] FrameList {
            get { return _frameList; }
        }

        public int Length {
            get { return _frameList.Length; }
        }

        //private List<MemoryFrame> _listFrames;
        MemoryStore _instance;
        //protected List<MemoryFrame> _frameList;
        protected MemoryFrame[] _frameList;
    }
}
