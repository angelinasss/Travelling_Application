using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class RejectedObjectsViewModel
    {
        public List<Car> RejectedCars { get; set; }
        public List<AirTicket> RejectedAirTickets { get; set; }
        public List<Entertainment> RejectedAttractions { get; set; }
        public List<Accomodation> RejectedAccomodation { get; set; }
    }
}
