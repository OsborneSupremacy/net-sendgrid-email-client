
namespace NetSendGridEmailClient.Interface;

public interface IAttachmentCollection
{
    public Outcome<bool> Add(IAttachment attachment);

    public void Remove(Guid attachmentId);

    public IList<IAttachment> GetAll();
}
