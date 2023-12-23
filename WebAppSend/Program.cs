using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

// RabbitMQ ConnectionFactory 
var factory = new ConnectionFactory
{
    UserName = "guest",
    Password = "guest",
    VirtualHost = "/",
    HostName = "rabbitmq",
    Port = 5672,
};

builder.Services.AddSingleton(factory);

var app = builder.Build();

// Configure the ASP.NET Core application
app.MapGet("/", async (HttpContext context) =>
{
    // Sender logic
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);

        await SendMessageAsync(channel, "hello", body);

        Console.WriteLine($" [x] Sent {message}");

        await context.Response.WriteAsync("Message sent to RabbitMQ. Check the console for details.");
    }
});

app.Run();

// Sender method
static async Task SendMessageAsync(IModel channel, string queueName, byte[] body)
{
    await Task.Run(() =>
    {
        channel.BasicPublish(exchange: string.Empty,
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    });
}
