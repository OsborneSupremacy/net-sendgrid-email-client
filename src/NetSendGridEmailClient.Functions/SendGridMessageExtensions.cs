using SendGrid.Helpers.Mail;

namespace NetSendGridEmailClient.Functions;

public static class SendGridMessageExtensions
{
    public enum RecipientType
    {
        To,
        Cc,
        Bcc
    }

    public static void AddTos(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.To);

    public static void AddCcs(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.Cc);

    public static void AddBccs(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.Bcc);

    public static void AddRecipients(
        this SendGridMessage msg,
        IList<string> recipientList,
        RecipientType recipientType)
    {
        if (!recipientList.AnyEmails())
            return;

        var emailList = recipientList.ToEmailList();

        switch(recipientType) {
            case RecipientType.Cc:
                msg.AddCcs(emailList);
                break;
            case RecipientType.Bcc:
                msg.AddBccs(emailList);
                break;
            default:
                msg.AddTos(emailList);
                break;
        }
    }
}
