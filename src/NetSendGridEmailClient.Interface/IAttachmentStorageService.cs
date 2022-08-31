namespace NetSendGridEmailClient.Interface;

public interface IAttachmentStorageService
{
    public Task SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment);

    public Task<IList<IAttachment>> GetAttachmentsAsync(Guid emailPayloadId);
}
