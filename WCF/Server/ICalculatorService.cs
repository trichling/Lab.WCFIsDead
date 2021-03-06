﻿using System;
using System.Runtime.Serialization;

#if WCF
using System.ServiceModel;
#endif

#if COREWCF
using CoreWCF;
#endif

namespace Lab.WCFIsDead.WCF.Server
{
    [ServiceContract]
    public interface ICalculatorService
    {
        [OperationContract]
        CalculationResult Execute(Calculation calculation);
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
