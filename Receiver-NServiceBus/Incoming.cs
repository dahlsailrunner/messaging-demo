using NServiceBus;

namespace Receiver_NServiceBus
{
    public class Incoming: IEvent
    {
        public string Message { get; set; }
        public int Id { get; set; }
    }
}
