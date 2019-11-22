using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Lab.WCFIsDead.gRPC.Shared;

namespace Lab.WCFIsDead.gRPC.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var calclulationClient =  new Calculator.CalculatorClient(channel);
            var calclulationReply = calclulationClient.Execute(new Calculation() { Operator = "+", Operand1 = 1, Operand2 = 2 });
            Console.WriteLine("Result: " + calclulationReply.Result);
            Console.WriteLine("Press any key to get random numbers...");
            Console.ReadKey();

            var randomNumberClient = new RandomNumberGenerator.RandomNumberGeneratorClient(channel);
            var randomNumberStream1 = randomNumberClient.GenerateRandomNumbers(new RandomNumberRequest() 
            { 
                RequestId = Guid.NewGuid().ToString(), DelayInMs = 1000, Count = 10
            });

            var randomNumberStream2 = randomNumberClient.GenerateRandomNumbers(new RandomNumberRequest() 
            { 
                RequestId = Guid.NewGuid().ToString(), DelayInMs = 2000, Count = 5
            });
            
            ReadAllRandomNumbers(randomNumberStream1);
            ReadAllRandomNumbers(randomNumberStream2);

            Console.ReadKey();
        }

        private static async Task ReadAllRandomNumbers(AsyncServerStreamingCall<RandomNumberResponse> response)
        {
            await foreach (var randomNumber in response.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"{randomNumber.RequestId}: {randomNumber.RandomNumber}");
            }
        }
    }
}
