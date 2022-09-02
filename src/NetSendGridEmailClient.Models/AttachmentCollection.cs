using NetSendGridEmailClient.Interface;

namespace NetSendGridEmailClient.Models;

public class AttachmentCollection : IAttachmentCollection
{
    private readonly Dictionary<Guid, IAttachment> _attachmentDictionary;

    public AttachmentCollection()
    {
        _attachmentDictionary = new();
    }

    public void Add(IAttachment attachment) =>
        _attachmentDictionary.Add(attachment.AttachmentId, attachment);

    public IList<IAttachment> GetAll() => 
        _attachmentDictionary
            .Values
            .Select(x => x)
            .ToList();

    public void Remove(Guid attachmentId) =>
        _attachmentDictionary.Remove(attachmentId);
}
