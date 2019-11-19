using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Mocking;
// using static Moq.Syntax;

namespace TestApp.NUnitTests
{
    public class InvoiceServiceTests
    {
        private Mock<IMessageSender> messageSender;
        private IInvoiceService invoiceService;
        private Invoice invoice;

        [SetUp]
        public void Setup()
        {
            messageSender = new Mock<IMessageSender>();
            invoiceService = new InvoiceService(messageSender.Object);
            invoice = new Invoice { Number = "a" };
        }

        [Test]
        public void Add_WhenIsAgreement_SendOrder()
        {
            invoice.Agreement = true;

            invoiceService.Add(invoice);

            messageSender.Verify(ms => ms.Send("Wysłano fakturę nr a"));

        }

        /* experimental
         
        [Test]
        public void Add_WhenIsAgreement_SendOrder2()
        {
            IMessageSender messageSender = Mock.Of<IMessageSender>();

            InvoiceService invoiceService = new InvoiceService(messageSender);
            
            messageSender.Send(Any<string>()).Returns());

            invoice.Agreement = true;

            invoiceService.Add(invoice);

            messageSender.Verify(ms => ms.Send("Wysłano fakturę nr a"));

        }

    */

        [Test]
        public void Add_WhenDisagreement_NotSendOrder()
        {
            invoice.Agreement = false;

            invoiceService.Add(invoice);

            messageSender.Verify(ms => ms.Send(It.IsAny<string>()), Times.Never);

        }
    }
}
