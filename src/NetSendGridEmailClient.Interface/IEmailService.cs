namespace NetSendGridEmailClient.Interface;

public interface IEmailService
{
    Task<Outcome<bool>> SendAsync(IEmailPayload emailPayload);
}
