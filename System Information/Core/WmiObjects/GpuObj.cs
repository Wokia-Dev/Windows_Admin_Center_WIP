namespace System_Information.Core.WmiObjects;

public class GpuObj
{
    public string? Caption { get; set; }
    public string? CompanyName { get; set; }
    public string? Name { get; set; }
    public string? VideoProcessor { get; set; }
    public string? VideoModeDescription { get; set; }
    public string? MemorySize { get; set; }
    public string? VideoArchitecture { get; set; }
    public string? VideoMemoryType { get; set; }
    public string? CurrentResolution { get; set; }
    public string? BitsPerPixel { get; set; }
    public string? CurrentNumberOfColors { get; set; }
    public string? AdapterDacType { get; set; }
    public string? InfFileName { get; set; }
    public string? InfSection { get; set; }
    public string? MinMaxRefreshRate { get; set; }
    public string? PnpDeviceId { get; set; }
    public string? DriverDate { get; set; }
    public string? DriverVersion { get; set; }
    public string? InstalledDisplayDrivers { get; set; }

    public GpuObj(string? caption, string? companyName, string? name, string? videoProcessor,
        string? videoModeDescription, string? memorySize, string? videoArchitecture, string? videoMemoryType,
        string? currentResolution, string? bitsPerPixel, string? currentNumberOfColors, string? adapterDacType,
        string? infFileName, string? infSection, string? minMaxRefreshRate, string? pnpDeviceId, string? driverDate,
        string? driverVersion, string? installedDisplayDrivers)
    {
        Caption = caption;
        CompanyName = companyName;
        Name = name;
        VideoProcessor = videoProcessor;
        VideoModeDescription = videoModeDescription;
        MemorySize = memorySize;
        VideoArchitecture = videoArchitecture;
        VideoMemoryType = videoMemoryType;
        CurrentResolution = currentResolution;
        BitsPerPixel = bitsPerPixel;
        CurrentNumberOfColors = currentNumberOfColors;
        AdapterDacType = adapterDacType;
        InfFileName = infFileName;
        InfSection = infSection;
        MinMaxRefreshRate = minMaxRefreshRate;
        PnpDeviceId = pnpDeviceId;
        DriverDate = driverDate;
        DriverVersion = driverVersion;
        InstalledDisplayDrivers = installedDisplayDrivers;
    }
}
