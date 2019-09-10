using System;

#if WCF
using System.ServiceModel;
#endif

#if COREWCF
using CoreWCF;
#endif

namespace Lab.WCFIsDead.WCF.Server
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
