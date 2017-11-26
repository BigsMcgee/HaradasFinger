using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class TekkenDirection {

        public static void InitDirections(XboxGamePad pad) {
            neutral = XboxGamePad.NEUTRAL;
            left = XboxGamePad.LEFT;
            right = XboxGamePad.RIGHT;
            up = XboxGamePad.UP;
            down = XboxGamePad.DOWN;
        }

        public static void UpdateFacing(uint characterDirection) {
            if (characterDirection == 0) { //on the left facing right
                forward = right;
                back = left;
            } else {
                forward = left;
                back = right;
            }
        }

        public static Direction FORWARD {
            get { return forward; }
        }
        public static Direction BACK {
            get { return back; }
        }
        public static Direction UP {
            get { return up; }
        }
        public static Direction DOWN {
            get { return down; }
        }
        public static Direction DOWN_FORWARD {
            get { return (down | forward); }
        }
        public static Direction DOWN_BACK {
            get { return (down | back); }
        }
        public static Direction UP_FORWARD {
            get { return up | forward; }
        }
        public static Direction UP_BACK {
            get { return up | back; }
        }
        public static Direction NEUTRAL {
            get { return neutral; }
        }

        private static Direction forward;
        private static Direction back;

        private static Direction left;
        private static Direction right;
        private static Direction up;
        private static Direction down;

        private static Direction neutral;
    }
}
