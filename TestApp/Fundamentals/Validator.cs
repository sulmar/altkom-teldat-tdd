using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public interface IValidator
    {
        bool IsValid(string number);
    }

    public class PeselValidator : ValidatorBase
    {
        protected override byte[] Weights => new byte[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        protected override int CheckControl(int sumControl) => 10 - sumControl % 10;
    }

    public class NIPValidator : ValidatorBase
    {
        protected override byte[] Weights => new byte[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
        protected override int CheckControl(int sumControl) => sumControl % 11;
    }

    public abstract class ValidatorBase : IValidator
    {
        private int CalculateSumControl(byte[] numbers, byte[] weights) => numbers
            .Take(numbers.Length - 1)
            .Select((number, index) => new { number, index })
            .Sum(n => n.number * weights[n.index]);

        protected abstract byte[] Weights { get; }

        protected abstract int CheckControl(int sumControl);

        private byte[] ToByteArray(string input) => input
                                                    .ToCharArray()
                                                    .Select(c => byte.Parse(c.ToString()))
                                                    .ToArray();

        public bool IsValid(string number)
        {
            Validate(number);

            byte[] numbers = ToByteArray(number);

            int controlSum = CalculateSumControl(numbers, this.Weights);

            int controlNum = CheckControl(controlSum);


            if (controlNum == 10)
            {
                controlNum = 0;
            }

            return controlNum == numbers.Last();
        }

        private void Validate(string number)
        {
            if (!number.All(char.IsDigit))
            {
                throw new FormatException($"Number must have only digits");
            }
            else if (number.Length != Weights.Length + 1)
            {
                throw new FormatException($"Number must have {Weights.Length} digits");
            }
        }
    }


}
