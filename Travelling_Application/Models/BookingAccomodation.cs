namespace Travelling_Application.Models
{
    public class BookingAccomodation
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int AccomodationId { get; set; }
        public int RoomId { get; set; }
        public bool VerifiedBooking { get; set; }
        public bool RejectedBooking { get; set; }
        public string RejectedMessage { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public DateTime ReturnDate { get; set; }
        public int Adults { get; set; }
        public string TypeOfRoom { get; set; }

        public bool CanceledBooking { get; set; }
  
    }
}
