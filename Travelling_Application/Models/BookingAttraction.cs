﻿namespace Travelling_Application.Models
{
    public class BookingAttraction
    {
        public int id {  get; set; }
        public int UserId { get; set; }
        public int AttractionId { get; set; }
        public bool VerifiedBooking { get; set; }
        public bool RejectedBooking { get; set; }
        public string RejectedMessage { get; set; }
        public int AmountOfTickets { get; set; }
        public DateTime Date {  get; set; }
        public bool CanceledBooking { get; set; }
    }
}