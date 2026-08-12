using System.Net.Mail;


namespace PanelSolar.Helpers
{
    public class MailService
    {
        IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmailOutlook(String emisor, String receptor, String asunto, String mensaje, String telefono)
        {
            String? usermail = this.configuration["usuariooutlook"];
            String? passwordmail = this.configuration["passwordoutlook"];
            String? smtpserver = this.configuration["host"];
            String? emailcc = this.configuration["emailcc"];
            int port = int.Parse(this.configuration["port"]);
            bool ssl = bool.Parse(this.configuration["ssl"]);
            bool defaultcreadentials = bool.Parse(this.configuration["defaultcredentials"]);
            SmtpClient smtpClient = new SmtpClient
            {
                Host = smtpserver,
                Port = port,
                EnableSsl = ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = defaultcreadentials,
                Credentials = new System.Net.NetworkCredential(usermail, passwordmail),
            };
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(usermail);
            mail.To.Add(new MailAddress(receptor));
            if (emailcc != null && emailcc != "")
                mail.CC.Add(new MailAddress(emailcc));

            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            smtpClient.Send(mail);
        }
    }
}
