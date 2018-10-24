using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Receiver_NServiceBus
{
    public class HelloMessageHandler : IHandleMessages<Incoming>
    {
        public Task Handle(Incoming message, IMessageHandlerContext context)
        {
            var x = message.Message;
            Console.WriteLine(x);
            return Task.CompletedTask;
        }
    }
}
