using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NGeoHash;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestApp.Mocking
{
    public class FileReader : IFileReader
    {
        public string Get(string path)
        {
            return File.ReadAllText(path);
        }
    }

    public class FakeValidFileReader : IFileReader
    {
        public string Get(string path)
        {
            Location location = new Location(53.125, 18.011111);

            return JsonConvert.SerializeObject(location);
        }
    }

    public class FakeInvalidFileReader : IFileReader
    {
        public string Get(string path)
        {
            return string.Empty;
        }
    }

    public interface IFileReader
    {
        string Get(string path);
    }

    // dotnet add package NGeoHash

    public class TrackingService
    {
        private readonly IFileReader fileReader;

        public TrackingService(IFileReader fileReader)
        {
            this.fileReader = fileReader;
        }

        public Location Get()
        {
            string json = fileReader.Get("tracking.txt");

            Location location = JsonConvert.DeserializeObject<Location>(json);

            if (location == null)
                throw new ApplicationException("Error parsing the location");

            return location;
        }


        // geohash.org
        public string GetPathAsGeoHash()
        {
            IList<string> path = new List<string>();

            using (var context = new TrackingContext())
            {
                var locations = context.Trackings.Where(t=>t.ValidGPS).Select(t=>t.Location).ToList();

                foreach (Location location in locations)
                {
                    path.Add(GeoHash.Encode(location.Latitude, location.Longitude));
                }

                return string.Join(",", path);
                    
            }
        }
    }


    public class LocationRepository
    {
        private readonly TrackingContext context;

        public LocationRepository(TrackingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Location> Get()
        {
            var locations = context.Trackings.Where(t => t.ValidGPS).Select(t => t.Location).ToList();

            return locations;

        }
    }

    public class TrackingContext : DbContext
    {
        public DbSet<Tracking> Trackings { get; set; }
    }

    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }

    }

    public class Tracking
    {
        public Location Location { get; set; }
        public byte Satellites { get; set; }
        public bool ValidGPS { get; set; }
    }


}
