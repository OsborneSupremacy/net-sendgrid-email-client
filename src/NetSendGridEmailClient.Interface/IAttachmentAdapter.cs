using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentAdapter
{
    public Task AddAsync(SendGridMessage msg, IList<IAttachment> attachments);

    public Task AddAsync(SendGridMessage msg, IAttachment attachment);
}
