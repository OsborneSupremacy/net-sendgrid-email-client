using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetSendGridEmailClient.Functions;
using NetSendGridEmailClient.Web.Models;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]
[Authorize(Policy = "Admin")]
public class EmailController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SendGridSettings _sendGridSettings;

    private readonly EmailPayloadValidator _emailPayloadValidator;

    public EmailController(
        ILogger<HomeController> logger,
        SendGridSettings sendGridSettings,
        EmailPayloadValidator emailPayloadValidator
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
        _emailPayloadValidator = emailPayloadValidator ?? throw new ArgumentNullException(nameof(emailPayloadValidator));
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
    public async Task<IActionResult> Index(EmailPayload model)
    {
        _emailPayloadValidator
            .Validate(model)
            .AddToModelState(ModelState);

        if (!ModelState.IsValid)
            return View(model);




        return View(model);
    }

}
