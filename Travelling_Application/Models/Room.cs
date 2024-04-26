namespace Travelling_Application.Models
{
    public class Room
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string AccomodationName { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public List<Photo> Photos { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public bool Bathroom { get; set; }
        public bool WashingMachine { get; set; }
        public bool Kitchen { get; set; }
        public bool WheelchairAccessible { get; set; }
        public bool ToiletWithGrabBars { get; set; }
        public bool BathtubWithgrabbars { get; set; }
        public bool BarrierFreeShower { get; set; }
        public bool ShowerWithoutEdge { get; set; }
        public bool HighToilet { get; set; }
        public bool LowSink { get; set; }
        public bool BathroomEmergencyButton { get; set; }
        public bool ShowerChair { get; set; }
        public bool BreakfastIncluded { get; set; }
        public bool ThreeMealsADay { get; set; }
        public bool AllInclusive { get; set; }
        public bool BreakfastAndDinnerIncluded { get; set; }
        public bool CoffeeMachine { get; set; }
        public bool CoffeeOrTea { get; set; }
        public bool ElectricKettle { get; set; }
        public bool SeaView { get; set; }
        public bool CityView { get; set; }
        public bool GardenView { get; set; }
        public bool ViewFromTheWindow { get; set; }
        public bool Soundproofing { get; set; }
        public bool Patio { get; set; }
        public bool FlatScreenTV { get; set; }
        public bool Balcony { get; set; }
        public bool Terrace { get; set; }
        public bool PrivatePool { get; set; }
        public bool Bath { get; set; }
        public bool PlaceToWorkOnALaptop { get; set; }
        public bool AirConditioner { get; set; }
        public bool PrivateBathroom { get; set; }
    }
}
