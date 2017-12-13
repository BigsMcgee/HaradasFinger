using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaradasFinger {
    interface IInterpreter {
        /// <summary>
        /// Consumes any new information received
        /// </summary>
        void Update();

        /// <summary>
        /// Resets the interpreter to default state
        /// </summary>
        void Reset();
    }
}
