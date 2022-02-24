using Strategy.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Strategy.Business.Strategies.Invoice
{
    public class EmailInvoiceStrategy : InvoiceStrategy
    {
        public override void Generate(Order order)
        {
            var body = GenerateTextInvoice(order);
            using var client = new SmtpClient("smtp.sengrid.net", 587);
            var credentials = new NetworkCredential("juanchosgm", "");
            client.Credentials = credentials;
            var message = new MailMessage("juanchosgm@hotmail.com", "juanchosgm@hotmail.com")
            {
                Subject = "We've created an invoice for your order",
                Body = body,
            };
            client.Send(message);
        }
    }
}
