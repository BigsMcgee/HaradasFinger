using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    static class TekkenButton {

        public static void InitButtons (XboxGamePad pad) {
            btn_0 = new Button("zero",  0); 
            btn_1 = XboxGamePad.X;
            btn_2 = XboxGamePad.Y;
            btn_3 = XboxGamePad.A;
            btn_4 = XboxGamePad.B;
            rage_art = XboxGamePad.RB;
        }

        public static Button BTN_1 {
            get { return btn_1; }
        }

        public static Button BTN_2 {
            get { return btn_2; }
        }

        public static Button BTN_3 {
            get { return btn_3; }
        }

        public static Button BTN_4 {
            get { return btn_4; }
        }

        public static Button BTN_1_2 {
            get { return (btn_1 | btn_2); }
        }

        public static Button BTN_3_4 {
            get { return (btn_3 | btn_4); }
        }

        public static Button RAGE_ART {
            get { return rage_art; }
        }

        private static Button btn_1;
        private static Button btn_2;
        private static Button btn_3;
        private static Button btn_4;
        private static Button rage_art;

        public static Button btn_0;
    }
}
