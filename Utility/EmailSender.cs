using LMS2.Models;
using System.Net;
using System.Net.Mail;
namespace LMS2.Utility
{



    /// <summary>
    /// Email Sender class
    /// </summary>
    public class EmailSender
    {
        private readonly string _Host;
        private readonly int _Port;
        private readonly string _SMTP_USERNAME;
        private readonly string _SMTP_PASSWORD;



        /// <summary>
        /// Email sender constructor
        /// </summary>
        public EmailSender()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _Host = configuration.GetValue<string>("AppConstraint:Host") ?? "";
            _Port = configuration.GetValue<int>("AppConstraint:Port");
            _SMTP_USERNAME = configuration.GetValue<string>("AppConstraint:SMTP_USERNAME") ?? "";
            _SMTP_PASSWORD = configuration.GetValue<string>("AppConstraint:SMTP_PASSWORD") ?? "";
        }



        /// <summary>
        /// Send Email message
        /// </summary>
        public void SendEmail(BorrowRecord borrowRecord)
        {
            //var client = new SmtpClient(_Host,_Port )
            //{
            //    Credentials = new NetworkCredential(_SMTP_USERNAME, _SMTP_PASSWORD ),
            //    EnableSsl = true
            //};
            var client = new SmtpClient("localhost", 25)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                EnableSsl = false
            };

            string subject = "Reminder: Your Book Due Date is Approaching";
            string body = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                                <style>
                                    body {{ font-family: Arial, sans-serif; }}
                                    .container {{ max-width: 600px; margin: auto; padding: 20px; }}
                                    .header {{ font-size: 18px; font-weight: bold; margin-bottom: 10px; }}
                                    .content {{ font-size: 14px; }}
                                    .footer {{ margin-top: 20px; font-size: 12px; color: gray; }}
                                </style>
                            </head>
                            <body>
                                <div class='container'>
                                    <div class='header'>Reminder: Book Due Date Approaching</div>
                                    <div class='content'>
                                        <p>Dear 
                                        {borrowRecord.Member?.Name},</p>
                                        <p>This is a friendly reminder that the book you borrowed, <strong>'{borrowRecord.Book?.Title}'</strong>, is due on <strong>{borrowRecord.DueDate.ToShortDateString()}</strong>.</p>
                                        <p>Please make sure to return it by the due date to avoid any late fees.</p>
                                        <p>Thank you!</p>
                                    </div>
                                    <div class='footer'>
                                        Best regards,<br>
                                        Your Library Team
                                    </div>
                                </div>
                            </body>
                            </html>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("abc@example.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(borrowRecord.Member?.Email ?? "cba@example.com");
            client.Send(mailMessage);
        }
    }
}
