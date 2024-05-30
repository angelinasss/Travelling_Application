using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class ForVerificationObjectsViewModel
    {
        public List<Car> Cars { get; set; }
        public List<AirTicket> AirTickets { get; set; }
        public List<Entertainment> Attractions { get; set; }
        public List<Accomodation> Accomodation { get; set; }
        public List<BookingCar> BookingCars { get; set; }
        public List<BookingAirTicket> BookingAirTickets { get; set; }
        public List<BookingAttraction> BookingAttractions { get; set; }
        public List<BookingAccomodation> BookingAccomodation { get; set; }
        public List<UserData> UsersDataAirTickets { get; set; }
        public List<UserData> UsersDataAttractions { get; set; }
        public List<UserData> UsersDataAccomodation { get; set; }
        public List<UserData> UsersDataCars { get; set; }
    }
}
