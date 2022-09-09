using Microsoft.AspNetCore.Mvc.Rendering;
using NetSendGridEmailClient.Interface;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]
[Authorize(Policy = "Admin")]
public class EmailController : Controller
{
    private readonly ILogger<EmailController> _logger;

    private readonly SendGridSettings _sendGridSettings;

    private readonly EmailPayloadValidator _emailPayloadValidator;

    private readonly IEmailService _emailService;

    private readonly IMarkdownService _markdownService;

    public EmailController(
        ILogger<EmailController> logger,
        SendGridSettings sendGridSettings,
        EmailPayloadValidator emailPayloadValidator,
        IEmailService emailService,
        IMarkdownService markdownService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
        _emailPayloadValidator = emailPayloadValidator ?? throw new ArgumentNullException(nameof(emailPayloadValidator));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _markdownService = markdownService ?? throw new ArgumentNullException(nameof(markdownService));
    }

    protected IActionResult SendToEditor(EmailPayload model)
    {
        ViewBag.FromDomain = new SelectList(
            _sendGridSettings
            .Domains
            .Select(x => x.Domain)
        );
        return View("Index", model);
    }

    public IActionResult Index()
    {
        EmailPayload model = new()
        {
            EmailPayloadId = Guid.NewGuid(),
            To = new List<string>() { string.Empty },
            Cc = new List<string>() { string.Empty },
            Bcc = new List<string>() { string.Empty },
            FromName = _sendGridSettings.Domains.First().DefaultUser,
            FromDomain = _sendGridSettings.Domains.First().Domain,
            Body = string.Empty
        };

        return SendToEditor(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Compose(EmailPayload model) =>
        SendToEditor(model);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Confirm(EmailPayload model)
    {
        _emailPayloadValidator
            .Validate(model)
            .AddToModelState(ModelState);

        model.HtmlBody = _markdownService.RenderHtml(model.Body);

        if (!ModelState.IsValid)
            return SendToEditor(model);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(EmailPayload model)
    {
        _emailPayloadValidator
            .Validate(model)
            .AddToModelState(ModelState);

        if (!ModelState.IsValid)
            return SendToEditor(model);

        var result = await _emailService.SendAsync(model);

        if(!result.Success)
        {
            foreach (var message in result.Messages)
                ModelState.AddModelError("SendGrid", message);
            return SendToEditor(model);
        }

        return View("Sent");
    }
}
