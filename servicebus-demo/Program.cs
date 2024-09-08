using servicebus_demo.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllers();
services.AddSingleton<ServiceBusReceiverService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

var serviceBusReceiver = app.Services.GetRequiredService<ServiceBusReceiverService>();
await serviceBusReceiver.StartProcessingAsync();

app.Run();
