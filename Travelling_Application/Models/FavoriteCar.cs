namespace Travelling_Application.Models
{
    public class FavoriteCar
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
