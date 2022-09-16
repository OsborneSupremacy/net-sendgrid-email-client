using LanguageExt.Common;

namespace NetSendGridEmailClient.Interface;

public interface IEmailService
{
    Task<Result<bool>> SendAsync(IEmailPayload emailPayload);
}
