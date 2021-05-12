using MessagingLib;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DispatchLogger
{
    public class MessageConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.QueueDeclare("scootapi",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                MessageEntity msg = JsonConvert.DeserializeObject<MessageEntity>(content);

                WebHook.WriteMessage(msg);
            };

            channel.BasicConsume(queue: "scootapi", autoAck: true, consumer: consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }
    }
}