using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.Fundamentals
{
    public abstract class Vehicle
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class Friend : Vehicle
    {
        public string Unit { get; set; }
    }


    public class Foe : Vehicle
    {
        public string Country { get; set; }
    }


    public static class Countries
    {
        public static IDictionary<string, string> Data =>
            new Dictionary<string, string>
        {
            { "SP", "Poland" },
            { "ES", "Estonia"},
            { "OK", "Czechy"},
            { "EW", "Białoruś"},
            { "D",  "Niemcy"},
            { "OM", "Słowacja"},
            { "UR", "Ukraina"},
            { "9A", "Chorwacja"},
            { "4O", "Czarnogóra"},
            { "S5", "Słowenia" },
        };
    }


    public class CountryFactory
    {
        private IDictionary<string, string> codes;

        public CountryFactory()
        {
            codes = Countries.Data;
        }
        
        
        public string Create(string code)
        {
            if (codes.TryGetValue(code, out string country))
            {
                return country;
            }
            else
            {
                throw new NotSupportedException($"{code} is not supported");
            }
        }
    }

    public static class VehicleFactory
    {
        // https://pl.wikipedia.org/wiki/Oznakowania_statk%C3%B3w_powietrznych
        public static Vehicle Create(string symbolIdentifier)
        {
            // ICAO Code = SP   -> Friend

            // ICAO Code in ( ...  )  -> Foe

            // throw new NotSupportedException()

            // return new Friend();
            
            if (symbolIdentifier == "SP")
                return new Friend { Code = symbolIdentifier };

            else
            {
                CountryFactory countryFactory = new CountryFactory();
                return new Foe { Country = countryFactory.Create(symbolIdentifier) };
            }

            throw new NotSupportedException();
        }
    }

   
}
