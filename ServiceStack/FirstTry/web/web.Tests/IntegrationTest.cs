using Funq;
using ServiceStack;
using NUnit.Framework;
using web.ServiceInterface;
using web.ServiceModel;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace web.Tests
{
    public class IntegrationTest
    {
        const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost appHost;

        class AppHost : AppSelfHostBase
        {
            public AppHost() : base(nameof(IntegrationTest), typeof(MyServices).Assembly) { }

            public override void Configure(Container container)
            {
                Plugins.Add(new ServerEventsFeature());
            }
        }

        public IntegrationTest()
        {
            appHost = new AppHost()
                .Init()
                .Start(BaseUri);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        public IServiceClient CreateClient() => new JsonServiceClient(BaseUri);

        public ServerEventsClient CreateServerEventsClient() => new ServerEventsClient(BaseUri);

        [Test]
        public void Can_call_Hello_Service()
        {
            var client = CreateClient();

            var response = client.Post(new Calculation { Operand1 = 1, Operand2 = 2, Operator = "+" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }

        [Test]
        public async Task Can_ReceiveServerEvents()
        {
            var client = CreateServerEventsClient();

            var connectMsg = await client.Connect();
            var joinMsg = await client.WaitForNextCommand();

            var firstSequence = Guid.NewGuid().ToString();
            client.SubscribeToChannels(firstSequence);
            client.ServiceClient.Post(new GenerateRandomNumbers
            {
                RequestId = firstSequence,
                Count = 10,
                DelayInMs = 1000
            });

            var secondSequence = Guid.NewGuid().ToString();
            client.SubscribeToChannels(secondSequence);
            client.ServiceClient.Post(new GenerateRandomNumbers
            {
                RequestId = secondSequence,
                Count = 5,
                DelayInMs = 2000
            });

            client.OnMessage = msg1 => Debug.WriteLine($"{msg1.Channel}: {msg1.Json}");


            await Task.Delay(20 * 1000);
        }
    }
}