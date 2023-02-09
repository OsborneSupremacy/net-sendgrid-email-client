using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentAdapter
{
    public Task<Outcome<bool>> AddAsync(SendGridMessage msg, IList<IAttachment> attachments);

    public Task<Outcome<bool>> AddAsync(SendGridMessage msg, IAttachment attachment);
}
