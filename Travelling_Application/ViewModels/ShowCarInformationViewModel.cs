﻿namespace Travelling_Application.ViewModels
{
    public class ShowCarInformationViewModel
    {
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsAirCondition { get; set; }
        public string Transmission { get; set; }
        public string Address { get; set; }
        public double Cost { get; set; }
        public byte[] CarPhoto { get; set; }
        public bool FreeCancellation { get; set; }
        public int AmountOfPassengers { get; set; }
        public bool TheftCoverage { get; set; }
        public bool CollisionDamageWaiver { get; set; }
        public bool LiabilityCoverage { get; set; }
        public bool UnlimitedMileage { get; set; }
        public string CarCategory { get; set; }
        public bool ElectricCar { get; set; }
        public string Description { get; set; }
        public List<DateTime> StartDates { get; set; }
        public List<DateTime> EndDates { get; set; }
    }
}