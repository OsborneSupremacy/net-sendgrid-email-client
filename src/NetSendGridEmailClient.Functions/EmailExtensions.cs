using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Functions;

public static class EmailExtensions
{
    public static List<EmailAddress> ToEmailList(this IList<string> input) =>
        input.Select(x => new EmailAddress(x)).ToList();

    public static EmailAddress ToEmail(this string input) =>
        new EmailAddress(input);
}
