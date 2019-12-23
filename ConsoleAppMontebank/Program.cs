using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using MbDotNet.Enums;

namespace ConsoleAppMontebank
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var _client = new MbDotNet.MountebankClient("http://localhost:2525/");

            var imposter = _client.CreateHttpImposter(4545, "My Imposter");
            imposter.AddStub()
                .OnPathAndMethodEqual("/customers/123", Method.Get)
                .ReturnsJson(HttpStatusCode.OK, new Customer { Email = "customer@test.com" });
            _client.Submit(imposter);

           var  res =  await "http://localhost:4545/customers/123"
                .GetAsync(CancellationToken.None)
                .ReceiveJson<Customer>();
        }
    }

    class Customer
    {
        public string Email { get; set; }
    }
}
