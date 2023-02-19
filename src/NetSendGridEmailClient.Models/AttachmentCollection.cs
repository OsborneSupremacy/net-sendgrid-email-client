namespace NetSendGridEmailClient.Models;

public class AttachmentCollection : IAttachmentCollection
{
    private readonly Dictionary<Guid, IAttachment> _attachmentDictionary;

    private const double MaxAggregateAttachmentSize = 20971520;

    private const double Base64toBytesFactor = 1.37;

    public AttachmentCollection()
    {
        _attachmentDictionary = new();
    }

    public Outcome<bool> Add(IAttachment attachment)
    {
        var currentSize = _attachmentDictionary
            .Values
            .Sum(x => x.Base64Content.Length);

        var newSize = (currentSize + attachment.Base64Content.Length) * Base64toBytesFactor;

        if (newSize > MaxAggregateAttachmentSize)
            return new Outcome<bool>(new NotSupportedException($"Total file size of attachment would exceed maximum of {newSize} bytes"));

        _attachmentDictionary.Add(attachment.AttachmentId, attachment);

        return true;
    }

    public IList<IAttachment> GetAll() =>
        _attachmentDictionary
            .Values
            .Select(x => x)
            .ToList();

    public void Remove(Guid attachmentId) =>
        _attachmentDictionary.Remove(attachmentId);
}
