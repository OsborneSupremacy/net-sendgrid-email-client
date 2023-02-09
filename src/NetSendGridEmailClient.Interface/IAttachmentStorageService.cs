
namespace NetSendGridEmailClient.Interface;

public interface IAttachmentStorageService
{
    public Task<Outcome<bool>> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment);

    public Task<IAttachmentCollection> GetAttachmentCollectionAsync(Guid emailPayloadId);

    public Task<Outcome<bool>> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId);

    public Task<Outcome<bool>> RemoveAllAsync(Guid emailPayloadId);
}
