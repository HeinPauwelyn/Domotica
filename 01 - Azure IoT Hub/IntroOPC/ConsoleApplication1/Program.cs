using Microsoft.Azure.Devices.Client;
using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Client.Exceptions;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "domotica-howest.azure-devices.net";
        static string deviceKey = "4L5XrntzEND+biZaYtLD9KIlhqBurqbPAzGk2ltBMNo=";

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Hein", deviceKey));

            while (Console.ReadLine() == "")
            {
                EasyDAClient client = new EasyDAClient();
                string data = client.ReadItem("", "Kepware.KEPServerEX.V5", "Hein_Kanaal.Hein_Device.Hein_Groep.Mijn_Ramp").ToString();

                SendDeviceToCloudMessagesAsync(data);
            }

            //client.OpenAsync();
        }

        private static async void SendDeviceToCloudMessagesAsync(string data)
        {
            var telemetryDataPoint = new
            {
                DeviceId = "Hein",
                Data = data
            };

            string messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            Microsoft.Azure.Devices.Client.Message message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

            Task.Delay(1000).Wait();
        }
    }
}
