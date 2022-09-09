namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IEmailPayloadFactory))]
public class EmailPayloadFactory : IEmailPayloadFactory
{
    private readonly ILogger<EmailPayloadFactory> _logger;

    private readonly SendGridSettings _sendGridSettings;

    public EmailPayloadFactory(
        ILogger<EmailPayloadFactory> logger,
        SendGridSettings sendGridSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
    }

    public T New<T>() where T : IEmailPayload, new() =>
        new()
        {
            EmailPayloadId = Guid.NewGuid(),
            To = new List<string>() { string.Empty },
            Cc = new List<string>() { string.Empty },
            Bcc = new List<string>() { string.Empty },
            FromName = _sendGridSettings.Domains.First().DefaultUser,
            FromDomain = _sendGridSettings.Domains.First().Domain,
            Body = string.Empty
        };
}
