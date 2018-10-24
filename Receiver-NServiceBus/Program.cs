using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Receiver_NServiceBus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string queueName = "wazzup";
            string hostName = "localhost";
            string user = "guest";
            string pwd = "guest";

            var endpointConfiguration = new EndpointConfiguration(queueName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString($"host={hostName};username={user};password={pwd};");
            transport.UsePublisherConfirms(true);

            transport.UseDurableExchangesAndQueues(false);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
