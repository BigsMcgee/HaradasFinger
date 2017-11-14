using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using NLog;

namespace Tekken7 {
    class TekkenDataController {

        private TekkenDataController() {
            if (!Init()) {
                logger.Trace("Failed to initialize data controller");
            }
        }

        public bool Init() {
            bool success = true;
            _gameDataLogger = new LoggerConfig("TekkenGameData", false);
            logger = LogManager.GetCurrentClassLogger();

            if (_reader == null) {
                Stopwatch timeout = new Stopwatch();
                timeout.Start();
                do {
                    try {
                        _reader = new TekkenReader("TekkenGame-Win64-Shipping");
                    } catch (Exception ex) {
                        success = false;
                        continue;
                    }
                    success = true;
                    break;
                } while (timeout.ElapsedMilliseconds < 10000); //try for 10 seconds
            }

            _frames = TekkenFrameCollection.Instance;

            return success;
        }

        public void Update() {
            ArrayList eightFrames = new ArrayList();
            TekkenFrame oneFrame = null;
            _reader.UpdateMainPointers();
            for (uint i = 0; i < 8; i++) {
                try {
                    oneFrame = GetCurrentFrames(i);
                    //logger.Trace(oneFrame.ToString());
                } catch (InvalidOperationException ex) {
                    logger.Error(ex.ToString());
                    return;
                }
                eightFrames.Add(oneFrame);
            }
            int framesAdded = _frames.AddFrames(eightFrames);
            //logger.Trace("Added {0} frames", framesAdded);
            _newestFrames = eightFrames;
        }

        public uint GetFrameOfLastLow() {
            /*if (_newestFrames.IsPlayerOneAttackingLow()) {
                return _newestFrames.FrameNum;
            }
            TekkenFrame frame;
            uint currentFameNum = _newestFrames.FrameNum;
            while (currentFameNum != 0) {
                frame = (TekkenFrame)_frames.TryGetFrame(currentFameNum);
                if (frame.IsPlayerOneAttackingLow()) {
                    return currentFameNum;
                } else {
                    currentFameNum--;
                }
            }*/
            return 0; //no low has been read
        }

        private TekkenFrame GetCurrentFrames(uint index) {
            uint frameCount = 0;
            if (_reader == null) {
                throw new InvalidOperationException("reader is null, please initialize before attempting to read");
            }

            frameCount = _reader.ReadFrameCount(index);
            //TODO: Need a more reliable way to reset the current frame object
            if (frameCount == 0) {
                _frames.Clear();
            }
            byte[] playersData = _reader.ReadPlayersDataBlock(index);
            TekkenFrame oneFrame = new TekkenFrame(frameCount);
            oneFrame.InitPlayers(playersData);

            return oneFrame;
        }

        private TekkenFrame ProcessPlayersDataBlock(byte[] data, long frameCount) {
            TekkenFrame oneFrame = new TekkenFrame((uint)frameCount);

            oneFrame.InitPlayers(data);

            return oneFrame;
        }

        private void ProcessNewestFrame() {
            //basically just calculate frame advantage for now
            int frameAdvantage = 0;
            for (int i = 0; i < 8; i++) {
                TekkenFrame frame = _newestFrames[i] as TekkenFrame;
                if (frame.DidEitherPlayerAttack) {
                    frameAdvantage = (int)frame.P2RecoveryFrames - (int)frame.P1RecoveryFrames;
                    //logger.Trace("Frame Advantage: {0}", frameAdvantage);
                    //logger.Trace("{0}", frame.ToString()); //dump the whole frame in hopes of finding a condition to use. Delete this if I forget.
                }
            }
        }

        public int TestGetLastMove()
        {
            //just a test method to see if this can work
            //
            //The basic idea is to read an entire list of frames for a single action.
            //Such as...Player 1 does an Electric...Can i grab every frame from start to finish of that Electric?
            TekkenFrame testFrame = _frames.FindLatestPlayer1StartupTransition();
            if (testFrame != null) {
                if (testFrame != lastAttackFrame) {
                    _currentP1FrameAdvantage = (int)testFrame.P2RecoveryFrames - (int)testFrame.P1RecoveryFrames;
                    logger.Trace("frame advantage: {0}", _currentP1FrameAdvantage);
                    logger.Trace(testFrame.ToString());
                    lastAttackFrame = testFrame;
                } else {
                    logger.Trace("No new attack frames...");
                }
            }
            return _currentP1FrameAdvantage;
        }

        public static TekkenDataController Instance {
            get {
                if (_instance == null) {
                    _instance = new TekkenDataController();
                }
                return _instance;
            }
        }

        public ArrayList NewestFrame {
            get { return _newestFrames; }
        }

        public int Player1FrameAdvantage {
            get {
                return _currentP1FrameAdvantage;
            }
        }

        private static TekkenDataController _instance;

        private TekkenReader _reader;
        private TekkenFrameCollection _frames;
        private ArrayList _newestFrames;


        private static Logger logger;
        private static LoggerConfig _gameDataLogger;
        private static TekkenFrame lastAttackFrame;

        private int _currentP1FrameAdvantage;
    }
}
