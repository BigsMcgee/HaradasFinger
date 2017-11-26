using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Btn = Tekken7.TekkenButton;

namespace Tekken7 {

    class TekkenInput {


        public TekkenInput(uint dirFacing) {
            _pad = new XboxGamePad("InputMap.xml");
            TekkenButton.InitButtons(_pad);
            TekkenDirection.InitDirections(_pad);
            TekkenDirection.UpdateFacing(dirFacing);
        }

        public void UP(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.UP, (button ?? Btn.btn_0));
        }

        public void DOWN(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.DOWN, (button ?? Btn.btn_0));
        }

        public void BACK(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.BACK, (button ?? Btn.btn_0));
        }

        public void FORWARD(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.FORWARD, (button ?? Btn.btn_0));
        }

        public void DF(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.DOWN, TekkenDirection.FORWARD, (button ?? Btn.btn_0));
        }

        public void DB(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.DOWN, TekkenDirection.BACK, (button ?? Btn.btn_0));
        }

        public void UF(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.UP, TekkenDirection.FORWARD, (button ?? Btn.btn_0));
        }

        public void UB(Button button = null, int waitFrames = 1) {
            _pad.SendInput(TekkenDirection.UP, TekkenDirection.BACK, (button ?? Btn.btn_0));
        }

        public void QCF() {

        }

        public void QCB() {

        }

        public void CrouchDash() {

        }

        public void Buttons(params Button[] buttons) {
            _pad.SendInput(buttons);
        }
        public void EMPTY(double waitFrames = 1) {
            _pad.SendInput(TekkenDirection.NEUTRAL, Btn.btn_0);
            InputBuffer.WaitFrames(waitFrames - 1);
        }

        public const double Taunt_JetUpper_Delay = 26.95; //find out if the 5% difference is universal
        public readonly Button emptyButton = new Button("", 0);

        private XboxGamePad _pad;
    }
}
