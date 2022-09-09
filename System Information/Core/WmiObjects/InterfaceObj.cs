namespace System_Information.Core.WmiObjects;

public class InterfaceObj
{
    public string ExternalReferenceDesignator { get; set; }
    public string ConnectorType { get; set; }
    public string PortType { get; set; }

    public InterfaceObj(string externalReferenceDesignator, string connectorType, string portType)
    {
        ExternalReferenceDesignator = externalReferenceDesignator;
        ConnectorType = connectorType;
        PortType = portType;
    }
}