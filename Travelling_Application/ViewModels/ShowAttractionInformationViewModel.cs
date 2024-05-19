namespace Travelling_Application.ViewModels
{
    public class ShowAttractionInformationViewModel
    {
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public int Rating { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public byte[] MainPhoto { get; set; }
        public List<byte[]> Photos { get; set; }
        public List<int> AmountOfTickets { get; set; }
        public List<string> Languages { get; set; }
        public string TimeOfDay { get; set; }
        public bool FreeCancellation { get; set; }
    }
}
