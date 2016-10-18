using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using OpcLabs.EasyOpc.DataAccess;
using OpcLabs.EasyOpc;

namespace LichtAanUit.server
{
    class Program
    {
        static void Main(string[] args)
        {
            // create client instance
            MqttClient client = new MqttClient(IPAddress.Parse("172.23.81.14"));

            // register to message received
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2
            client.Subscribe(new string[] { "/sensors/lamp" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string bericht = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine(bericht);

            EasyDAClient daClient = new EasyDAClient();

            if (bericht.ToLower() == "aan")
            {
                daClient.WriteItemValue("", "Kepware.KEPServerEX.V5", "Ethernet.PCL3.Lamp", true);
            }
            else
            {
                daClient.WriteItemValue("", "Kepware.KEPServerEX.V5", "Ethernet.PCL3.Lamp", false);
            }

            
        }
    }
}
