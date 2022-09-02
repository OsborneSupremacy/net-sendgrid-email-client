namespace NetSendGridEmailClient.Interface;

public interface IAttachmentCollection
{
    public void Add(IAttachment attachment);

    public IList<IAttachment> GetAll();
}
