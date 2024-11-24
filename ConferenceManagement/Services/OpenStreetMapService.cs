using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ConferenceManagement.Models;

namespace ConferenceManagement.Services
{
    public class OpenStreetMapService
    {
        private readonly HttpClient _httpClient;

        public OpenStreetMapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ConferenceManagementApp/1.0");
        }

        public async Task<Location> GetLocationDetailsAsync(string address)
        {
            var encodedAddress = Uri.EscapeDataString(address);
            var response = await _httpClient.GetAsync(
                $"/search?q={encodedAddress}&format=json&limit=1");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<OpenStreetMapResponse[]>(content);

                if (results?.Length > 0)
                {
                    var result = results[0];
                    return new Location
                    {
                        Latitude = double.Parse(result.Lat),
                        Longitude = double.Parse(result.Lon),
                        Address = address
                    };
                }
            }

            return null;
        }
    }

    class OpenStreetMapResponse
    {
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string DisplayName { get; set; }
    }
}