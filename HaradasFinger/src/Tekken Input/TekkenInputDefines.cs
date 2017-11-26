using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class TekkenInputDefines {
        //BASE DEFINES
        public const uint NEUTRAL = 0x00;
        public const uint LEFT = 0x01;
        public const uint RIGHT = 0x02;
        public const uint UP = 0x04;
        public const uint DOWN = 0x08;

        public const uint BUTTON_ONE = 0x10;
        public const uint BUTTON_TWO = 0x20;
        public const uint BUTTON_THREE = 0x40;
        public const uint BUTTON_FOUR = 0x80;

        public const uint ONE_TWO = BUTTON_ONE | BUTTON_TWO;
        public const uint ONE_THREE = BUTTON_ONE | BUTTON_THREE;
        public const uint ONE_FOUR = BUTTON_ONE | BUTTON_FOUR;
        public const uint TWO_THREE = BUTTON_TWO | BUTTON_THREE;
        public const uint TWO_FOUR = BUTTON_TWO | BUTTON_FOUR;
        public const uint THREE_FOUR = BUTTON_THREE | BUTTON_FOUR;

        public const uint ONE_TWO_THREE = ONE_TWO | BUTTON_THREE;
        public const uint ONE_TWO_FOUR = ONE_TWO | BUTTON_FOUR;
        public const uint ONE_THREE_FOUR = ONE_THREE | BUTTON_FOUR;
        public const uint TWO_THREE_FOUR = TWO_THREE | BUTTON_FOUR;

        public const uint ONE_TWO_THREE_FOUR = ONE_TWO | THREE_FOUR;
    }
}
