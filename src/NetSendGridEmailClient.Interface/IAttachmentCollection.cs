using NetSendGridEmailClient.Functions;

namespace NetSendGridEmailClient.Interface;

public interface IAttachmentCollection
{
    public IResultIota Add(IAttachment attachment);

    public void Remove(Guid attachmentId);

    public IList<IAttachment> GetAll();
}
