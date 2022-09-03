using Markdig;

namespace NetSendGridEmailClient.Services;

public class MarkdownService : IMarkdownService
{
    public string RenderHtml(string input)
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
        return Markdown.ToHtml(input ?? string.Empty, pipeline);
    }
}
