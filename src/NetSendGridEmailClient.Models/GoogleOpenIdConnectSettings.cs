using FluentValidation;

namespace NetSendGridEmailClient.Models;

public record GoogleOpenIdConnectSettings
{
    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? SignedOutCallbackPath { get; set; }
}

public class GoogleOpenIdConnectSettingsValidator : AbstractValidator<GoogleOpenIdConnectSettings>
{
    public GoogleOpenIdConnectSettingsValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ClientSecret).NotEmpty();
        RuleFor(x => x.SignedOutCallbackPath).NotEmpty();
    }
}
