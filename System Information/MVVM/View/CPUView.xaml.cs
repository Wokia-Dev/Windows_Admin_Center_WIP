using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using log4net;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.Core;

namespace System_Information.MVVM.View
{

    /// <summary>
    /// Logique d'interaction pour CPUView.xaml
    /// </summary>
    public partial class CpuView : UserControl
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        WmIqueryManagement _wmIqueyManager = new WmIqueryManagement();
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
                    var returnValue = _wmIqueyManager.WmIquery("Win32_Processor", new []{ "Name", 
                        "Manufacturer", "Family", "Architecture", "Description", "SocketDesignation", 
                        "VirtualizationFirmwareEnabled", "AddressWidth", "DataWidth", "PowerManagementSupported",
                        "Version", "ProcessorId", "Status"
                    });
                    
                    // Name property
                    ProcessorName = (string)returnValue.PropertiesResultList[0, 0];
                    
                    // Manufacturer property
                    ProcessorManufacturer = (string)returnValue.PropertiesResultList[0, 1];
                    
                    // Family property
                    var family = (ushort)returnValue.PropertiesResultList[0, 2];

                    switch (family)
                    {
                        case 1:
							ProcessorFamily = "Other";
							break;

						case 2:
							ProcessorFamily = "Unknown";
							break;

						case 3:
							ProcessorFamily = "8086";
							break;

						case 4:
							ProcessorFamily = "80286";
							break;

						case 5:
							ProcessorFamily = "80386";
							break;

						case 6:
							ProcessorFamily = "8048";
							break;

						case 7:
							ProcessorFamily = "808";
							break;

						case 8:
							ProcessorFamily = "027";
							break;

						case 9:
							ProcessorFamily = "80387";
							break;

						case 10:
							ProcessorFamily = "80487";
							break;

						case 11:
							ProcessorFamily = "Pentium(R) brand";
							break;

						case 12:
							ProcessorFamily = "Pentium(R) Pro";
							break;

						case 13:
							ProcessorFamily = "Pentium(R) II";
							break;

						case 14:
							ProcessorFamily = "Pentium(R) processor with MMX(TM) technology";
							break;

						case 15:
							ProcessorFamily = "Celeron(TM)";
							break;

						case 16:
							ProcessorFamily = "Pentium(R) II Xeon(TM)";
							break;

						case 17:
							ProcessorFamily = "Pentium(R) III";
							break;

						case 18:
							ProcessorFamily = "M1 Family";
							break;

						case 19:
							ProcessorFamily = "M2 Family";
							break;

						case 20:
							ProcessorFamily = "Intel(R) Celeron(R) M processor";
							break;

						case 21:
							ProcessorFamily = "Intel(R) Pentium(R) 4 HT processor";
							break;

                        case 24:
							ProcessorFamily = "K5 Family";
							break;

						case 25:
							ProcessorFamily = "K6 Family";
							break;

						case 26:
							ProcessorFamily = "K6-2";
							break;

						case 27:
							ProcessorFamily = "K6-3";
							break;

						case 28:
							ProcessorFamily = "AMD Athlon(TM) Processor Family";
							break;

						case 29:
							ProcessorFamily = "AMD(R) Duron(TM) Processor";
							break;

						case 30:
							ProcessorFamily = "AMD29000 Family";
							break;

						case 31:
							ProcessorFamily = "K6-2+";
							break;

						case 32:
							ProcessorFamily = "Power PC Family";
							break;

						case 33:
							ProcessorFamily = "Power PC 601";
							break;

						case 34:
							ProcessorFamily = "Power PC 603";
							break;

						case 35:
							ProcessorFamily = "Power PC 603+";
							break;

						case 36:
							ProcessorFamily = "Power PC 604";
							break;

						case 37:
							ProcessorFamily = "Power PC 620";
							break;

						case 38:
							ProcessorFamily = "Power PC X704";
							break;

						case 39:
							ProcessorFamily = "Power PC 750";
							break;

						case 40:
							ProcessorFamily = "Intel(R) Core(TM) Duo processor";
							break;

						case 41:
							ProcessorFamily = "Intel(R) Core(TM) Duo mobile processor";
							break;

						case 42:
							ProcessorFamily = "Intel(R) Core(TM) Solo mobile processor";
							break;

						case 43:
							ProcessorFamily = "Intel(R) Atom(TM) processor";
							break;

                        case 48:
							ProcessorFamily = "Alpha Family";
							break;

						case 49:
							ProcessorFamily = "Alpha 21064";
							break;

						case 50:
							ProcessorFamily = "Alpha 21066";
							break;

						case 51:
							ProcessorFamily = "Alpha 21164";
							break;

						case 52:
							ProcessorFamily = "Alpha 21164PC";
							break;

						case 53:
							ProcessorFamily = "Alpha 21164a";
							break;

						case 54:
							ProcessorFamily = "Alpha 21264";
							break;

						case 55:
							ProcessorFamily = "Alpha 21364";
							break;

						case 56:
							ProcessorFamily = "AMD Turion(TM) II Ultra Dual-Core Mobile M Processor Family";
							break;

						case 57:
							ProcessorFamily = "AMD Turion(TM) II Dual-Core Mobile M Processor Family";
							break;

						case 58:
							ProcessorFamily = "AMD Athlon(TM) II Dual-Core Mobile M Processor Family";
							break;

						case 59:
							ProcessorFamily = "AMD Opteron(TM) 6100 Series Processor";
							break;
						
						case 63:
							ProcessorFamily = "Available for assignment";
							break;

						case 60:
							ProcessorFamily = "AMD Opteron(TM) 4100 Series Processor";
							break;

						case 64:
							ProcessorFamily = "MIPS Family";
							break;

						case 65:
							ProcessorFamily = "MIPS R4000";
							break;

						case 66:
							ProcessorFamily = "MIPS R4200";
							break;

						case 67:
							ProcessorFamily = "MIPS R4400";
							break;

						case 68:
							ProcessorFamily = "MIPS R4600";
							break;

						case 69:
							ProcessorFamily = "MIPS R10000";
							break;

						case 80:
							ProcessorFamily = "SPARC Family";
							break;

						case 81:
							ProcessorFamily = "SuperSPARC";
							break;

						case 82:
							ProcessorFamily = "microSPARC II";
							break;

						case 83:
							ProcessorFamily = "microSPARC IIep";
							break;

						case 84:
							ProcessorFamily = "UltraSPARC";
							break;

						case 85:
							ProcessorFamily = "UltraSPARC II";
							break;

						case 86:
							ProcessorFamily = "UltraSPARC IIi";
							break;

						case 87:
							ProcessorFamily = "UltraSPARC III";
							break;

						case 88:
							ProcessorFamily = "UltraSPARC IIIi";
							break;

						case 96:
							ProcessorFamily = "68040";
							break;

						case 97:
							ProcessorFamily = "68xxx Family";
							break;

						case 98:
							ProcessorFamily = "68000";
							break;

						case 99:
							ProcessorFamily = "68010";
							break;

						case 100:
							ProcessorFamily = "68020";
							break;

						case 101:
							ProcessorFamily = "68030";
							break;

						case 112:
							ProcessorFamily = "Hobbit Family";
							break;

						case 120:
							ProcessorFamily = "Crusoe(TM) TM5000 Family";
							break;

						case 121:
							ProcessorFamily = "Crusoe(TM) TM3000 Family";
							break;

						case 122:
							ProcessorFamily = "Efficeon(TM) TM8000 Family";
							break;

						case 128:
							ProcessorFamily = "Weitek";
							break;

						case 130:
							ProcessorFamily = "Itanium(TM) Processor";
							break;

						case 131:
							ProcessorFamily = "AMD Athlon(TM) 64 Processor Family";
							break;

						case 132:
							ProcessorFamily = "AMD Opteron(TM) Processor Family";
							break;

						case 133:
							ProcessorFamily = "AMD Sempron(TM) Processor Family";
							break;

						case 134:
							ProcessorFamily = "AMD Turion(TM) 64 Mobile Technology";
							break;

						case 135:
							ProcessorFamily = "Dual-Core AMD Opteron(TM) Processor Family";
							break;

						case 136:
							ProcessorFamily = "AMD Athlon(TM) 64 X2 Dual-Core Processor Family";
							break;

						case 137:
							ProcessorFamily = "AMD Turion(TM) 64 X2 Mobile Technology";
							break;

						case 138:
							ProcessorFamily = "Quad-Core AMD Opteron(TM) Processor Family";
							break;

						case 139:
							ProcessorFamily = "Third-Generation AMD Opteron(TM) Processor Family";
							break;

						case 140:
							ProcessorFamily = "AMD Phenom(TM) FX Quad-Core Processor Family";
							break;

						case 141:
							ProcessorFamily = "AMD Phenom(TM) X4 Quad-Core Processor Family";
							break;

						case 142:
							ProcessorFamily = "AMD Phenom(TM) X2 Dual-Core Processor Family";
							break;

						case 143:
							ProcessorFamily = "AMD Athlon(TM) X2 Dual-Core Processor Family";
							break;

						case 144:
							ProcessorFamily = "PA-RISC Family";
							break;

						case 145:
							ProcessorFamily = "PA-RISC 8500";
							break;

						case 146:
							ProcessorFamily = "PA-RISC 8000";
							break;

						case 147:
							ProcessorFamily = "PA-RISC 7300LC";
							break;

						case 148:
							ProcessorFamily = "PA-RISC 7200";
							break;

						case 149:
							ProcessorFamily = "PA-RISC 7100LC";
							break;

						case 150:
							ProcessorFamily = "PA-RISC 7100";
							break;

						case 160:
							ProcessorFamily = "V30 Family";
							break;

						case 161:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 3200 Series";
							break;

						case 162:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 3000 Series";
							break;

						case 163:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 5300 Series";
							break;

						case 164:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 5100 Series";
							break;

						case 165:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 5000 Series";
							break;

						case 166:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor LV";
							break;

						case 167:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor ULV";
							break;

						case 168:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 7100 Series";
							break;

						case 169:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 5400 Series";
							break;

						case 170:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor";
							break;

						case 171:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 5200 Series";
							break;

						case 172:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 7200 Series";
							break;

						case 173:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 7300 Series";
							break;

						case 174:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 7400 Series";
							break;

						case 175:
							ProcessorFamily = "Multi-Core Intel(R) Xeon(R) processor 7400 Series";
							break;

						case 176:
							ProcessorFamily = "Pentium(R) III Xeon(TM)";
							break;

						case 177:
							ProcessorFamily = "Pentium(R) III Processor with Intel(R) SpeedStep(TM) Technology";
							break;

						case 178:
							ProcessorFamily = "Pentium(R) 4";
							break;

						case 179:
							ProcessorFamily = "Intel(R) Xeon(TM)";
							break;

						case 180:
							ProcessorFamily = "AS400 Family";
							break;

						case 181:
							ProcessorFamily = "Intel(R) Xeon(TM) processor MP";
							break;

						case 182:
							ProcessorFamily = "AMD Athlon(TM) XP Family";
							break;

						case 183:
							ProcessorFamily = "AMD Athlon(TM) MP Family";
							break;

						case 184:
							ProcessorFamily = "Intel(R) Itanium(R) 2";
							break;

						case 185:
							ProcessorFamily = "Intel(R) Pentium(R) M processor";
							break;

						case 186:
							ProcessorFamily = "Intel(R) Celeron(R) D processor";
							break;

						case 187:
							ProcessorFamily = "Intel(R) Pentium(R) D processor";
							break;

						case 188:
							ProcessorFamily = "Intel(R) Pentium(R) Processor Extreme Edition";
							break;

						case 189:
							ProcessorFamily = "Intel(R) Core(TM) Solo Processor";
							break;

						case 190:
							ProcessorFamily = "K7";
							break;

						case 191:
							ProcessorFamily = "Intel(R) Core(TM)2 Duo Processor";
							break;

						case 192:
							ProcessorFamily = "Intel(R) Core(TM)2 Solo processor";
							break;

						case 193:
							ProcessorFamily = "Intel(R) Core(TM)2 Extreme processor";
							break;

						case 194:
							ProcessorFamily = "Intel(R) Core(TM)2 Quad processor";
							break;

						case 195:
							ProcessorFamily = "Intel(R) Core(TM)2 Extreme mobile processor";
							break;

						case 196:
							ProcessorFamily = "Intel(R) Core(TM)2 Duo mobile processor";
							break;

						case 197:
							ProcessorFamily = "Intel(R) Core(TM)2 Solo mobile processor";
							break;

						case 198:
							ProcessorFamily = "Intel(R) Core(TM) i7 processor";
							break;

						case 199:
							ProcessorFamily = "Dual-Core Intel(R) Celeron(R) Processor";
							break;

						case 200:
							ProcessorFamily = "S/390 and zSeries Family";
							break;

						case 201:
							ProcessorFamily = "ESA/390 G4";
							break;

						case 202:
							ProcessorFamily = "ESA/390 G5";
							break;

						case 203:
							ProcessorFamily = "ESA/390 G6";
							break;

						case 204:
							ProcessorFamily = "z/Architectur base";
							break;

						case 205:
							ProcessorFamily = "Intel(R) Core(TM) i5 processor";
							break;

						case 206:
							ProcessorFamily = "Intel(R) Core(TM) i3 processor";
							break;

						case 207:
							ProcessorFamily = "Intel(R) Core(TM) i9 processor";
							break;

						case 210:
							ProcessorFamily = "VIA C7(TM)-M Processor Family";
							break;

						case 211:
							ProcessorFamily = "VIA C7(TM)-D Processor Family";
							break;

						case 212:
							ProcessorFamily = "VIA C7(TM) Processor Family";
							break;

						case 213:
							ProcessorFamily = "VIA Eden(TM) Processor Family";
							break;

						case 214:
							ProcessorFamily = "Multi-Core Intel(R) Xeon(R) processor";
							break;

						case 215:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 3xxx Series";
							break;

						case 216:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 3xxx Series";
							break;

						case 217:
							ProcessorFamily = "VIA Nano(TM) Processor Family";
							break;

						case 218:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 5xxx Series";
							break;

						case 219:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 5xxx Series";
							break;

						case 221:
							ProcessorFamily = "Dual-Core Intel(R) Xeon(R) processor 7xxx Series";
							break;

						case 222:
							ProcessorFamily = "Quad-Core Intel(R) Xeon(R) processor 7xxx Series";
							break;

						case 223:
							ProcessorFamily = "Multi-Core Intel(R) Xeon(R) processor 7xxx Series";
							break;

						case 224:
							ProcessorFamily = "Multi-Core Intel(R) Xeon(R) processor 3400 Series";
							break;

						case 230:
							ProcessorFamily = "Embedded AMD Opteron(TM) Quad-Core Processor Family";
							break;

						case 231:
							ProcessorFamily = "AMD Phenom(TM) Triple-Core Processor Family";
							break;

						case 232:
							ProcessorFamily = "AMD Turion(TM) Ultra Dual-Core Mobile Processor Family";
							break;

						case 233:
							ProcessorFamily = "AMD Turion(TM) Dual-Core Mobile Processor Family";
							break;

						case 234:
							ProcessorFamily = "AMD Athlon(TM) Dual-Core Processor Family";
							break;

						case 235:
							ProcessorFamily = "AMD Sempron(TM) SI Processor Family";
							break;

						case 236:
							ProcessorFamily = "AMD Phenom(TM) II Processor Family";
							break;

						case 237:
							ProcessorFamily = "AMD Athlon(TM) II Processor Family";
							break;

						case 238:
							ProcessorFamily = "Six-Core AMD Opteron(TM) Processor Family";
							break;

						case 239:
							ProcessorFamily = "AMD Sempron(TM) M Processor Family";
							break;

						case 250:
							ProcessorFamily = "i860";
							break;

						case 251:
							ProcessorFamily = "i960";
							break;

						case 254:
							ProcessorFamily = "Reserved (SMBIOS Extension)";
							break;

						case 255:
							ProcessorFamily = "Reserved (Un-initialized Flash Content - Lo)";
							break;

						case 260:
							ProcessorFamily = "SH-3";
							break;

						case 261:
							ProcessorFamily = "SH-4";
							break;

						case 280:
							ProcessorFamily = "ARM";
							break;

						case 281:
							ProcessorFamily = "StrongARM";
							break;

						case 300:
							ProcessorFamily = "6x86";
							break;

						case 301:
							ProcessorFamily = "MediaGX";
							break;

						case 302:
							ProcessorFamily = "MII";
							break;

						case 320:
							ProcessorFamily = "WinChip";
							break;

						case 350:
							ProcessorFamily = "DSP";
							break;

						case 500:
							ProcessorFamily = "Video Processor";
							break;

						case 65534:
							ProcessorFamily = "Reserved (For Future Special Purpose Assignment)";
							break;

						case 65535:
							ProcessorFamily = "Reserved (Un-initialized Flash Content - Hi";
							break;
						
						default: 
							ProcessorFamily = "Unknown";
							break;

                    }
                    
                    // Architecture
                    var architecture = (ushort)returnValue.PropertiesResultList[0, 3];

                    switch (architecture)
                    {
	                    case 0:
		                    ProcessorArchitecture = "x86";
		                    break;

	                    case 1:
		                    ProcessorArchitecture = "MIPS";
		                    break;

	                    case 2:
		                    ProcessorArchitecture = "Alpha";
		                    break;

	                    case 3:
		                    ProcessorArchitecture = "PowerPC";
		                    break;

	                    case 5:
		                    ProcessorArchitecture = "ARM";
		                    break;

	                    case 6:
		                    ProcessorArchitecture = "ia4";
		                    break;

	                    case 9:
		                    ProcessorArchitecture = "x64";
		                    break;

	                    case 12:
		                    ProcessorArchitecture = "ARM6";
		                    break;
	                    
	                    default:
		                    ProcessorArchitecture = "Unknown";
		                    break;

                    }
                    
                    // Description
                    ProcessorDescription = (string)returnValue.PropertiesResultList[0, 4];
                    
                    // Socket Designation
                    SocketDesignation = (string)returnValue.PropertiesResultList[0, 5];
                    
                    // Virtualization Firmware
                    var virtualizationFirmware = (bool)returnValue.PropertiesResultList[0, 6];

                    switch (virtualizationFirmware)
                    {
	                    case true:
		                    Virtualization = "Yes";
		                    break;

	                    case false:
		                    Virtualization = "No";
		                    break;
	                    
                    }
                    
                    // Address Width
                    var addressWidth = (ushort)returnValue.PropertiesResultList[0, 7];

                    switch (addressWidth)
                    {
	                    case 32:
		                    AddressWidth = "32-bit";
		                    break;
	                    
	                    case 64:
		                    AddressWidth = "64-bit";
		                    break;
	                    
	                    default:						
		                    AddressWidth = "Unknown";
		                    break;
                    }
                    
                    // Data Width
                    var dataWidth = (ushort)returnValue.PropertiesResultList[0, 8];

                    switch (dataWidth)
                    {
	                    case 32:
		                    DataWidth = "32-bit";
		                    break;
	                    
	                    case 64:
		                    DataWidth = "64-bit";
		                    break;
	                    
	                    default:
		                    DataWidth = "Unknown";
		                    break;
                    }
                    
                    // Power Management Supported
                    var powerManagementSupported = (bool)returnValue.PropertiesResultList[0, 9];

                    switch (powerManagementSupported)
                    {
	                    case true:
		                    PowerManagementSupported = "True";
		                    break;
	                    
	                    case false:
		                    PowerManagementSupported = "False";
		                    break;
                    }
                    
                    // Version
                    Version = (string)returnValue.PropertiesResultList[0, 10];
                    
                    // ProcessorId
                    ProcessorId = (string)returnValue.PropertiesResultList[0, 11];
                    
                    // Status
                    Status = (string)returnValue.PropertiesResultList[0, 12];
                }),
                
                // Get CPU Characteristics
                Task.Run(() =>
                {
                    Log.Info("Get CPU Characteristics");
                    // Query return
                    var returnValue = _wmIqueyManager.WmIquery("Win32_Processor", new[] { "Characteristics" });
                    
                    // Get uint value from return
                    var uintValue = (uint)returnValue.PropertiesResultList[0, 0];
                    
                    // Get bits from uint value and convert to string
                    var bitsString = "";
                    var bits = new uint[32];
                    for (var i = 0; i < 32; i++)
                    {
	                    bits[i] = (uintValue >> i) & 1;
                    }
                    for (var i = 0; i < 32; i++)
                    {
	                    bitsString += bits[i];
                    }
                    
                    // Get bits value and set the CPU Characteristics
                    // 64-bit Capable
                    switch (bitsString[2])
                    {
	                    case '0':
		                    X64BitCapable = "False";
		                    break;
	                    case '1':
		                    X64BitCapable = "True";
		                    break;
	                    default:
		                    X64BitCapable = "Unknown";
		                    break;
                    }
                    // Multi-Core Processor
                    switch (bitsString[3])
					{
	                    case '0':
		                    MultiCore = "False";
		                    break;
	                    case '1':
		                    MultiCore = "True";
		                    break;
	                    default:
		                    MultiCore = "Unknown";
		                    break;
					}
                    // Hardware Thread
                    switch (bitsString[4])
                    {
	                    case '0':
		                    HardwareThread = "False";
		                    break;
	                    case '1':
		                    HardwareThread = "True";
		                    break;
	                    default:
		                    HardwareThread = "Unknown";
		                    break;
                    }
                    // Execute Protection
                    switch (bitsString[5])
					{
	                    case '0':
		                    ExecuteProtection = "False";
		                    break;
	                    case '1':
		                    ExecuteProtection = "True";
		                    break;
	                    default:
		                    ExecuteProtection = "Unknown";
		                    break;
					}
                    // Enhanced Virtualization
                    switch (bitsString[6])
                    {
	                    case '0':
		                    EnhancedVirtualization = "False";
		                    break;
	                    case '1':
		                    EnhancedVirtualization = "True";
		                    break;
	                    default:
		                    EnhancedVirtualization = "Unknown";
		                    break;
                    }
                    // Power/Performance Control
                    switch (bitsString[7])
					{
	                    case '0':
		                    PowerPerformanceControl = "False";
		                    break;
	                    case '1':
		                    PowerPerformanceControl = "True";
		                    break;
	                    default:
		                    PowerPerformanceControl = "Unknown";
		                    break;
					}
                    
                }),
                
                // Get CPU Metrics
                Task.Run(() =>
                {
	                Log.Info("Get CPU Metrics");
                    // Query return
                    var returnValue = _wmIqueyManager.WmIquery("Win32_Processor", new[]
                    {
	                    "CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores", 
	                    "NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
	                    "L2CacheSize", "L3CacheSize"
                    });
                    
                    // CurrentClockSpeed
                    var currentClockSpeed = (uint)returnValue.PropertiesResultList[0, 0];
                    CurrentClockSpeed = currentClockSpeed + " MHz";
                    
                    // MaxClockSpeed
                    var maxClockSpeed = (uint)returnValue.PropertiesResultList[0, 1];
                    MaxClockSpeed = maxClockSpeed + " MHz";
                    
                    // ExtClock
                    var extClock = (uint)returnValue.PropertiesResultList[0, 2];
                    ExternalClockSpeed = extClock + " MHz";
                    
                    // CurrentVoltage
                    var currentVoltage = (ushort)returnValue.PropertiesResultList[0, 3];
                    CurrentVoltage = (currentVoltage / 10.0) + " Volts";
                    
                    // NumberOfCores and NumberOfEnabledCore
                    var numberOfCores = (uint)returnValue.PropertiesResultList[0, 4];
                    var numberOfEnabledCores = (uint)returnValue.PropertiesResultList[0, 5];
                    NumberOfCores = numberOfCores + " (" + numberOfEnabledCores + " enabled)";
                    
                    // NumberOfLogicalProcessors
                    var numberOfLogicalProcessors = (uint)returnValue.PropertiesResultList[0, 6];
                    NumberOfLogicalProcessors = numberOfLogicalProcessors.ToString();
                    
                    // ThreadCount
                    var threadCount = (uint)returnValue.PropertiesResultList[0, 7];
                    NumberOfThreads = threadCount.ToString();
                    
                    // LoadPercentage
                    var loadPercentage = (ushort)returnValue.PropertiesResultList[0, 8];
                    LoadPercentage = loadPercentage + " %";
                    
                    // L2CacheSize KB and MB
                    var l2CacheSize = (uint)returnValue.PropertiesResultList[0, 9];
                    L2CacheSize = l2CacheSize + " KB" + " (" + l2CacheSize / 1024 + "MB)";
                    
                    // L3CacheSize
                    var l3CacheSize = (uint)returnValue.PropertiesResultList[0, 10];
                    L3CacheSize = l3CacheSize + " KB" + " (" + l3CacheSize / 1024 + "MB)";

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

	        var dispatcherTimer = new DispatcherTimer();
	        dispatcherTimer.Tick += dispatcherTimer_Tick;
	        dispatcherTimer.Interval = new TimeSpan(0,0,0,1);
	        dispatcherTimer.Start();
        }
        

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
	        if (_canUpdateMetrics)
	        {
		        _canUpdateMetrics = false;
		        Trace.WriteLine("Update...");
	        
		        var currentClockSpeedString = "N/A";
		        var maxClockSpeedString = "N/A";
		        var externalClockSpeedString = "N/A";
		        var currentVoltageString = "N/A";
		        var numberOfCoresString = "N/A";
		        var numberOfLogicalProcessorsString = "N/A";
		        var numberOfThreadsString = "N/A";
		        var loadPercentageString = "N/A";
		        var l2CacheSizeString = "N/A";
		        var l3CacheSizeString = "N/A";
		        
		        Task.Run(() =>
		        {
					Log.Info("Get CPU Metrics For Update");

					// Query Return
					var returnValue = _wmIqueyManager.WmIquery("Win32_Processor", new[]
					{
						"CurrentClockSpeed", "MaxClockSpeed", "ExtClock", "CurrentVoltage", "NumberOfCores", 
						"NumberOfEnabledCore", "NumberOfLogicalProcessors", "ThreadCount", "LoadPercentage",
						"L2CacheSize", "L3CacheSize"
					});
					
			        var currentClockSpeed = (uint)returnValue.PropertiesResultList[0, 0];
			        currentClockSpeedString = currentClockSpeed + " MHz";
	                    
			        // MaxClockSpeed
			        var maxClockSpeed = (uint)returnValue.PropertiesResultList[0, 1];
			        maxClockSpeedString = maxClockSpeed + " MHz";
	                    
			        // ExtClock
			        var extClock = (uint)returnValue.PropertiesResultList[0, 2];
			        externalClockSpeedString = extClock + " MHz";
	                    
			        // CurrentVoltage
			        var currentVoltage = (ushort)returnValue.PropertiesResultList[0, 3];
			        currentVoltageString = (currentVoltage / 10.0) + " Volts";
	                    
			        // NumberOfCores and NumberOfEnabledCore
			        var numberOfCores = (uint)returnValue.PropertiesResultList[0, 4];
			        var numberOfEnabledCores = (uint)returnValue.PropertiesResultList[0, 5];
			        numberOfCoresString = numberOfCores + " (" + numberOfEnabledCores + " enabled)";
	                    
			        // NumberOfLogicalProcessors
			        var numberOfLogicalProcessors = (uint)returnValue.PropertiesResultList[0, 6];
			        numberOfLogicalProcessorsString = numberOfLogicalProcessors.ToString();
	                    
			        // ThreadCount
			        var threadCount = (uint)returnValue.PropertiesResultList[0, 7];
			        numberOfThreadsString = threadCount.ToString();
	                    
			        // LoadPercentage
			        var loadPercentage = (ushort)returnValue.PropertiesResultList[0, 8];
			        loadPercentageString = loadPercentage + " %";
			        
			        // L2CacheSize KB and MB
			        var l2CacheSize = (uint)returnValue.PropertiesResultList[0, 9];
			        l2CacheSizeString = l2CacheSize + " KB" + " (" + l2CacheSize / 1024 + "MB)";
	                    
			        // L3CacheSize
			        var l3CacheSize = (uint)returnValue.PropertiesResultList[0, 10];
			        l3CacheSizeString = l3CacheSize + " KB" + " (" + l3CacheSize / 1024 + "MB)";
					
			        
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
				        NumberOfLogicalProcessorsNode.Content = "Number of Logical Processors : " + numberOfLogicalProcessorsString;
	            
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
        }

        private void MainTreeView_OnKeyDown(object sender, KeyEventArgs e)
        {
            // Copy selected element value to clipboard
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var selectedItem = (TreeViewNode)MainTreeView.SelectedItem;
                Clipboard.SetText(selectedItem.Content.ToString());
                Log.Info("Copied to clipboard : " + selectedItem.Content);
            }
        }

        #region Values

        private string ProcessorName { get; set; }
        private string ProcessorManufacturer { get; set; }
        private string ProcessorFamily { get; set; }
        private string ProcessorArchitecture { get; set; }
        private string ProcessorDescription { get; set; }
        private string SocketDesignation { get; set; }
        private string Virtualization { get; set; }
        private string AddressWidth { get; set; }
        private string DataWidth { get; set; }
        private string PowerManagementSupported { get; set; }
        private string Version { get; set; }
        private string ProcessorId { get; set; }
        private string Status { get; set; }
        private string X64BitCapable { get; set; }
        private string MultiCore { get; set; }
        private string HardwareThread { get; set; }
        private string ExecuteProtection { get; set; }
        private string EnhancedVirtualization { get; set; }
        private string PowerPerformanceControl { get; set; }
        private string CurrentClockSpeed { get; set; }
        private string MaxClockSpeed { get; set; }
        private string ExternalClockSpeed { get; set; }
        private string CurrentVoltage { get; set; }
        private string NumberOfCores { get; set; }
        private string NumberOfLogicalProcessors { get; set; }
        private string NumberOfThreads { get; set; }
        private string LoadPercentage { get; set; }
        private string L2CacheSize { get; set; }
        private string L3CacheSize { get; set; }
        
        // Metrics Nodes
        private TreeViewNode CurrentClockSpeedNode { get; set; }
        private TreeViewNode MaxClockSpeedNode { get; set; }
        private TreeViewNode ExternalClockSpeedNode { get; set; }
        private TreeViewNode CurrentVoltageNode { get; set; }
        private TreeViewNode NumberOfCoresNode { get; set; }
        private TreeViewNode NumberOfLogicalProcessorsNode { get; set; }
        private TreeViewNode NumberOfThreadsNode { get; set; }
        private TreeViewNode LoadPercentageNode { get; set; }
        private TreeViewNode L2CacheSizeNode { get; set; }
        private TreeViewNode L3CacheSizeNode { get; set; }

        #endregion
        
        
    }
}
