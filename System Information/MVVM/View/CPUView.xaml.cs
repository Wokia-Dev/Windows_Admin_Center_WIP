using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using log4net;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.Core;

namespace System_Information.MVVM.View;

/// <summary>
///     Logique d'interaction pour CPUView.xaml
/// </summary>
public partial class CpuView
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    private readonly WmIqueryManagement _wmiQueryManager = new();
    private bool _canUpdateMetrics = true;

    public CpuView()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("MaterialLight"));


        var wmiTasks = new List<Task>
        {
            // Get Basic CPU Information
            Task.Run(() =>
            {
                Log.Info("Get Basic CPU Information");
                try
                {
                    // Query return
                    var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                    {
                        "Name",
                        "Manufacturer", "Family", "Architecture", "Description", "SocketDesignation",
                        "VirtualizationFirmwareEnabled", "AddressWidth", "DataWidth", "PowerManagementSupported",
                        "Version", "ProcessorId", "Status"
                    });

                    // CPU Name
                    try
                    {
                        // Name property
                        ProcessorName = (string)returnValue.PropertiesResultList[0, 0];
                        Log.Info("Successfully get CPU Name");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Name", e);
                        ProcessorName = "N/A";
                    }

                    // CPU Manufacturer
                    try
                    {
                        // Manufacturer property
                        ProcessorManufacturer = (string)returnValue.PropertiesResultList[0, 1];
                        Log.Info("Successfully get CPU Manufacturer");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Manufacturer", e);
                        ProcessorManufacturer = "N/A";
                    }

                    // CPU Family
                    try
                    {
                        // Family property
                        var family = (ushort)returnValue.PropertiesResultList[0, 2];

                        ProcessorFamily = family switch
                        {
                            1 => "Other",
                            2 => "Unknown",
                            3 => "8086",
                            4 => "80286",
                            5 => "80386",
                            6 => "8048",
                            7 => "808",
                            8 => "027",
                            9 => "80387",
                            10 => "80487",
                            11 => "Pentium(R) brand",
                            12 => "Pentium(R) Pro",
                            13 => "Pentium(R) II",
                            14 => "Pentium(R) processor with MMX(TM) technology",
                            15 => "Celeron(TM)",
                            16 => "Pentium(R) II Xeon(TM)",
                            17 => "Pentium(R) III",
                            18 => "M1 Family",
                            19 => "M2 Family",
                            20 => "Intel(R) Celeron(R) M processor",
                            21 => "Intel(R) Pentium(R) 4 HT processor",
                            24 => "K5 Family",
                            25 => "K6 Family",
                            26 => "K6-2",
                            27 => "K6-3",
                            28 => "AMD Athlon(TM) Processor Family",
                            29 => "AMD(R) Duron(TM) Processor",
                            30 => "AMD29000 Family",
                            31 => "K6-2+",
                            32 => "Power PC Family",
                            33 => "Power PC 601",
                            34 => "Power PC 603",
                            35 => "Power PC 603+",
                            36 => "Power PC 604",
                            37 => "Power PC 620",
                            38 => "Power PC X704",
                            39 => "Power PC 750",
                            40 => "Intel(R) Core(TM) Duo processor",
                            41 => "Intel(R) Core(TM) Duo mobile processor",
                            42 => "Intel(R) Core(TM) Solo mobile processor",
                            43 => "Intel(R) Atom(TM) processor",
                            48 => "Alpha Family",
                            49 => "Alpha 21064",
                            50 => "Alpha 21066",
                            51 => "Alpha 21164",
                            52 => "Alpha 21164PC",
                            53 => "Alpha 21164a",
                            54 => "Alpha 21264",
                            55 => "Alpha 21364",
                            56 => "AMD Turion(TM) II Ultra Dual-Core Mobile M Processor Family",
                            57 => "AMD Turion(TM) II Dual-Core Mobile M Processor Family",
                            58 => "AMD Athlon(TM) II Dual-Core Mobile M Processor Family",
                            59 => "AMD Opteron(TM) 6100 Series Processor",
                            63 => "Available for assignment",
                            60 => "AMD Opteron(TM) 4100 Series Processor",
                            64 => "MIPS Family",
                            65 => "MIPS R4000",
                            66 => "MIPS R4200",
                            67 => "MIPS R4400",
                            68 => "MIPS R4600",
                            69 => "MIPS R10000",
                            80 => "SPARC Family",
                            81 => "SuperSPARC",
                            82 => "microSPARC II",
                            83 => "microSPARC IIep",
                            84 => "UltraSPARC",
                            85 => "UltraSPARC II",
                            86 => "UltraSPARC IIi",
                            87 => "UltraSPARC III",
                            88 => "UltraSPARC IIIi",
                            96 => "68040",
                            97 => "68xxx Family",
                            98 => "68000",
                            99 => "68010",
                            100 => "68020",
                            101 => "68030",
                            112 => "Hobbit Family",
                            120 => "Crusoe(TM) TM5000 Family",
                            121 => "Crusoe(TM) TM3000 Family",
                            122 => "Efficeon(TM) TM8000 Family",
                            128 => "Weitek",
                            130 => "Itanium(TM) Processor",
                            131 => "AMD Athlon(TM) 64 Processor Family",
                            132 => "AMD Opteron(TM) Processor Family",
                            133 => "AMD Sempron(TM) Processor Family",
                            134 => "AMD Turion(TM) 64 Mobile Technology",
                            135 => "Dual-Core AMD Opteron(TM) Processor Family",
                            136 => "AMD Athlon(TM) 64 X2 Dual-Core Processor Family",
                            137 => "AMD Turion(TM) 64 X2 Mobile Technology",
                            138 => "Quad-Core AMD Opteron(TM) Processor Family",
                            139 => "Third-Generation AMD Opteron(TM) Processor Family",
                            140 => "AMD Phenom(TM) FX Quad-Core Processor Family",
                            141 => "AMD Phenom(TM) X4 Quad-Core Processor Family",
                            142 => "AMD Phenom(TM) X2 Dual-Core Processor Family",
                            143 => "AMD Athlon(TM) X2 Dual-Core Processor Family",
                            144 => "PA-RISC Family",
                            145 => "PA-RISC 8500",
                            146 => "PA-RISC 8000",
                            147 => "PA-RISC 7300LC",
                            148 => "PA-RISC 7200",
                            149 => "PA-RISC 7100LC",
                            150 => "PA-RISC 7100",
                            160 => "V30 Family",
                            161 => "Quad-Core Intel(R) Xeon(R) processor 3200 Series",
                            162 => "Dual-Core Intel(R) Xeon(R) processor 3000 Series",
                            163 => "Quad-Core Intel(R) Xeon(R) processor 5300 Series",
                            164 => "Dual-Core Intel(R) Xeon(R) processor 5100 Series",
                            165 => "Dual-Core Intel(R) Xeon(R) processor 5000 Series",
                            166 => "Dual-Core Intel(R) Xeon(R) processor LV",
                            167 => "Dual-Core Intel(R) Xeon(R) processor ULV",
                            168 => "Dual-Core Intel(R) Xeon(R) processor 7100 Series",
                            169 => "Quad-Core Intel(R) Xeon(R) processor 5400 Series",
                            170 => "Quad-Core Intel(R) Xeon(R) processor",
                            171 => "Dual-Core Intel(R) Xeon(R) processor 5200 Series",
                            172 => "Dual-Core Intel(R) Xeon(R) processor 7200 Series",
                            173 => "Quad-Core Intel(R) Xeon(R) processor 7300 Series",
                            174 => "Quad-Core Intel(R) Xeon(R) processor 7400 Series",
                            175 => "Multi-Core Intel(R) Xeon(R) processor 7400 Series",
                            176 => "Pentium(R) III Xeon(TM)",
                            177 => "Pentium(R) III Processor with Intel(R) SpeedStep(TM) Technology",
                            178 => "Pentium(R) 4",
                            179 => "Intel(R) Xeon(TM)",
                            180 => "AS400 Family",
                            181 => "Intel(R) Xeon(TM) processor MP",
                            182 => "AMD Athlon(TM) XP Family",
                            183 => "AMD Athlon(TM) MP Family",
                            184 => "Intel(R) Itanium(R) 2",
                            185 => "Intel(R) Pentium(R) M processor",
                            186 => "Intel(R) Celeron(R) D processor",
                            187 => "Intel(R) Pentium(R) D processor",
                            188 => "Intel(R) Pentium(R) Processor Extreme Edition",
                            189 => "Intel(R) Core(TM) Solo Processor",
                            190 => "K7",
                            191 => "Intel(R) Core(TM)2 Duo Processor",
                            192 => "Intel(R) Core(TM)2 Solo processor",
                            193 => "Intel(R) Core(TM)2 Extreme processor",
                            194 => "Intel(R) Core(TM)2 Quad processor",
                            195 => "Intel(R) Core(TM)2 Extreme mobile processor",
                            196 => "Intel(R) Core(TM)2 Duo mobile processor",
                            197 => "Intel(R) Core(TM)2 Solo mobile processor",
                            198 => "Intel(R) Core(TM) i7 processor",
                            199 => "Dual-Core Intel(R) Celeron(R) Processor",
                            200 => "S/390 and zSeries Family",
                            201 => "ESA/390 G4",
                            202 => "ESA/390 G5",
                            203 => "ESA/390 G6",
                            204 => "z/Architectur base",
                            205 => "Intel(R) Core(TM) i5 processor",
                            206 => "Intel(R) Core(TM) i3 processor",
                            207 => "Intel(R) Core(TM) i9 processor",
                            210 => "VIA C7(TM)-M Processor Family",
                            211 => "VIA C7(TM)-D Processor Family",
                            212 => "VIA C7(TM) Processor Family",
                            213 => "VIA Eden(TM) Processor Family",
                            214 => "Multi-Core Intel(R) Xeon(R) processor",
                            215 => "Dual-Core Intel(R) Xeon(R) processor 3xxx Series",
                            216 => "Quad-Core Intel(R) Xeon(R) processor 3xxx Series",
                            217 => "VIA Nano(TM) Processor Family",
                            218 => "Dual-Core Intel(R) Xeon(R) processor 5xxx Series",
                            219 => "Quad-Core Intel(R) Xeon(R) processor 5xxx Series",
                            221 => "Dual-Core Intel(R) Xeon(R) processor 7xxx Series",
                            222 => "Quad-Core Intel(R) Xeon(R) processor 7xxx Series",
                            223 => "Multi-Core Intel(R) Xeon(R) processor 7xxx Series",
                            224 => "Multi-Core Intel(R) Xeon(R) processor 3400 Series",
                            230 => "Embedded AMD Opteron(TM) Quad-Core Processor Family",
                            231 => "AMD Phenom(TM) Triple-Core Processor Family",
                            232 => "AMD Turion(TM) Ultra Dual-Core Mobile Processor Family",
                            233 => "AMD Turion(TM) Dual-Core Mobile Processor Family",
                            234 => "AMD Athlon(TM) Dual-Core Processor Family",
                            235 => "AMD Sempron(TM) SI Processor Family",
                            236 => "AMD Phenom(TM) II Processor Family",
                            237 => "AMD Athlon(TM) II Processor Family",
                            238 => "Six-Core AMD Opteron(TM) Processor Family",
                            239 => "AMD Sempron(TM) M Processor Family",
                            250 => "i860",
                            251 => "i960",
                            254 => "Reserved (SMBIOS Extension)",
                            255 => "Reserved (Un-initialized Flash Content - Lo)",
                            260 => "SH-3",
                            261 => "SH-4",
                            280 => "ARM",
                            281 => "StrongARM",
                            300 => "6x86",
                            301 => "MediaGX",
                            302 => "MII",
                            320 => "WinChip",
                            350 => "DSP",
                            500 => "Video Processor",
                            65534 => "Reserved (For Future Special Purpose Assignment)",
                            65535 => "Reserved (Un-initialized Flash Content - Hi",
                            _ => "N/A"
                        };

                        Log.Info("Successfully get CPU Family");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Family", e);
                        ProcessorFamily = "N/A";
                    }

                    // CPU Architecture
                    try
                    {
                        // Architecture
                        var architecture = (ushort)returnValue.PropertiesResultList[0, 3];

                        ProcessorArchitecture = architecture switch
                        {
                            0 => "x86",
                            1 => "MIPS",
                            2 => "Alpha",
                            3 => "PowerPC",
                            5 => "ARM",
                            6 => "ia4",
                            9 => "x64",
                            12 => "ARM6",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Architecture");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Architecture", e);
                        ProcessorArchitecture = "N/A";
                    }

                    // CPU Description
                    try
                    {
                        // Description
                        ProcessorDescription = (string)returnValue.PropertiesResultList[0, 4];
                        Log.Info("Successfully get CPU Description");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Description", e);
                        ProcessorDescription = "N/A";
                    }

                    // CPU Socket Designation
                    try
                    {
                        // Socket Designation
                        SocketDesignation = (string)returnValue.PropertiesResultList[0, 5];
                        Log.Info("Successfully get CPU Socket Designation");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Socket Designation", e);
                        SocketDesignation = "N/A";
                    }

                    // Get Virtualization
                    try
                    {
                        // Virtualization Firmware
                        var virtualizationFirmware = (bool)returnValue.PropertiesResultList[0, 6];

                        Virtualization = virtualizationFirmware switch
                        {
                            true => "Yes",
                            false => "No"
                        };
                        Log.Info("Successfully get Virtualization");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Virtualization", e);
                        Virtualization = "N/A";
                    }

                    // CPU Address Width
                    try
                    {
                        // Address Width
                        var addressWidth = (ushort)returnValue.PropertiesResultList[0, 7];

                        AddressWidth = addressWidth switch
                        {
                            32 => "32-bit",
                            64 => "64-bit",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Address Width");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Address Width", e);
                        AddressWidth = "N/A";
                    }

                    // CPU Data Width
                    try
                    {
                        // Data Width
                        var dataWidth = (ushort)returnValue.PropertiesResultList[0, 8];

                        DataWidth = dataWidth switch
                        {
                            32 => "32-bit",
                            64 => "64-bit",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Data Width");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Data Width", e);
                        DataWidth = "N/A";
                    }

                    // Power Management
                    try
                    {
                        // Power Management Supported
                        var powerManagementSupported = (bool)returnValue.PropertiesResultList[0, 9];

                        PowerManagementSupported = powerManagementSupported switch
                        {
                            true => "True",
                            false => "False"
                        };
                        Log.Info("Successfully get Power Management Supported");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Power Management Supported", e);
                        PowerManagementSupported = "N/A";
                    }

                    // CPU Version
                    try
                    {
                        // Version
                        Version = (string)returnValue.PropertiesResultList[0, 10];
                        Log.Info("Successfully get CPU Version");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Version", e);
                        Version = "N/A";
                    }

                    // CPU ID
                    try
                    {
                        // ProcessorId
                        ProcessorId = (string)returnValue.PropertiesResultList[0, 11];
                        Log.Info("Successfully get CPU ID");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU ID", e);
                        ProcessorId = "N/A";
                    }

                    // CPU Status
                    try
                    {
                        // Status
                        Status = (string)returnValue.PropertiesResultList[0, 12];
                        Log.Info("Successfully get CPU Status");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Status", e);
                        Status = "N/A";
                    }

                    Log.Info("Successful get CPU Characteristics with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get CPU Characteristics with WMI", e);
                    ProcessorName = "N/A";
                    ProcessorManufacturer = "N/A";
                    ProcessorFamily = "N/A";
                    ProcessorArchitecture = "N/A";
                    ProcessorDescription = "N/A";
                    SocketDesignation = "N/A";
                    Virtualization = "N/A";
                    AddressWidth = "N/A";
                    DataWidth = "N/A";
                    PowerManagementSupported = "N/A";
                    Version = "N/A";
                    ProcessorId = "N/A";
                    Status = "N/A";
                }
            }),

            // Get CPU Characteristics
            Task.Run(() =>
            {
                Log.Info("Get CPU Characteristics");
                try
                {
                    // Query return
                    var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[] { "Characteristics" });

                    // Get uint value from return
                    var uintValue = (uint)returnValue.PropertiesResultList[0, 0];

                    // Get bits from uint value and convert to string
                    var bitsString = "";
                    var bits = new uint[32];
                    for (var i = 0; i < 32; i++) bits[i] = (uintValue >> i) & 1;
                    for (var i = 0; i < 32; i++) bitsString += bits[i];

                    // Get bits value and set the CPU Characteristics

                    // 64-bit Capable
                    try
                    {
                        // 64-bit Capable
                        X64BitCapable = bitsString[2] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU X64 Bit Capable");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU X64 Bit Capable", e);
                        X64BitCapable = "N/A";
                    }

                    // Multi-Core
                    try
                    {
                        // Multi-Core Processor
                        MultiCore = bitsString[3] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Multi-Core");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Multi-Core", e);
                        MultiCore = "N/A";
                    }

                    // Hardware Thread
                    try
                    {
                        // Hardware Thread
                        HardwareThread = bitsString[4] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Hardware Thread");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Hardware Thread", e);
                        HardwareThread = "N/A";
                    }

                    // Execute Protection
                    try
                    {
                        // Execute Protection
                        ExecuteProtection = bitsString[5] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Execute Protection");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Execute Protection", e);
                        ExecuteProtection = "N/A";
                    }

                    // Enhanced Virtualization
                    try
                    {
                        // Enhanced Virtualization
                        EnhancedVirtualization = bitsString[6] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Enhanced Virtualization");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Enhanced Virtualization", e);
                        EnhancedVirtualization = "N/A";
                    }

                    // Power/Performance Control
                    try
                    {
                        // Power/Performance Control
                        PowerPerformanceControl = bitsString[7] switch
                        {
                            '0' => "False",
                            '1' => "True",
                            _ => "N/A"
                        };
                        Log.Info("Successfully get CPU Power/Performance Control");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Power/Performance Control", e);
                        PowerPerformanceControl = "N/A";
                    }

                    Log.Info("Successfully get CPU Characteristics");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get CPU Characteristics", e);
                    X64BitCapable = "N/A";
                    MultiCore = "N/A";
                    HardwareThread = "N/A";
                    ExecuteProtection = "N/A";
                    EnhancedVirtualization = "N/A";
                    PowerPerformanceControl = "N/A";
                }
            }),

            // Get CPU Metrics
            Task.Run(() =>
            {
                Log.Info("Get CPU Metrics");
                try
                {
                    // Query return
                    var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                    {
                        "CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores",
                        "NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
                        "L2CacheSize", "L3CacheSize"
                    });

                    // Current Clock Speed
                    try
                    {
                        // CurrentClockSpeed
                        var currentClockSpeed = (uint)returnValue.PropertiesResultList[0, 0];
                        CurrentClockSpeed = currentClockSpeed + " MHz";
                        Log.Info("Successfully get CPU Current Clock Speed");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Current Clock Speed", e);
                        CurrentClockSpeed = "N/A";
                    }

                    // Max Clock Speed
                    try
                    {
                        // MaxClockSpeed
                        var maxClockSpeed = (uint)returnValue.PropertiesResultList[0, 1];
                        MaxClockSpeed = maxClockSpeed + " MHz";
                        Log.Info("Successfully get CPU Max Clock Speed");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Max Clock Speed", e);
                        MaxClockSpeed = "N/A";
                    }

                    // Ext Clock
                    try
                    {
                        // ExtClock
                        var extClock = (uint)returnValue.PropertiesResultList[0, 2];
                        ExternalClockSpeed = extClock + " MHz";
                        Log.Info("Successfully get CPU External Clock Speed");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Ext Clock", e);
                        ExternalClockSpeed = "N/A";
                    }

                    // Current Voltage
                    try
                    {
                        // CurrentVoltage
                        var currentVoltage = (ushort)returnValue.PropertiesResultList[0, 3];
                        CurrentVoltage = currentVoltage / 10.0 + " Volts";
                        Log.Info("Successfully get CPU Current Voltage");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Current Voltage", e);
                        CurrentVoltage = "N/A";
                    }

                    // Number of Cores and Enabled Cores
                    try
                    {
                        // NumberOfCores and NumberOfEnabledCore
                        var numberOfCores = (uint)returnValue.PropertiesResultList[0, 4];
                        var numberOfEnabledCores = (uint)returnValue.PropertiesResultList[0, 5];
                        NumberOfCores = numberOfCores + " (" + numberOfEnabledCores + " enabled)";
                        Log.Info("Successfully get CPU Number of Cores and Enabled Cores");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Number of Cores and Enabled Cores", e);
                        NumberOfCores = "N/A";
                    }

                    // Number of Logical Processors
                    try
                    {
                        // NumberOfLogicalProcessors
                        var numberOfLogicalProcessors = (uint)returnValue.PropertiesResultList[0, 6];
                        NumberOfLogicalProcessors = numberOfLogicalProcessors.ToString();
                        Log.Info("Successfully get CPU Number of Logical Processors");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Number of Logical Processors", e);
                        NumberOfLogicalProcessors = "N/A";
                    }

                    // Thread Count
                    try
                    {
                        // ThreadCount
                        var threadCount = (uint)returnValue.PropertiesResultList[0, 7];
                        NumberOfThreads = threadCount.ToString();
                        Log.Info("Successfully get CPU Thread Count");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Thread Count", e);
                        NumberOfThreads = "N/A";
                    }

                    // Load Percentage
                    try
                    {
                        // LoadPercentage
                        var loadPercentage = (ushort)returnValue.PropertiesResultList[0, 8];
                        LoadPercentage = loadPercentage + " %";
                        Log.Info("Successfully get CPU Load Percentage");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Load Percentage", e);
                        LoadPercentage = "N/A";
                    }

                    // L2 Cache Size
                    try
                    {
                        // L2CacheSize KB and MB
                        var l2CacheSize = (uint)returnValue.PropertiesResultList[0, 9];
                        L2CacheSize = l2CacheSize + " KB" + " (" + l2CacheSize / 1024 + "MB)";
                        Log.Info("Successfully get CPU L2 Cache Size");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU L2 Cache Size", e);
                        L2CacheSize = "N/A";
                    }

                    // L3 Cache Size
                    try
                    {
                        // L3CacheSize
                        var l3CacheSize = (uint)returnValue.PropertiesResultList[0, 10];
                        L3CacheSize = l3CacheSize + " KB" + " (" + l3CacheSize / 1024 + "MB)";
                        Log.Info("Successfully get CPU L3 Cache Size");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU L3 Cache Size", e);
                        L3CacheSize = "N/A";
                    }

                    Log.Info("Successfully get CPU Metrics");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get CPU Metrics", e);
                    CurrentClockSpeed = "N/A";
                    MaxClockSpeed = "N/A";
                    ExternalClockSpeed = "N/A";
                    CurrentVoltage = "N/A";
                    NumberOfCores = "N/A";
                    NumberOfLogicalProcessors = "N/A";
                    NumberOfThreads = "N/A";
                    LoadPercentage = "N/A";
                    L2CacheSize = "N/A";
                    L3CacheSize = "N/A";
                }
            })
        };

        Task.WaitAll(wmiTasks.ToArray());


        // Add Manufacturer to main node
        var manufacturerNode = new TreeViewNode();
        manufacturerNode.Content = "Manufacturer : " + ProcessorManufacturer;

        // Add Family to main node
        var familyNode = new TreeViewNode();
        familyNode.Content = "Family : " + ProcessorFamily;

        // Add Architecture to main node
        var architectureNode = new TreeViewNode();
        architectureNode.Content = "Architecture : " + ProcessorArchitecture;

        // Add Description to main node
        var descriptionNode = new TreeViewNode();
        descriptionNode.Content = "Description : " + ProcessorDescription;

        // Add Socket Designation to main node
        var socketDesignationNode = new TreeViewNode();
        socketDesignationNode.Content = "Socket Designation : " + SocketDesignation;

        // Add Virtualization to main node
        var virtualizationNode = new TreeViewNode();
        virtualizationNode.Content = "Virtualization : " + Virtualization;

        // Add AddressWidth to main node
        var addressWidthNode = new TreeViewNode();
        addressWidthNode.Content = "Address Width : " + AddressWidth;

        // Add DataWidth to main node
        var dataWidthNode = new TreeViewNode();
        dataWidthNode.Content = "Data Width : " + DataWidth;

        // Add PowerManagement to main node
        var powerManagementNode = new TreeViewNode();
        powerManagementNode.Content = "Power Management Supported : " + PowerManagementSupported;

        // Add Version to main node
        var versionNode = new TreeViewNode();
        versionNode.Content = "Version : " + Version;

        // Add ProcessorId to main node
        var processorIdNode = new TreeViewNode();
        processorIdNode.Content = "Processor Id : " + ProcessorId;

        // Add Status to main node
        var statusNode = new TreeViewNode();
        statusNode.Content = "Status : " + Status;

        // Add 64-bit Capable to characteristics node
        var bit64Node = new TreeViewNode();
        bit64Node.Content = "64-bit Capable : " + X64BitCapable;

        // Add Multi Core to characteristics node
        var multiCoreNode = new TreeViewNode();
        multiCoreNode.Content = "Multi Core : " + MultiCore;

        // Add HardwareThread to characteristics node
        var hardwareThreadNode = new TreeViewNode();
        hardwareThreadNode.Content = "Hardware Thread : " + HardwareThread;

        // Add ExecuteProtection to characteristics node
        var executeProtectionNode = new TreeViewNode();
        executeProtectionNode.Content = "Execute Protection : " + ExecuteProtection;

        // Add Enhanced Virtualization to characteristics node
        var enhancedVirtualizationNode = new TreeViewNode();
        enhancedVirtualizationNode.Content = "Enhanced Virtualization : " + EnhancedVirtualization;

        // Add Power/Performance Control to characteristics node
        var powerPerformanceControlNode = new TreeViewNode();
        powerPerformanceControlNode.Content = "Power/Performance Control : " + PowerPerformanceControl;

        // Add Characteristics to main node
        var characteristicsNode = new TreeViewNode();
        characteristicsNode.Content = "Characteristics";
        characteristicsNode.IsExpanded = true;
        characteristicsNode.ChildNodes.Add(bit64Node);
        characteristicsNode.ChildNodes.Add(multiCoreNode);
        characteristicsNode.ChildNodes.Add(hardwareThreadNode);
        characteristicsNode.ChildNodes.Add(executeProtectionNode);
        characteristicsNode.ChildNodes.Add(enhancedVirtualizationNode);
        characteristicsNode.ChildNodes.Add(powerPerformanceControlNode);

        // Add CurrentClockSpeed to Metrics node
        CurrentClockSpeedNode = new TreeViewNode();
        CurrentClockSpeedNode.Content = "Current Clock Speed : " + CurrentClockSpeed;

        // Add MaxClockSpeed to Metrics node
        MaxClockSpeedNode = new TreeViewNode();
        MaxClockSpeedNode.Content = "Max Clock Speed : " + MaxClockSpeed;

        // Add ExternalClockSpeed to Metrics node
        ExternalClockSpeedNode = new TreeViewNode();
        ExternalClockSpeedNode.Content = "External Clock Speed : " + ExternalClockSpeed;

        // Add CurrentVoltage to Metrics node
        CurrentVoltageNode = new TreeViewNode();
        CurrentVoltageNode.Content = "Current Voltage : " + CurrentVoltage;

        // Add NumberOfCores to Metrics node
        NumberOfCoresNode = new TreeViewNode();
        NumberOfCoresNode.Content = "Number of Cores : " + NumberOfCores;

        // Add NumberOfLogicalProcessors to Metrics node
        NumberOfLogicalProcessorsNode = new TreeViewNode();
        NumberOfLogicalProcessorsNode.Content = "Number of Logical Processors : " + NumberOfLogicalProcessors;

        // Add NumberOfThreads to Metrics node
        NumberOfThreadsNode = new TreeViewNode();
        NumberOfThreadsNode.Content = "Number of Threads : " + NumberOfThreads;

        // Add LoadPercentage to Metrics node
        LoadPercentageNode = new TreeViewNode();
        LoadPercentageNode.Content = "Load Percentage : " + LoadPercentage;

        // Add L2CacheSize to Metrics node
        L2CacheSizeNode = new TreeViewNode();
        L2CacheSizeNode.Content = "L2 Cache Size : " + L2CacheSize;

        // Add L3CacheSize to Metrics node
        L3CacheSizeNode = new TreeViewNode();
        L3CacheSizeNode.Content = "L3 Cache Size : " + L3CacheSize;


        // Add Metrics to main node
        var metricsNode = new TreeViewNode();
        metricsNode.Content = "Metrics";
        metricsNode.IsExpanded = true;
        metricsNode.ChildNodes.Add(CurrentClockSpeedNode);
        metricsNode.ChildNodes.Add(MaxClockSpeedNode);
        metricsNode.ChildNodes.Add(ExternalClockSpeedNode);
        metricsNode.ChildNodes.Add(CurrentVoltageNode);
        metricsNode.ChildNodes.Add(NumberOfCoresNode);
        metricsNode.ChildNodes.Add(NumberOfLogicalProcessorsNode);
        metricsNode.ChildNodes.Add(NumberOfThreadsNode);
        metricsNode.ChildNodes.Add(LoadPercentageNode);
        metricsNode.ChildNodes.Add(L2CacheSizeNode);
        metricsNode.ChildNodes.Add(L3CacheSizeNode);

        // Main node for CPU
        var mainNode = new TreeViewNode();
        mainNode.Content = ProcessorName;
        mainNode.IsExpanded = true;
        mainNode.ChildNodes.Add(manufacturerNode);
        mainNode.ChildNodes.Add(familyNode);
        mainNode.ChildNodes.Add(architectureNode);
        mainNode.ChildNodes.Add(descriptionNode);
        mainNode.ChildNodes.Add(socketDesignationNode);
        mainNode.ChildNodes.Add(virtualizationNode);
        mainNode.ChildNodes.Add(addressWidthNode);
        mainNode.ChildNodes.Add(dataWidthNode);
        mainNode.ChildNodes.Add(powerManagementNode);
        mainNode.ChildNodes.Add(versionNode);
        mainNode.ChildNodes.Add(processorIdNode);
        mainNode.ChildNodes.Add(statusNode);
        mainNode.ChildNodes.Add(characteristicsNode);
        mainNode.ChildNodes.Add(metricsNode);

        // Main treeView Nodes Collection
        var treeViewNodes = new TreeViewNodeCollection();
        treeViewNodes.Add(mainNode);

        // Main treeView
        MainTreeView.Nodes = treeViewNodes;

        // Update Values each second
        var dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Tick += dispatcherTimer_Tick;
        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
        dispatcherTimer.Start();
    }


    private void dispatcherTimer_Tick(object? sender, EventArgs e)
    {
        if (_canUpdateMetrics)
        {
            _canUpdateMetrics = false;
            Log.Info("Updating CPU Metrics (" + DateTime.Now.ToString("HH:mm:ss") + ")...");

            string currentClockSpeedString;
            string maxClockSpeedString;
            string externalClockSpeedString;
            string currentVoltageString;
            string numberOfCoresString;
            string numberOfLogicalProcessorsString;
            string numberOfThreadsString;
            string loadPercentageString;
            string l2CacheSizeString;
            string l3CacheSizeString;

            Task.Run(() =>
            {
                Log.Info("Get CPU Metrics For Update");

                try
                {
                    // Query Return
                    var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                    {
                        "CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores",
                        "NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
                        "L2CacheSize", "L3CacheSize"
                    });

                    // CurrentClockSpeed
                    try
                    {
                        // CurrentClockSpeed
                        var currentClockSpeed = (uint)returnValue.PropertiesResultList[0, 0];
                        currentClockSpeedString = currentClockSpeed + " MHz";
                        Log.Info("Successfully get CurrentClockSpeed for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get CurrentClockSpeed for Update", exception);
                        currentClockSpeedString = "N/A";
                    }

                    // MaxClockSpeed
                    try
                    {
                        // MaxClockSpeed
                        var maxClockSpeed = (uint)returnValue.PropertiesResultList[0, 1];
                        maxClockSpeedString = maxClockSpeed + " MHz";
                        Log.Info("Successfully get MaxClockSpeed for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get MaxClockSpeed for Update", exception);
                        maxClockSpeedString = "N/A";
                    }

                    // ExternalClockSpeed
                    try
                    {
                        // ExtClock
                        var extClock = (uint)returnValue.PropertiesResultList[0, 2];
                        externalClockSpeedString = extClock + " MHz";
                        Log.Info("Successfully get ExtClock for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get ExternalClockSpeed for Update", exception);
                        externalClockSpeedString = "N/A";
                    }

                    // CurrentVoltage
                    try
                    {
                        // CurrentVoltage
                        var currentVoltage = (ushort)returnValue.PropertiesResultList[0, 3];
                        currentVoltageString = currentVoltage / 10.0 + " Volts";
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get CurrentVoltage for Update", exception);
                        currentVoltageString = "N/A";
                    }

                    // NumberOfCores and EnabledCore
                    try
                    {
                        // NumberOfCores and NumberOfEnabledCore
                        var numberOfCores = (uint)returnValue.PropertiesResultList[0, 4];
                        var numberOfEnabledCores = (uint)returnValue.PropertiesResultList[0, 5];
                        numberOfCoresString = numberOfCores + " (" + numberOfEnabledCores + " enabled)";
                        Log.Info("Successfully get NumberOfCores and NumberOfEnabledCores for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get NumberOfCores and EnabledCore for Update", exception);
                        numberOfCoresString = "N/A";
                    }

                    // NumberOfLogicalProcessors
                    try
                    {
                        // NumberOfLogicalProcessors
                        var numberOfLogicalProcessors = (uint)returnValue.PropertiesResultList[0, 6];
                        numberOfLogicalProcessorsString = numberOfLogicalProcessors.ToString();
                        Log.Info("Successfully get NumberOfLogicalProcessors for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get NumberOfLogicalProcessors for Update", exception);
                        numberOfLogicalProcessorsString = "N/A";
                    }

                    // ThreadCount
                    try
                    {
                        // ThreadCount
                        var threadCount = (uint)returnValue.PropertiesResultList[0, 7];
                        numberOfThreadsString = threadCount.ToString();
                        Log.Info("Successfully get ThreadCount for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get ThreadCount for Update", exception);
                        numberOfThreadsString = "N/A";
                    }

                    // LoadPercentage
                    try
                    {
                        // LoadPercentage
                        var loadPercentage = (ushort)returnValue.PropertiesResultList[0, 8];
                        loadPercentageString = loadPercentage + " %";
                        Log.Info("Successfully get LoadPercentage for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get LoadPercentage for Update", exception);
                        loadPercentageString = "N/A";
                    }

                    // L2CacheSize
                    try
                    {
                        // L2CacheSize KB and MB
                        var l2CacheSize = (uint)returnValue.PropertiesResultList[0, 9];
                        l2CacheSizeString = l2CacheSize + " KB" + " (" + l2CacheSize / 1024 + "MB)";
                        Log.Info("Successfully get L2CacheSize for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get L2CacheSize for Update", exception);
                        l2CacheSizeString = "N/A";
                    }

                    // L3CacheSize
                    try
                    {
                        // L3CacheSize
                        var l3CacheSize = (uint)returnValue.PropertiesResultList[0, 10];
                        l3CacheSizeString = l3CacheSize + " KB" + " (" + l3CacheSize / 1024 + "MB)";
                        Log.Info("Successfully get L3CacheSize for Update");
                    }
                    catch (Exception exception)
                    {
                        Log.Error("Failed to get L3CacheSize for Update", exception);
                        l3CacheSizeString = "N/A";
                    }

                    Log.Info("Successfully got CPU Metrics For Update");
                }
                catch (Exception exception)
                {
                    Log.Error("Failed to get CPU Metrics For Update", exception);
                    currentClockSpeedString = "N/A";
                    maxClockSpeedString = "N/A";
                    externalClockSpeedString = "N/A";
                    currentVoltageString = "N/A";
                    numberOfCoresString = "N/A";
                    numberOfLogicalProcessorsString = "N/A";
                    numberOfThreadsString = "N/A";
                    loadPercentageString = "N/A";
                    l2CacheSizeString = "N/A";
                    l3CacheSizeString = "N/A";
                }

                Dispatcher.Invoke(() =>
                {
                    // Add CurrentClockSpeed to Metrics node
                    CurrentClockSpeedNode.Content = "Current Clock Speed : " + currentClockSpeedString;

                    // Add MaxClockSpeed to Metrics node
                    MaxClockSpeedNode.Content = "Max Clock Speed : " + maxClockSpeedString;

                    // Add ExternalClockSpeed to Metrics node
                    ExternalClockSpeedNode.Content = "External Clock Speed : " + externalClockSpeedString;

                    // Add CurrentVoltage to Metrics node
                    CurrentVoltageNode.Content = "Current Voltage : " + currentVoltageString;

                    // Add NumberOfCores to Metrics node
                    NumberOfCoresNode.Content = "Number of Cores : " + numberOfCoresString;

                    // Add NumberOfLogicalProcessors to Metrics node
                    NumberOfLogicalProcessorsNode.Content =
                        "Number of Logical Processors : " + numberOfLogicalProcessorsString;

                    // Add NumberOfThreads to Metrics node
                    NumberOfThreadsNode.Content = "Number of Threads : " + numberOfThreadsString;

                    // Add LoadPercentage to Metrics node
                    LoadPercentageNode.Content = "Load Percentage : " + loadPercentageString;

                    // Add L2CacheSize to Metrics node
                    L2CacheSizeNode.Content = "L2 Cache Size : " + l2CacheSizeString;

                    // Add L3CacheSize to Metrics node
                    L3CacheSizeNode.Content = "L3 Cache Size : " + l3CacheSizeString;

                    Log.Info("CPU Metrics Updated (" + DateTime.Now.ToString("HH:mm:ss") + ")");
                });
                _canUpdateMetrics = true;
            });
        }
        else
        {
            Log.Info("Update Skipped (" + DateTime.Now.ToString("HH:mm:ss") + ")");
        }
    }


    private void ExpandAll_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Get Current Value of text block
        var senderBorder = (Border)sender;
        var textBlock = (TextBlock)senderBorder.Child;
        var currentValue = textBlock.Text;

        switch (currentValue)
        {
            case "Collapse All":
                textBlock.Text = "Expand All";
                MainTreeView.CollapseAll();
                break;
            case "Expand All":
                textBlock.Text = "Collapse All";
                MainTreeView.ExpandAll();
                break;
        }
    }

    private void ExportData_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Try to export data to html file
        Log.Info("try to export data");
        Task.Run(() =>
        {
            try
            {
                // html template
                var htmlString = "<!DOCTYPE html><head> <title>Data Export - Windows " +
                                 "Admin Center</title></head><body style=\"font-family: monospace," +
                                 " sans-serif;\"> <h1 style=\"text-align: center; margin: 50px 0;\">" +
                                 " Windows Admin Center - System Information (CPU) </h1> <ul> <li> <h2>" +
                                 $"{ProcessorName}</h2> <ul> <li> <h3>Manufacturer : {ProcessorManufacturer}" +
                                 $"</h3> </li> <li> <h3>Family : {ProcessorFamily}</h3> </li> <li> " +
                                 $"<h3>Architecture : {ProcessorArchitecture}</h3> </li> <li> <h3>Description" +
                                 $" : {ProcessorDescription}</h3> </li> <li> <h3>Socket Designation : " +
                                 $"{SocketDesignation}</h3> </li> <li> <h3>Virtualization : {Virtualization}" +
                                 $"</h3> </li> <li> <h3>Address Width : {AddressWidth}</h3> </li> <li> " +
                                 $"<h3>Data Width : {DataWidth}</h3> </li> <li> <h3>Power Management " +
                                 $"Supported : {PowerManagementSupported}</h3> </li> <li> <h3>Version : " +
                                 $"{Version}</h3> </li> <li> <h3>Processor Id : {ProcessorId}</h3> " +
                                 $"</li> <li> <h3>Status : {Status}</h3> </li> <li> " +
                                 "<h3>Characteristics</h3> <ul> <li> <h3>64 bit Capable : " +
                                 $"{X64BitCapable}</h3> </li> <li> <h3>Multi-Core : {MultiCore}</h3> </li> <li>" +
                                 $" <h3>Hardware Thread : {HardwareThread}</h3> </li> <li> <h3>Execute Protection" +
                                 $" : {ExecuteProtection}</h3> </li> <li> <h3>Enhanced Virtualization : " +
                                 $"{EnhancedVirtualization}</h3> </li> <li> <h3>Power/Performance Control : " +
                                 $"{PowerPerformanceControl}</h3> </li> </ul> </li> <li> <h3>Metrics</h3> <ul> " +
                                 $"<li> <h3>Current Clock Speed : {CurrentClockSpeed}</h3> </li> <li> <h3>Max " +
                                 $"Clock Speed : {MaxClockSpeed}</h3> </li> <li> <h3>External Clock : " +
                                 $"{ExternalClockSpeed}</h3> </li> <li> <h3>Current Voltage : {CurrentVoltage}" +
                                 $"</h3> </li> <li> <h3>Number Of Cores : {NumberOfCores}</h3> </li> <li>" +
                                 $" <h3>Number Of Logical Processors : {NumberOfLogicalProcessors}</h3> </li> " +
                                 $"<li> <h3>Number Of Thread : {NumberOfThreads}</h3> </li> <li> <h3>" +
                                 $"Load Percentage : {LoadPercentage}</h3> </li> <li> <h3>L2 Cache Size : " +
                                 $"{L3CacheSize}</h3> </li> <li> <h3>L3 Cache Size : {L3CacheSize}</h3> " +
                                 "</li> </ul> </li> </ul> </li> </ul></body></html>";

                // Create a file and ask user to save it
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML File (*.html)|*.html",
                    Title = "Save HTML File",
                    FileName = "System Information(CPU)_" + DateTime.Now.ToString("yyyy-MM-dd") + ".html"
                };
                if (saveFileDialog.ShowDialog())
                {
                    File.WriteAllText(saveFileDialog.FileName, htmlString);
                    var process = new Process();
                    process.StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                    {
                        UseShellExecute = true
                    };
                    process.Start();
                    Log.Info("html data file exported : " + saveFileDialog.FileName);
                }

                Log.Info("data exported");
            }
            // Catch error
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error with export data to html", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Log.Error("error with export data to html : " + exception.Message);
            }
        }).Wait();
    }

    private void MainTreeView_OnKeyDown(object sender, KeyEventArgs e)
    {
        // Copy selected element value to clipboard
        if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            var selectedItem = (TreeViewNode)MainTreeView.SelectedItem;
            Clipboard.SetText(selectedItem.Content.ToString() ?? string.Empty);
            Log.Info("Copied to clipboard : " + selectedItem.Content);
        }
    }

    #region Values

    private string ProcessorName { get; set; } = "";
    private string ProcessorManufacturer { get; set; } = "";
    private string ProcessorFamily { get; set; } = "";
    private string ProcessorArchitecture { get; set; } = "";
    private string ProcessorDescription { get; set; } = "";
    private string SocketDesignation { get; set; } = "";
    private string Virtualization { get; set; } = "";
    private string AddressWidth { get; set; } = "";
    private string DataWidth { get; set; } = "";
    private string PowerManagementSupported { get; set; } = "";
    private string Version { get; set; } = "";
    private string ProcessorId { get; set; } = "";
    private string Status { get; set; } = "";
    private string X64BitCapable { get; set; } = "";
    private string MultiCore { get; set; } = "";
    private string HardwareThread { get; set; } = "";
    private string ExecuteProtection { get; set; } = "";
    private string EnhancedVirtualization { get; set; } = "";
    private string PowerPerformanceControl { get; set; } = "";
    private string CurrentClockSpeed { get; set; } = "";
    private string MaxClockSpeed { get; set; } = "";
    private string ExternalClockSpeed { get; set; } = "";
    private string CurrentVoltage { get; set; } = "";
    private string NumberOfCores { get; set; } = "";
    private string NumberOfLogicalProcessors { get; set; } = "";
    private string NumberOfThreads { get; set; } = "";
    private string LoadPercentage { get; set; } = "";
    private string L2CacheSize { get; set; } = "";
    private string L3CacheSize { get; set; } = "";

    // Metrics Nodes
    private TreeViewNode CurrentClockSpeedNode { get; }
    private TreeViewNode MaxClockSpeedNode { get; }
    private TreeViewNode ExternalClockSpeedNode { get; }
    private TreeViewNode CurrentVoltageNode { get; }
    private TreeViewNode NumberOfCoresNode { get; }
    private TreeViewNode NumberOfLogicalProcessorsNode { get; }
    private TreeViewNode NumberOfThreadsNode { get; }
    private TreeViewNode LoadPercentageNode { get; }
    private TreeViewNode L2CacheSizeNode { get; }
    private TreeViewNode L3CacheSizeNode { get; }

    #endregion
}