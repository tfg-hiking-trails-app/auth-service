using AuthService.Domain.Interfaces.Messaging;
using Common.Domain.Interfaces.Messaging;
using Common.Infrastructure.Messaging;

namespace AuthService.Infrastructure.Messaging.Producer;

public class UsernameChangeQueueProducer : AbstractRabbitMqQueueProducer, IUsernameChangeQueueProducer
{
    public override string QueueName { get; }
    public override string ExchangeName { get; }

    public UsernameChangeQueueProducer(IRabbitMqQueueProvider channelProvider) : base(channelProvider)
    {
        QueueName = GetUsingEnvironmentVariable("RABBITMQ_QUEUE_AUTH_TO_ACCOUNT");
        ExchangeName = GetUsingEnvironmentVariable("RABBITMQ_EXCHANGE_AUTH_SERVICE");
    }
}
