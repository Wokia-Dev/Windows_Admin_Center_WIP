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

namespace System_Information.MVVM.View
{
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
                    // Query return
                    var returnValue =
                        wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "Manufacturer", "Model" });

                    // Set final string of Computer System
                    ComputerSystem = (string)returnValue.PropertiesResultList[0, 0] + " " +
                                     (string)returnValue.PropertiesResultList[0, 1];
                }),

                // Get System Type
                Task.Run(() =>
                {
                    Log.Info("Get System Type with WMI");
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
                        _ => "Unknown"
                    };

                    // Set final string of System Type
                    SystemType = (string)returnValue.PropertiesResultList[0, 0] + " (" + pcSystemType + ")";
                }),

                // Get System Name
                Task.Run(() =>
                {
                    Log.Info("Get System Name with WMI");
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "Caption" });

                    // Set final string of System Name
                    SystemName = (string)returnValue.PropertiesResultList[0, 0];
                }),

                // Get Current User
                Task.Run(() =>
                {
                    Log.Info("Get Current User with WMI");
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_ComputerSystem", new[] { "UserName" });

                    // Set final string of Current User
                    CurrentUser = returnValue.PropertiesResultList[0, 0].ToString()?.Split('\\')[1];
                }),

                // Get OS Name
                Task.Run(() =>
                {
                    Log.Info("Get OS Name with WMI");
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "Caption" });

                    // Set final string of OS Name
                    OsName = (string)returnValue.PropertiesResultList[0, 0];
                }),

                // Get OS Version
                Task.Run(() =>
                {
                    Log.Info("Get OS Version with WMI");
                    // Query return
                    var returnValue =
                        wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "Version", "BuildNumber" });

                    // Set final string of OS Version
                    OsVersion = (string)returnValue.PropertiesResultList[0, 0] + " (build " +
                                (string)returnValue.PropertiesResultList[0, 1] + ")";
                }),

                // Get OS Install Date
                Task.Run(() =>
                {
                    Log.Info("Get OS Install Date with WMI");
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_OperatingSystem", new[] { "InstallDate" });

                    var date = (string)returnValue.PropertiesResultList[0, 0];

                    // Set final string of OS Install Date (date format : yyyy-MM-dd-HH-mm-ss.000000+000)
                    OsInstallDate = date.Substring(0, 4) + "-" + date.Substring(4, 2) +
                                    "-" + date.Substring(6, 2) + " " + date.Substring(8, 2) +
                                    ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
                }),

                // Get Time Zone
                Task.Run(() =>
                {
                    Log.Info("Get Time Zone with WMI");
                    // Query return
                    var returnValue = wmiQueryManager.WmIquery("Win32_TimeZone", new[] { "Caption" });

                    // Set final string of Time Zone
                    TimeZone = (string)returnValue.PropertiesResultList[0, 0];
                })
            };

            // Wait for all tasks to be completed
            Task.WaitAll(wmiTasks.ToArray());


            // add Computer System Node to TreeView
            var computerSystemNode = new TreeViewNode();
            computerSystemNode.Content = $"Computer System : {ComputerSystem}";

            // add System Type Node to TreeView
            var systemTypeNode = new TreeViewNode();
            systemTypeNode.Content = $"System Type : {SystemType}";

            // add System Name Node to TreeView
            var systemNameNode = new TreeViewNode();
            systemNameNode.Content = $"System Name : {SystemName}";

            // add Current User Node to TreeView
            var currentUserNode = new TreeViewNode();
            currentUserNode.Content = $"Current User : {CurrentUser}";

            // add OS Name Node to TreeView
            var osNameNode = new TreeViewNode();
            osNameNode.Content = $"OS Name : {OsName}";

            // add OS Version Node to TreeView
            var osVersionNode = new TreeViewNode();
            osVersionNode.Content = $"OS Version : {OsVersion}";

            // add OS Install Date Node to TreeView
            var osInstallDateNode = new TreeViewNode();
            osInstallDateNode.Content = $"OS Install Date : {OsInstallDate}";

            // add Time Zone Node to TreeView
            var timeZoneNode = new TreeViewNode();
            timeZoneNode.Content = $"Time Zone : {TimeZone}";


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
                        var process = new Process();
                        process.StartInfo = new ProcessStartInfo(saveFileDialog.FileName)
                        {
                            UseShellExecute = true
                        };
                        process.Start();
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
}