using NetSendGridEmailClient.Interface;

namespace NetSendGridEmailClient.Models;

public class AttachmentCollection : IAttachmentCollection
{
    private readonly List<IAttachment> _attachments;

    public AttachmentCollection()
    {
        _attachments = new List<IAttachment>();
    }

    public void Add(IAttachment attachment) => 
        _attachments.Add(attachment);
}
