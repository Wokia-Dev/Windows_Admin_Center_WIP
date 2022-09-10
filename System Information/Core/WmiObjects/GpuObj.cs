namespace System_Information.Core.WmiObjects;

public class GpuObj
{
    public string? Caption { get; }
    public string? CompanyName { get; }
    public string? Name { get; }
    public string? VideoProcessor { get; }
    public string? VideoModeDescription { get; }
    public string? MemorySize { get; }
    public string? VideoArchitecture { get; }
    public string? VideoMemoryType { get; }
    public string? CurrentResolution { get; }
    public string? BitsPerPixel { get; }
    public string? CurrentNumberOfColors { get; }
    public string? AdapterDacType { get; }
    public string? InfFileName { get; }
    public string? InfSection { get; }
    public string? MinMaxRefreshRate { get; }
    public string? PnpDeviceId { get; }
    public string? DriverDate { get; }
    public string? DriverVersion { get; }
    public string? InstalledDisplayDrivers { get; }
    public string? Status { get; }
    public string? Availability { get; }

    public GpuObj(string? caption, string? companyName, string? name, string? videoProcessor,
        string? videoModeDescription, string? memorySize, string? videoArchitecture, string? videoMemoryType,
        string? currentResolution, string? bitsPerPixel, string? currentNumberOfColors, string? adapterDacType,
        string? infFileName, string? infSection, string? minMaxRefreshRate, string? pnpDeviceId, string? driverDate,
        string? driverVersion, string? installedDisplayDrivers, string? status, string? availability)
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
        Status = status;
        Availability = availability;
    }
}
