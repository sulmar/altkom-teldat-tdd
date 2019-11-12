using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp
{

    public class DiscountCalculator 
    {
        public decimal CalculateDiscount(Order order)
        {
            throw new NotImplementedException();
        }
    }

    public class Order
    {
        public DateTime OrderedDate { get; set; }

        public Order()
        {
            Details = new List<OrderDetail>();
        }

        public List<OrderDetail> Details { get; set; }

        public decimal Total => Details.Sum(d => d.Total);
    }

    public class OrderDetail
    {
        public OrderDetail(decimal unitPrice, short quantity = 1)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }

    
}
