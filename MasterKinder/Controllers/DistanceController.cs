using Microsoft.AspNetCore.Mvc;

namespace MasterKinder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanceController : ControllerBase
    {
        [HttpGet("walking-time")]
        public ActionResult<double> GetWalkingTime(double lat1, double lon1, double lat2, double lon2)
        {
            var distance = DistanceCalculator.CalculateDistance(lat1, lon1, lat2, lon2);
            var time = TimeCalculator.CalculateWalkingTime(distance);
            return Ok(time);
        }
    }

    public static class TimeCalculator
    {
        public static double CalculateWalkingTime(double distanceInKm)
        {
            const double walkingSpeedKmPerHour = 5.0;
            return distanceInKm / walkingSpeedKmPerHour; // Time in hours
        }
    }
    public static class DistanceCalculator
    {
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in kilometers
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // Distance in kilometers
        }
    }
}
