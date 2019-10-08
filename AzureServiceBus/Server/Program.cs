using Lab.WCFIsDead.AzureServiceBus.Shared;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.AzureServiceBus.Server
{
    class Program
    {

        static IQueueClient requestQueue;
        static IQueueClient responseQueue;
        private static Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();


        static async Task Main(string[] args)
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });


            requestQueue = new QueueClient(Constants.ServiceBusConnectionString, Constants.RequestQueueName);
            responseQueue = new QueueClient(Constants.ServiceBusConnectionString, Constants.ResponseQueueName);

            requestQueue.RegisterMessageHandler(ProcessMessagesAsync, new MessageHandlerOptions(ExceptionReceivedHandler));

            Console.ReadKey();

            await requestQueue.CloseAsync();
            await responseQueue.CloseAsync();
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var messageBody = Encoding.UTF8.GetString(message.Body);
            var calculation = JsonConvert.DeserializeObject<Calculation>(messageBody);

            var result = Operations[calculation.Operator](calculation);

            await requestQueue.CompleteAsync(message.SystemProperties.LockToken);

            var responseBody = JsonConvert.SerializeObject(result);
            var response = new Message(Encoding.UTF8.GetBytes(responseBody));
            Console.WriteLine($"Sending message: {responseBody}");

            await responseQueue.SendAsync(response);
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
