using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace servicebus_demo.Controllers;

public class ServiceBusReceiverService
{
    private readonly string _queueName = "sample";
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    public WeatherForecast CustomWeatherMessage = null;

    public ServiceBusReceiverService(IConfiguration configuration)
    {
        _client = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
        _processor = _client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());
    }

    public async Task StartProcessingAsync()
    {
        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        await _processor.StartProcessingAsync();
    }

    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();

        CustomWeatherMessage = JsonConvert.DeserializeObject<WeatherForecast>(body);
        Console.WriteLine($"Received: {body}");

        // Complete the message. Messages are deleted from the queue after this.
        await args.CompleteMessageAsync(args.Message);
    }

    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    public async Task StopProcessingAsync()
    {
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }
}
