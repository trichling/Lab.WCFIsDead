using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using web.ServiceModel;

namespace web.ServiceInterface
{
    public class MyServices : Service
    {
        private Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();

        public MyServices()
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });
        }

        public IServerEvents ServerEvents { get; set; }

        public CalculationResult Post(Calculation request)
        {
            return Operations[request.Operator](request);
        }

        public void Any(GenerateRandomNumbers request)
        {
            Task.Run(async () =>
            {
                for (var counter = 0; counter < request.Count; counter++)
                {
                    await Task.Delay(request.DelayInMs);
                    //ServerEvents.NotifySubscription(request.RequestId.ToString(), "", new Random().NextDouble());
                    ServerEvents.NotifyChannel(request.RequestId.ToString(), "", new Random().NextDouble());
                }
            });
        }
    }
}
