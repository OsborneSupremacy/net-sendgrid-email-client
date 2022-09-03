namespace NetSendGridEmailClient.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy() => View();

    public IActionResult TermsOfService() => View();

    [GoogleScopedAuthorize]
    public IActionResult SignIn()
    {
        return View("Index");
    }

    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync();
        return View("SignedOut");
    }

    public IActionResult SignedOut() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
