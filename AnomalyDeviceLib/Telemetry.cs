namespace SimulatedDevice
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;
    using AnomalyDeviceLib;

    public class Telemetry
    {
        private const string IotHubUri = "hton.azure-devices.net";

        private const string DeviceKey = "lZvsotUeMRZ8pyYM+WLxuQZDtRvMKVSckU6Gp9IBaVY=";
        private const string DeviceId = "myFirstDevice";
        //private const string DeviceKey = "TT+24WpGw+WDfSGlbhs065TFTxFD78Rx5BhAEoyAS0Q=";
        //private const string DeviceId = "anomaliDevice"; // det är den andra devicen jag skapade
        private const double MinTemperature = 20;
        private const double TempDeviation = 2;

        private const double MinHumidity = 60;
        private static readonly Random Rand = new Random();
        private static DeviceClient _deviceClient;
        private static int _messageId = 1;

        private static async void SendDeviceToCloudMessagesAsync(IValueContainer valueContainer)
        {
            while (true)
            {
                try{

                    var currentTemperature = valueContainer.Value + Rand.NextDouble() * valueContainer.Deviation;
                    var currentHumidity = MinHumidity + Rand.NextDouble() * 20;
                    
                    await SendTelemetryData(currentTemperature, currentHumidity);

                    valueContainer.Pingback(currentTemperature);

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        private static async Task SendTelemetryData(double currentTemperature, double currentHumidity)
        {
            var telemetryDataPoint = new
            {
                messageId = _messageId++,
                deviceId = DeviceId,
                temperature = currentTemperature,
                humidity = currentHumidity
            };
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await _deviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
        }

        public static void Go(IValueContainer valueContainer)
        {
            Console.WriteLine("Anomali device\n");
            _deviceClient = DeviceClient.Create(IotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, DeviceKey), TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync(valueContainer);
            Console.ReadLine();
        }
    }
}
