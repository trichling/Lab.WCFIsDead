using Lab.WCFIsDead.WCF.Client.CalculatorClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.WCF.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var calulatorClient = new CalculatorServiceClient(new InstanceContext(new RandomNumberReceiver()));

            var result = calulatorClient.Execute(new Calculation()
            {
                Operand1 = 1,
                Operand2 = 2,
                Opertor = "+"
            });

            Console.WriteLine(result.Result);

            calulatorClient.GenerateRandomNumbers(Guid.NewGuid(), 10, 5000);
            calulatorClient.GenerateRandomNumbers(Guid.NewGuid(), 20, 2500);

            Console.ReadLine();

        }
    }

    public class RandomNumberReceiver : ICalculatorServiceCallback
    {
        public void Receive(Guid requestId, double randomNumber)
        {
            Console.WriteLine($"{requestId}: {randomNumber}");
        }
    }
}
