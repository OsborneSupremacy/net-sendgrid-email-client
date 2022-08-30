namespace NetSendGridEmailClient.Models;

public record SendGridSettings
{
    public IList<SendGridDomain> Domains { get; set; } = Enumerable.Empty<SendGridDomain>().ToList();
}

public record SendGridDomain
{
    public string DefaultUser { get; set; } = default!;

    public string Domain { get; set; } = default!;

    public string ApiKey { get; set; } = default!;
}

public record SendGridDomainModel
{
    public string DefaultUser { get; set; } = default!;

    public string Domain { get; set; } = default!;
}
