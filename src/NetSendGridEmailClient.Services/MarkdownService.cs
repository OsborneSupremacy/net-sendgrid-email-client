using Markdig;

namespace NetSendGridEmailClient.Services;

[ServiceLifetime(ServiceLifetime.Singleton)]
[RegistrationTarget(typeof(IMarkdownService))]
public class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline markdownPipeline;

    public MarkdownService(MarkdownPipeline markdownPipeline)
    {
        this.markdownPipeline = markdownPipeline ?? throw new ArgumentNullException(nameof(markdownPipeline));
    }

    public string RenderHtml(string input) =>
        Markdown.ToHtml(input ?? string.Empty, markdownPipeline);
}
