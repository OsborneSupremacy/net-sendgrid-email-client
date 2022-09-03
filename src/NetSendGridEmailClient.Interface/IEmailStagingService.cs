using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Interface;

public interface IEmailStagingService
{
    public Task<SendGridMessage> StageAsync(IEmailPayload emailPayload);
}
