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
                // Query return
                var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                {
                    "Name",
                    "Manufacturer", "Family", "Architecture", "Description", "SocketDesignation",
                    "VirtualizationFirmwareEnabled", "AddressWidth", "DataWidth", "PowerManagementSupported",
                    "Version", "ProcessorId", "Status"
                });

                // Name property
                ProcessorName = (string)returnValue.PropertiesResultList[0, 0] ?? "N/A";

                // Manufacturer property
                ProcessorManufacturer = (string)returnValue.PropertiesResultList[0, 1] ?? "N/A";

                // Family property
                var family = returnValue.PropertiesResultList[0, 2] ?? 0;

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
                    _ => "Unknown"
                };

                // Architecture
                var architecture = returnValue.PropertiesResultList[0, 3] ?? 100;

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
                    _ => "Unknown"
                };

                // Description
                ProcessorDescription = (string)returnValue.PropertiesResultList[0, 4] ?? "N/A";

                // Socket Designation
                SocketDesignation = (string)returnValue.PropertiesResultList[0, 5] ?? "N/A";

                // Virtualization Firmware
                var virtualizationFirmware = returnValue.PropertiesResultList[0, 6] ?? null;

                Virtualization = virtualizationFirmware switch
                {
                    true => "Yes",
                    false => "No",
                    null => "N/A",
                    _ => ""
                };

                // Address Width
                var addressWidth = returnValue.PropertiesResultList[0, 7] ?? 0;

                AddressWidth = addressWidth switch
                {
                    32 => "32-bit",
                    64 => "64-bit",
                    _ => "Unknown"
                };

                // Data Width
                var dataWidth = returnValue.PropertiesResultList[0, 8] ?? 0;

                DataWidth = dataWidth switch
                {
                    32 => "32-bit",
                    64 => "64-bit",
                    _ => "Unknown"
                };

                // Power Management Supported
                var powerManagementSupported = returnValue.PropertiesResultList[0, 9] ?? null;

                PowerManagementSupported = powerManagementSupported switch
                {
                    true => "True",
                    false => "False",
                    null => "N/A",
                    _ => ""
                };

                // Version
                Version = (string)returnValue.PropertiesResultList[0, 10] ?? "N/A";

                // ProcessorId
                ProcessorId = (string)returnValue.PropertiesResultList[0, 11] ?? "N/A";

                // Status
                Status = (string)returnValue.PropertiesResultList[0, 12] ?? "N/A";
            }),

            // Get CPU Characteristics
            Task.Run(() =>
            {
                Log.Info("Get CPU Characteristics");
                // Query return
                var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[] { "Characteristics" });

                // Get uint value from return
                var uintValue = returnValue.PropertiesResultList[0, 0] ?? 0;

                // Get bits from uint value and convert to string
                var bitsString = "";
                var bits = new uint[32];
                for (var i = 0; i < 32; i++) bits[i] = ((uint)uintValue >> i) & 1;
                for (var i = 0; i < 32; i++) bitsString += bits[i];

                // Get bits value and set the CPU Characteristics
                // 64-bit Capable
                X64BitCapable = bitsString[2] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
                // Multi-Core Processor
                MultiCore = bitsString[3] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
                // Hardware Thread
                HardwareThread = bitsString[4] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
                // Execute Protection
                ExecuteProtection = bitsString[5] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
                // Enhanced Virtualization
                EnhancedVirtualization = bitsString[6] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
                // Power/Performance Control
                PowerPerformanceControl = bitsString[7] switch
                {
                    '0' => "False",
                    '1' => "True",
                    _ => "Unknown"
                };
            }),

            // Get CPU Metrics
            Task.Run(() =>
            {
                Log.Info("Get CPU Metrics");
                // Query return
                var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                {
                    "CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores",
                    "NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
                    "L2CacheSize", "L3CacheSize"
                });

                // CurrentClockSpeed
                var currentClockSpeed = returnValue.PropertiesResultList[0, 0] ?? "N/A";
                CurrentClockSpeed = currentClockSpeed + " MHz";

                // MaxClockSpeed
                var maxClockSpeed = returnValue.PropertiesResultList[0, 1] ?? "N/A";
                MaxClockSpeed = maxClockSpeed + " MHz";
                
                // ExtClock
                var extClock = returnValue.PropertiesResultList[0, 2] ?? "N/A";
                ExternalClockSpeed = extClock + " MHz";

                // CurrentVoltage
                var currentVoltage = returnValue.PropertiesResultList[0, 3] ?? "N/A";
                CurrentVoltage = (ushort)currentVoltage / 10.0 + " Volts";

                // NumberOfCores and NumberOfEnabledCore
                var numberOfCores = returnValue.PropertiesResultList[0, 4] ?? "N/A";
                var numberOfEnabledCores = returnValue.PropertiesResultList[0, 5] ?? "N/A";
                NumberOfCores = numberOfCores + " (" + numberOfEnabledCores + " enabled)";

                // NumberOfLogicalProcessors
                var numberOfLogicalProcessors = returnValue.PropertiesResultList[0, 6] ?? "N/A";
                NumberOfLogicalProcessors = numberOfLogicalProcessors.ToString() ?? "N/A";

                // ThreadCount
                var threadCount = returnValue.PropertiesResultList[0, 7] ?? "N/A";
                NumberOfThreads = threadCount.ToString() ?? "N/A";

                // LoadPercentage
                var loadPercentage = returnValue.PropertiesResultList[0, 8] ?? "N/A";
                LoadPercentage = loadPercentage + " %";

                // L2CacheSize KB and MB
                var l2CacheSize = returnValue.PropertiesResultList[0, 9] ?? "N/A";
                L2CacheSize = l2CacheSize + " KB" + " (" + (uint)l2CacheSize / 1024 + "MB)";

                // L3CacheSize
                var l3CacheSize = returnValue.PropertiesResultList[0, 10] ?? "N/A";
                L3CacheSize = l3CacheSize + " KB" + " (" + (uint)l3CacheSize / 1024 + "MB)";
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
            Trace.WriteLine("Update...");

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

                // Query Return
                var returnValue = _wmiQueryManager.WmIquery("Win32_Processor", new[]
                {
                    "CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores",
                    "NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
                    "L2CacheSize", "L3CacheSize"
                });

                var currentClockSpeed = returnValue.PropertiesResultList[0, 0] ?? "N/A";
                currentClockSpeedString = currentClockSpeed + " MHz";

                // MaxClockSpeed
                var maxClockSpeed = returnValue.PropertiesResultList[0, 1] ?? "N/A";
                maxClockSpeedString = maxClockSpeed + " MHz";

                // ExtClock
                var extClock = returnValue.PropertiesResultList[0, 2] ?? "N/A";
                externalClockSpeedString = extClock + " MHz";
                
                // CurrentVoltage
                var currentVoltage = returnValue.PropertiesResultList[0, 3] ?? "N/A";
                currentVoltageString = (ushort)currentVoltage / 10.0 + " Volts";

                // NumberOfCores and NumberOfEnabledCore
                var numberOfCores = returnValue.PropertiesResultList[0, 4] ?? "N/A";
                var numberOfEnabledCores = returnValue.PropertiesResultList[0, 5] ?? "N/A";
                numberOfCoresString = numberOfCores + " (" + numberOfEnabledCores + " enabled)";

                // NumberOfLogicalProcessors
                var numberOfLogicalProcessors = returnValue.PropertiesResultList[0, 6] ?? "N/A";
                numberOfLogicalProcessorsString = numberOfLogicalProcessors.ToString() ?? "N/A";

                // ThreadCount
                var threadCount = returnValue.PropertiesResultList[0, 7] ?? "N/A";
                numberOfThreadsString = threadCount.ToString() ?? "N/A";

                // LoadPercentage
                var loadPercentage = returnValue.PropertiesResultList[0, 8] ?? "N/A";
                loadPercentageString = loadPercentage + " %";

                // L2CacheSize KB and MB
                var l2CacheSize = returnValue.PropertiesResultList[0, 9] ?? "N/A";
                l2CacheSizeString = l2CacheSize + " KB" + " (" + (uint)l2CacheSize / 1024 + "MB)";

                // L3CacheSize
                var l3CacheSize = returnValue.PropertiesResultList[0, 10] ?? "N/A";
                l3CacheSizeString = l3CacheSize + " KB" + " (" + (uint)l3CacheSize / 1024 + "MB)";


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

                    Trace.WriteLine("Update Done");
                });
                _canUpdateMetrics = true;
            });
        }
        else
        {
            Trace.WriteLine("Update Skipped");
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
                                 $"</h3> </li> <li> <h3>Family :{ProcessorFamily}</h3> </li> <li> " +
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
                if (saveFileDialog.ShowDialog() == true)
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