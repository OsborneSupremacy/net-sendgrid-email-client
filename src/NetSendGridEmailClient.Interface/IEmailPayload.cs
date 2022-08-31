namespace NetSendGridEmailClient.Interface;

public interface IEmailPayload
{
    public string FromName { get; set; }

    public string FromDomain { get; set; }

    public string FromAddress { get; }

    public string? Subject { get; set; }

    public string Body { get; set; }

    public string? HtmlBody { get; set; }

    public IList<string> To { get; set; }

    public IList<string> Cc { get; set; }

    public IList<string> Bcc { get; set; }
}
