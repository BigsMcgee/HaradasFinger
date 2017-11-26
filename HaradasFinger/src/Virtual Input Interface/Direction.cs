using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekken7 {
    class Direction : InputItem{
        public Direction (string desc, uint index)
            :base(desc, index) {
        }
       
        #region COMMON OVERLOADS
        public override string ToString() => this.Name;

        public static Direction operator |(Direction direction1, Direction direction2) =>
            new Direction(direction1.Name + "_" + direction2.Name, direction1.Value | direction2.Value);

        //Could possibly need to use the name propery to check for equality as well
        public static bool operator ==(Direction direction1, Direction button2) =>
            (direction1.Value == button2.Value);

        public static bool operator !=(Direction direction1, Direction button2) =>
            (direction1.Value == button2.Value);

#endregion
    }
}
