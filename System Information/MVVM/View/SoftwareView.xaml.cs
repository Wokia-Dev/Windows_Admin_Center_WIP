using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
///     Logique d'interaction pour SoftwareView.xaml
/// </summary>
public partial class SoftwareView
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    public SoftwareView()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("MaterialLight"));

        var wmiQueryManager = new WmIqueryManagement();

        var wmiTasks = new List<Task>
        {
            // Get Computer System
            Task.Run(() =>
            {
                Log.Info("Get Computer System Info");
                try
                {
                    // Query return
                    var returnValue =
                        wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "Manufacturer", "Model" });

                    // Set final string of Computer System
                    ComputerSystem = (string)returnValue.PropertiesResultList[0, 0] + " " +
                                     (string)returnValue.PropertiesResultList[0, 1];

                    Log.Info("Successfully get Computer System Info");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Computer System Info", e);
                    ComputerSystem = "N/A";
                }
            }),

            // Get System Type
            Task.Run(() =>
            {
                Log.Info("Get System Type with WMI");
                try
                {
                    // Query return
                    var returnValue =
                        wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "SystemType", "PCSystemType" });

                    // Get Properties Values
                    var result = (ushort)returnValue.PropertiesResultList[0, 1];

                    // Get PC System Type
                    var pcSystemType = result switch
                    {
                        0 => "Unspecified",
                        1 => "Desktop",
                        2 => "Mobile",
                        3 => "Workstation",
                        4 => "Enterprise Server",
                        5 => "Small Office and Home Office",
                        6 => "Appliance PC",
                        7 => "Performance Server",
                        8 => "Maximum",
                        _ => "N/A"
                    };

                    // Set final string of System Type
                    SystemType = (string)returnValue.PropertiesResultList[0, 0] + " (" + pcSystemType + ")";

                    Log.Info("Successfully get System Type");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get System Type", e);
                    SystemType = "N/A";
                }
            }),

            // Get System Name
            Task.Run(() =>
            {
                Log.Info("Get System Name with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "Caption" });

                    // Set final string of System Name
                    SystemName = (string)returnValue.PropertiesResultList[0, 0];

                    Log.Info("Successfully get System Name");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get System Name", e);
                    SystemName = "N/A";
                }
            }),

            // Get Current User
            Task.Run(() =>
            {
                Log.Info("Get Current User with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "UserName" });

                    // Set final string of Current User
                    CurrentUser = returnValue.PropertiesResultList[0, 0].ToString()?.Split('\\')[1] ?? "N/A";

                    Log.Info("Successfully get Current User");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Current User", e);
                    CurrentUser = "N/A";
                }
            }),

            // Get OS Name
            Task.Run(() =>
            {
                Log.Info("Get OS Name with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "Caption" });

                    // Set final string of OS Name
                    OsName = (string)returnValue.PropertiesResultList[0, 0];

                    Log.Info("Successfully get OS Name");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get OS Name", e);
                    OsName = "N/A";
                }
            }),

            // Get OS Version
            Task.Run(() =>
            {
                Log.Info("Get OS Version with WMI");
                try
                {
                    // Query return
                    var returnValue =
                        wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "Version", "BuildNumber" });

                    // Set final string of OS Version
                    OsVersion = (string)returnValue.PropertiesResultList[0, 0] + " (build " +
                                (string)returnValue.PropertiesResultList[0, 1] + ")";

                    Log.Info("Successfully get OS Version");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get OS Version", e);
                    OsVersion = "N/A";
                }
            }),

            // Get OS Install Date
            Task.Run(() =>
            {
                Log.Info("Get OS Install Date with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "InstallDate" });

                    var date = (string)returnValue.PropertiesResultList[0, 0];

                    // Set final string of OS Install Date (date format : yyyy-MM-dd-HH-mm-ss.000000+000)
                    OsInstallDate = date[..4] + "-" + date.Substring(4, 2) +
                                    "-" + date.Substring(6, 2) + " " + date.Substring(8, 2) +
                                    ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);

                    Log.Info("Successfully get OS Install Date");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get OS Install Date", e);
                    OsInstallDate = "N/A";
                }
            }),

            // Get Time Zone
            Task.Run(() =>
            {
                Log.Info("Get Time Zone with WMI");
                try
                {
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_TimeZone", new[] { "Caption" });

                    // Set final string of Time Zone
                    TimeZone = (string)returnValue.PropertiesResultList[0, 0];

                    Log.Info("Successfully get Time Zone");
                }
                catch (Exception e)
                {
                    Log.Error("Failed to get Time Zone", e);
                    TimeZone = "N/A";
                }
            })
        };

        // Wait for all tasks to be completed
        Task.WaitAll(wmiTasks.ToArray());


        // add Computer System Node to TreeView
        var computerSystemNode = new TreeViewNode
        {
            Content = $"Computer System : {ComputerSystem}"
        };

        // add System Type Node to TreeView
        var systemTypeNode = new TreeViewNode
        {
            Content = $"System Type : {SystemType}"
        };

        // add System Name Node to TreeView
        var systemNameNode = new TreeViewNode
        {
            Content = $"System Name : {SystemName}"
        };

        // add Current User Node to TreeView
        var currentUserNode = new TreeViewNode
        {
            Content = $"Current User : {CurrentUser}"
        };

        // add OS Name Node to TreeView
        var osNameNode = new TreeViewNode
        {
            Content = $"OS Name : {OsName}"
        };

        // add OS Version Node to TreeView
        var osVersionNode = new TreeViewNode
        {
            Content = $"OS Version : {OsVersion}"
        };

        // add OS Install Date Node to TreeView
        var osInstallDateNode = new TreeViewNode
        {
            Content = $"OS Install Date : {OsInstallDate}"
        };

        // add Time Zone Node to TreeView
        var timeZoneNode = new TreeViewNode
        {
            Content = $"Time Zone : {TimeZone}"
        };


        // Main TreeView Node Collection
        var treeViewNodes = new TreeViewNodeCollection();
        treeViewNodes.Add(computerSystemNode);
        treeViewNodes.Add(systemTypeNode);
        treeViewNodes.Add(systemNameNode);
        treeViewNodes.Add(currentUserNode);
        treeViewNodes.Add(osNameNode);
        treeViewNodes.Add(osVersionNode);
        treeViewNodes.Add(osInstallDateNode);
        treeViewNodes.Add(timeZoneNode);

        // Main TreeView
        MainTreeView.Nodes = treeViewNodes;
    }

    private void ExpandAll_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Expand all the nodes in the TreeView and change the text of the button.
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
        // try to export the data to a html file
        Log.Info("Try to export data");
        Task.Run(() =>
        {
            try
            {
                // html template
                var htmlTemplate =
                    "<!DOCTYPE html><head> <title>Data Export - Windows Admin Center</title></head><body " +
                    "style=\"font-family: monospace, sans-serif\"><h1 style=\"text-align: center; margin: 50px" +
                    " 0;\"> Windows Admin Center - System Information (Software) </h1> <ul> <li> <h2>Computer " +
                    $"System : {ComputerSystem}</h2> </li> <li> <h2>System Type : {SystemType}</h2> </li> <li> " +
                    $"<h2>System Name : {SystemName}</h2> </li> <li> <h2>Current " +
                    $"User : {CurrentUser}</h2> </li> <li> <h2>OS Name : {OsName}</h2> </li> <li> <h2>OS Version : " +
                    $"{OsVersion}</h2> </li> <li> <h2>OS Install Date : {OsInstallDate}</h2> </li> <li> <h2>Time Zone" +
                    $" : {TimeZone}</h2> </li> </ul></body></html>";

                // Create a file and ask the user to save it
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML File (*.html)|*.html",
                    Title = "Save HTML File",
                    FileName = "System Information(Software)_ " + DateTime.Now.ToString("yyyy-MM-dd"),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, htmlTemplate);
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                        {
                            UseShellExecute = true
                        }
                    }.Start();
                    Log.Info("html data exported : " + saveFileDialog.FileName);
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
        if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
        {
            var selectedItem = (TreeViewNode)MainTreeView.SelectedItem;
            Clipboard.SetText(selectedItem.Content.ToString() ?? string.Empty);
            Log.Info("Copied to clipboard : " + selectedItem.Content);
        }
    }

    #region Values

    private string ComputerSystem { get; set; } = "";
    private string SystemType { get; set; } = "";
    private string SystemName { get; set; } = "";
    private string? CurrentUser { get; set; } = "";
    private string OsName { get; set; } = "";
    private string OsVersion { get; set; } = "";
    private string OsInstallDate { get; set; } = "";
    private string TimeZone { get; set; } = "";

    #endregion
}