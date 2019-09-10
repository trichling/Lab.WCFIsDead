using Lab.WCFIsDead.WCF.Client.CalculatorClient;
using Lab.WCFIsDead.WCF.Client.RandomNumberClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Calculation = Lab.WCFIsDead.WCF.Client.CalculatorClient.Calculation;
using CalculatorServiceClient = Lab.WCFIsDead.WCF.Client.CalculatorClient.CalculatorServiceClient;

namespace Lab.WCFIsDead.WCF.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var calulatorClient = new CalculatorServiceClient("BasicHttpBinding_ICalculatorService");

            var result = calulatorClient.Execute(new Calculation()
            {
                Operand1 = 1,
                Operand2 = 2,
                Opertor = "+"
            });

            Console.WriteLine(result.Result);

            var randomNumberClient = new RandomNumberGeneratorClient(new InstanceContext(new RandomNumberReceiver()));

            randomNumberClient.GenerateRandomNumbers(Guid.NewGuid(), 10, 5000);
            randomNumberClient.GenerateRandomNumbers(Guid.NewGuid(), 20, 2500);

            Console.ReadLine();

        }
    }

    public class RandomNumberReceiver : IRandomNumberGeneratorCallback
    {
        public void Receive(Guid requestId, double randomNumber)
        {
            Console.WriteLine($"{requestId}: {randomNumber}");
        }
    }
}
