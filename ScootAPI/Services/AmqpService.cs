using MessagingLib;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using ServiceStack;
using System;
using System.Text;

namespace ScootAPI.Services
{
    public class AmqpService : IAmqpService
    {
        private IConnection _connection;
        private readonly IConfiguration _configuration;

        public AmqpService(IConfiguration configuration)
        {
            _configuration = configuration;
            CreateConnection();
        }

        public void SendMessage(MessageEntity message)
        {
            if(ConnectionExists())
            {
                using var channel = _connection.CreateModel();
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

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["Amqp:HostName"],
                    Port = _configuration["Amqp:Port"].ToInt(),
                    UserName = _configuration["Amqp:UserName"],
                    Password = _configuration["Amqp:Password"]
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
