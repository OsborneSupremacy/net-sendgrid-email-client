using LanguageExt.Common;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentStorageService
{
    public Task<Result<bool>> SaveAttachmentAsync(Guid emailPayloadId, IAttachment attachment);

    public Task<IAttachmentCollection> GetAttachmentCollectionAsync(Guid emailPayloadId);

    public Task<Result<bool>> RemoveAttachmentAsync(Guid emailPayloadId, Guid attachmentId);

    public Task<Result<bool>> RemoveAllAsync(Guid emailPayloadId);
}
