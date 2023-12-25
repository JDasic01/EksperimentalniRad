using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

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

app.MapGet("/", async () =>
{
    await SendMessageAsync();
    return "Message sent!";
});

app.Run();

async Task SendMessageAsync()
{
    var car = new CarData { Name = "Bentley", Price = 350000 };
    var message = Newtonsoft.Json.JsonConvert.SerializeObject(car);

    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var messageBody = Encoding.UTF8.GetBytes(message);

        // Make the message publishing asynchronous
        await Task.Run(() => channel.BasicPublish(exchange: string.Empty, routingKey: "hello", basicProperties: null, body: messageBody));

        Console.WriteLine($" [x] Sent {message}");
    }
}

public class CarData
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
