using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class Button : InputItem {

        public Button(string desc, uint index)
            :base(desc, index) {
            _isPressed = false;
        }

        #region COMMON OVERLOADS
        public override string ToString() => this.Name;

        public static Button operator | (Button button1, Button button2) =>
            new Button(button1.Name + "_" + button2.Name, button1.Value | button2.Value);

        //Could possibly need to use the name propery to check for equality as well
        public static bool operator ==(Button button1, Button button2) =>
            (button1.Value == button2.Value);

        public static bool operator !=(Button button1, Button button2) =>
            (button1.Value == button2.Value);

        #endregion

        public bool IsPressed {
            get { return _isPressed; }
        }

        private bool _isPressed;
    }
}
