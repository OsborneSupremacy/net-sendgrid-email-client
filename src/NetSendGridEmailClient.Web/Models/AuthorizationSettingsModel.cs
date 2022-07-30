namespace NetSendGridEmailClient.Web.Models;

public record AuthorizationSettings
{
    public string[] Admins { get; set; } = default!;
}
