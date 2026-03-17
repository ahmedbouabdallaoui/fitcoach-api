using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using FitCoach.Api.Infrastructure.Messaging;

namespace FitCoach.Api.Infrastructure.Messaging;

public class InjuryAlertPublisher : IInjuryAlertPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<InjuryAlertPublisher> _logger;

    private const string ExchangeName = "fitunity.events";
    private const string QueueName = "injury.alerts";
    private const string RoutingKey = "injury.high_risk";

    public InjuryAlertPublisher(
        IConfiguration configuration,
        ILogger<InjuryAlertPublisher> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        // Declare exchange and queue — idempotent, safe to call every startup
        _channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true
        ).GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        ).GetAwaiter().GetResult();

        _channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: RoutingKey
        ).GetAwaiter().GetResult();
    }

    public async Task PublishAsync(InjuryAlertEvent alertEvent)
    {
        try
        {
            var json = JsonSerializer.Serialize(alertEvent);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true,  // message survives RabbitMQ restart
                ContentType = "application/json",
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(
                exchange: ExchangeName,
                routingKey: RoutingKey,
                mandatory: false,
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation(
                "Injury alert published for user {UserId} with risk score {RiskScore}",
                alertEvent.UserId,
                alertEvent.RiskScore
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to publish injury alert for user {UserId}",
                alertEvent.UserId
            );
            // We don't rethrow — a failed notification should not break the user's experience
        }
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}