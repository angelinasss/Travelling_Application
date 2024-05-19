namespace Travelling_Application.Models
{
    public class Accomodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<string> TypesOfNutrition { get; set; }
        public string Address { get; set; }
        public int Rating { get; set; }
        public string TypeOfAccomodation { get; set; }
        public string Description { get; set; }
        public List<Room> Rooms { get; set; }
        public bool Parking { get; set; }
        public int SwimmingPool { get; set; }
        public bool FreeWIFI { get; set; }
        public bool PrivateBeach { get; set; }
        public int LineOfBeach { get; set; }
        public int Restaurants { get; set; }
        public bool SPA { get; set; }
        public bool Bar { get; set; }
        public bool Garden { get; set; }
        public bool TransferToAirport { get; set; }
        public bool TactileSigns { get; set; }
        public bool SmookingRooms { get; set; }
        public bool FamilyRooms { get; set; }
        public bool CarChargingStation { get; set; }
        public bool WheelchairAccessible { get; set; }
        public bool FitnessCentre { get; set; }
        public bool PetsAllowed { get; set; }
        public bool DeliveryFoodToTheRoom { get; set; }
        public bool EveryHourFrontDesk { get; set; }
        public bool VerifiedByAdmin { get; set; }
        public bool RejectedByAdmin { get; set; }
        public string RejectedMessage { get; set; }
        public int PublisherId { get; set; }
        public byte[] MainPhoto { get; set; }
    }
}
