namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IAttachmentAdapter))]
public class AttachmentAdapter : IAttachmentAdapter
{
    public async Task<Outcome<bool>> AddAsync(SendGridMessage msg, IAttachment attachment)
    {
        try
        {
            using var contentStream =
                new MemoryStream(Convert.FromBase64String(attachment.Base64Content));

            await msg.AddAttachmentAsync(
                attachment.FileName,
                contentStream,
                attachment.Type,
                "attachment",
                null,
                CancellationToken.None
                );

            return true;
        } catch (Exception ex)
        {
            return new Outcome<bool>(ex);
        }
    }

    public async Task<Outcome<bool>> AddAsync(SendGridMessage msg, IList<IAttachment> attachments)
    {
        try
        {
            foreach (var attachment in attachments)
                await AddAsync(msg, attachment);
            return true;
        } catch (Exception ex)
        {
            return new Outcome<bool>(ex);
        }
    }
}
