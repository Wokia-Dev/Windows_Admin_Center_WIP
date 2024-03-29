using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;
using Microsoft.Win32;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.Core;

namespace System_Information.MVVM.View;

/// <summary>
///     Logique d'interaction pour BIOSView.xaml
/// </summary>
public partial class BiosView
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    public BiosView()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("MaterialLight"));

        var wmiQueryManager = new WmIqueryManagement();

        // Set all list to new list
        BiosCharacteristics = new List<string>();

        var wmiTasks = new List<Task>
        {
            // Get Bios Name
            Task.Run(() =>
            {
                Log.Info("Get Bios Name with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "Name" });

                    // Set final string of BIOS Name
                    BiosName = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Bios Name with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Name with WMI", e);
                    BiosName = "N/A";
                }
            }),

            // Get Bios Manufacturer
            Task.Run(() =>
            {
                Log.Info("Get Bios Manufacturer with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "Manufacturer" });

                    // Set final string of BIOS Manufacturer
                    BiosManufacturer = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Bios Manufacturer with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Manufacturer with WMI", e);
                    BiosManufacturer = "N/A";
                }
            }),

            // Get Primary Bios Bool
            Task.Run(() =>
            {
                Log.Info("Get Primary Bios Bool with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "PrimaryBIOS" });

                    // Set final string of Primary Bios
                    var result = (bool)returnValue.PropertiesResultList[0, 0];
                    BiosPrimaryBios = result switch
                    {
                        true => "Yes",
                        false => "No"
                    };
                    Log.Info("Successful get Primary Bios Bool with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Primary Bios Bool with WMI", e);
                    BiosPrimaryBios = "N/A";
                }
            }),

            // Get Bios Release Date
            Task.Run(() =>
            {
                Log.Info("Get Bios Release Date with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "ReleaseDate" });

                    var date = (string)returnValue.PropertiesResultList[0, 0];

                    // Set final string of BIOS Release Date (date format : yyyy-MM-dd)
                    BiosReleaseDate = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);
                    Log.Info("Successful get Bios Release Date with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Release Date with WMI", e);
                    BiosReleaseDate = "N/A";
                }
            }),

            // Get Current Language
            Task.Run(() =>
            {
                Log.Info("Get Current Language with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "CurrentLanguage" });

                    // Set final string of Current Language
                    BiosCurrentLanguage = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Current Language with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Current Language with WMI", e);
                    BiosCurrentLanguage = "N/A";
                }
            }),

            // Get Bios Serial Number
            Task.Run(() =>
            {
                Log.Info("Get Bios Serial Number with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "SerialNumber" });

                    // Set final string of BIOS Serial Number
                    BiosSerialNumber = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Bios Serial Number with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Serial Number with WMI", e);
                    BiosSerialNumber = "N/A";
                }
            }),

            // Get Bios Version
            Task.Run(() =>
            {
                Log.Info("Get Bios Version with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "Version" });

                    // Set final string of BIOS Version
                    BiosVersion = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Bios Version with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Version with WMI", e);
                    BiosVersion = "N/A";
                }
            }),

            // Get Bios Mode
            Task.Run(() =>
            {
                Log.Info("Get Bios Mode with WMI");
                try
                {
                    // Get disk partition where is the primary disk
                    var searcher = new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT Type FROM Win32_DiskPartition WHERE PrimaryPartition = True");

                    // check if the disk partition is a GPT partition
                    var isGpt = false;
                    foreach (var queryObj in searcher.Get())
                    {
                        var partitionType = queryObj["Type"].ToString();
                        if (partitionType != null) isGpt = partitionType.Contains("GPT");
                    }

                    // Set final string of BIOS Mode
                    BiosMode = isGpt switch
                    {
                        true => "UEFI",
                        false => "BIOS"
                    };
                    Log.Info("Successful get Bios Mode with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Mode with WMI", e);
                    BiosMode = "N/A";
                }
            }),

            // Get Software Element State
            Task.Run(() =>
            {
                Log.Info("Get Software Element State with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "SoftwareElementState" });

                    var result = (ushort)returnValue.PropertiesResultList[0, 0];

                    // Set final string of Software Element State
                    BiosSoftwareElementState = result switch
                    {
                        0 => "Deployable",
                        1 => "Installable",
                        2 => "Executable",
                        3 => "Running",
                        _ => "N/A"
                    };
                    Log.Info("Successful get Software Element State with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Software Element State with WMI", e);
                    BiosSoftwareElementState = "N/A";
                }
            }),

            // Get Bios Status
            Task.Run(() =>
            {
                Log.Info("Get Bios Status with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "Status" });

                    // Set final string of BIOS Status
                    BiosStatus = (string)returnValue.PropertiesResultList[0, 0];
                    Log.Info("Successful get Bios Status with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Status with WMI", e);
                    BiosStatus = "N/A";
                }
            }),

            // Get Bios Characteristics
            Task.Run(() =>
            {
                Log.Info("Get Bios Characteristics with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_BIOS", new[] { "BiosCharacteristics" });

                    var result = (ushort[])returnValue.PropertiesResultList[0, 0];

                    // Add all Bios Characteristics to list
                    foreach (var characteristic in result)
                        switch (characteristic)
                        {
                            case 0:
                                BiosCharacteristics.Add("Reserved");
                                break;
                            case 1:
                                BiosCharacteristics.Add("Reserved");
                                break;
                            case 2:
                                BiosCharacteristics.Add("Unknown");
                                break;
                            case 3:
                                BiosCharacteristics.Add("BIOS Characteristics Not Supported");
                                break;
                            case 4:
                                BiosCharacteristics.Add("ISA is supported");
                                break;
                            case 5:
                                BiosCharacteristics.Add("MCA is supported");
                                break;
                            case 6:
                                BiosCharacteristics.Add("EISA is supported");
                                break;
                            case 7:
                                BiosCharacteristics.Add("PCI is supported");
                                break;
                            case 8:
                                BiosCharacteristics.Add("PC Card (PCMCIA) is supported");
                                break;
                            case 9:
                                BiosCharacteristics.Add("Plug and Play is supported");
                                break;
                            case 10:
                                BiosCharacteristics.Add("APM is supported");
                                break;
                            case 11:
                                BiosCharacteristics.Add("BIOS is Upgradeable (Flash)");
                                break;
                            case 12:
                                BiosCharacteristics.Add("BIOS shadowing is allowed");
                                break;
                            case 13:
                                BiosCharacteristics.Add("VL-VESA is supported");
                                break;
                            case 14:
                                BiosCharacteristics.Add("ESCD support is available");
                                break;
                            case 15:
                                BiosCharacteristics.Add("Boot from CD is supported");
                                break;
                            case 16:
                                BiosCharacteristics.Add("Selectable Boot is supported");
                                break;
                            case 17:
                                BiosCharacteristics.Add("BIOS ROM is socketed");
                                break;
                            case 18:
                                BiosCharacteristics.Add("Boot From PC Card (PCMCIA) is supported");
                                break;
                            case 19:
                                BiosCharacteristics.Add("EDD (Enhanced Disk Drive) Specification is supported");
                                break;
                            case 20:
                                BiosCharacteristics.Add(
                                    "Int 13h - Japanese Floppy for NEC 9800 1.2mb (3.5\", 1k Bytes/Sector, 360 RPM) is supported");
                                break;
                            case 21:
                                BiosCharacteristics.Add(
                                    "Int 13h - Japanese Floppy for Toshiba 1.2mb (3.5\", 360 RPM) is supported");
                                break;
                            case 22:
                                BiosCharacteristics.Add("Int 13h - 5.25\" / 360 KB Floppy Services are supported");
                                break;
                            case 23:
                                BiosCharacteristics.Add("Int 13h - 5.25\" /1.2MB Floppy Services are supported");
                                break;
                            case 24:
                                BiosCharacteristics.Add("Int 13h - 3.5\" / 720 KB Floppy Services are supported");
                                break;
                            case 25:
                                BiosCharacteristics.Add("Int 13h - 3.5\" / 2.88 MB Floppy Services are supported");
                                break;
                            case 26:
                                BiosCharacteristics.Add("Int 5h, Print Screen Service is supported");
                                break;
                            case 27:
                                BiosCharacteristics.Add("Int 9h, 8042 Keyboard services are supported");
                                break;
                            case 28:
                                BiosCharacteristics.Add("Int 14h, Serial Services are supported");
                                break;
                            case 29:
                                BiosCharacteristics.Add("Int 17h, printer services are supported");
                                break;
                            case 30:
                                BiosCharacteristics.Add("Int 10h, CGA/Mono Video Services are supported");
                                break;
                            case 31:
                                BiosCharacteristics.Add("NEC PC-98");
                                break;
                            case 32:
                                BiosCharacteristics.Add("ACPI supported");
                                break;
                            case 33:
                                BiosCharacteristics.Add("USB Legacy is supported");
                                break;
                            case 34:
                                BiosCharacteristics.Add("AGP is supported");
                                break;
                            case 35:
                                BiosCharacteristics.Add("I2O boot is supported");
                                break;
                            case 36:
                                BiosCharacteristics.Add("LS-120 boot is supported");
                                break;
                            case 37:
                                BiosCharacteristics.Add("ATAPI ZIP Drive boot is supported");
                                break;
                            case 38:
                                BiosCharacteristics.Add("1394 boot is supported");
                                break;
                            case 39:
                                BiosCharacteristics.Add("Smart Battery supported");
                                break;
                            default:
                                Log.Warn("Unknown Bios Characteristic");
                                break;
                        }

                    Log.Info("Successful get Bios Characteristics with WMI");
                }
                catch (Exception e)
                {
                    Log.Error("Error get Bios Characteristics with WMI", e);
                }
            })
        };

        Task.WaitAll(wmiTasks.ToArray());

        // Add Manufacturer to main Node
        var manufacturerNode = new TreeViewNode
        {
            Content = $"Manufacturer : {BiosManufacturer}"
        };

        // Add Name to main Node
        var nameNode = new TreeViewNode
        {
            Content = $"Name : {BiosName}"
        };

        // Add PrimaryBIOS to main Node
        var primaryBiosNode = new TreeViewNode
        {
            Content = $"Primary BIOS : {BiosPrimaryBios}"
        };

        // Add ReleaseDate to main Node
        var releaseDateNode = new TreeViewNode
        {
            Content = $"Release Date : {BiosReleaseDate}"
        };

        // Add CurrentLanguage to main Node
        var currentLanguageNode = new TreeViewNode
        {
            Content = $"Current Language : {BiosCurrentLanguage}"
        };

        // Add SerialNumber to main Node
        var serialNumberNode = new TreeViewNode
        {
            Content = $"Serial Number : {BiosSerialNumber}"
        };

        // Add Version to main Node
        var versionNode = new TreeViewNode
        {
            Content = $"Version : {BiosVersion}"
        };

        // Add Mode to main Node
        var modeNode = new TreeViewNode
        {
            Content = $"Mode : {BiosMode}"
        };

        // Add SoftwareElementState to main Node
        var softwareElementStateNode = new TreeViewNode
        {
            Content = $"Software Element State : {BiosSoftwareElementState}"
        };

        // Add Status to main Node
        var statusNode = new TreeViewNode
        {
            Content = $"Status : {BiosStatus}"
        };

        // Add Characteristics to main Node
        var characteristicsNode = new TreeViewNode
        {
            Content = "Characteristics",
            IsExpanded = true
        };
        foreach (var characteristicNode in BiosCharacteristics.Select(characteristic => new TreeViewNode
        {
            Content = characteristic
        }))
        {
            characteristicsNode.ChildNodes.Add(characteristicNode);
        }

        // Main Node for Bios
        var mainNode = new TreeViewNode
        {
            Content = BiosName,
            IsExpanded = true
        };
        mainNode.ChildNodes.Add(manufacturerNode);
        mainNode.ChildNodes.Add(nameNode);
        mainNode.ChildNodes.Add(primaryBiosNode);
        mainNode.ChildNodes.Add(releaseDateNode);
        mainNode.ChildNodes.Add(currentLanguageNode);
        mainNode.ChildNodes.Add(serialNumberNode);
        mainNode.ChildNodes.Add(versionNode);
        mainNode.ChildNodes.Add(modeNode);
        mainNode.ChildNodes.Add(softwareElementStateNode);
        mainNode.ChildNodes.Add(statusNode);
        mainNode.ChildNodes.Add(characteristicsNode);

        // Main treeView Nodes Collection
        var treeViewNodes = new TreeViewNodeCollection();
        treeViewNodes.Add(mainNode);

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
        // Try to export data to html file
        Log.Info("try to export data");
        Task.Run(() =>
        {
            try
            {
                // Get list of nodes
                var characteristicsString =
                    BiosCharacteristics.Aggregate("", (current, str) => current + "<li><h4>" + str + "</h4></li>");

                // html template
                var htmlString =
                    "<!DOCTYPE html><head> <title>Data Export - Windows Admin Center</title></head><body style=" +
                    "\"font-family: monospace, sans-serif;\"> <h1 style=\"text-align: center; margin: 50px 0;\"> " +
                    $"Windows Admin Center - System Information (BIOS) </h1> <ul> <li> <h2>{BiosName}</h2> <ul> " +
                    $"<li> <h3>Manufacturer : {BiosManufacturer}</h3> </li> <li> <h3>Name :{BiosName}</h3> </li> " +
                    $"<li> <h3>Primary BIOS : {BiosPrimaryBios}</h3> </li> <li> <h3>Release Date : {BiosReleaseDate}" +
                    $"</h3> </li> <li> <h3>Current Language : {BiosCurrentLanguage}</h3> </li> <li> <h3>Serial Number" +
                    $" : {BiosSerialNumber}</h3> </li> <li> <h3>Version : {BiosVersion}</h3> </li> <li> <h3>BIOS Mode" +
                    $" : {BiosMode}</h3> </li> <li> <h3>Software Element State : {BiosSoftwareElementState}</h3> " +
                    $"</li> <li> <h3>Status : {BiosStatus}</h3> </li> <li> <h3>BIOS Characteristics :</h3> <ul> " +
                    $"{characteristicsString} </ul> </li> </ul> </li> </ul></body></html>";

                // Create a file and ask user to save it
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML File (*.html)|*.html",
                    Title = "Save HTML File",
                    FileName = "System Information(BIOS)_" + DateTime.Now.ToString("yyyy-MM-dd"),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, htmlString);
                    new Process
                    {
                        StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                        {
                            UseShellExecute = true
                        }
                    }.Start();
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

    private string BiosName { get; set; } = "";
    private string BiosManufacturer { get; set; } = "";
    private string BiosPrimaryBios { get; set; } = "";
    private string BiosReleaseDate { get; set; } = "";
    private string BiosCurrentLanguage { get; set; } = "";
    private string BiosSerialNumber { get; set; } = "";
    private string BiosVersion { get; set; } = "";
    private string BiosMode { get; set; } = "";
    private string BiosSoftwareElementState { get; set; } = "";
    private string BiosStatus { get; set; } = "";
    private List<string> BiosCharacteristics { get; }

    #endregion
}