// WebAppSend/Program.cs

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/", async (HttpContext context) =>
{
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "messageQueue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var car = new CarData { Name = "Bentley", Price = 350000 };
        var body = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(car));

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "messageQueue",
                             basicProperties: null,
                             body: body);

        Console.WriteLine($" [x] Sent message about inserting car: {car.Name}, {car.Price}");

        await context.Response.WriteAsync($"Message sent to RabbitMQ. Check the console for details. Car: {car.Name}, {car.Price}");
    }
});

app.Run();

public class CarData
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}