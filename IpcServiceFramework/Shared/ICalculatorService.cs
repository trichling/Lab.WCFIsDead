using System;
using System.Runtime.Serialization;

// Namespace in Server und Client muss gleich sein, sonst klappt das Deserialsieren bei TCP nicht (binary!!)
namespace Lab.WCFIsDead.IpcServiceFramework.Shared
{
    public interface ICalculatorService
    {

        CalculationResult Execute(Calculation calculation);

    }

    public class Calculation
    {
        public decimal Operand1 { get; set; }
        public decimal Operand2 { get; set; }
        public string Opertor { get; set; }
    }

    public class CalculationResult
    {
        public Calculation Calculation { get; set; }

        public decimal Result { get; set; }

    }
}
