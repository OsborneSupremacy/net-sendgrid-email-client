namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IAttachmentAdapter))]
public class AttachmentAdapter : IAttachmentAdapter
{
    public Task AddAsync(SendGridMessage msg, IAttachment attachment)
    {
        using var contentStream =
            new MemoryStream(Convert.FromBase64String(attachment.Base64Content));

        return msg.AddAttachmentAsync(
            attachment.FileName,
            contentStream,
            attachment.Type,
            "attachment",
            null,
            CancellationToken.None
            );
    }

    public async Task AddAsync(SendGridMessage msg, IList<IAttachment> attachments)
    {
        foreach (var attachment in attachments)
            await AddAsync(msg, attachment);
    }
}
