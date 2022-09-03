using NetSendGridEmailClient.Functions;

namespace NetSendGridEmailClient.Interface;

public interface IEmailService
{
    Task<IResultIota> SendAsync(IEmailPayload emailPayload);
}
