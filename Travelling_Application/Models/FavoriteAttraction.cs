namespace Travelling_Application.Models
{
    public class FavoriteAttraction
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int AttractionId { get; set; }
        public int AmountOfTickets { get; set; }
        public DateTime Date { get; set; }
    }
}
