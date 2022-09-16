using LanguageExt.Common;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentCollection
{
    public Result<bool> Add(IAttachment attachment);

    public void Remove(Guid attachmentId);

    public IList<IAttachment> GetAll();
}
