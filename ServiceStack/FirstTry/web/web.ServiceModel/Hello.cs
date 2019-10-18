using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace web.ServiceModel
{
    [Route("/calculations")]
    public class Calculation : IReturn<CalculationResult>
    {
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Operator { get; set; }
    }

    public class CalculationResult
    {
        public Calculation Calculation { get; set; }
        public decimal Result { get; set; }

    }

    [Route("/randomNumbers")]
    public class GenerateRandomNumbers : IReturnVoid
    {
        public string RequestId { get; set; }
        public int Count { get; set; }
        public int DelayInMs { get; set; }
    }
}
