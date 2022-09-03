using NetSendGridEmailClient.Functions;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentStorageService
{
    public Task<IResultIota> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment);

    public Task<IList<IAttachment>> GetAttachmentsAsync(Guid emailPayloadId);

    public Task<IResultIota> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId);

    public Task<IResultIota> RemoveAllAsync(Guid emailPayloadId);
}
