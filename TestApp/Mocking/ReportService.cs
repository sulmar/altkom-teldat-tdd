using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Mocking
{

    public class SalesReportBuilder
    {
        private IEnumerable<Order> orders;

        public void AddOrders(IEnumerable<Order> orders)
        {
            this.orders = orders;
        }

        public SalesReport Build()
        {
            return Create(orders);
        }

        private static SalesReport Create(IEnumerable<Order> orders)
        {
            SalesReport salesReport = new SalesReport();

            salesReport.TotalAmount = orders.Sum(o => o.Total);

            return salesReport;
        }
    }

    public interface IUserService
    {
        IEnumerable<User> GetBosses();

        Bot GetBot();
    }

    public interface IMailService
    {
        Task Send(SalesReport salesReport, User sender, IEnumerable<User> recipients);
    }

    public class SendGridOptions
    {
        public string SecretKey { get; set; }
    }

    public class SendGridMailService : IMailService
    {
        private readonly ISendGridClient client;
        private readonly ILogger logger;

        public delegate void ReportSentHandler(object sender, ReportSentEventArgs e);
        public event ReportSentHandler ReportSent;

        public SendGridMailService(IOptions<SendGridOptions> options, ILogger logger)
        {
            this.client = new SendGridClient(options.Value.SecretKey);
            this.logger = logger;
        }

        public async Task Send(
            SalesReport report, 
            User sender, 
            IEnumerable<User> recipients)
        {
            foreach (var recipient in recipients)
            {
                if (recipient.Email == null)
                    continue;

                var message = MailHelper.CreateSingleEmail(
                    new EmailAddress(sender.Email, $"{sender.FirstName} {sender.LastName}"),
                    new EmailAddress(recipient.Email, $"{recipient.FirstName} {recipient.LastName}"),
                    "Raport sprzedaży",
                    report.ToString(),
                    report.ToHtml());


                logger.Info($"Wysyłanie raportu do {recipient.FirstName} {recipient.LastName} <{recipient.Email}>...");

                var response = await client.SendEmailAsync(message);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    ReportSent?.Invoke(this, new ReportSentEventArgs(DateTime.Now));

                    logger.Info($"Raport został wysłany.");
                }
                else
                {
                    logger.Error($"Błąd podczas wysyłania raportu.");

                    throw new ApplicationException("Błąd podczas wysyłania raportu.");
                }
            }
        }
    }

    public class DbUserService : IUserService
    {
        private readonly SalesContext salesContext;

        public DbUserService(SalesContext salesContext)
        {
            this.salesContext = salesContext;
        }

        public IEnumerable<User> GetBosses()
        {
            return salesContext.Users.OfType<Employee>().Where(e => e.IsBoss).ToList();

        }

        public Bot GetBot()
        {
            return salesContext.Users.OfType<Bot>().Single();
        }
    }

    public class ReportService
    {
        private readonly IOrderService orderService;        
        private readonly IUserService userService;
        private readonly IMailService mailService;
        private readonly SalesReportBuilder salesReportBuilder;

        public ReportService(
            IOrderService orderService,
            IUserService userService, 
            IMailService mailService)
        {
            this.orderService = orderService;
            this.userService = userService;
            this.mailService = mailService;

            this.salesReportBuilder = new SalesReportBuilder();
        }

        public async Task SendSalesReportEmailAsync(DateTime date)
        {
            var orders = orderService.Get(date.AddDays(-7), date);

            if (!orders.Any())
            {
                return;
            }
           
            salesReportBuilder.AddOrders(orders);

            SalesReport report = salesReportBuilder.Build();

            var sender = userService.GetBot();

            var recipients = userService.GetBosses();

            await mailService.Send(report, sender, recipients);
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

    public interface IOrderService
    {
        IEnumerable<Order> Get(DateTime from, DateTime to);
    }

    public class OrderService : IOrderService
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