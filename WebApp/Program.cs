using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", (HttpContext context) =>
{
    var cs = "Host=database;Port=5432;Database=your_database_name;User Id=your_username;Password=your_password";
    
    var car = new CarData { Name = "Bentley", Price = 350000 };
    var body = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(car));
    var message = Encoding.UTF8.GetString(body);
    var carData = Newtonsoft.Json.JsonConvert.DeserializeObject<CarData>(message);
    
    using (var con = new NpgsqlConnection(cs))
    {
        con.Open();

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = con;
            cmd.CommandText = $"INSERT INTO cars(name, price) VALUES('{carData.Name}', {carData.Price})";
            cmd.ExecuteNonQuery();
        }
    }
    context.Response.StatusCode = 200;
    return context.Response.WriteAsync("OK");
});

app.Run();

public class CarData
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}


