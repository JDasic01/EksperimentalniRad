using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var factory = new ConnectionFactory
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    HostName = "rabbitmq",
    Port = 5672,
};

app.MapGet("/", () =>
{
    SendMessage("Hello, World!");
    return "Message sent!";
});

app.Run();

void SendMessage(string message)
{
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

        Console.WriteLine($" [x] Sent {message}");
    }
}
