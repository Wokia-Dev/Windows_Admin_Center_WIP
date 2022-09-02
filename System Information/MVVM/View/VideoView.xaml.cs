using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.Core;
using System_Information.Core.WmiObjects;

namespace System_Information.MVVM.View
{
    /// <summary>
    /// Logique d'interaction pour VideoView.xaml
    /// </summary>
    public partial class VideoView
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        public VideoView()
        {
            InitializeComponent();
            SfSkinManager.SetTheme(this, new Theme("MaterialLight"));
            
            var wmiQueryManager = new WmIqueryManagement();

            // Set list to new list
            GpuList = new List<GpuObj>();
            
           
                // Gpu Caption and Nb of GPU
                Task.Run(() =>
                {
                    Log.Info("Get Gpu Caption with WMI");
                    try
                    {
                        // Query return
                        var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                            new[] { "Caption", "AdapterCompatibility", "Name" });
                        Trace.WriteLine(returnValue.NbResult);

                        // Set final string to gpu list
                        for (var i = 0; i < returnValue.NbResult; i++)
                        {
                            var gpuObj = new GpuObj((string)returnValue.PropertiesResultList[i, 0],
                                (string)returnValue.PropertiesResultList[i, 1],
                                (string)returnValue.PropertiesResultList[i, 2]);

                            GpuList.Add(gpuObj);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error get Gpu Caption with WMI", e);
                        foreach (var gpuObj in GpuList)
                        {
                            gpuObj.Caption = "N/A";
                        }
                    }
                });


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
                Clipboard.SetText(selectedItem.Content.ToString() ?? string.Empty);
                Log.Info("Copied to clipboard : " + selectedItem.Content);
            }
        }

        #region Values

        private List<GpuObj> GpuList { get; set; }

        #endregion
    }
}
