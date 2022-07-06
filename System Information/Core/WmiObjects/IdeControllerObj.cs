namespace System_Information.Core.WmiObjects
{
    public class IdeControllerObj
    {
        public string Caption { get; set; }
        public string Manufacturer { get; set; }
        public string ProtocolSupported { get; set; }
        public string Status { get; set; }

        public IdeControllerObj(string caption, string manufacturer, string protocolSupported, string status)
        {
            Caption = caption;
            Manufacturer = manufacturer;
            ProtocolSupported = protocolSupported;
            Status = status;
        }
    }
}