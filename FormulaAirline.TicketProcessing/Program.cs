// See https://aka.ms/new-console-template for more information
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Welcome to the ticketing service");

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "mypass",
    VirtualHost = "/"
};

var conn = factory.CreateConnection();
using var channel = conn.CreateModel();
channel.QueueDeclare("bookings",durable: true,exclusive: false);
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model,eventArgs) => 
{
    //Get the Byte[]
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    System.Console.WriteLine($"A message has been received  - {message}");
};

channel.BasicConsume("bookings",true,consumer);
Console.ReadKey();