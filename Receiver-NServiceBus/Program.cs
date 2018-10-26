using NServiceBus;
using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

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

            endpointConfiguration.Pipeline.Register(new AssumeMessageTypeBehavior(typeof(Incoming)), "Assume all messages are provided type.");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    class AssumeMessageTypeBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        private Type _messageType;
        public AssumeMessageTypeBehavior(Type type)
        {
            _messageType = type;
        }

        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            context.Message.Headers[Headers.EnclosedMessageTypes] = _messageType.FullName;
            return next();
        }
    }
}
