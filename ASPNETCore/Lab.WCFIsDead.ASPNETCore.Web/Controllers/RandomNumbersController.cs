using Lab.WCFIsDead.ASPNETCore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab.WCFIsDead.ASPNETCore.Web.Controllers
{
    [ApiController]
    [Route("randomNumbers")]
    public class RandomNumbersController : ControllerBase
    {
        [HttpGet("generate")]
        public async Task GetRandomNumbers(Guid requestId, int count, int delay)
        {
            Response.Headers["Cache-Control"] = "no-cache"; // https://serverfault.com/a/801629
            Response.Headers["X-Accel-Buffering"] = "no";
            Response.ContentType = "text/event-stream";
            using (var client = new StreamWriter(Response.Body))
            {
                try
                {
                    await client.WriteAsync("event: connected\ndata:\n\n");
                    await client.FlushAsync();
                    await Generate(client, requestId, count, delay);
                    // für Verbindungen die gehlaten werden sollen muss der "client" gespeichert und folgende Zeile verwendet werden:
                    // await Task.Delay(Timeout.Infinite, HttpContext.RequestAborted);
                    await client.WriteAsync("event: end-of-stream\ndata:\n\n");
                    await client.FlushAsync();
                }
                catch (TaskCanceledException) { }
            }

            await Response.CompleteAsync();
        }

        private async Task Generate(StreamWriter client, Guid requestId, int count, int delayInMs)
        {
            for (var counter = 0; counter < count; counter++)
            {
                await Task.Delay(delayInMs);
                await client.WriteAsync(AsEvent(new RandomNumberResult() { RequestId = requestId, RandomNumber = new Random().NextDouble() }));
                await client.FlushAsync();
            }

        }

        private string AsEvent(RandomNumberResult result)
        {
            return $"data: {System.Text.Json.JsonSerializer.Serialize(result)} \n\n";
        }
    }
}
