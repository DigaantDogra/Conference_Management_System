using ConferenceManagement.Models;

namespace ConferenceManagement.ViewModels
{
    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string EventTitle { get; set; }
        public PaymentProvider[] PaymentProviders { get; set; }
    }

    public class BankTransferViewModel
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string EventTitle { get; set; }
        public BankDetails BankDetails { get; set; }
    }

    public class BankDetails
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string SwiftCode { get; set; }
    }
}