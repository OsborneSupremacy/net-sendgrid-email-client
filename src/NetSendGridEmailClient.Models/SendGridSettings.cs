using FluentValidation;

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

public class SendGridSettingsValidator : AbstractValidator<SendGridSettings>
{
    public SendGridSettingsValidator()
    {
        RuleFor(x => x.Domains).Must(x => x.Any());
        RuleForEach(x => x.Domains).SetValidator(new SendGridDomainValidator());
    }
}

public class SendGridDomainValidator : AbstractValidator<SendGridDomain>
{
    public SendGridDomainValidator()
    {
        RuleFor(x => x.DefaultUser).NotEmpty();
        RuleFor(x => x.Domain).NotEmpty();
        RuleFor(x => x.ApiKey).NotEmpty();
    }
}
