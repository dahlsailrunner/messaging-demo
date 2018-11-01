using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BlueReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string queueName = "unified-green";
            string hostName = "rcdrpfaprmq001.realpage.com";            
            string user = "esuser";
            string pwd = "esuser123";
            string virtualHost = "/";

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = user,
                Password = pwd,
                VirtualHost = virtualHost                
            };

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
            var message = JsonConvert.DeserializeObject<BlueMessage>(Encoding.UTF8.GetString(ea.Body));
            Console.WriteLine(" [x] Received {0}", message.DisplayName);

            // acknowledging the event takes it off the queue
            var m = model as EventingBasicConsumer;
            if (m != null)
            {
                // if you comment out this line the message will be re-queued when thread stops
                m.Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }
    }
}
