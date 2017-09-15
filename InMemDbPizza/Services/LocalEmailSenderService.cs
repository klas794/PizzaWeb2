using InMemDbPizza.Services;
using MimeKit;
using ProjectPizzaWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPizzaWeb.Services
{
    public class LocalEmailSenderService : IEmailSender
    {
        public void SendConfirmationEmail(Order order)
        {
            var message = string.Format(
                "Your order of {0} items has been processed. The order was created {1} {2}.", 
                order.Cart.CartItems.Count(), 
                order.OrderTime.ToShortDateString(),
                order.OrderTime.ToShortTimeString());

            SendEmailAsync(order.Address.Email, "Order confirmation", message);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Administrator", "klas.hasselquist@yh.nackademin.se"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (StreamWriter data = System.IO.File.CreateText("c:\\smtppickup\\email.txt"))
            {
                emailMessage.WriteTo(data.BaseStream);
            }
        }
    }
}
