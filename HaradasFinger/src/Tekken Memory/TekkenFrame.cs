using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using NLog;

namespace Tekken7 {
    class TekkenFrame : MemoryFrame {
        public TekkenFrame(uint frameNumber)
            :base(frameNumber) {
            //do other stuff
            _player1 = new PlayerDataModel();
            _player2 = new PlayerDataModel();

            logger = LogManager.GetCurrentClassLogger();
        }

        public void InitPlayers(byte[] playerData) {
            _player1._characterID = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.CHAR_ID);
            _player2._characterID = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.CHAR_ID + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._moveTimer = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.MOVE_TIMER);
            _player2._moveTimer = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.MOVE_TIMER + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._attackDamage = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_DAMAGE);
            _player2._attackDamage = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_DAMAGE + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._moveID = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.MOVE_ID);
            _player2._moveID = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.MOVE_ID + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._recovery = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.RECOVERY);
            _player2._recovery = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.RECOVERY + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._hitResult = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.HIT_RESULT);
            _player2._hitResult = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.HIT_RESULT + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._attackType = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_TYPE);
            _player2._attackType = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_TYPE + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._rageFlag = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.RAGE);
            _player2._rageFlag = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.RAGE + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._side = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.P1_FACING_DIRECTION);
            _player2._side = (_player1._side ^ 1) & 0x01; //just mask it to be safe

            _player1._posX = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_X);
            _player2._posX = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_X + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._posY = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_Y);
            _player2._posY = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_Y + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._posZ = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_Z);
            _player2._posZ = MemoryUtils.GetFloatFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.POS_Z + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._percentHealth = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.PERCENT_HP);
            _player2._percentHealth = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.PERCENT_HP + TekkenDataOffsets.P2_OBJECT_OFFSET);

            _player1._attackStartup = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_STARTUP);
            _player2._attackStartup = MemoryUtils.GetDwordFromBlock(playerData, (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_STARTUP + TekkenDataOffsets.P2_OBJECT_OFFSET);
        }

        public override string ToString() {
            return String.Format("Frame#: {0}\nP1: {1}\nP2: {2}", FrameNum, _player1.ToString(), _player2.ToString());
        }

        #region PROPERTIES
        public bool DidEitherPlayerAttack => (_player1.DidAttack || _player2.DidAttack);

        public bool DidPlayer1Attack => (_player1.DidAttack);

        public bool DidPlayer2Attack => (_player2.DidAttack);

        public bool IsHitOutcomeNonZero {
            get {
                if (_player1._hitResult != 0 || _player2._hitResult != 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public bool IsP1HitOutcomeNonZero => (_player2._hitResult != 0);
        public bool IsP2HitOutcomeNonZero => (_player1._hitResult != 0);

        //TODO: REDO THESE PROPERLY. TOOLASSISTED FIGURED IT OUT, SO CAN I
        public uint P1RecoveryFrames => (_player1._recovery - _player1._attackStartup);
        public uint P2RecoveryFrames => (_player2._recovery - _player2._attackStartup);

        //simple accessors
        public PlayerDataModel Player1 => _player1;
        public PlayerDataModel Player2 => _player2;

        #endregion
        private PlayerDataModel _player1;
        private PlayerDataModel _player2;

        private static Logger logger;
    }

    class TekkenFrameCollection : MemoryStore {

        private TekkenFrameCollection() {
            _frameList = new MemoryFrame[100000]; //not sure what to do about array sizing yet
            _frameDict = new Dictionary<uint, MemoryFrame>();
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Clear() {
            Array.Clear(_frameList, 0, _frameList.Length);
        }

        public int AddFrames(ArrayList eightFrames) {
            int added = 0;
            for(int i = 0; i < 8; i++) {
                if (eightFrames[i] != null) {
                    TekkenFrame frame = eightFrames[i] as TekkenFrame;
                    try {
                        if (AddFrame(frame)) {
                            added++;
                        }
                    } catch (Exception ex) {
                        logger.Trace("failed to insert frame: {0}", frame.FrameNum);
                        logger.Trace(ex.ToString());
                    }
                }
            }
            return added;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A TekkenFrame objects representing the first frame of an "attack"</returns>
        public TekkenFrame FindLatestPlayer1StartupTransition() {
            TekkenFrame frame = null, checkFrame = null;
            var localFrameList = _frameList.ToArray();
            int lastIndex = localFrameList.GetUpperBound(0);
            for (int i = lastIndex; i > 0; --i) {
                frame = localFrameList[i] as TekkenFrame;
                if (frame == null)
                    continue;
                if (frame.DidPlayer1Attack) {
                    //try reading the move_timer and jumping back that far
                    int jumpBack = 0;
                    jumpBack = (int)(frame.Player1._moveTimer - (int)frame.Player1._attackStartup) - 1;
                    try {
                        frame = localFrameList[i - (jumpBack)] as TekkenFrame;
                    } catch (IndexOutOfRangeException ex) {
                        //do nothing i guess
                    }
                    if (frame == null)
                        continue;
                        //yay we found the start of the animation
                        //IDEA: from here, try moving forward to the end of the startup region, try to grab frame data from here. Can I move straight there?
                    return frame;
                }
            }

            //logger.Trace("No attack frame found");
            return null;
        }

        //TODO: Consolidate the functionality of this method, repeat code is bad
        public TekkenFrame FindLatestPlayer2StartupTransition() {
            TekkenFrame frame = null;
            var localFrameList = _frameList.ToArray();
            int lastIndex = localFrameList.GetUpperBound(0);
            for (int i = lastIndex; i > 0; --i) {
                frame = localFrameList[i] as TekkenFrame;
                if (frame == null)
                    continue;
                if (frame.DidPlayer2Attack) {
                    //try reading the move_timer and jumping back that far
                    int jumpBack = 0;
                    jumpBack = (int)(frame.Player2._moveTimer - (int)frame.Player2._attackStartup) - 1;
                    frame = localFrameList[i - (jumpBack)] as TekkenFrame;
                    if (frame == null)
                        continue;
                    //yay we found the start of the animation
                    //IDEA: from here, try moving forward to the end of the startup region, try to grab frame data from here. Can I move straight there?
                    return frame;
                }
            }

            //logger.Trace("No attack frame found");
            return null;
        }

        public TekkenFrame FindLatestStartupTransition(uint lastFrameRead) {
            TekkenFrame retFrame = null;
            for (uint i = lastFrameRead; i > 0; --i) {
                retFrame = _frameList[i] as TekkenFrame;
                if (retFrame == null)
                    continue;
                if(retFrame.DidEitherPlayerAttack) {
                    int jumpback;
                    if (retFrame.DidPlayer1Attack) {
                        jumpback = (int)(retFrame.Player1._moveTimer - (int)retFrame.Player1._attackStartup) - 1;
                    } else {
                        jumpback = (int)(retFrame.Player2._moveTimer - (int)retFrame.Player2._attackStartup) - 1;
                    }

                    retFrame = _frameList[i - jumpback] as TekkenFrame;
                    if (retFrame == null)
                        continue;

                    return retFrame;
                }
            }
            return null;
        }

        public static TekkenFrameCollection Instance {
            get {
                if (_instance == null) {
                    _instance = new TekkenFrameCollection();
                }
                return _instance;
            }
        }

        private static TekkenFrameCollection _instance;

        private static Logger logger;
    }
}
