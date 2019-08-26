using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.WCF.Server
{
    public class CalculatorService : ICalculatorService
    {
        private Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();

        public CalculatorService()
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });
        }

        public CalculationResult Execute(Calculation calculation)
        {
            return Operations[calculation.Opertor](calculation);
        }

        public void GenerateRandomNumbers(Guid requestId, int count, int delayInMs)
        {
            var receiver = OperationContext.Current.GetCallbackChannel<IRandomNumber>();
            Task.Run(async () =>
            {
                for (var counter = 0; counter < count; counter++)
                {
                    await Task.Delay(delayInMs);
                    receiver.Receive(requestId, new Random().NextDouble());
                }
            });
        }
    }
}
