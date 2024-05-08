namespace Travelling_Application.Models
{
    public class Entertainment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public List<byte[]> EntertainmentPhotos { get; set; }
        public int Rating { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<int> AmountOfTickets { get; set; }
        public List<string> Languages { get; set; }
        public string TimeOfDay { get; set; }
        public int PublisherId { get; set; }
        public bool FreeCancellation { get; set; }
        public bool VerifiedByAdmin { get; set; }
        public bool RejectedByAdmin { get; set; }
    }
}
