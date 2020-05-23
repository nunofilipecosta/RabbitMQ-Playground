using System;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Playground.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string queueName = "testqueue";

            try
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672,
                    RequestedConnectionTimeout = 3000
                };

                using (var rabbitConnection = connectionFactory.CreateConnection())
                {
                    using (var channel = rabbitConnection.CreateModel())
                    {
                        channel.QueueDeclare(queue : queueName, durable: false, exclusive : false, autoDelete : false, arguments: null);
                        while (true)
                        {
                            string body = $"A nice random message : {DateTime.Now.Ticks}";
                            channel.BasicPublish(exchange : string.Empty, routingKey : queueName, basicProperties : null, body: Encoding.UTF8.GetBytes(body));
                            Console.WriteLine("Message Sent");
                            await Task.Delay(5000);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine("End");
            Console.Read();
        }
    }
}
