using Lab.WCFIsDead.gRPC.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.gRPC.Server.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        private Dictionary<string, Func<Calculation, CalculationResult>> Operations = new Dictionary<string, Func<Calculation, CalculationResult>>();

        public CalculatorService()
        {
            Operations.Add("+", c => new CalculationResult() { Calculation = c, Result = c.Operand1 + c.Operand2 });
            Operations.Add("-", c => new CalculationResult() { Calculation = c, Result = c.Operand1 - c.Operand2 });
            Operations.Add("*", c => new CalculationResult() { Calculation = c, Result = c.Operand1 * c.Operand2 });
            Operations.Add("/", c => new CalculationResult() { Calculation = c, Result = c.Operand1 / c.Operand2 });
        }

        public override Task<CalculationResult> Execute(Calculation request, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(Operations[request.Operator](request));
        }
        

    }
}