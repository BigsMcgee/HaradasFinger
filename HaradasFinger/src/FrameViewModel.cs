using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Tekken7;
using System.Speech.Synthesis;
using NLog;

namespace HaradasFinger
{
    class FrameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { }; //?

        public FrameViewModel() {
            _dataController = TekkenDataController.Instance;
            _logger = LogManager.GetCurrentClassLogger();
            _player1FrameAdvantage = ((Int64)0).ToString();
            _player2FrameAdvantage = ((Int64)0).ToString();
            //TEST
            Task.Run(async () => {
                Int64 i = 0;
                while (true) {
                    await Task.Delay(67);
                    try {
                        _dataController.Update();
                    } catch (Exception ex) {
                        _logger.Trace(ex.ToString());
                    }
                    Player1FrameAdvantage = _dataController.GetP1LastMove().ToString();//_dataController.Player1FrameAdvantage.ToString();
                    Player2FrameAdvantage = _dataController.GetP2LastMove().ToString();//_dataController.Player2FrameAdvantage.ToString();
                    Player1Startup = _dataController.P1Startup;
                    Player2Startup = _dataController.P2Startup;
                    LatestFrameNum = _dataController.CurrentFrameNum;
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
                //_speech.Speak(Player1FrameAdvantage);
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

        public uint Player1Startup {
            get {
                return _player1Startup;
            } set {
                if (value == _player1Startup)
                    return;

                _player1Startup = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Player1Startup)));
            }
        }

        public uint Player2Startup {
            get {
                return _player2Startup;
            }
            set {
                if (value == _player2Startup)
                    return;

                _player2Startup = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Player2Startup)));
            }
        }

        public uint LatestFrameNum {
            get {
                return _latestFrameNum;
            } set {
                if (value == _latestFrameNum)
                    return;

                _latestFrameNum = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LatestFrameNum)));
            }
        }

        private string _player1FrameAdvantage;
        private string _player2FrameAdvantage;
        private uint _player1Startup;
        private uint _player2Startup;
        private uint _latestFrameNum;

        TekkenDataController _dataController;
        private SpeechSynthesizer _speech;

        private Logger _logger;
    }
}
