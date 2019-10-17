using Lab.WCFIsDead.AzureServiceBus.Shared;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.AzureServiceBus.Client
{
    class Program
    {
        
        static IQueueClient requestQueue;
        static IQueueClient responseQueue;

        static async Task Main(string[] args)
        {
            requestQueue = new QueueClient(Constants.ServiceBusConnectionString, Constants.RequestQueueName);
            responseQueue = new QueueClient(Constants.ServiceBusConnectionString, Constants.ResponseQueueName);

            responseQueue.RegisterMessageHandler(ProcessMessagesAsync, new MessageHandlerOptions(ExceptionReceivedHandler));

            for (int i = 0; i < 1; i++)
            {
                var calculation = new Calculation()
                {
                    Operand1 = i,
                    Operand2 = 2 * i,
                    Operator = "+"
                };

                var messageBody = JsonConvert.SerializeObject(calculation);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                Console.WriteLine($"Sending message: {messageBody}");

               await requestQueue.SendAsync(message);
            }

            Console.ReadLine();

            await requestQueue.CloseAsync();
            await responseQueue.CloseAsync();
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            var calculationResult = JsonConvert.DeserializeObject<CalculationResult>(messageBody);

            Console.WriteLine(calculationResult.Result);

            await responseQueue.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
