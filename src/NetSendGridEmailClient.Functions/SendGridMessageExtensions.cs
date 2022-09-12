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

    public static SendGridMessage AddTos(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.To);

    public static SendGridMessage AddCcs(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.Cc);

    public static SendGridMessage AddBccs(
        this SendGridMessage msg,
        IList<string> recipientList) => AddRecipients(msg, recipientList, RecipientType.Bcc);

    public static SendGridMessage AddRecipients(
        this SendGridMessage msg,
        IList<string> recipientList,
        RecipientType recipientType)
    {
        if (!recipientList.AnyEmails())
            return msg;

        var emailList = recipientList.ToEmailList();
        msg.GetRecipientAddDelegate(recipientType)(emailList);
        return msg;
    }

    public static Action<List<EmailAddress>> GetRecipientAddDelegate(
        this SendGridMessage msg,
        RecipientType recipientType) =>
        recipientType switch
        {
            RecipientType.To => (List<EmailAddress> emails) => { msg.AddTos(emails); }
            ,
            RecipientType.Cc => (List<EmailAddress> emails) => { msg.AddCcs(emails); }
            ,
            RecipientType.Bcc => (List<EmailAddress> emails) => { msg.AddBccs(emails); }
            ,
            _ => throw new NotSupportedException()
        };
}
