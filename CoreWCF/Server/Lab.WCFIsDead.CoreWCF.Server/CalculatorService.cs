using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreWCF;
using Lab.WCFIsDead.CoreWCF.Shared.Contract;

namespace Lab.WCFIsDead.CoreWCF.Server
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
