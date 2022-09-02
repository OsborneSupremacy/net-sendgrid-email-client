namespace NetSendGridEmailClient.Interface;

public interface IAttachmentCollection
{
    public void Add(IAttachment attachment);

    public void Remove(Guid attachmentId);

    public IList<IAttachment> GetAll();
}
