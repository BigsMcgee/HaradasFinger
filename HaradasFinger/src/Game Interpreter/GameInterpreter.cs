using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Tekken7;
using Memory;

namespace HaradasFinger {
    class GameInterpreter {
        ///Main controller for interpreting frame data into discrete game events. The biggest goal for this class is to be able to translate in game data into IGameAction objects
        ///Ideas:
        ///     -Use objects like "Movement Interpreter", "Attack Interpreter", and "Game State Interpreter". How would I dispatch to these?
        ///     -They are going to be "readonly" in regards to the frame collection and they will be posting events in order to communicate
        ///     -Try running this object on its own thread and then running the other interpreter routines as async
        ///     -How do I know when the frame list has new data to be parsed?

        public GameInterpreter(TekkenFrameCollection frameList) {
            //stuff
            _frameList = frameList;
            _log = LogManager.GetCurrentClassLogger();
            _interpreters = new List<IInterpreter>();
            //InitializeInterpreters();
        }

        public void UpdateActiveInterpreters() {
            //stuff
            _attackInterpreter.Update();
        }

        /// <summary>
        /// Initializes each individual interpreter object as needed and then adds it to the list
        /// </summary>
        /// <returns>the number of objects initialized and added to the list</returns>
        public int InitializeInterpreters() {
            int num = 0;

            _attackInterpreter = new AttackInterpreter(_frameList);
            _interpreters.Add(_attackInterpreter);

            return num;
        }

        public void Reset() {
            _attackInterpreter.Reset();
        }

        /// <summary>
        /// Stored reference to the frame collection, not owned by this object
        /// </summary>
        private TekkenFrameCollection _frameList;

        /// <summary>
        /// List of IInterpreter objects to be used when interpreting game data
        /// </summary>
        private List<IInterpreter> _interpreters;

        /// <summary>
        /// current class logger
        /// </summary>
        private static Logger _log;

        private AttackInterpreter _attackInterpreter;
    }
}
