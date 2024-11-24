using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ConferenceManagement.Models;

namespace ConferenceManagement.Services
{
    public class PayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly bool _isSandbox;

        public PayPalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _clientId = configuration["PayPal:ClientId"];
            _clientSecret = configuration["PayPal:ClientSecret"];
            _isSandbox = configuration.GetValue<bool>("PayPal:UseSandbox", true);

            _httpClient.BaseAddress = new Uri(_isSandbox
                ? "https://api.sandbox.paypal.com"
                : "https://api.paypal.com");
        }

        public async Task<string> CreatePaymentAsync(Booking booking)
        {
            var accessToken = await GetAccessTokenAsync();

            var paymentRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = booking.EventBook.Fee.ToString("0.00")
                        },
                        description = $"Booking for {booking.EventBook.Title}"
                    }
                },
                application_context = new
                {
                    return_url = "https://yourdomain.com/payment/success",
                    cancel_url = "https://yourdomain.com/payment/cancel"
                }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(paymentRequest),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.PostAsync("/v2/checkout/orders", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var paymentResponse = JsonSerializer.Deserialize<PayPalOrderResponse>(result);
                return paymentResponse.Id;
            }

            throw new ApplicationException("Failed to create PayPal payment");
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));

            var content = new StringContent(
                "grant_type=client_credentials",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            var response = await _httpClient.PostAsync("/v1/oauth2/token", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var token = JsonSerializer.Deserialize<PayPalTokenResponse>(result);
                return token.AccessToken;
            }

            throw new ApplicationException("Failed to get PayPal access token");
        }

        public async Task<bool> VerifyPaymentAsync(string orderId)
        {
            var accessToken = await GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"/v2/checkout/orders/{orderId}");
            return response.IsSuccessStatusCode;
        }
    }
}