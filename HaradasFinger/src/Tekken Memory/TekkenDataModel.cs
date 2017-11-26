using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class PlayerDataModel {
        public override string ToString() {
            return String.Format("ID:{0} | MoveTimer: {1} | Recovery: {4} | Startup: {13} | AttackDmg: {2} | MoveID: {3} | HitResult: {5}, AttackType: {6} | Rage: {7} | Side: {8} | X: {9} | Y: {10} | Z: {11} | HP: {12}%",
                _characterID, _moveTimer, _attackDamage, _moveID, _recovery, _hitResult, _attackType, _rageFlag, _side, _posX, _posY, _posZ, _percentHealth, _attackStartup);
        }

        #region PROPERTIES

        public bool IsInRage {
            get { return (_rageFlag == 1); }
        }

        public bool WasHit {
            get {
                if (_hitResult > 1) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public bool DidAttack {
            get {
                if (_attackStartup > 0) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        public bool IsDead => _percentHealth == 0;

        public bool IsInJuggle => false;

        #endregion

        //TODO: Create a struct thats aligned with the data block read in from memory, this will make setting the data trivial
        #region FIELDS
        public uint _side;
        public uint _characterID;
        public uint _moveTimer;     //NOTES: This seems like it represents the current frame of a specific state or animation. Such as the block stun animation, or hit recovery animation. I can probably figure out a lot of information by looking at this value.
        public uint _attackDamage;
        public uint _moveID;
        public uint _recovery;
        public uint _hitResult;
        public uint _attackType;
        public float _posX;
        public float _posY;
        public float _posZ;
        public uint _percentHealth;
        public uint _inputCounter; //unused for now
        public uint _attackStartup;
        public uint _attackStartupEnd; //does this imply extra active frames? //unused for now
        public uint _rageFlag;
        #endregion
    }

    class GameDataModel {
        public uint _p1RoundWins; //unused for now
        public uint _p2RoundWins; //unused for now
    }
}
