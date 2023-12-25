using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Register RabbitMQMessageService as an implementation for IMessageService
builder.Services.AddSingleton<IMessageService, RabbitMQMessageService>(provider => 
{
    var factory = new ConnectionFactory
    {
        UserName = "guest",
        Password = "guest",
        VirtualHost = "/",
        HostName = "rabbitmq",
        Port = 5672,
    };
    return new RabbitMQMessageService(factory);
});

var app = builder.Build();

app.MapGet("/", async () =>
{
    var messageService = app.Services.GetRequiredService<IMessageService>();
    await messageService.SendMessageAsync(new CarData { Name = "Bentley", Price = 350000 });
    
    return "Message sent!";
});

app.Run();

public interface IMessageService
{
    Task SendMessageAsync(CarData car);
}

public class RabbitMQMessageService : IMessageService
{
    private readonly ConnectionFactory _factory;

    public RabbitMQMessageService(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task SendMessageAsync(CarData car)
    {
        using (var connection = _factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = Newtonsoft.Json.JsonConvert.SerializeObject(car);
            var body = Encoding.UTF8.GetBytes(message);

            await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body));
        }
    }
}

public class CarData
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
