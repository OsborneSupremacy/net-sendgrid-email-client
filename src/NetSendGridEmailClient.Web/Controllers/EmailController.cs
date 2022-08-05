using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetSendGridEmailClient.Web.Models;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]
[Authorize(Policy = "Admin")]
public class EmailController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SendGridSettings _sendGridSettings;

    public EmailController(
        ILogger<HomeController> logger,
        SendGridSettings sendGridSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _sendGridSettings = sendGridSettings ?? throw new ArgumentNullException(nameof(sendGridSettings));
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

}
