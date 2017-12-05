using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {

    static class TekkenDataValues {
        struct CharacterID {    //TODO: Fill these out as I come across them
            const uint LAW = 1;
            const uint YOSHIMITSU = 3;
            const uint BRYAN = 7;
            const uint HEIHACHI = 8;
            const uint KAZUYA = 9;
            const uint LARS = 18;
            const uint PANDA = 34;
        }

        /// <summary>
        /// Refers to the different types of attacks available in Tekken
        /// </summary>
        struct AttackType {
            const uint NONE = 0;
            const uint LOW = 1;
            const uint MID = 2;
            const uint HIGH = 5;
            const uint UNBLOCKABLE = 7; //sometimes seems to 
            const uint THROW = 10;
        }

        /// <summary>
        /// Hit Outcome field references the player getting hit, not the player executing the hit.
        /// </summary>
        struct HitOutcome {
            const uint NONE = 0;
            const uint BLOCK = 1;
            const uint JUGGLE1 = 3;
            const uint JUGGLE2 = 4;//whats the difference between these two values, why does it swap back and forth mid juggle?
            const uint UNKNOWN = 9;
            const uint COUNTER_HIT = 10;
            const uint NORMAL_HIT = 12;

        }

        /// <summary>
        /// Directional inputs from the DPad/Stick. These are oriented in relation to the character(i.e. forward/back instead of left/right)
        /// </summary>
        struct InputDirection {
            const uint NEUTRAL = 32;
            const uint BACK = 16;
            const uint FORWARD = 64;
            const uint DOWN = 4;
            const uint DOWN_FORWARD = 8;
            const uint DOWN_BACK = 2;
            const uint UP = 256;
            const uint UP_BACK = 128;
            const uint UP_FORWARD = 512;
        }

        struct Rage {
            const uint NONE = 0;
            const uint IN_RAGE = 1;
        }

        struct MoveProperties {
            //TODO: Figure out the "simple move state" stuff
        }

        struct Side {
            const uint LEFT = 0;
            const uint RIGHT = 1;
        }

        public static string GetCharacterString(uint charID) {
            switch(charID) {
                case 1:
                    return "LAW";
                case 2:
                    return "??";
                case 3:
                    return "YOSHIMITSU";
                    //4, 5, 6
                case 7:
                    return "BRYAN";
                default:
                    return "??";
            }
        }

        public static string PROCESS_NAME = "TekkenGame-Win64-Shipping";
    }

    static class TekkenDataOffsets {
        //TODO: Finish adding in useful data values, currently have enough to determine frame data, but there is still a bunch of useful things to know
        public enum PlayerOffsets : uint {
            CHAR_ID = 0x00D4,
            MOVE_TIMER = 0x01F0,
            ATTACK_DAMAGE = 0x02FC,
            MOVE_ID = 0x031C,
            RECOVERY = 0x0360,
            HIT_RESULT = 0x039C,
            ATTACK_TYPE = 0x03D4,
            RAGE = 0x099A,
            P1_FACING_DIRECTION = 0x0AC4,
            POS_X = 0x0BF0,
            POS_Y = 0x0BF4,
            POS_Z = 0x0BF8,
            PERCENT_HP = 0x11E8,
            BUTTON_INPUT = 0x14EC,
            DIRECTIONAL_INPUT = 0x14F0,
            //ATTACK_STARTUP = 0x66A0,
            ATTACK_STARTUP = 0x6800,
        };

        //public const uint P2_OBJECT_OFFSET = 0x66B0;
        public const uint P2_OBJECT_OFFSET = 0x6810;
        public const uint ROLLBACK_FRAME_OFFSET = 0x1A4F0;

        public enum GameStateOffsets : uint {
            FRAME_COUNT = 0x1A050,
            //P1_ROUND_WINS = 0x19AEC,
            P1_ROUND_WINS = 0x1A06C,
            P2_ROUND_WINS = 0x19BB4
        }
    }



    /*
    class GameDataOffsets { //some of these may seem like they are player related, but they aren't part of the player data objects since their addresses don't align
        
        
        public const uint FRAME_COUNT = 0x19AD0;
        public const uint P1_ROUND_WINS = 0x19AEC;
        public const uint P2_ROUND_WINS = 0x19BB4;
        public const uint P1_FACING_DIRECTION = 0x0AC4;
        
    }*/
}
