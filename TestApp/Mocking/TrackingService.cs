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

    public interface ITrackingRepository
    {
        IEnumerable<Tracking> GetValid();
    }

    public class DbTrackingRepository : ITrackingRepository
    {
        private readonly TrackingContext context;

        public DbTrackingRepository(TrackingContext context)
        {
            this.context = context;
        }

        public IEnumerable<Tracking> GetValid()
        {
            return context.Trackings
                .Where(t => t.ValidGPS);

        }
    }

    public interface ILocationService
    {
        IEnumerable<Location> Get();
    }

    public class LocationService : ILocationService
    {
        private readonly ITrackingRepository trackingRepository;

        public IEnumerable<Location> Get()
        {
            return trackingRepository.GetValid().Select(t=>t.Location).ToList();
        }
    }




    // dotnet add package NGeoHash

    public class TrackingService
    {
        private readonly IFileReader fileReader;
        private readonly ILocationService locationService;

        public TrackingService(IFileReader fileReader, ILocationService locationService = null)
        {
            this.fileReader = fileReader;
            this.locationService = locationService;
        }

        public Location Get()
        {
            string json = fileReader.Get("tracking.json");

            Location location = JsonConvert.DeserializeObject<Location>(json);

            if (location == null)
                throw new ApplicationException("Error parsing the location");

            return location;
        }


        // geohash.org
        public string GetPathAsGeoHash()
        {

           // var locations = locationService.Get();

            //var path = locationService.Get().Select(l => GeoHash.Encode(l.Latitude, l.Longitude));            
            return string.Join(",", locationService.Get().Select(l => GeoHash.Encode(l.Latitude, l.Longitude)));                    
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
