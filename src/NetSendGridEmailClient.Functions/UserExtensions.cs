using System.Security.Claims;

namespace NetSendGridEmailClient.Functions;

public static class UserExtensions
{
    public static bool IsLoggedIn(this ClaimsPrincipal input) =>
        input.Identity?.IsAuthenticated ?? false;

    public static string Name(this ClaimsPrincipal input)
    {
        if (!input.IsLoggedIn()) return string.Empty;

        return input.Claims
            .Where(x => x.Type == "name")?
            .FirstOrDefault()?
            .Value ?? "User";
    }
}
