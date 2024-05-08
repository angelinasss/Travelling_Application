namespace Travelling_Application.Models
{
    public class AirTicket
    {
        public int Id { get; set; }
        public string CityFrom { get; set; }
        public string CountryFrom { get; set; }
        public string CityTo { get; set; }
        public string CountryTo { get; set; }
        public double CostEC { get; set; }
        public double CostBC { get; set; }
        public double CostFC { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public int AmountOfTicketsEC { get; set; }
        public int AmountOfTicketsBC { get; set; }
        public int AmountOfTicketsFC { get; set; }
        public string FlightNumber { get; set; }
        public int PublisherId { get; set; }
        public bool FreeCancellation { get; set; }
        public bool IncludeLuggageEC { get; set; }
        public bool IncludeLuggageBC { get; set; }
        public bool IncludeLuggageFC { get; set; }
        public string DepartureCountryCode { get; set; }
        public string ArrivalCountryCode { get; set; }
        public bool VerifiedByAdmin { get; set; }
        public bool RejectedByAdmin { get; set; }
    }
}