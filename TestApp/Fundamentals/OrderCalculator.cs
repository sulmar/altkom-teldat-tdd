using System;
using System.Collections.Generic;
using System.Linq;

namespace TestApp
{

    public static class DateTimeExtensions
    {
        public static bool IsBlackFriday(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Friday
                 && date.Month == 11
                 && date.AddDays(7).Month == 12;
        }
    }

    public interface IDiscountCalculator
    {
        decimal CalculateDiscount(Order order);
    }

    public class HappyHoursDiscountCalculator : IDiscountCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;
        private readonly decimal fixedAmount;

        public HappyHoursDiscountCalculator(TimeSpan from, TimeSpan to, decimal fixedAmount)
        {
            this.from = from;
            this.to = to;
            this.fixedAmount = fixedAmount;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            if (order.OrderedDate.TimeOfDay.IsBetween(from, to))
            {
                return fixedAmount;
            }


            return 0m;
        }
    }


    public class BlackFridayDiscountCalculator : IDiscountCalculator
    {
        private readonly decimal percentage;

        public BlackFridayDiscountCalculator(decimal percentage)
        {
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            if (order.OrderedDate.IsBlackFriday())
            {
                return order.Total * percentage;
            }

       
            return 0m;
        }
    }

    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }


    public interface IDiscountAmountStrategy
    {
        decimal Discount(decimal amount);
    }

    public class PercentageDiscountAmountStrategy : IDiscountAmountStrategy
    {
        private readonly decimal percentage;

        public decimal Discount(decimal amount) => amount * percentage;
    }

    public class FixedDiscountAmountStrategy : IDiscountAmountStrategy
    {
        private readonly decimal fixedAmount;

        public FixedDiscountAmountStrategy(decimal fixedAmount)
        {
            this.fixedAmount = fixedAmount;
        }

        public decimal Discount(decimal amount) => fixedAmount;
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

        public bool CanDiscount(Order order)
        {
            return order.OrderedDate.TimeOfDay.IsBetween(from, to);
        }
    }

    public class DiscountCalculator : IDiscountCalculator
    {
        private readonly ICanDiscountStrategy canDiscountStrategy;
        private readonly IDiscountAmountStrategy discountAmountStrategy;

        public DiscountCalculator(ICanDiscountStrategy canDiscountStrategy, IDiscountAmountStrategy discountAmountStrategy)
        {
            this.canDiscountStrategy = canDiscountStrategy;
            this.discountAmountStrategy = discountAmountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();

            if (canDiscountStrategy.CanDiscount(order))
            {
                return discountAmountStrategy.Discount(order.Total);
            }


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

    
}
