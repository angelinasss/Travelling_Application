namespace Travelling_Application.Models
{
    public class Accomodation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsParking { get; set; }
        public bool IsSwimmingPool { get; set; }
        public bool IsFreeWIFI { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public string TypeOfAccomodation { get; set; }

        public double Cost { get; set; }
        public string Description { get; set; }
    }
}
