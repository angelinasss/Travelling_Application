namespace Travelling_Application.Models
{
    public class CarAvailability
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
