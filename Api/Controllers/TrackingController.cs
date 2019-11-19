using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Mocking;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly TrackingService trackingService;

        public TrackingController()
        {
            trackingService = new TrackingService(new FileReader(), new LocationService());
        }

        [HttpGet("path")]
        public IActionResult GetPath()
        {
            string path = trackingService.GetPathAsGeoHash();

            return Ok(path);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World");
        }
    }
}
