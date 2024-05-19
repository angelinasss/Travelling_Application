using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class UnverifiedObjectsViewModel
    {
        public List<Car> UnverifiedCars { get; set; }
        public List<AirTicket> UnverifiedAirTickets { get; set; }
        public List<Entertainment> UnverifiedAttractions { get; set; }
        public List<Accomodation> UnverifiedAccomodation { get; set; }
    }
}
