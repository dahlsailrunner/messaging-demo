using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueName = "wazzup";
            string hostName = "localhost";
            string user = "guest";
            string pwd = "guest";

            var factory = new ConnectionFactory() {HostName = hostName, UserName = user, Password = pwd};           

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // using passive here to simply consume stuff from it -- we don't want to create it
                channel.QueueDeclarePassive(queueName);  // this will fail if the queue does not exist

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += ProcessQueueItem;

                channel.BasicConsume(queue: queueName, consumer: consumer); // no auto-ack going on here

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        static void ProcessQueueItem(object model, BasicDeliverEventArgs ea)
        {            
            var message = JsonConvert.DeserializeObject<Incoming>(Encoding.UTF8.GetString(ea.Body));
            Console.WriteLine(" [x] Received {0}", message.Message);

            // acknowledging the event takes it off the queue
            var m = model as EventingBasicConsumer;
            if (m != null)
            {
                m.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }
    }
}
