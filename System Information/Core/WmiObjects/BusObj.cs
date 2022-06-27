namespace System_Information.Core.WmiObjects
{
    public class BusObj
    {
        public string DeviceId { get; set; }
        public string BusType { get; set; }
        public string? BusNumber { get; set; }

        public BusObj(string deviceId, string busType, string? busNumber)
        {
            DeviceId = deviceId;
            BusType = busType;
            BusNumber = busNumber;
        }
    }
}