using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.ASPNETCore.Web.Hubs
{
    public class RandomNumberHub : Hub
    {

        public RandomNumberHub()
        {
        }

        public ChannelReader<RandomNumberResult> GetRandomNumbers(Guid requestId, int count, int delayInMs)
        {
            var channel = Channel.CreateUnbounded<RandomNumberResult>();

            _ = Generate(channel.Writer, requestId, count, delayInMs);

            return channel.Reader;
        }

        private async Task Generate(ChannelWriter<RandomNumberResult> writer, Guid requestId, int count, int delayInMs)
        {
            for (var counter = 0; counter < count; counter++)
            {
                await Task.Delay(delayInMs);
                await writer.WriteAsync(new RandomNumberResult() { RequestId = requestId, RandomNumber = new Random().NextDouble() });
            }

            writer.TryComplete();
        }

    }

    public class RandomNumberResult
    {

        public Guid RequestId { get; set; }
        public double RandomNumber { get; set; }

    }
}
