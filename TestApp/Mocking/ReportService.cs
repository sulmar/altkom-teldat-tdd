using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Mocking
{
    public class ReportService
    {
        private const string apikey = "your_secret_key";

        public delegate void ReportSentHandler(object sender, ReportSentEventArgs e);
        public event ReportSentHandler ReportSent;

        public Action<string> Log { get; set; }

        public async Task SendSalesReportEmailAsync(DateTime date)
        {
            OrderService orderService = new OrderService();

            var orders = orderService.Get(date.AddDays(-7), date);

            if (!orders.Any())
            {
                return;
            }

            SalesReport report = Create(orders);

            SendGridClient client = new SendGridClient(apikey);

            SalesContext salesContext = new SalesContext();

            var recipients = salesContext.Users.OfType<Employee>().Where(e => e.IsBoss).ToList();

            var sender = salesContext.Users.OfType<Bot>().Single();

            foreach (var recipient in recipients)
            {
                if (recipient.Email == null)
                    continue;

                var message = MailHelper.CreateSingleEmail(
                    new EmailAddress(sender.Email, sender.FullName), 
                    new EmailAddress(recipient.Email, recipient.FullName), 
                    "Raport sprzedaży",
                    report.ToString(),
                    report.ToHtml());

                var response = await client.SendEmailAsync(message);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ReportSent?.Invoke(this, new ReportSentEventArgs(DateTime.Now));

                    Log?.Invoke($"Raport został wysłany.");
                }
                else
                {
                    throw new ApplicationException("Błąd podczas wysyłania raportu");
                }
            }
        }

        private static SalesReport Create(IEnumerable<Order> orders)
        {
            SalesReport salesReport = new SalesReport();

            salesReport.TotalAmount = orders.Sum(o => o.Total);

            return salesReport;
        }
    }

    public class ReportSentEventArgs : EventArgs
    {
        public readonly DateTime SentDate;

        public ReportSentEventArgs(DateTime sentDate)
        {
            this.SentDate = sentDate;
        }
    }

    public class OrderService
    {
        private readonly SalesContext context;

        public OrderService()
        {
            context = new SalesContext();
        }

        public IEnumerable<Order> Get(DateTime from, DateTime to)
        {
            return context.Orders.Where(o => o.OrderedDate > from && o.OrderedDate < to).ToList();
        }
    }

    public class SalesContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }


    #region Models

    public abstract class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
   

    public class Employee : User
    {
        public bool IsBoss { get; set; }
    }

    public class Bot : User
    {

    }

    public abstract class Report
    {
        public DateTime CreatedOn { get; set; }

        public string Name { get; }

        public Report()
        {
            CreatedOn = DateTime.Now;
        }
    }

    public class SalesReport : Report
    {
        public TimeSpan TotalTime { get; set; }

        public decimal TotalAmount { get; set; }


        public override string ToString()
        {
            return $"Report created on {CreatedOn} \r\n TotalAmount: {TotalAmount}";
        }

        public string ToHtml()
        {
            return $"<html>Report created on <b>{CreatedOn}</b> <p>TotalAmount: {TotalAmount}<p></html>";
        }
    }

    #endregion

}