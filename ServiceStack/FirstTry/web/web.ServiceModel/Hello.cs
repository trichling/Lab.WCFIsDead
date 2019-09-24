using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace web.ServiceModel
{
    [DataContract]
    [Route("/hello")]
    [Route("/hello/{Name}")]
    public class Hello : IReturn<HelloResponse>
    {
        public string Name { get; set; }
    }

    [DataContract]
    public class HelloResponse
    {
        public string Result { get; set; }
    }

    [DataContract]
    [Route("/calculations")]
    public class Calculation : IReturn<CalculationResult>
    {
        [DataMember]
        public decimal Operand1 { get; set; }
        [DataMember]
        public decimal Operand2 { get; set; }
        [DataMember]
        public string Operator { get; set; }
    }

    [DataContract]
    public class CalculationResult
    {
        [DataMember]
        public Calculation Calculation { get; set; }

        [DataMember]
        public decimal Result { get; set; }

    }

    [Route("/randomNumbers")]
    public class GenerateRandomNumbers : IReturnVoid
    {
        [DataMember]
        public String RequestId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public int DelayInMs { get; set; }
    }
}
