﻿using Travelling_Application.Models;

namespace Travelling_Application.ViewModels
{
    public class WaitingBookingsViewModel
    {
        public List<Car> Cars { get; set; }
        public List<AirTicket> AirTickets { get; set; }
        public List<Entertainment> Attractions { get; set; }
        public List<Accomodation> Accomodation { get; set; }
        public List<BookingCar> BookingCars { get; set; }
        public List<BookingAirTicket> BookingAirTickets { get; set; }
        public List<BookingAttraction> BookingAttractions { get; set; }
        public List<BookingAccomodation> BookingAccomodation { get; set; }
    }
}
