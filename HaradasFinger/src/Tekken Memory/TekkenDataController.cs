using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;
using System.Speech.Synthesis;
using NLog;

namespace Tekken7 {
    class TekkenDataController {

        private TekkenDataController() {
            if (!Init()) {
                logger.Trace("Failed to initialize data controller");
            }
            Reset();
        }
        #region METHODS
        public bool Init() {
            bool success = true;
            _gameDataLogger = new LoggerConfig("TekkenGameData", false);
            logger = LogManager.GetCurrentClassLogger();
            _speech = new SpeechSynthesizer();
            _speech.Rate = 3;
            if (_reader == null) {
                Stopwatch timeout = new Stopwatch();
                timeout.Start();
                do {
                    try {
                        _reader = new TekkenReader(TekkenDataValues.PROCESS_NAME);
                    } catch (Exception ex) {
                        success = false;
                        logger.Error(ex.ToString());
                        continue;
                    }
                    success = true;
                    break;
                } while (timeout.ElapsedMilliseconds < 2000); //try for 10 seconds
            }

            _frames = TekkenFrameCollection.Instance;
            _currentFrameNum = 0;

            return success;
        }

        public void Update() {
            Stopwatch timer = new Stopwatch();
            TekkenFrame[] eightFrames = new TekkenFrame[8];
            TekkenFrame oneFrame = null;
            if (!_reader.UpdateMainPointers()) {
                Reset();
                return;
            }
            for (uint i = 0; i < 8; i++) {
                try {
                    oneFrame = GetCurrentFrames(i);
                    //logger.Trace(oneFrame.ToString());
                } catch (InvalidOperationException ex) {
                    logger.Trace(ex.ToString());
                    return;
                }
                if (oneFrame.FrameNum < Int32.MaxValue)
                    eightFrames[i] = oneFrame;
                if (oneFrame.FrameNum > _currentFrameNum) {
                    _currentFrameNum = oneFrame.FrameNum;
                }
            }
            Array.Sort(eightFrames);
            try {
                int framesAdded = _frames.AddFrames(eightFrames);
            } catch (Exception ex) {
                logger.Debug(ex.ToString());
            }
            _newestFrames = eightFrames;
        }

        private bool Reset() {
            _frames.Clear();
            _currentP1FrameAdvantage = 0;
            _currentP1Startup = 0;
            _currentP2FrameAdvantage = 0;
            _currentP2Startup = 0;
            _currentFrameNum = 0;
            _currentFrameAdvantage = 0;
            try {
                _reader.AcquireProcess(TekkenDataValues.PROCESS_NAME);
            } catch {
                logger.Trace("Could not find Tekken process...trying again");
                return false;
            }

            return true;
        }

        private TekkenFrame GetCurrentFrames(uint index) {
            uint frameCount;
            if (_reader == null) {
                throw new InvalidOperationException("reader is null, please initialize before attempting to read");
            }

            frameCount = _reader.ReadFrameCount(index);
            //TODO: Need a more reliable way to reset the current frame object
            if (frameCount == 0) {
                Reset();
                logger.Trace("Reset");
            }
            byte[] playersData = _reader.ReadPlayersDataBlock(index);
            TekkenFrame oneFrame = new TekkenFrame(frameCount);
            try {
                oneFrame.InitPlayers(playersData);
            } catch (Exception ex) {
                logger.Trace(ex.ToString());
            }

            return oneFrame;
        }

        public int GetP1LastMove() {
            //just a test method to see if this can work
            //
            //The basic idea is to read an entire list of frames for a single action.
            //Such as...Player 1 does an Electric...Can i grab every frame from start to finish of that Electric?
            //
            //Improvments: Get attack startup from the beginning of the moveTimer. Get the frame advantage from the begining of recovery, not the same place.
            //              Again, try to index all of the frames for an entire move + recovery, instead of always dealing with single frames.
            TekkenFrame newestAttackFrame = _frames.FindLatestPlayer1StartupTransition(_currentFrameNum);
            if (newestAttackFrame != null) {
                if (newestAttackFrame != lastP1AttackFrame && newestAttackFrame.IsP1HitOutcomeNonZero) {
                    _currentP1FrameAdvantage = (int)newestAttackFrame.P2RecoveryFrames - (int)newestAttackFrame.P1RecoveryFrames;
                    _currentP1Startup = newestAttackFrame.Player1._attackStartup;
                    logger.Trace("P1 frame advantage: {0}", _currentP1FrameAdvantage);
                    logger.Trace(newestAttackFrame.ToString());
                    lastP1AttackFrame = newestAttackFrame;
                }
            }
            return _currentP1FrameAdvantage;
        }

        public int GetP2LastMove() {
            TekkenFrame newestAttackFrame = _frames.FindLatestPlayer2StartupTransition(_currentFrameNum);

            if (newestAttackFrame != null) {
                if (newestAttackFrame != lastP2AttackFrame && newestAttackFrame.IsP2HitOutcomeNonZero) {
                    _currentP2FrameAdvantage = (int)newestAttackFrame.P1RecoveryFrames - (int)newestAttackFrame.P2RecoveryFrames;
                    _currentP2Startup = newestAttackFrame.Player2._attackStartup;
                    logger.Trace("P2 frame advantage: {0}", _currentP2FrameAdvantage);
                    logger.Trace(newestAttackFrame.ToString());
                    lastP2AttackFrame = newestAttackFrame;
                }
            }

            return _currentP2FrameAdvantage;
        }

        public int GetLastMove() {
            TekkenFrame newestAttackFrame = _frames.FindLatestStartupTransition(_currentFrameNum);
            if (newestAttackFrame != null) {
                if (newestAttackFrame != lastAttackFrame && newestAttackFrame.IsHitOutcomeNonZero) {
                    _currentFrameAdvantage = (int)newestAttackFrame.P2RecoveryFrames - (int)newestAttackFrame.P1RecoveryFrames;
                    lastAttackFrame = newestAttackFrame;
                    if (!lastAttackFrame.IsEitherPlayerDead)
                        SpeakFrameAdvantage(_currentFrameAdvantage);
                }
            }
            return _currentFrameAdvantage;
        }

        //TODO: make this only work if not in a combo/juggle
        private void SpeakFrameAdvantage(int frames) {
            string speak = (frames > -1) ? "+" + frames.ToString() : frames.ToString();
            _speech.SpeakAsyncCancelAll();
            _speech.SpeakAsync(speak);
        }
        #endregion //METHODS
        #region PROPERTIES
        public static TekkenDataController Instance {
            get {
                if (_instance == null) {
                    _instance = new TekkenDataController();
                }
                return _instance;
            }
        }

        public TekkenFrame[] NewestFrame {
            get { return _newestFrames; }
        }

        public int Player1FrameAdvantage => _currentP1FrameAdvantage;
        public int Player2FrameAdvantage => _currentP2FrameAdvantage;
        public uint P1Startup => _currentP1Startup;
        public uint P2Startup => _currentP2Startup;
        public uint CurrentFrameNum => _currentFrameNum;
        #endregion //PROPERTIES
        #region FIELDS
        private static TekkenDataController _instance;
        private TekkenReader _reader;
        private TekkenFrameCollection _frames;
        private TekkenFrame[] _newestFrames;
        private static Logger logger;
        private static LoggerConfig _gameDataLogger;
        private static TekkenFrame lastP1AttackFrame;
        private static TekkenFrame lastP2AttackFrame;
        private static TekkenFrame lastAttackFrame;
        private int _currentP1FrameAdvantage; //specific to moves that P1 has performed
        private int _currentP2FrameAdvantage; //specific to moves that P2 has performed
        private int _currentFrameAdvantage; //refers to player1's current frame advantage no matter who has performed the last action
        private uint _currentP1Startup;
        private uint _currentP2Startup;
        private uint _currentFrameNum;

        private SpeechSynthesizer _speech;
        #endregion //FIELDS
    }
}
