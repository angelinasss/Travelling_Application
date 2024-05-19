namespace Travelling_Application.Models
{
    public class Room
    {
        public int ID { get; set; }
        public int AccomodationId { get; set; }
        public string RoomName { get; set; }
        public List<DateTime> AvailableDatesRoom { get; set; }
        public List<int> AmountOfAvailableSameRooms { get; set; }
       // public List<Photo> Photos { get; set; }
        public double RoomCost { get; set; }
        public string RoomDescription { get; set; }
        public bool WashingMachine { get; set; }
        public bool Kitchen { get; set; }
        public bool WheelchairAccessibleRoom { get; set; }
        public bool ToiletWithGrabBars { get; set; }
        public bool BathtubWithgrabbars { get; set; }
        public bool BarrierFreeShower { get; set; }
        public byte[] MainPhoto { get; set; }
        public bool ShowerWithoutEdge { get; set; }
        public bool HighToilet { get; set; }
        public bool LowSink { get; set; }
        public bool BathroomEmergencyButton { get; set; }
        public bool ShowerChair { get; set; }
        public string TypeOfNutritionRoom { get; set; }
        public bool CoffeeMachine { get; set; }
        public bool CoffeeOrTea { get; set; }
        public bool ElectricKettle { get; set; }
        public string View { get; set; }
        public bool Soundproofing { get; set; }
        public bool Patio { get; set; }
        public bool FlatScreenTV { get; set; }
        public bool Balcony { get; set; }
        public bool Terrace { get; set; }
        public bool PrivatePool { get; set; }
        public bool Bath { get; set; }
        public bool PlaceToWorkOnALaptop { get; set; }
        public bool FreeCancellation { get; set; }
        public bool AirConditioner { get; set; }
        public bool PrivateBathroom { get; set; }
    }
}
