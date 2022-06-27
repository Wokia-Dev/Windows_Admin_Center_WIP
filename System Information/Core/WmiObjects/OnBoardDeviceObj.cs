namespace System_Information.Core.WmiObjects
{
    public class OnBoardDeviceObj
    {
        public string Description { get; set; }
        public string DeviceType { get; set; }

        public OnBoardDeviceObj(string description, string deviceType)
        {
            Description = description;
            DeviceType = deviceType;
        }
    }
}