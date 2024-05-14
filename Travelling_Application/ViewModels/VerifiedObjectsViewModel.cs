using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class VerifiedObjectsViewModel
    {
        public List<Car> VerifiedCars { get; set; }
        public List<AirTicket> VerifiedAirTickets { get; set; }
        public List<Entertainment> VerifiedAttractions { get; set; }
    }
}
