using System;
using CoreWCF;

// Namespace in Server und Client muss gleich sein, sonst klappt das Deserialsieren bei TCP nicht (binary!!)
namespace Lab.WCFIsDead.CoreWCF.Shared.Contract
{
    [ServiceContract(CallbackContract = typeof(IRandomNumber))]
    public interface IRandomNumberGenerator
    {


        [OperationContract]
        void GenerateRandomNumbers(Guid requestId, int count, int delayInMs);

    }

    [ServiceContract]
    public interface IRandomNumber
    {

        [OperationContract(IsOneWay = true)]
        void Receive(Guid requestId, double randomNumber);

    }
}
