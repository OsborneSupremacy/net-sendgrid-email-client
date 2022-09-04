namespace NetSendGridEmailClient.Interface;

public interface IEmailRegistrationService
{
    public void RegisterSentEmail(Guid emailPayloadId);

    public (bool result, DateTime? timeSent) HasEmailBeenSent(Guid emailPayloadId);
}
