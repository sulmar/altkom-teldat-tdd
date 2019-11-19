using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Mocking
{
    public class Invoice
    {
        public string Number { get; set; }
        public bool Agreement { get; set; }
    }

    public interface IMessageSender
    {
        void Send(string message);
    }

    public class SmsMessageSender : IMessageSender
    {
        public void Send(string message)
        {
            Console.WriteLine($"Sending {message}");
        }
    }

    public interface IInvoiceService
    {
        void Add(Invoice invoice);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly IMessageSender messageSender;

        public InvoiceService(IMessageSender messageSender)
        {
            this.messageSender = messageSender;
        }

        public void Add(Invoice invoice)
        {
            // save db

            // send sms 
            if (invoice.Agreement)
                messageSender.Send($"Wysłano fakturę nr {invoice.Number}");

        }

    }
}
