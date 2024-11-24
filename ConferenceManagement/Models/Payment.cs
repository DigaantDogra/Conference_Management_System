using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceManagement.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentProvider
    {
        PayPal,
        BankTransfer
    }

    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }
        public PaymentProvider Provider { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Currency { get; set; } = "USD";
        public string PayerEmail { get; set; }
        public string PaymentNotes { get; set; }
    }
}