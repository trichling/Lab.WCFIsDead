using Lab.WCFIsDead.ZeroMQ.Shared;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Lab.WCFIsDead.ZeroMQ.Server
{
    class Program
    {

        private static Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();

        static void Main(string[] args)
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });


            using (var server = new ResponseSocket())
            {
                server.Bind("tcp://*:5555");
                while (true)
                {
                    var message = server.ReceiveFrameString();
                    Console.WriteLine("Received {0}", message);

                    var calculation = System.Text.Json.JsonSerializer.Deserialize<Calculation>(message);
                    var result = Operations[calculation.Operator](calculation);

                    var resultMessage = JsonSerializer.Serialize(result);
                    Console.WriteLine("Sending {0}", resultMessage);
                    server.SendFrame(resultMessage);
                }
            }
        }
    }
}
