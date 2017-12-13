using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaradasFinger {
    class BlockEventArgs : EventArgs {
        private readonly int _frameAdvantage;

        public BlockEventArgs(int frameAdvantage) {
            _frameAdvantage = frameAdvantage;
        }

        public int FrameAdvantage {
            get { return _frameAdvantage; }
        }
    }
}
