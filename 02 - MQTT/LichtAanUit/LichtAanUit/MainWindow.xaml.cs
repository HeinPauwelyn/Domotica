using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace LichtAanUit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Licht.Checked += Licht_Checked;
            Licht.Unchecked += Licht_Checked;
        }

        private void Licht_Checked(object sender, RoutedEventArgs e)
        {
            // create client instance
            MqttClient client = new MqttClient(IPAddress.Parse("172.23.81.14"));

            // register to message received

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            
            if (Licht.IsChecked == true)
            {
                // publish a message on "/home/temperature" topic with QoS 2
                client.Publish("/sensors/lamp", Encoding.UTF8.GetBytes("Aan"));//, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);
            }
            else
            {
                // publish a message on "/home/temperature" topic with QoS 2
                client.Publish("/sensors/lamp", Encoding.UTF8.GetBytes("Uit"));//, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE);
            }
        }
    }
}
