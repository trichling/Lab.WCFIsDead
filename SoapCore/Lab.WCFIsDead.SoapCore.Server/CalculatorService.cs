using Lab.WCFIsDead.SoapCore.Shared.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.SoapCore.Server
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

    }
}
