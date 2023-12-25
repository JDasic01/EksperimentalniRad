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

using (var con = new NpgsqlConnection("Host=database;Port=5432;Database=your_database_name;User Id=your_username;Password=your_password"))
{
    var tableName = "cars";
    con.Open();

    using (var cmd = new NpgsqlCommand())
    {
        cmd.Connection = con;
        cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} (id SERIAL PRIMARY KEY, name VARCHAR(255), price DECIMAL)";
        cmd.ExecuteNonQuery();
    }
}

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
        var cs = "Host=database;Port=5432;Database=your_database_name;User Id=your_username;Password=your_password";
        var tableName = "cars";
        
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
                    var carData = Newtonsoft.Json.JsonConvert.DeserializeObject<CarData>(receivedMessage);
                    using (var con = new NpgsqlConnection(cs))
                    {
                        con.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = $"INSERT INTO {tableName}(name, price) VALUES('{carData.Name}', {carData.Price})";
                            Thread.Sleep(100);
                            cmd.ExecuteNonQuery();
                        }
                    }
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

public class CarData
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

