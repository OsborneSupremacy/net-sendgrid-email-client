namespace NetSendGridEmailClient.Interface;

public interface IEmailPayloadFactory
{
    public T New<T>() where T : IEmailPayload, new();
}
