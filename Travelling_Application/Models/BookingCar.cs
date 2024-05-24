namespace Travelling_Application.Models
{
    public class BookingCar
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public bool VerifiedBooking { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
