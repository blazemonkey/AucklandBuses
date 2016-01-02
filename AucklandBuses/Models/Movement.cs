using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AucklandBuses.Models
{
    public class MovementError
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }

    public class MovementExtension
    {
        public string Priority { get; set; }
        public string Text { get; set; }
    }

    public class Movement
    {
        public DateTime? ActualArrivalTime { get; set; }
        public DateTime? ActualDepartureTime { get; set; }
        public string ArrivalBoardingActivity { get; set; }
        public object ArrivalPlatformName { get; set; }
        public string ArrivalStatus { get; set; }
        public string DepartureBoardingActivity { get; set; }
        public object DeparturePlatformName { get; set; }
        public string DestinationDisplay { get; set; }
        public DateTime? ExpectedArrivalTime { get; set; }
        public DateTime? ExpectedDepartureTime { get; set; }
        public bool InCongestion { get; set; }
        public bool Monitored { get; set; }
        public string Route { get; set; }
        public string Stop { get; set; }
        public DateTime TimeStamp { get; set; }
        public object VehicleJourneyName { get; set; }
    }

    public class MovementResponse
    {
        public MovementError Error { get; set; }
        public List<MovementExtension> Extensions { get; set; }
        public List<Movement> Movements { get; set; }
    }
}
