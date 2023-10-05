using Microsoft.AspNetCore.Mvc;
using System.Net;
using Jayride.Model;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.OpenApi.Services;
using System.Linq;

namespace Jayride.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {
        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Get location details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var candidateInfo = new
            {
                name = "test",
                phone = "test"
            };
            return Ok(candidateInfo);
        }

        public async Task<T?> GetAsync<T>(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return default;
                }
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// List the location based on I.P
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("locationdetails")]
        public async Task<IActionResult> GetLocationDetailsAsync(string ipAddress)
        {
            var result = await GetAsync<Model.Location>(string.Format(Constants.api_url, ipAddress));
            return Ok(result);
        }

        /// <summary>
        /// Get the price list based on capacity of passengers
        /// </summary>
        /// <param name="passengerNumber"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getpricelist")]
        public async Task<IActionResult> GetPriceList(int passengerNumber)
        {
            if(passengerNumber <= 0)
            {
                return Ok(StatusCodes.Status406NotAcceptable);
            }
            var result = await GetAsync<ListingResult>(Constants.api_pricelisturl);

            var filteredListings = result.Listings.Where(listing => listing.vehicleType.maxPassengers >= passengerNumber).ToList();

            foreach (var listing in filteredListings) { listing.totalPrice = passengerNumber * listing.pricePerPassenger; }

            var priceresult = filteredListings.OrderBy(x=> x.totalPrice);

            return Ok(priceresult);
        }
    }
}