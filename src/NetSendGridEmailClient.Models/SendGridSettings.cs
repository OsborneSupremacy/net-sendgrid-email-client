namespace NetSendGridEmailClient.Models;

public record SendGridSettings
{
    public string DefaultUser { get; set; } = default!;

    public string Domain { get; set; } = default!;

    public string ApiKey { get; set; } = default!;
}
