using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueName = "wazzup";
            string hostName = "localhost";
            string user = "guest";
            string pwd = "guest";

            var factory = new ConnectionFactory() { HostName = hostName, UserName = user, Password = pwd };
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var msg = new BlueMessage // this serializes the same as the "Incoming" class but is independently defined
                {
                    Message = "hello world",
                    Id = 123,
                    MoreStuff = "Hi Rupesh!!"
                };
                
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: new BasicProperties {MessageId = Guid.NewGuid().ToString()},  // NServiceBus requires this
                    body: body);

                Console.WriteLine(" [x] Sent {0}", msg.Message);
            }
        }
    }
}
