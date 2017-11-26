using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    abstract class InputItem {

        public InputItem(string desc, uint index) {
            _description = desc;
            _index = index;
        }

        public static UInt32 operator |(InputItem item1, InputItem item2) {
                return (item1.Value | item2.Value);
        }

        public static UInt32 operator +(InputItem item1, InputItem item2) {
            return item1.Value + item2.Value;
        }
            

        public uint Value {
            get { return ((uint)0x01 << (int)(_index - 1)); }
        }

        public uint Index {
            get { return _index; }
        }

        public string Name {
            get { return _description; }
        }

        private uint _index;
        private string _description;
    }
}
