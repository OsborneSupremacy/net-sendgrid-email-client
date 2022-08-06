namespace NetSendGridEmailClient.Models;

public record AuthorizationSettings
{
    public string[] Admins { get; set; } = default!;
}
