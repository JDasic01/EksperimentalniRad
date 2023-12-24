using System;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Npgsql;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ReceiveMessagesService>();
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

public class ReceiveMessagesService : BackgroundService
{
    private readonly ILogger<ReceiveMessagesService> _logger;
    private readonly ConnectionFactory _factory;

    public ReceiveMessagesService(IServiceProvider services,
        ILogger<ReceiveMessagesService> logger)
    {
        Services = services;
        _logger = logger;
        _factory = new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            HostName = "rabbitmq",
            Port = 5672,
        };
    }

    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Receive Messages Service running.");

        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Receive Messages Service is working.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var receivedMessage = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {receivedMessage}");
                };

                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" [*] Waiting for messages.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Receive Messages Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}