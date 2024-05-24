namespace Travelling_Application.Models
{
    public class BookingAirTicket
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int AirTicketId { get; set; }
        public bool VerifiedBooking { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public DateTime ReturnDate { get; set; }
        public int Passengers { get; set; }
    }
}
