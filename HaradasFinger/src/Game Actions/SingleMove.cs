using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekken7;

namespace HaradasFinger {
    class SingleMove : IGameAction {
        //This should be the base class for all moves in the game
        public SingleMove(List<TekkenFrame> frames) {
            frames = _frames;
            Init();
        }

        private void Init() {
            _frames.Sort();
            _startFrame = _frames.First();
            _endFrame = _frames.Last();

            if (_startFrame.DidPlayer1Attack) {

            }


            _startup = GetStartup();

            _firstRecoveryFrame = GetFirstRecoveryFrame();
            _recovery = GetAttackerRecovery();
            _defenderRecovery = GetDefenderRecovery();
        }

        //TODO: How to handle both players attacking simultaneously?
        private uint GetStartup() {

            if (_startFrame.DidPlayer1Attack) {
                return _startFrame.Player1._attackStartup;
            }

            if (_startFrame.DidPlayer2Attack) {
                return _startFrame.Player2._attackStartup;
            }

            return 0;
        }

        private TekkenFrame GetFirstRecoveryFrame() {
            if (_startFrame.DidPlayer1Attack) {
                return _frames.Find(x => x.Player1._moveTimer == (GetStartup() + 1));
            }

            if (_startFrame.DidPlayer2Attack) {
                return _frames.Find(x => x.Player2._moveTimer == (GetStartup() + 1));
            }

            return null;
        }

        private uint GetAttackerRecovery() {

            if (_startFrame.DidPlayer1Attack) {
                return _firstRecoveryFrame.Player1._recovery;
            }

            if (_startFrame.DidPlayer2Attack) {
                return _firstRecoveryFrame.Player2._recovery;
            }

            return 0;
        }

        private uint GetDefenderRecovery() {
            if (_startFrame.DidPlayer1Attack) {
                return _firstRecoveryFrame.Player2._recovery;
            }

            if (_startFrame.DidPlayer2Attack) {
                return _firstRecoveryFrame.Player1._recovery;
            }

            return 0;
        }

        public override string ToString() {
            return base.ToString();
        }
        #region PROPERTIES
        //regarding the following two properties, a single move such as a DF+2 should have True for both IsFirst and IsLast
        /// <summary>
        /// Returns true if the move has no linked moves preceding it
        /// </summary>
        public bool IsFirst => false;

        /// <summary>
        /// Returns true if the move has no linked moves following
        /// </summary>
        public bool IsLast => false;

        public int BlockAdvantage => (int)(_recovery - _startup);

        #endregion

        #region FIELDS
        /// <summary>
        /// List emcompassing the entirety of a move from first startup frame to the final recovery frame
        /// </summary>
        private List<TekkenFrame> _frames;
        private TekkenFrame _firstRecoveryFrame;
        private TekkenFrame _startFrame;
        private TekkenFrame _endFrame;
        private PlayerDataModel _attackingPlayer;
        private PlayerDataModel _defendingPlayer;

        private uint _startup;
        private uint _recovery;
        private uint _defenderRecovery;

        //do I want this here?
        private string _inputNotation;

        #endregion
    }
}
