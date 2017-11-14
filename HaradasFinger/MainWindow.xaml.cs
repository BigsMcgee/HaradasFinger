using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HaradasFinger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetWindowPositionAndSize();
            ///LIST OF POTENTIAL FEATURES
            //Current Frame Counter, not super useful to the user but it would give me an idea of the pace the data is being read
            //Attack Startup
            //Hit Level
            //"Latest" throw break.
            //Change the font color depending on how safe or unsafe a move is. plus frames, safe but negative, unsafe, launch punishable?
            //
            //For the data controller, maybe keep track of how many frames in between attack strings. This could be a good way to predicting button presses by opponent
            //Along the same lines, keep track of the distance at which a button is pressed.
            //Find a way to index sequences of attacks.
            ///
        }

        public void SetWindowPositionAndSize()
        {
            Height = System.Windows.SystemParameters.PrimaryScreenHeight / 10;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
        }
    }
}
