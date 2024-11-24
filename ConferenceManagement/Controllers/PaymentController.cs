using Microsoft.AspNetCore.Mvc;
using ConferenceManagement.Services;
using ConferenceManagement.Models;
using System;
using System.Threading.Tasks;
using ConferenceManagement.ViewModels;

namespace ConferenceManagement.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PayPalService _paypalService;

        public PaymentController(PayPalService paypalService)
        {
            _paypalService = paypalService;
        }

        public IActionResult Checkout(int bookingId)
        {
            var booking = DummyDataService.GetBookingById(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            var viewModel = new PaymentViewModel
            {
                BookingId = bookingId,
                Amount = Convert.ToDecimal(booking.EventBook.Fee),
                EventTitle = booking.EventBook.Title,
                PaymentProviders = new[]
                {
                    PaymentProvider.PayPal,
                    PaymentProvider.BankTransfer
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InitiatePayment([FromBody] PaymentInitiateRequest request)
        {
            var booking = DummyDataService.GetBookingById(request.BookingId);
            if (booking == null)
            {
                return NotFound();
            }

            try
            {
                switch (request.Provider)
                {
                    case PaymentProvider.PayPal:
                        var paypalOrderId = await _paypalService.CreatePaymentAsync(booking);
                        return Json(new { success = true, orderId = paypalOrderId });

                    case PaymentProvider.BankTransfer:
                        return Json(new { success = true, redirectUrl = Url.Action("BankTransfer", new { bookingId = request.BookingId }) });

                    default:
                        return BadRequest("Invalid payment provider");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> Success(string orderId)
        {
            if (await _paypalService.VerifyPaymentAsync(orderId))
            {
                // Update booking status
                return View("Success");
            }
            return View("Failed");
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }

    public class PaymentInitiateRequest
    {
        public int BookingId { get; set; }
        public PaymentProvider Provider { get; set; }
    }
}