using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Fakers
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsRemoved { get; set; }
        public Gender Gender { get; set; }
        public decimal Salary { get; set; }
      //  public string City { get; set; }
    }

    public enum Gender
    {
        Male,
        Fale
    }

    // dotnet add package Bogus
    public class CustomerFaker : Faker<Customer>
    {
        public CustomerFaker()
        {
            StrictMode(true);
            RuleFor(p => p.Id, f => f.IndexFaker);
            RuleFor(p => p.FirstName, f => f.Person.FirstName);
            RuleFor(p => p.LastName, f => f.Person.LastName);
            RuleFor(p => p.Gender,  f => (Gender) f.Person.Gender);
            RuleFor(p => p.IsRemoved, f => f.Random.Bool(0.3f));
            RuleFor(p => p.Salary, f => f.Random.Decimal(1000, 2000));
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }

    public interface IEntityRepository<TEntity>
    {
        IEnumerable<TEntity> Get();
    }

    public interface ICustomerRepository : IEntityRepository<Customer>
    {
        IEnumerable<Customer> Get(string city);
    }

    public interface IProductRepository : IEntityRepository<Product>
    {

    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly IEnumerable<Customer> customers;

        public CustomerRepository()
        {
            CustomerFaker customerFaker = new CustomerFaker();

            customers = customerFaker.Generate(100);
        }


        public IEnumerable<Customer> Get(string city)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> Get()
        {
            return customers;
        }
    }
}
