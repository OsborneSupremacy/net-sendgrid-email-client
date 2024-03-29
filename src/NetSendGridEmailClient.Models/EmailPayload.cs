﻿using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace NetSendGridEmailClient.Models;

public record EmailPayload : IEmailPayload
{
    public Guid EmailPayloadId { get; init; }

    [Display(Name = "From Name")]
    public string FromName { get; set; } = default!;

    [Display(Name = "From Domain")]
    public string FromDomain { get; set; } = default!;

    [Display(Name = "From")]
    public string FromAddress => $"{FromName}@{FromDomain}";

    public string? Subject { get; set; } = default!;

    public string Body { get; set; } = default!;

    public string? HtmlBody { get; set; }

    [Display(Name = "To")]
    public IList<string> To { get; set; } = default!;

    [Display(Name = "CC")]
    public IList<string> Cc { get; set; } = default!;

    [Display(Name = "BCC")]
    public IList<string> Bcc { get; set; } = default!;
}

public class EmailPayloadValidator : AbstractValidator<EmailPayload>
{
    public EmailPayloadValidator()
    {
        RuleFor(x => x.EmailPayloadId).NotEmpty();

        RuleFor(x => x.FromName).NotEmpty();
        RuleFor(x => x.FromDomain).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();

        RuleFor(x => x.To).Must(x => x.Any());
        RuleForEach(x => x.To).NotEmpty();
        RuleForEach(x => x.To).EmailAddress();

        RuleForEach(x => x.Cc).EmailAddress();
        RuleForEach(x => x.Bcc).EmailAddress();
    }
}
