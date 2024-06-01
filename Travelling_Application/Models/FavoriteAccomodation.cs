namespace Travelling_Application.Models
{
    public class FavoriteAccomodation
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int AccomodationId { get; set; }
        public int RoomId { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public DateTime ReturnDate { get; set; }
        public int Rooms { get; set; }
        public string TypeOfRoom { get; set; }
    }
}
