using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using log4net;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.Core;

namespace System_Information.MVVM.View;

/// <summary>
///     Logique d'interaction pour HardwareView.xaml
/// </summary>
public partial class HardwareView
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    public HardwareView()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("MaterialLight"));

        var convertorUtils = new ConvertorUtils();
        var wmiQueryManager = new WmIqueryManagement();

        // Set all list to new list
        MemoryList = new List<string>();
        DiskList = new List<string>();
        GpuList = new List<string>();
        AudioDeviceList = new List<string>();
        NetworkAdapterList = new List<string>();
        KeyboardList = new List<string>();
        MouseList = new List<string>();

        var wmiTasks = new List<Task>
        {
            // Get Chassis Types
            Task.Run(() =>
            {
                Log.Info("Get Chassis Types with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_SystemEnclosure", new[] { "ChassisTypes" });

                    // Get Properties values
                    var result = (ushort[])returnValue.PropertiesResultList[0, 0];

                    // List of all chassis types
                    var chassisTypesString = new List<string>();

                    // Add all  chassis types to List
                    foreach (var num in result)
                        switch (num)
                        {
                            case 1:
                                chassisTypesString.Add("Other");
                                break;
                            case 2:
                                chassisTypesString.Add("Unknown");
                                break;
                            case 3:
                                chassisTypesString.Add("Desktop");
                                break;
                            case 4:
                                chassisTypesString.Add("Low Profile Desktop");
                                break;
                            case 5:
                                chassisTypesString.Add("Pizza Box");
                                break;
                            case 6:
                                chassisTypesString.Add("Mini Tower");
                                break;
                            case 7:
                                chassisTypesString.Add("Tower");
                                break;
                            case 8:
                                chassisTypesString.Add("Portable");
                                break;
                            case 9:
                                chassisTypesString.Add("Laptop");
                                break;
                            case 10:
                                chassisTypesString.Add("Notebook");
                                break;
                            case 11:
                                chassisTypesString.Add("Hand Held");
                                break;
                            case 12:
                                chassisTypesString.Add("Docking Station");
                                break;
                            case 13:
                                chassisTypesString.Add("All in One");
                                break;
                            case 14:
                                chassisTypesString.Add("Sub Notebook");
                                break;
                            case 15:
                                chassisTypesString.Add("Space-Saving");
                                break;
                            case 16:
                                chassisTypesString.Add("Lunch Box");
                                break;
                            case 17:
                                chassisTypesString.Add("Main System Chassis");
                                break;
                            case 18:
                                chassisTypesString.Add("Expansion Chassis");
                                break;
                            case 19:
                                chassisTypesString.Add("SubChassis");
                                break;
                            case 20:
                                chassisTypesString.Add("Bus Expansion Chassis");
                                break;
                            case 21:
                                chassisTypesString.Add("Peripheral Chassis");
                                break;
                            case 22:
                                chassisTypesString.Add("Storage Chassis");
                                break;
                            case 23:
                                chassisTypesString.Add("Rack Mount Chassis");
                                break;
                            case 24:
                                chassisTypesString.Add("Sealed-Case PC");
                                break;
                            case 30:
                                chassisTypesString.Add("Tablet");
                                break;
                            case 31:
                                chassisTypesString.Add("Convertible");
                                break;
                            case 32:
                                chassisTypesString.Add("Detachable");
                                break;
                            default:
                                Log.Warn("Unknown Chassis Type");
                                break;
                        }

                    // Set final string value of chassis Types
                    if (chassisTypesString.Count > 1)
                        for (var i = 0; i < chassisTypesString.Count; i++)
                            if (i == chassisTypesString.Count - 1)
                                ChassisTypes += chassisTypesString[i];
                            else
                                ChassisTypes += chassisTypesString[i] + " / ";
                    else
                        ChassisTypes = chassisTypesString[0];

                    Log.Info("Successfully get Chassis Types");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Chassis Types", e);
                    ChassisTypes = "N/A";
                }
            }),

            // Get MotherBoard Model
            Task.Run(() =>
            {
                Log.Info("Get MotherBoard Model with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BaseBoard", new[] { "Product" });

                    // Set final string of MotherBoard Model
                    MotherBoardModel = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successfully get MotherBoard Model");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get MotherBoard Model", e);
                    MotherBoardModel = "N/A";
                }
            }),

            // Get Bios Name
            Task.Run(() =>
            {
                Log.Info("Get Bios Name with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "Name" });

                    // Set final string of Bios Name
                    BiosName = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successfully get Bios Name");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Bios Name", e);
                    BiosName = "N/A";
                }
            }),

            // Get UUID
            Task.Run(() =>
            {
                Log.Info("Get UUID with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_ComputerSystemProduct", new[] { "UUID" });

                    // Set final string of UUID
                    Uuid = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successfully get UUID");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get UUID", e);
                    Uuid = "N/A";
                }
            }),

            // Get Processor Info
            Task.Run(() =>
            {
                Log.Info("Get Processor Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_Processor",
                        new[] { "Name", "Manufacturer", "Architecture" });

                    // CPU Name
                    try
                    {
                        // CPU Name
                        CpuName = (string)returnValue.PropertiesResultList[0, 0];
                        Log.Info("Successfully get CPU Name");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Name", e);
                        CpuName = "N/A";
                    }

                    // CPU Manufacturer
                    try
                    {
                        // CPU Manufacturer
                        CpuManufacturer = (string)returnValue.PropertiesResultList[0, 1];
                        Log.Info("Successfully get CPU Manufacturer");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Manufacturer", e);
                        CpuManufacturer = "N/A";
                    }

                    // CPU Architecture
                    try
                    {
                        // get property value
                        var result = (ushort)returnValue.PropertiesResultList[0, 2];

                        // Set CPU Architecture string 
                        CpuArchitecture = result switch
                        {
                            0 => "x86",
                            1 => "MIPS",
                            2 => "Alpha",
                            3 => "PowerPC",
                            5 => "ARM",
                            6 => "ia64",
                            9 => "x64",
                            12 => "ARM64",
                            _ => CpuArchitecture
                        };
                        Log.Info("Successfully get CPU Architecture");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get CPU Architecture", e);
                        CpuArchitecture = "N/A";
                    }

                    Log.Info("Successfully get Processor Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Processor Info", e);
                    CpuName = "N/A";
                    CpuManufacturer = "N/A";
                    CpuArchitecture = "N/A";
                }
            }),

            // Get Memory Info
            Task.Run(() =>
            {
                Log.Info("Get Memory Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_PhysicalMemory",
                        new[] { "Capacity", "ConfiguredClockSpeed" });

                    // Memory Nb
                    try
                    {
                        // Set final string of memory Nb
                        MemoryNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Memory Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Memory Nb", e);
                        MemoryNb = "N/A";
                    }

                    // Memory List
                    try
                    {
                        // Add to memory list each memory component
                        for (var i = 0; i < returnValue.NbResult; i++)
                        {
                            // format return value to string
                            string capacity;
                            var bytes = (ulong)returnValue.PropertiesResultList[i, 0];
                            if (bytes > 1000000000)
                                capacity =
                                    convertorUtils.BytsToGigaBytes((ulong)returnValue.PropertiesResultList[i, 0]) +
                                    "GB";
                            else
                                capacity = convertorUtils.BytesToMegaBytes(
                                               (ulong)returnValue.PropertiesResultList[i, 0]) +
                                           "MB";
                            var speed = returnValue.PropertiesResultList[i, 1] + "MHz";

                            // add final string
                            MemoryList.Add(capacity + " " + speed);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Memory Capacity", e);
                        MemoryList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Memory Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Memory Info", e);
                    MemoryNb = "N/A";
                    MemoryList = new List<string> { "N/A" };
                }
            }),

            // Get Disk Drive Info
            Task.Run(() =>
            {
                Log.Info("Get Disk Drive Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_DiskDrive", new[] { "Caption", "Size" });

                    // Disk Drive Nb
                    try
                    {
                        // Set final string of disk drive Nb
                        DiskNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Disk Drive Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Disk Drive Nb", e);
                        DiskNb = "N/A";
                    }

                    // Disk Drive List
                    try
                    {
                        // Add to disk List each disk drive
                        for (var i = 0; i < returnValue.NbResult; i++)
                        {
                            // format return value to string
                            var size = convertorUtils.BytsToGigaBytes((ulong)returnValue.PropertiesResultList[i, 1]) +
                                       "GB";
                            var caption = (string)returnValue.PropertiesResultList[i, 0];

                            DiskList.Add(caption + " (" + size + ")");
                        }

                        Log.Info("Successfully get Disk Drive List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Disk Drive List", e);
                        DiskList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Disk Drive Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Disk Drive Info", e);
                    DiskNb = "N/A";
                    DiskList = new List<string> { "N/A" };
                }
            }),

            // Get GPU Info
            Task.Run(() =>
            {
                Log.Info("Get GPU Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController", new[] { "Caption" });

                    // GPU Nb
                    try
                    {
                        // set final string of GPU Nb
                        GpuNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get GPU Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get GPU Nb", e);
                        GpuNb = "N/A";
                    }

                    // GPU List
                    try
                    {
                        // Add to gpu List each gpu
                        for (var i = 0; i < returnValue.NbResult; i++)
                            GpuList.Add((string)returnValue.PropertiesResultList[i, 0]);

                        Log.Info("Successfully get GPU List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get GPU List", e);
                        GpuList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get GPU Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get GPU Info", e);
                    GpuNb = "N/A";
                    GpuList = new List<string> { "N/A" };
                }
            }),

            // Get Audio Devices Info
            Task.Run(() =>
            {
                Log.Info("Get Audio Devices Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_SoundDevice", new[] { "Caption" });

                    // Audio Devices Nb
                    try
                    {
                        // set final string to audi device Nb
                        AudioDeviceNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Audio Devices Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Audio Devices Nb", e);
                        AudioDeviceNb = "N/A";
                    }

                    // Audio Devices List
                    try
                    {
                        // Add to audio device List each audio device
                        for (var i = 0; i < returnValue.NbResult; i++)
                            AudioDeviceList.Add((string)returnValue.PropertiesResultList[i, 0]);

                        Log.Info("Successfully get Audio Devices List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Audio Devices List", e);
                        AudioDeviceList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Audio Devices Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Audio Devices Info", e);
                    AudioDeviceNb = "N/A";
                    AudioDeviceList = new List<string> { "N/A" };
                }
            }),

            // Get Network Adapter Info
            Task.Run(() =>
            {
                Log.Info("Get Network Adapter Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_NetworkAdapter",
                        new[] { "Description", "NetConnectionStatus" }, "NetConnectionStatus");

                    // Network Adapter Nb
                    try
                    {
                        // set final string to network adapter Nb
                        NetworkAdapterNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Network Adapter Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Network Adapter Nb", e);
                        NetworkAdapterNb = "N/A";
                    }

                    // Network Adapter List
                    try
                    {
                        // Add to network adapter List each network adapter
                        for (var i = 0; i < returnValue.NbResult; i++)
                        {
                            // format return value to string
                            var description = (string)returnValue.PropertiesResultList[i, 0];
                            var result = (ushort)returnValue.PropertiesResultList[i, 1];

                            var status = result switch
                            {
                                0 => "Disconnected",
                                1 => "Connecting",
                                2 => "Connected",
                                3 => "Disconnecting",
                                4 => "Hardware not present",
                                5 => "Hardware disabled",
                                6 => "Hardware malfunction",
                                7 => "Media disconnected",
                                8 => "Authenticating",
                                9 => "Authentication succeeded",
                                10 => "Authentication failed",
                                11 => "Invalid address",
                                12 => "Credentials required",
                                _ => "N/A"
                            };

                            NetworkAdapterList.Add(description + " (" + status + ")");
                        }

                        Log.Info("Successfully get Network Adapter List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Network Adapter List", e);
                        NetworkAdapterList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Network Adapter Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Network Adapter Info", e);
                    NetworkAdapterNb = "N/A";
                    NetworkAdapterList = new List<string> { "N/A" };
                }
            }),

            // Get Keyboard Info
            Task.Run(() =>
            {
                Log.Info("Get Keyboard Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_Keyboard", new[] { "Description" });

                    // Keyboard Nb
                    try
                    {
                        // set final string to keyboard Nb
                        KeyboardNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Keyboard Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Keyboard Info", e);
                        KeyboardNb = "N/A";
                    }

                    // Keyboard List
                    try
                    {
                        // Add to keyboard List each keyboard
                        for (var i = 0; i < returnValue.NbResult; i++)
                            KeyboardList.Add((string)returnValue.PropertiesResultList[i, 0]);

                        Log.Info("Successfully get Keyboard List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Keyboard List", e);
                        KeyboardList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Keyboard Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Keyboard Info", e);
                    KeyboardNb = "N/A";
                    KeyboardList = new List<string> { "N/A" };
                }
            }),

            // Get Mouse Info
            Task.Run(() =>
            {
                Log.Info("Get Mouse Info with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_PointingDevice", new[] { "Description" });

                    // Mouse Nb
                    try
                    {
                        // set final string to mouse Nb
                        MouseNb = "X" + returnValue.NbResult;
                        Log.Info("Successfully get Mouse Nb");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Mouse Nb", e);
                        MouseNb = "N/A";
                    }

                    // Mouse List
                    try
                    {
                        // Add to mouse List each mouse device
                        for (var i = 0; i < returnValue.NbResult; i++)
                            MouseList.Add((string)returnValue.PropertiesResultList[i, 0]);

                        Log.Info("Successfully get Mouse List");
                    }
                    catch (Exception e)
                    {
                        Log.Error("Failed to get Mouse List", e);
                        MouseList = new List<string> { "N/A" };
                    }

                    Log.Info("Successfully get Mouse Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Mouse Info", e);
                    MouseNb = "N/A";
                    MouseList = new List<string> { "N/A" };
                }
            }),

            // Get Win SAT Info
            Task.Run(() =>
            {
                Log.Info("Get Win SAT Info with WMI");
                // Read last Win Sat Run Result
                var dirName = Environment.ExpandEnvironmentVariables(@"%WinDir%\Performance\WinSAT\DataStore\");
                var dirInfo = new DirectoryInfo(dirName);
                var file = dirInfo.EnumerateFileSystemInfos("*Formal.Assessment*.xml").MaxBy(fi
                    => fi.LastWriteTime);

                if (file != null)
                {
                    var doc = XDocument.Load(file.FullName);

                    MemoryScore = doc.Descendants("MemoryScore").First().Value;
                    CpuScore = doc.Descendants("CpuScore").First().Value;
                    PrimaryDiskScore = doc.Descendants("DiskScore").First().Value;
                    Graphics3dScore = doc.Descendants("GamingScore").First().Value;
                    GraphicsScore = doc.Descendants("GraphicsScore").First().Value;
                    Log.Info("Win SAT file found");
                }
                else
                {
                    MemoryScore = "N/A";
                    CpuScore = "N/A";
                    PrimaryDiskScore = "N/A";
                    Graphics3dScore = "N/A";
                    GraphicsScore = "N/A";
                    Log.Warn("Win SAT file not found");
                }
            })
        };

        Task.WaitAll(wmiTasks.ToArray());


        var basicHardwareNodeList = new List<TreeViewNode>();

        // first parent child > child
        var mouseNodeList = new List<TreeViewNode>();
        foreach (var mouse in MouseList)
        {
            var node = new TreeViewNode();
            node.Content = mouse;
            mouseNodeList.Add(node);
        }

        // first parent child
        var mouseNode = new TreeViewNode();
        mouseNode.Content = $"Mouse {MouseNb}";
        mouseNode.IsExpanded = true;
        foreach (var node in mouseNodeList) mouseNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(mouseNode);

        // first parent child > child
        var keyboardNodeList = new List<TreeViewNode>();
        foreach (var keyboard in KeyboardList)
        {
            var node = new TreeViewNode();
            node.Content = keyboard;
            keyboardNodeList.Add(node);
        }

        // first parent child
        var keyboardNode = new TreeViewNode();
        keyboardNode.Content = $"Keyboard {KeyboardNb}";
        keyboardNode.IsExpanded = true;
        foreach (var node in keyboardNodeList) keyboardNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(keyboardNode);

        // first parent child > child 
        var networkAdapterNodeList = new List<TreeViewNode>();
        foreach (var networkAdapter in NetworkAdapterList)
        {
            var node = new TreeViewNode();
            node.Content = networkAdapter;
            networkAdapterNodeList.Add(node);
        }

        // first parent child
        var networkAdapterNode = new TreeViewNode();
        networkAdapterNode.Content = $"Network Adapter {NetworkAdapterNb}";
        networkAdapterNode.IsExpanded = true;
        foreach (var node in networkAdapterNodeList) networkAdapterNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(networkAdapterNode);

        // first parent child > child
        var audioDeviceNodeList = new List<TreeViewNode>();
        foreach (var audioDevice in AudioDeviceList)
        {
            var node = new TreeViewNode();
            node.Content = audioDevice;
            audioDeviceNodeList.Add(node);
        }

        // first parent child
        var audioDeviceNode = new TreeViewNode();
        audioDeviceNode.Content = $"Audio Device {AudioDeviceNb}";
        audioDeviceNode.IsExpanded = true;
        foreach (var node in audioDeviceNodeList) audioDeviceNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(audioDeviceNode);

        // first parent child > child
        var gpuNodeList = new List<TreeViewNode>();
        foreach (var gpu in GpuList)
        {
            var node = new TreeViewNode();
            node.Content = gpu;
            gpuNodeList.Add(node);
        }

        // first parent child
        var gpuNode = new TreeViewNode();
        gpuNode.Content = $"GPU {GpuNb}";
        gpuNode.IsExpanded = true;
        foreach (var node in gpuNodeList) gpuNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(gpuNode);

        // first parent child > child
        var diskDriveNodeList = new List<TreeViewNode>();
        foreach (var disk in DiskList)
        {
            var node = new TreeViewNode();
            node.Content = disk;
            diskDriveNodeList.Add(node);
        }

        // first parent child
        var diskDriveNode = new TreeViewNode();
        diskDriveNode.Content = $"Disk Drive {DiskNb}";
        diskDriveNode.IsExpanded = true;
        foreach (var node in diskDriveNodeList) diskDriveNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(diskDriveNode);

        // first parent child > child
        var memoryNodeList = new List<TreeViewNode>();
        foreach (var memory in MemoryList)
        {
            var node = new TreeViewNode();
            node.Content = memory;
            memoryNodeList.Add(node);
        }

        // first parent child
        var memoryNode = new TreeViewNode();
        memoryNode.Content = $"Memory {MemoryNb}";
        memoryNode.IsExpanded = true;
        foreach (var node in memoryNodeList) memoryNode.ChildNodes.Add(node);
        basicHardwareNodeList.Add(memoryNode);

        // first parent child > child
        var chassisTypeNode = new TreeViewNode();
        chassisTypeNode.Content = $"Chassis Types : {ChassisTypes}";

        // first parent child > child
        var modelCmNode = new TreeViewNode();
        modelCmNode.Content = $"Model : {MotherBoardModel}";

        // first parent child > child
        var biosNameNode = new TreeViewNode();
        biosNameNode.Content = $"BIOS : {BiosName}";

        // first parent child > child
        var uuidNode = new TreeViewNode();
        uuidNode.Content = $"UUID : {Uuid}";

        // first parent child
        var motherboardNode = new TreeViewNode();
        motherboardNode.Content = "Motherboard";
        motherboardNode.IsExpanded = true;
        motherboardNode.ChildNodes.Add(chassisTypeNode);
        motherboardNode.ChildNodes.Add(modelCmNode);
        motherboardNode.ChildNodes.Add(biosNameNode);
        motherboardNode.ChildNodes.Add(uuidNode);
        basicHardwareNodeList.Add(motherboardNode);

        // first parent child > child
        var cpuNameNode = new TreeViewNode();
        cpuNameNode.Content = $"Name : {CpuName}";

        // first parent child > child
        var cpuManufacturerNode = new TreeViewNode();
        cpuManufacturerNode.Content = $"Manufacturer : {CpuManufacturer}";

        // first parent child > child
        var cpuArchitectureNode = new TreeViewNode();
        cpuArchitectureNode.Content = $"Architecture : {CpuArchitecture}";

        // first parent child
        var cpuNode = new TreeViewNode();
        cpuNode.Content = "CPU";
        cpuNode.IsExpanded = true;
        cpuNode.ChildNodes.Add(cpuNameNode);
        cpuNode.ChildNodes.Add(cpuManufacturerNode);
        cpuNode.ChildNodes.Add(cpuArchitectureNode);
        basicHardwareNodeList.Add(cpuNode);


        // first parent Node
        var basicHardwareNode = new TreeViewNode();
        basicHardwareNode.Content = "Basic Hardware Information";
        basicHardwareNode.IsExpanded = true;
        basicHardwareNodeList.Reverse();
        foreach (var treeViewNode in basicHardwareNodeList) basicHardwareNode.ChildNodes.Add(treeViewNode);


        // second parent child
        var cpuScoreNode = new TreeViewNode();
        cpuScoreNode.Content = $"CPU Score : {CpuScore}";

        // second parent child
        var graphicsScoreNode = new TreeViewNode();
        graphicsScoreNode.Content = $"Graphics Score : {GraphicsScore}";

        // second parent child
        var graphics3dScoreNode = new TreeViewNode();
        graphics3dScoreNode.Content = $"3D Graphics Score : {Graphics3dScore}";

        // second parent child
        var primaryDiskScoreNode = new TreeViewNode();
        primaryDiskScoreNode.Content = $"Primary Disk Score : {PrimaryDiskScore}";

        // second parent child
        var memoryScoreNode = new TreeViewNode();
        memoryScoreNode.Content = $"Memory Score : {MemoryScore}";

        // second parent Node
        var windowsPerfNode = new TreeViewNode();
        windowsPerfNode.Content = "Windows Performance score";
        windowsPerfNode.IsExpanded = true;
        windowsPerfNode.ChildNodes.Add(cpuScoreNode);
        windowsPerfNode.ChildNodes.Add(graphicsScoreNode);
        windowsPerfNode.ChildNodes.Add(graphics3dScoreNode);
        windowsPerfNode.ChildNodes.Add(primaryDiskScoreNode);
        windowsPerfNode.ChildNodes.Add(memoryScoreNode);

        // Main treeView Nodes Collection
        var treeViewNodes = new TreeViewNodeCollection();
        treeViewNodes.Add(basicHardwareNode);
        treeViewNodes.Add(windowsPerfNode);

        // Main treeView
        MainTreeView.Nodes = treeViewNodes;
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
            default:
                Log.Error("Error Expand/Collapse All");
                break;
        }
    }

    private void ExportData_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Try to export data to a html file
        Log.Info("try to export data");
        Task.Run(() =>
        {
            try
            {
                // Get list of nodes
                var memoryString =
                    MemoryList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var diskDriveString =
                    DiskList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var gpuString = GpuList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var audioDeviceString =
                    AudioDeviceList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var networkAdapterString =
                    NetworkAdapterList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var keyboardString =
                    KeyboardList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");
                var mouseString = MouseList.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");

                // html Template
                var htmlString =
                    "<!DOCTYPE html><head> <title>Data Export - Windows Admin Center</title></head><body " +
                    "style=\"font-family: monospace, sans-serif\"> " +
                    "<h1 style=\"text-align: center; margin: 50px 0;\"> Windows Admin Center - System Information" +
                    " (Hardware) </h1> <ul> <li> <h2>Basic Hardware Information</h2> <ul> <li> <h3>CPU</h3> <ul> " +
                    $"<li> <h4>Name : {CpuName}</h4> </li> <li> <h4>Manufacturer : {CpuManufacturer}</h4> </li> <li> " +
                    $"<h4>Architecture : {CpuArchitecture}</h4> </li> </ul> </li> <li> <h3>Motherboard</h3> <ul> <li>" +
                    $" <h4>Chassis Types : {ChassisTypes}</h4> </li> <li> <h4>Model : {MotherBoardModel}</h4> </li> " +
                    $"<li> <h4>BIOS : {BiosName}</h4> </li> <li> <h4>UUID : {Uuid}</h4> </li> </ul> </li> <li> <h3>" +
                    $"Memory {MemoryNb}</h3> <ul> {memoryString} </ul> </li> <li> <h3>Disk Drive {DiskNb}</h3> <ul> " +
                    $"{diskDriveString} </ul> </li> <li> <h3>GPU {GpuNb}</h3> <ul> {gpuString} </ul> </li> <li> <h3>" +
                    $"Audio Device {AudioDeviceNb}</h3> <ul> {audioDeviceString} </ul> </li> <li> <h3>Network Adapter" +
                    $" {NetworkAdapterNb}</h3> <ul> {networkAdapterString} </ul> </li> <li> <h3>Keyboard {KeyboardNb}" +
                    $"</h3> <ul> {keyboardString} </ul> </li> <li> <h3>Mouse {MouseNb}</h3> <ul> {mouseString} </ul> " +
                    $"</li> </ul> </li> <li> <h2>Windows Performance Score</h2> <ul> <li> <h3>CPU Score : {CpuScore}" +
                    $"</h3> </li> <li> <h3>Graphics Score : {GraphicsScore}</h3> </li> <li> <h3>3D Graphics Score : " +
                    $"{Graphics3dScore}</h3> </li> <li> <h3>Primary Disk Score : {PrimaryDiskScore}</h3> </li> <li>" +
                    $" <h3>Memory Score : {MemoryScore}</h3> </li> </ul> </li> </ul></body></html>";

                // Create a file and ask user to save it
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML File (*.html)|*.html",
                    Title = "Save HTML File",
                    FileName = "System Information(Hardware)_" + DateTime.Now.ToString("yyyy-MM-dd"),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
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

    // On Key Down in treeView
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

    private string ChassisTypes { get; set; } = "";
    private string MotherBoardModel { get; set; } = "";
    private string BiosName { get; set; } = "";
    private string Uuid { get; set; } = "";
    private string CpuName { get; set; } = "";
    private string CpuManufacturer { get; set; } = "";
    private string CpuArchitecture { get; set; } = "";
    private string MemoryNb { get; set; } = "";
    private List<string> MemoryList { get; set; }
    private string DiskNb { get; set; } = "";
    private List<string> DiskList { get; set; }
    private string GpuNb { get; set; } = "";
    private List<string> GpuList { get; set; }
    private string AudioDeviceNb { get; set; } = "";
    private List<string> AudioDeviceList { get; set; }
    private string NetworkAdapterNb { get; set; } = "";
    private List<string> NetworkAdapterList { get; set; }
    private string KeyboardNb { get; set; } = "";
    private List<string> KeyboardList { get; set; }
    private string MouseNb { get; set; } = "";
    private List<string> MouseList { get; set; }
    private string CpuScore { get; set; } = "";
    private string GraphicsScore { get; set; } = "";
    private string Graphics3dScore { get; set; } = "";
    private string PrimaryDiskScore { get; set; } = "";
    private string MemoryScore { get; set; } = "";

    #endregion
}