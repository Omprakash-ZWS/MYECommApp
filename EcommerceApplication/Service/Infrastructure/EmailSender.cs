using EcommerceApplication.Models.EmailModel;
using EcommerceApplication.Service.Interface;
using MimeKit;
using MailKit.Net.Smtp;

namespace EcommerceApplication.Service.Infrastructure
{
    public class EmailSender : IEmailService
    {
        //define a property emailconfiguration getting all the configuration from appsetting file.
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig) => _emailConfig = emailConfig;

        //this send email calling a mrthod called CreateEmailMessage

        public async Task SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }
        //Create a Separate method for creating a message
        private MimeMessage CreateEmailMessage(Message message)
        {
            //get object of MimeMessage
            var emailMessage = new MimeMessage();
            //From Which we are going to send that set in appsetting.json
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            //from whom are going to send.
            emailMessage.To.AddRange(message.To);
            //Subject for the message.
            emailMessage.Subject = message.Subject;
            //This is the body whatever we want to send in the content
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }
        //this method use for sending the massege
        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                //first we connect
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                //then Authenticate
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //then authenticate a client by providing username & Password.
                //that is also providing in appsetting.json file
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                //send email
                client.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
