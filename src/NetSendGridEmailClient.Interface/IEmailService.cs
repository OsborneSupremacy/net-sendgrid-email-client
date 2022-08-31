namespace NetSendGridEmailClient.Interface;

public interface IEmailService
{
    Task<(bool success, string details)> SendAsync(IEmailPayload emailPayload);
}
