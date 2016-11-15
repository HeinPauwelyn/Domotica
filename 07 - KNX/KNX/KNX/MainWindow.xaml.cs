using KNXDemo;
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

namespace KNX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KNXCommunicator _communicator;

        public MainWindow()
        {
            InitializeComponent();

            _communicator = new KNXCommunicator("172.23.49.0", 1289, lstOntvangen);
            _communicator.RunClient();
            _communicator.KNXMessage += _communicator_KNXMessage;
        }

        private void _communicator_KNXMessage(object sender, KNXMessageEventArgs e)
        {
            lstOntvangen.Items.Add(e.Message);
        }
    }
}
