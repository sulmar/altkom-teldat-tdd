using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp
{
    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface IDiscountStrategy
    {
        decimal Discount(decimal amount);
    }

    public interface IDiscountCalculator
    {
        decimal CalculateDiscount(Order order);
    }

    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly IDiscountStrategy discountStrategy;

        public DiscountCalculator(
            ICanDiscountStrategy canDiscountStrategy, 
            IDiscountStrategy discountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.discountStrategy = discountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (canDiscountStrategy.CanDiscount(order))
            {
                return discountStrategy.Discount(order.Total);
            }
            else
                return 0m;
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

    public class DayOfWeekCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly DayOfWeek dayOfWeek;

        public DayOfWeekCanDiscountStrategy(DayOfWeek dayOfWeek) => this.dayOfWeek = dayOfWeek;

        public bool CanDiscount(Order order) => order.OrderedDate.DayOfWeek == dayOfWeek;
    }


    public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.from = from;
            this.to = to;
        }

        public bool CanDiscount(Order order) => order.OrderedDate.TimeOfDay.IsBetween(from, to);
    }
}
