using Azure.Storage.Blobs.Models;
using MTAA_Backend.Domain.Exceptions;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Domain.Resources.Locations;
using Nest;
using NetTopologySuite.Geometries;
using System.CodeDom;

namespace MTAA_Backend.Application.Services.Locations
{
    /// <summary>
    /// Provides services for normalizing geographic location coordinates and related parameters.
    /// </summary>
    public class NormalizeLocationService : INormalizeLocationService
    {
        /// <summary>
        /// The maximum allowed number of tiles for radius normalization.
        /// </summary>
        public const double MAX_ALLOWED_TILES_COUNT = 20;

        /// <summary>
        /// Normalizes latitude and longitude to ensure they are within valid ranges.
        /// </summary>
        /// <param name="latitude">The latitude to normalize, modified in place.</param>
        /// <param name="longitude">The longitude to normalize, modified in place.</param>
        public void NormalizeLocation(ref double latitude, ref double longitude)
        {
            if (latitude < LocationConstants.MIN_LATITUDE || latitude > LocationConstants.MAX_LATITUDE)
            {
                latitude = latitude % LocationConstants.MAX_LATITUDE;
            }
            if (longitude < LocationConstants.MIN_LONGITUDE || longitude > LocationConstants.MAX_LONGITUDE)
            {
                longitude = longitude % LocationConstants.MAX_LONGITUDE;
            }
        }

        /// <summary>
        /// Normalizes latitude, longitude, radius, and zoom level for location-based queries.
        /// </summary>
        /// <param name="latitude">The latitude to normalize, modified in place.</param>
        /// <param name="longitude">The longitude to normalize, modified in place.</param>
        /// <param name="radius">The radius to normalize, modified in place.</param>
        /// <param name="zoomLevel">The zoom level to normalize.</param>
        public void NormalizeLocation(ref double latitude, ref double longitude, ref double radius, int zoomLevel)
        {
            NormalizeLocation(ref latitude, ref longitude);
            if (zoomLevel < LocationConstants.MIN_ZOOM_LEVEL) zoomLevel = LocationConstants.MIN_ZOOM_LEVEL;
            if (zoomLevel > LocationConstants.MAX_ZOOM_LEVEL) zoomLevel = LocationConstants.MAX_ZOOM_LEVEL;

            double secondLon = longitude + LocationConstants.MAX_LONGITUDE / Math.Pow(2, zoomLevel);

            if (secondLon > LocationConstants.MAX_LONGITUDE) secondLon = LocationConstants.MAX_LONGITUDE;

            double distance = DistanceBetweenPoints(latitude, longitude, latitude, secondLon);

            if(radius > distance* MAX_ALLOWED_TILES_COUNT)
            {
                radius = distance * MAX_ALLOWED_TILES_COUNT;
            }
            if (radius < 0) radius = 0;
        }

        //Taken from https://stackoverflow.com/questions/6366408/calculating-distance-between-two-latitude-and-longitude-geocoordinates
        /// <summary>
        /// Calculates the distance between two geographic points using the Haversine formula.
        /// </summary>
        /// <param name="lat1Deg">The latitude of the first point in degrees.</param>
        /// <param name="lon1Deg">The longitude of the first point in degrees.</param>
        /// <param name="lat2Deg">The latitude of the second point in degrees.</param>
        /// <param name="lon2Deg">The longitude of the second point in degrees.</param>
        /// <returns>The distance between the points in meters.</returns>
        public static double DistanceBetweenPoints(double lat1Deg, double lon1Deg, double lat2Deg, double lon2Deg)
        {
            const double EarthRadius = 6371000;

            double lat1Rad = DegreesToRadians(lat1Deg);
            double lon1Rad = DegreesToRadians(lon1Deg);
            double lat2Rad = DegreesToRadians(lat2Deg);
            double lon2Rad = DegreesToRadians(lon2Deg);

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EarthRadius * c;
            return distance;
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="deg">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        private static double DegreesToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }
    }
}
