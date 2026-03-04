using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RelatorioApi.Settings;

namespace RelatorioApi.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly RabbitMqSettings _settings;
    private readonly string _queueName = "relatorios_queue";

   public RabbitMqService(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    public async Task PublicarMensagemAsync(object mensagem)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password
        };
        
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _queueName, 
            durable: true, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);

        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty, 
            routingKey: _queueName, 
            body: body);
    }
}