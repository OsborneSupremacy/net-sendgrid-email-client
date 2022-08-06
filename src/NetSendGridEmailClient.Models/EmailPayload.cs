using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Markdig;

namespace NetSendGridEmailClient.Models;

public record EmailPayload
{
    [Display(Name = "From")]
    public string FromName { get; set; } = default!;

    public string FromDomain { get; set; } = default!;

    [Display(Name = "From")]
    public string FromAddress => $"{FromName}@{FromDomain}";

    public string Subject { get; set; } = default!;

    public string Body { get; set; } = default!;

    public string HtmlBody
    {
        get
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
            return Markdown.ToHtml(Body ?? string.Empty, pipeline);
        }
    }

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
        RuleFor(x => x.FromName).NotEmpty();
        RuleFor(x => x.Body).NotEmpty();

        RuleFor(x => x.To).Must(x => x.Any());
        RuleForEach(x => x.To).NotEmpty();
        RuleForEach(x => x.To).EmailAddress();

        RuleForEach(x => x.Cc).EmailAddress();
        RuleForEach(x => x.Bcc).EmailAddress();
    }
}
