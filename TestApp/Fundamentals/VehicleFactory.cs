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

    public class Mine : Vehicle
    {
        public string Unit { get; set; }
    }


    public class Alien : Vehicle
    {
        public string Country { get; set; }
    }


    public static class VehicleFactory
    {
        // https://pl.wikipedia.org/wiki/Oznakowania_statk%C3%B3w_powietrznych
        public static Vehicle Create(string code)
        {
            // ICAO Code = SP   -> Mine

            // ICAO Code <> SP  -> Alien



            return new Mine();
        }
    }
}
