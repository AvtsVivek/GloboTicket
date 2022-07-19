using GloboTicket.Integration.Messages;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace GloboTicket.Integration.MessagingBus
{
    public class AzServiceBusMessageBus : IMessageBus
    {
        //TODO: read from settings
        //private string serviceBusConnectionString = "Endpoint=sb://mewurksb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=rozpFNEuYQNr5jkYB48DhaplinrtO7AkLy2F8A+54pU=";
        private readonly string serviceBusConnectionString = string.Empty;
        private readonly IConfiguration _configuration;
        public AzServiceBusMessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            if (string.IsNullOrWhiteSpace(serviceBusConnectionString))
            {
                Debugger.Break();
                throw new ArgumentNullException(serviceBusConnectionString);
            }
        }

        public async Task PublishMessage(IntegrationBaseMessage message, string topicName)
        {
            ISenderClient topicClient = new TopicClient(serviceBusConnectionString, topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await topicClient.SendAsync(serviceBusMessage);
            Console.WriteLine($"Sent message to {topicClient.Path}");
            await topicClient.CloseAsync();
        }
    }
}
