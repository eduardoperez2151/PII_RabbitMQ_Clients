using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;

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

            String nombreExchange = "EXCHANGE_PRUEBA";
            channel.ExchangeDeclare(nombreExchange, ExchangeType.Topic, true, false, null);
            Data data = new Data();
            data.Name = "Nombre Ejemplo";

            for (int i = 0; i < 1000; i++)
            {
                data.Age = i;
                string jsonObject = JsonSerializer.Serialize(data);
                byte[] payload = Encoding.ASCII.GetBytes(jsonObject);
                channel.BasicPublish(nombreExchange, "profes", null, payload);
                Console.WriteLine(data);
            }

            channel.Close();
            connection.Close();
        }
    }
}
