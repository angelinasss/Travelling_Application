namespace Travelling_Application.Models
{
    public class AirTicket
    {
        public int Id { get; set; }
        public string CityFrom { get; set; }
        public string CountryFrom { get; set; }
        public string CityTo { get; set; }
        public string CountryTo { get; set; }
        public double Cost { get; set; }
        public DateTime DateTime { get; set; }
        public string FlightNumber { get; set; }
    }
}
