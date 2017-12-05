using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memory;
using System.Diagnostics;

namespace Tekken7 {
    class TekkenReader: MemoryReader {
        public TekkenReader(string processName)
            :base(processName){
            _mainAddr = (UInt64)_baseAddress + _mainOffset;
            UpdateMainPointers();
        }

        public bool UpdateMainPointers() {
            _gameObject = GetMainPointer();
            _playerAddr = (ulong)ReadValueAtAddress(_gameObject);

            if (_playerAddr == 0 || _gameObject == 0) { //these will NEVER be zero while in game, only if in the menu/loading/etc.
                return false;
            }
            return true;
        }

        public long ReadValue(UInt64 offset) {
            return ReadValueAtAddress(_playerAddr + offset);
        }
        
        public uint ReadFrameCount(uint index) {
            ulong potentialAddr = _playerAddr + (TekkenDataOffsets.ROLLBACK_FRAME_OFFSET * index); //store these or just recalculate?
            return (uint)ReadValueAtAddress(potentialAddr + (ulong)TekkenDataOffsets.GameStateOffsets.FRAME_COUNT);
            //potential_second_address = second_address_base + (i * MemoryAddressOffsets.rollback_frame_offset.value)
            //potential_frame_count = self.GetValueFromAddress(processHandle, potential_second_address + GameDataAddress.frame_count.value)
        }

        public byte[] ReadPlayersDataBlock(uint index) {
            return ReadBlock((_playerAddr + (TekkenDataOffsets.ROLLBACK_FRAME_OFFSET * index)), PLAYER_BLOCK_SIZE);
        }

        private byte[] ReadBlock(UInt64 readAddress, uint blockSize) {
            return ReadBlockAtAddress(readAddress, blockSize);
        }
        
        private UInt64 GetMainPointer() {
         
            return (UInt64)ReadValueAtAddress(_mainAddr);

        }

        public UInt64 PlayerAddr {
            get { return _mainAddr; }
        }

        private UInt64 _mainAddr;
        private UInt64 _gameObject;
        private UInt64 _playerAddr;
        private const UInt64 _mainOffset = 0x033DED38;

        //TODO: These will change as read model is updated
        //private const uint PLAYER_BLOCK_SIZE    = 0xCD54;
        private const uint PLAYER_BLOCK_SIZE = (uint)TekkenDataOffsets.P2_OBJECT_OFFSET + (uint)TekkenDataOffsets.PlayerOffsets.ATTACK_STARTUP;
        private const uint GAME_BLOCK_SIZE      = 0x00C8; //unused
    }
}
