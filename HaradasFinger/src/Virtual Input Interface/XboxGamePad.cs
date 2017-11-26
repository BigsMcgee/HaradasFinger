using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class XboxGamePad /* : IInputDevice*/ {
        public XboxGamePad(string mapPath) {
            Init(mapPath);
        }

        public bool Init(string mapPath) {
            bool success = true;
            
            if (LoadMap(mapPath)) {
                if (!MapInputs()) {
                    success = false;
                }
                PlugInDevice();
            } else {
                success = false;
            }

            return success;
        }

        public void SendInput(params InputItem[] input) { //TODO: Figure out how to reincorporate "waitFrames"
            double waitFrames = 1;
            uint testVal = input.Aggregate((uint)0, (val, next) => (val | next.Value));
            _inputBuffer.WriteToMap(testVal, waitFrames);
        }

        private bool MapInputs() {
            bool success = true;

            if (!MapInputFromDescription(nameof(LEFT), out LEFT)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(RIGHT), out RIGHT)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(UP), out UP)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(DOWN), out DOWN)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(X), out X)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(Y), out Y)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(A), out A)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(B), out B)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(START), out START)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(SELECT), out SELECT)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(LB), out LB)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(RB), out RB)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(LT), out LT)) {
                success = false;
            }
            if (!MapInputFromDescription(nameof(RT), out RT)) {
                success = false;
            }

            NEUTRAL = new Direction("NEUTRAL", 0);

            return success;
        }

        private bool MapInputFromDescription(string description, out Button inputCmd) {
            bool success = false;
            InputItem value;
            inputCmd = null;

            if (_inputDict.TryGetValue(description, out value)) {
                inputCmd = (Button)value;
                success = true;
            }

            return success;
        }

        private bool MapInputFromDescription(string description, out Direction inputCmd) {
            bool success = false;
            InputItem value;
            inputCmd = null;

            if (_inputDict.TryGetValue(description, out value)) {
                inputCmd = (Direction)value;
                success = true;
            }

            return success;
        }

        private bool LoadMap(string mapPath) {

            try {
                InputMap.LoadInputMap(mapPath, out _inputDict, out connectionName);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }

        private void PlugInDevice() {
            _inputBuffer = new InputBuffer(connectionName);
        }

        #region FIELDS
        private Dictionary<string, InputItem> _inputDict;
        private InputBuffer _inputBuffer;
        private string connectionName;
        #endregion

        #region Input Constants

        public static Direction NEUTRAL;

        public static Direction LEFT;
        public static Direction RIGHT;
        public static Direction UP;
        public static Direction DOWN;

        public static Button X;
        public static Button Y;
        public static Button A;
        public static Button B;

        public static Button START;
        public static Button SELECT;
        public static Button LB;
        public static Button RB;
        public static Button LT;
        public static Button RT;

        #endregion
    }
}
