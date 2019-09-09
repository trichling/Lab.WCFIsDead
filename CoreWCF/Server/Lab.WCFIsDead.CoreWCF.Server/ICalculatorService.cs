using System;
using System.Runtime.Serialization;
using CoreWCF;

// Namespace in Server und Client muss gleich sein, sonst klappt das Deserialsieren bei TCP nicht (binary!!)
namespace Lab.WCFIsDead.CoreWCF.Shared.Contract
{
    [ServiceContract(CallbackContract = typeof(IRandomNumber))]
    public interface ICalculatorService
    {

        [OperationContract]
        CalculationResult Execute(Calculation calculation);

        [OperationContract]
        void GenerateRandomNumbers(Guid requestId, int count, int delayInMs);

    }

    [ServiceContract]
    public interface IRandomNumber
    {

        [OperationContract(IsOneWay = true)]
        void Receive(Guid requestId, double randomNumber);

    }

    [DataContract]
    public class Calculation
    {
        [DataMember]
        public decimal Operand1 { get; set; }
        [DataMember]
        public decimal Operand2 { get; set; }
        [DataMember]
        public string Opertor { get; set; }
    }

    [DataContract]
    public class CalculationResult
    {
        [DataMember]
        public Calculation Calculation { get; set; }

        [DataMember]
        public decimal Result { get; set; }

    }
}
