namespace NetSendGridEmailClient.Interface;

public interface IAttachment
{
    public Guid AttachmentId { get; set; }

    public Guid EmailPayloadId { get; init; }

    public string Base64Content { get; init; }

    public string FileName { get; init; }

    public string Type { get; init; }
}
