using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory {
    class MemoryFrame : IEquatable<MemoryFrame>, IComparable<MemoryFrame> {
        ///This class will be the outline for a single frame of data as read from the game's memory. Will not be in charge of reading
        ///Base class will mainly be empty since the contents rely almost entirely on implementation
        protected MemoryFrame(uint frameNumber) {
            _frameNumber = frameNumber;
        }

        public uint FrameNum {
            get { return _frameNumber; }
        }
        
        private uint _frameNumber;

        #region IEquatable/IComparable Implementation
        public bool Equals(MemoryFrame other) {
            if (other == null)
                return false;

            if (FrameNum == other.FrameNum) 
                return true;
            else 
                return false;
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            MemoryFrame frameObj = obj as MemoryFrame;
            if (frameObj == null)
                return false;
            else
                return Equals(frameObj);
        }

        public override int GetHashCode() {
            return (int)FrameNum;
        }

        public int CompareTo(MemoryFrame other) {
            if (other == null)
                return 1;

            return FrameNum.CompareTo(other.FrameNum);
        }

        public static bool operator == (MemoryFrame frame1, MemoryFrame frame2) {
            if((object)frame1 == null || (object)frame2 == null) {
                return Object.Equals(frame1, frame2);
            }

            return frame1.Equals(frame2);
        }

        public static bool operator != (MemoryFrame frame1, MemoryFrame frame2) {
            if ((object)frame1 == null || (object)frame2 == null) {
                return !Object.Equals(frame1, frame2);
            }

            return !frame1.Equals(frame2);
        }

        public static bool operator > (MemoryFrame frame1, MemoryFrame frame2) {
            return frame1.CompareTo(frame2) == 1;
        }

        public static bool operator < (MemoryFrame frame1, MemoryFrame frame2) {
            return frame1.CompareTo(frame2) == -1;
        }

        public static bool operator >= (MemoryFrame frame1, MemoryFrame frame2) {
            return frame1.CompareTo(frame2) >= 0;
        }

        public static bool operator <=(MemoryFrame frame1, MemoryFrame frame2) {
            return frame1.CompareTo(frame2) <= 0 ;
        }

        #endregion

    }
}
