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
}
