using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Tekken7;

namespace HaradasFinger
{
    class FrameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { }; //?

        public FrameViewModel() {
            _dataController = TekkenDataController.Instance;
            _player1FrameAdvantage = ((Int64)0).ToString();
            _player2FrameAdvantage = ((Int64)0).ToString();
            _testBullshit1 = -69;
            _testBullshit2 = 69;
            //TEST
            Task.Run(async () => {
                Int64 i = 0;
                while (true) {
                    _dataController.Update();
                    Player1FrameAdvantage = _dataController.TestGetLastMove().ToString();
                    await Task.Delay(67);
                }
            }
            );
        }

        //Display Properties
        public string Player1FrameAdvantage {
            get {
                if (Int64.Parse(_player1FrameAdvantage) > -1)
                    return "+" + _player1FrameAdvantage;
                else
                    return _player1FrameAdvantage;
            } set {
                if (value == _player1FrameAdvantage)
                    return;

                _player1FrameAdvantage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Player1FrameAdvantage)));
            }
        }

        public string Player2FrameAdvantage {
            get {
                if (Int64.Parse(_player2FrameAdvantage) > -1)
                    return "+" + _player2FrameAdvantage.ToString();
                else
                    return _player2FrameAdvantage;
            }
            set {
                if (value == _player2FrameAdvantage)
                    return;

                _player2FrameAdvantage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Player2FrameAdvantage)));
            }
        }

        public int TestBullshit1 {
            get {
                return _testBullshit1;
            }
        }

        public int TestBullshit2 {
            get {
                return _testBullshit2;
            }
        }

        private string _player1FrameAdvantage;
        private string _player2FrameAdvantage;

        private int _testBullshit1;
        private int _testBullshit2;
        TekkenDataController _dataController;
    }
}
