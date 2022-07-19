using GloboTicket.Integration.MessagingBus;
using GloboTicket.Services.Ordering.Entities;
using GloboTicket.Services.Ordering.Messages;
using GloboTicket.Services.Ordering.Repositories;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GloboTicket.Services.Ordering.Messaging
{
    public class AzServiceBusConsumer: IAzServiceBusConsumer
    {
        private readonly string subscriptionName = "globoticketorder";
        private readonly IReceiverClient checkoutMessageReceiverClient;
        private readonly IReceiverClient orderPaymentUpdateMessageReceiverClient;

        private readonly IConfiguration _configuration;

        private readonly OrderRepository _orderRepository;
        private readonly IMessageBus _messageBus;

        private readonly string checkoutMessageTopic;
        private readonly string orderPaymentRequestMessageTopic;
        private readonly string orderPaymentUpdatedMessageTopic;

        public AzServiceBusConsumer(IConfiguration configuration, 
            IMessageBus messageBus, 
            OrderRepository orderRepository)
        {
            _configuration = configuration;
            _orderRepository = orderRepository;
            // _logger = logger;
            _messageBus = messageBus;

            var serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            checkoutMessageTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            orderPaymentRequestMessageTopic = _configuration.GetValue<string>("OrderPaymentRequestMessageTopic");
            orderPaymentUpdatedMessageTopic = _configuration.GetValue<string>("OrderPaymentUpdatedMessageTopic");

            checkoutMessageReceiverClient = new SubscriptionClient(serviceBusConnectionString, checkoutMessageTopic, subscriptionName);
            orderPaymentUpdateMessageReceiverClient = new SubscriptionClient(serviceBusConnectionString, orderPaymentUpdatedMessageTopic, subscriptionName);
        }

        public void Start()
        {
            var messageHandlerOptions = new MessageHandlerOptions(OnServiceBusException) { MaxConcurrentCalls = 4 };

            checkoutMessageReceiverClient.RegisterMessageHandler(OnCheckoutMessageReceived, messageHandlerOptions);
            orderPaymentUpdateMessageReceiverClient.RegisterMessageHandler(OnOrderPaymentUpdateReceived, messageHandlerOptions);
        }

        private async Task OnOrderPaymentUpdateReceived(Message message, CancellationToken arg2)
        {
            var body = Encoding.UTF8.GetString(message.Body);//json from service bus
            OrderPaymentUpdateMessage orderPaymentUpdateMessage =
                JsonConvert.DeserializeObject<OrderPaymentUpdateMessage>(body);

            await _orderRepository.UpdateOrderPaymentStatus(orderPaymentUpdateMessage.OrderId, orderPaymentUpdateMessage.PaymentSuccess);
        }

        private async Task OnCheckoutMessageReceived(Message message, CancellationToken arg2)
        {
            var body = Encoding.UTF8.GetString(message.Body);//json from service bus

            //save order with status not paid
            BasketCheckoutMessage basketCheckoutMessage = JsonConvert.DeserializeObject<BasketCheckoutMessage>(body);

            Guid orderId = Guid.NewGuid();

            Order order = new()
            {
                UserId = basketCheckoutMessage.UserId,
                Id = orderId,
                OrderPaid = false,
                OrderPlaced = DateTime.Now,
                OrderTotal = basketCheckoutMessage.BasketTotal
            };

            await _orderRepository.AddOrder(order);

            //send order payment request message
            OrderPaymentRequestMessage orderPaymentRequestMessage = new()
            {
                CardExpiration = basketCheckoutMessage.CardExpiration,
                CardName = basketCheckoutMessage.CardName,
                CardNumber = basketCheckoutMessage.CardNumber,
                OrderId = orderId,
                Total = basketCheckoutMessage.BasketTotal
            };

            try
            {
                await _messageBus.PublishMessage(orderPaymentRequestMessage, orderPaymentRequestMessageTopic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private Task OnServiceBusException(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var exceptionMessage = exceptionReceivedEventArgs.Exception.Message;
            var action = exceptionReceivedEventArgs.ExceptionReceivedContext.Action;
            var clientId = exceptionReceivedEventArgs.ExceptionReceivedContext.ClientId;
            var endPoint = exceptionReceivedEventArgs.ExceptionReceivedContext.Endpoint;
            var entityPath = exceptionReceivedEventArgs.ExceptionReceivedContext.EntityPath;

            Console.WriteLine($"exceptionMessage: {exceptionMessage}" + Environment.NewLine +
                $"action: {action}" + Environment.NewLine +
                $"clientId: {clientId}" + Environment.NewLine +
                $"endPoint: {endPoint}" + Environment.NewLine +
                $"entityPath: {entityPath}" + Environment.NewLine +
                $"Check the connection string to azure service bus." + Environment.NewLine +
                $"If needed, visit the following url to genereate the credentials" + Environment.NewLine +
                $"https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-quickstart-topics-subscriptions-portal#get-the-connection-string" + Environment.NewLine +
                $"Still errors? Visit the following url." + Environment.NewLine + 
                @"https://www.code4it.dev/blog/azure-service-bus-errors" 
                );

            return Task.CompletedTask;
        }

        public void Stop()
        {
        }
    }
}
