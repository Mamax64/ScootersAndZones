using Newtonsoft.Json;
using RabbitMQ.Client;
using ScootAPI.Models.Messaging;
using System;
using System.Text;

namespace ScootAPI.Services
{
    public class AmqpService : IAmqpService
    {
        private IConnection _connection;

        public AmqpService()
        {
            CreateConnection();
        }

        public void SendMessage(Message message)
        {
            if(ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "scootapi",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var json = JsonConvert.SerializeObject(message);

                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: "scootapi", basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not create connection: {e.Message}");
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if(_connection != null)
            {
                return true;
            }

            CreateConnection();
            return _connection != null;
        }
    }
}
