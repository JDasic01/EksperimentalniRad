// WebAppReceive/Program.cs

using System;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitMQListenerService>();
        services.AddSingleton(new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            HostName = "rabbitmq",
            Port = 5672,
        });
    })
    .Build();

host.Run();

public class RabbitMQListenerService : IHostedService
{
    private readonly ConnectionFactory _factory;

    public RabbitMQListenerService(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "messageQueue",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($" [x] Received message: {message}");
                };

                channel.BasicConsume(queue: "messageQueue",
                                    autoAck: true,
                                    consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " Error starting RabbitMQ listener service");
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
