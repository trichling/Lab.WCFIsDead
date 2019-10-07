using Lab.WCFIsDead.ZeroMQ.Shared;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Text.Json;

namespace Lab.WCFIsDead.ZeroMQ.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new RequestSocket())
            {
                client.Connect("tcp://localhost:5555");
                for (int i = 0; i < 10; i++)
                {
                    var calculation = new Calculation()
                    {
                        Operand1 = i,
                        Operand2 = 2 * i,
                        Operator = "+"
                    };

                    var calculationMessage = JsonSerializer.Serialize(calculation);

                    Console.WriteLine("Sending {0}", calculationMessage);
                    client.SendFrame(calculationMessage);

                    var message = client.ReceiveFrameString();
                    Console.WriteLine("Received {0}", message);
                    var result = JsonSerializer.Deserialize<CalculationResult>(message);

                    Console.WriteLine(result.Result);
                }
            }
        }
    }
}
