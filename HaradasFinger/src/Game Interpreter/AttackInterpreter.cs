using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekken7;
using Memory;
using NLog;

namespace HaradasFinger {
    class AttackInterpreter : IInterpreter {
        public delegate void BlockEventHandler(object sender, BlockEventArgs e);
        public event BlockEventHandler Block;

        public AttackInterpreter(TekkenFrameCollection frames) {
            _frames = frames;
            _log = LogManager.GetCurrentClassLogger();
            _lastFrameRead = 0;
            _frameList = _frames.FrameList;
        }

        protected virtual void OnBlock(BlockEventArgs e) {
            Block?.Invoke(this, e);
        }

        /// <summary>
        /// Reads until there are no new frame objects found
        /// </summary>
        public void Update() {
            TekkenFrame readFrame = null;
            if (_lastFrameRead == 0) { //start condition
                readFrame = (TekkenFrame)_frames.GetFirst();
                if (readFrame != null) {
                    _lastFrameRead = readFrame.FrameNum;
                    CheckIfBlockFrame(readFrame);
                }
            }

            bool block;
            do {
                readFrame = (TekkenFrame)_frames.GetFrame(++_lastFrameRead);
                if (readFrame != null) {
                    block = CheckIfBlockFrame(readFrame);
                    _log.Trace("{0} IsBlock: {1}", readFrame.FrameNum, block);
                }
            } while (readFrame != null);
        }

        public void Reset() {
            _lastFrameRead = 0;
        }

        private bool CheckIfBlockFrame(TekkenFrame checkFrame) {
            if (checkFrame == null)
                return false;

            return (checkFrame.Player1._hitResult | checkFrame.Player2._hitResult) == 1;
        }

        /// <summary>
        /// Reference to the frame collection object, not owned by this
        /// </summary>
        private TekkenFrameCollection _frames;

        /// <summary>
        /// Readonly reference to the frame list
        /// </summary>
        private readonly MemoryFrame[] _frameList;

        /// <summary>
        /// Class logger
        /// </summary>
        private Logger _log;

        /// <summary>
        /// Stores the value of the last frame read so that we don't read frames that have already been parsed
        /// </summary>
        private uint _lastFrameRead;

        

    }
}
