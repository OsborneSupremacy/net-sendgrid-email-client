using System.Diagnostics;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NetSendGridEmailClient.Web.Models;

namespace NetSendGridEmailClient.Web.Controllers;

[GoogleScopedAuthorize]

public class EmailController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public EmailController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

}
