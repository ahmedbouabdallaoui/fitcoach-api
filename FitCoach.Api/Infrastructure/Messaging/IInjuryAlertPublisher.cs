namespace FitCoach.Api.Infrastructure.Messaging;

public interface IInjuryAlertPublisher
{
    Task PublishAsync(InjuryAlertEvent alertEvent);
}