namespace System_Information.Core.WmiObjects;

public class UsbControllerObj
{
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public string Description { get; set; }
    public string ProtocolSupported { get; set; }
    public string Status { get; set; }

    public UsbControllerObj(string name, string manufacturer, string description, string protocolSupported, string status)
    {
        Name = name;
        Manufacturer = manufacturer;
        Description = description;
        ProtocolSupported = protocolSupported;
        Status = status;
    }
}