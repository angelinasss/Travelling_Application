using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class SearchFlightViewModel
    {
        public List<string> CitiesFrom { get; set; }
        public List<string> CitiesTo { get; set; }
        public List<AirTicket> Results { get; set; }
    }
}
