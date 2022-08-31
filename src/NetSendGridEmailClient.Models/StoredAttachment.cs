using NetSendGridEmailClient.Interface;

namespace NetSendGridEmailClient.Models;

public record StoredAttachment : IAttachment
{
    public StoredAttachment(
        Guid attachmentId,
        Guid emailPayloadId,
        string base64Content,
        string fileName,
        string type
    )
    {
        AttachmentId = attachmentId;
        EmailPayloadId = emailPayloadId;
        Base64Content = base64Content ?? throw new ArgumentNullException(nameof(base64Content));
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public Guid AttachmentId { get; set; }

    public Guid EmailPayloadId { get; init; }

    public string Base64Content { get; init; }

    public string FileName { get; init; }

    public string Type { get; init; }
}
