using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Functions;

public static class EmailExtensions
{
    public static bool AnyEmails(this IList<string> input) =>
        input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Any();

    public static List<EmailAddress> ToEmailList(this IList<string> input) =>
        input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new EmailAddress(x))
            .ToList();

    public static EmailAddress ToEmail(this string input) =>
        new(input);
}
