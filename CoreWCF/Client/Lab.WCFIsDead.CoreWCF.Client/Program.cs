using Lab.WCFIsDead.CoreWCF.Shared.Contract;
using System;
using System.ServiceModel;

namespace Lab.WCFIsDead.CoreWCF.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CallViaTcp();
        }

        private static void CallViaBasicHttp()
        {
            var factory = new ChannelFactory<ICalculatorService>(new BasicHttpBinding(), new EndpointAddress("http://localhost:8080/basichttp"));
            factory.Open();
            var channel = factory.CreateChannel();
            ((IClientChannel)channel).Open();

            var result = channel.Execute(new Calculation()
            {
                Operand1 = 1,
                Operand2 = 2,
                Opertor = "+"
            });

            ((IClientChannel)channel).Close();
            factory.Close();
        }

        private static void CallViaTcp()
        {
            using (var factory = new DuplexChannelFactory<ICalculatorService>(new InstanceContext(new RandomNumberReceiver()), new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8808/nettcp")))
            {
                var channel = factory.CreateChannel();
                ((ICommunicationObject)channel).Open();

                var result = channel.Execute(new Calculation()
                {
                    Operand1 = 1,
                    Operand2 = 2,
                    Opertor = "+"
                });

                Console.WriteLine(result.Result);

                channel.GenerateRandomNumbers(Guid.NewGuid(), 10, 5000);
                channel.GenerateRandomNumbers(Guid.NewGuid(), 20, 2500);

                Console.ReadLine();
            }
        }
    }

    public class RandomNumberReceiver : IRandomNumber
    {
        public void Receive(Guid requestId, double randomNumber)
        {
            Console.WriteLine($"{requestId}: {randomNumber}");
        }
    }
}
