using System.Reflection;

namespace Jayride.Model
{
    public class Location
    {
        public string continent_name { get; set; }
        public string country_name { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
    }

    public class ListingResult
    {
        public List<listings> Listings { get; set; }
    }
    public class listings
    {
        public string name { get; set; }
        public double pricePerPassenger { get; set; }
        public vehicleType vehicleType { get; set; }
        public double totalPrice { get; set; }
    }

    public class vehicleType
    {
        public string name { get; set; }
        public int maxPassengers { get; set; }
    }

    public class Constants
    {
        public const string api_url = @"http://api.ipstack.com/{0}?access_key=fb2c63fe1d454699efba39f59c4d774f&format=1";
        public const string api_pricelisturl = @"https://jayridechallengeapi.azurewebsites.net/api/QuoteRequest";
    }
}
