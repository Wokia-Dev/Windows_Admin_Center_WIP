namespace System_Information.Core.WmiObjects;

public class GpuObj
{
    public string Caption { get; set; }
    public string CompanyName { get; set; }
    public string Name { get; set; }

    public GpuObj(string caption, string companyName, string name)
    {
        Caption = caption;
        CompanyName = companyName;
        Name = name;
    }
    
}