using Microsoft.AspNetCore.Mvc;

namespace NetSendGridEmailClient.Web.Controllers;

public class AccountController : Controller
{
    public IActionResult AccessDenied(string returnUrl)
    {
        return View();
    }
}
