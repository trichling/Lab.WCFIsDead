using System;
using System.Threading.Tasks;
using Grpc.Core;
using Lab.WCFIsDead.gRPC.Shared;

namespace Lab.WCFIsDead.gRPC.Server.Services
{
    public class RandomNumberService : RandomNumberGenerator.RandomNumberGeneratorBase
    {
        
        public override Task GenerateRandomNumbers(RandomNumberRequest request, IServerStreamWriter<RandomNumberResponse> responseStream, Grpc.Core.ServerCallContext context)
        {

            return Task.Run(async () =>
            {
                for (var counter = 0; counter < request.Count; counter++)
                {
                    await Task.Delay(request.DelayInMs);
                    await responseStream.WriteAsync(new RandomNumberResponse() {
                        RequestId = request.RequestId,
                        RandomNumber = new Random().NextDouble()
                    });
                }
            });

        }

    }
}