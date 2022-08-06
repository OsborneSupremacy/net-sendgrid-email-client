using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]
[Authorize(Policy = "Admin")]
public class EmailController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SendGridSettings _sendGridSettings;

    private readonly EmailPayloadValidator _emailPayloadValidator;

    private readonly SendGridEmailService _sendGridEmailService;

    public EmailController(
        ILogger<HomeController> logger,
        SendGridSettings sendGridSettings,
        EmailPayloadValidator emailPayloadValidator,
        SendGridEmailService sendGridEmailService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
        _emailPayloadValidator = emailPayloadValidator ?? throw new ArgumentNullException(nameof(emailPayloadValidator));
        _sendGridEmailService = sendGridEmailService ?? throw new ArgumentNullException(nameof(sendGridEmailService));
    }

    public IActionResult Index()
    {
        EmailPayload model = new()
        {
            To = new List<string>() { string.Empty },
            Cc = new List<string>() { string.Empty },
            Bcc = new List<string>() { string.Empty },
            FromName = _sendGridSettings.DefaultUser,
            FromDomain = _sendGridSettings.Domain,
            Body = string.Empty
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Compose(EmailPayload model) =>
        View("Index", model);

    [HttpPost]
    public IActionResult Confirm(EmailPayload model)
    {
        _emailPayloadValidator
            .Validate(model)
            .AddToModelState(ModelState);

        if (!ModelState.IsValid)
            return View("Index", model);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Send(EmailPayload model)
    {
        _emailPayloadValidator
            .Validate(model)
            .AddToModelState(ModelState);

        if (!ModelState.IsValid)
            return View("Index", model);

        await _sendGridEmailService.SendAsync(model);

        return View("Sent");
    }
}
