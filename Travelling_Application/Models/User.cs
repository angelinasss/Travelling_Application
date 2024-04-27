using System.Data;

namespace Travelling_Application.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public DateTime Birthday { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        public string CountryCode { get; set; }
        public byte[] UserPhotoUrl { get; set; }
        public List<Accomodation> FavoriteAccomodation { get; set; }
        public List<Accomodation> BookingAccomodation { get; set; }
        public List<AirTicket> FavoriteAirTicket { get; set; }
        public List<AirTicket> BookingAirTicket { get; set; }
        public List<Car> FavoriteCars { get; set; }
        public List<Car> BookingCars { get; set; }
        public List<Entertainment> FavoriteEntertainment { get; set; }
        public List<Entertainment> BookingEntertainment { get; set; }
    }
}
