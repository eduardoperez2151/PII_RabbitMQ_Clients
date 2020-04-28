using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.UserName = "p2team";
            connectionFactory.Password = "p2team2020";
            connectionFactory.VirtualHost = "/";
            connectionFactory.HostName = "159.89.39.151";
            connectionFactory.Port = 5672;

            IConnection connection = connectionFactory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.ExchangeDeclare("EXCHANGE_PRUEBA", ExchangeType.Topic, true, false, null);
            string queueName = channel.QueueDeclare();

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                string jsonData = Encoding.ASCII.GetString(ea.Body.ToArray());

                Data data = JsonSerializer.Deserialize<Data>(jsonData);

                Console.WriteLine($"Nombre: {data.Name} Edad: {data.Age}");
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            channel.QueueBind(queueName, "EXCHANGE_PRUEBA", "profes");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            channel.QueueUnbind(queueName, "EXCHANGE_PRUEBA", "profes", null);

            channel.Close();
            connection.Close();
        }
    }
}
