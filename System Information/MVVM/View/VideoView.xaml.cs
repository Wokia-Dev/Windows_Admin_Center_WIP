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
                Log.Info("Get Gpu info with WMI");
                try
                {
                        // Query return
                        var returnValueNb = wmiQueryManager.WmIquery("Win32_VideoController",
                        new[] { "Caption" });
                    var nbGpu = returnValueNb.NbResult;

                    var wmiTasks = new List<Task>
                    {
                            Task.Run(() =>
                            {
                                CaptionList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) CaptionList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "Caption" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (CaptionList != null)
                                            CaptionList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Caption");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Caption", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                CompanyList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) CompanyList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "AdapterCompatibility" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (CompanyList != null)
                                            CompanyList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Company");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Company", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                NameList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) NameList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "Name" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (NameList != null)
                                            NameList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Name");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Name", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                VideoProcessorList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) VideoProcessorList?.Add("N/A");

                                try
                                {
                                    
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "VideoProcessor" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (VideoProcessorList != null)
                                            VideoProcessorList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Video Processor");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Video Processor", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                VideoModeDescriptionList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) VideoModeDescriptionList?.Add("N/A");

                                try
                                {
                                    
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "VideoModeDescription" });
                                
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (VideoModeDescriptionList != null)
                                            VideoModeDescriptionList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Video Mode Description");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Video Mode Description", e);
                                }

                            }),

                            Task.Run(() =>
                            {
                                MemorySizeList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) MemorySizeList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "AdapterRAM" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (MemorySizeList != null)
                                            //var memorySizeInByte = converter.
                                            MemorySizeList[i] = ((uint)returnValue.PropertiesResultList[i, 0]).ToString();
                                    }
                                    Log.Info("Successful get Gpu Memory Size");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Memory Size", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                VideoArchitectureList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) VideoArchitectureList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "VideoArchitecture" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (VideoArchitectureList != null)
                                            VideoArchitectureList[i] = (ushort)returnValue.PropertiesResultList[i, 0] switch
                                            {
                                                1 => "Other",
                                                2 => "Unknown",
                                                3 => "CGA",
                                                4 => "EGA",
                                                5 => "VGA",
                                                6 => "SVGA",
                                                7 => "MDA",
                                                8 => "HGC",
                                                9 => "MCGA",
                                                10 => "8514A",
                                                11 => "XGA",
                                                12 => "Linear Frame Buffer",
                                                160 => "PC-98",
                                                _ => "N/A"

                                            };
                                    }
                                    Log.Info("Successful get Gpu Video Architecture");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Video Architecture", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                VideoMemoryTypeList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) VideoMemoryTypeList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "VideoMemoryType" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (VideoMemoryTypeList != null)
                                            VideoMemoryTypeList[i] = (ushort)returnValue.PropertiesResultList[i, 0] switch
                                            {
                                                1 => "Other",
                                                2 => "Unknown",
                                                3 => "VRAM",
                                                4 => "DRAM",
                                                5 => "SRAM",
                                                6 => "WRAM",
                                                7 => "EDO RAM",
                                                8 => "Burst Synchronous DRAM",
                                                9 => "Pipelined Burst SRAM",
                                                10 => "CDRAM",
                                                11 => "3DRAM",
                                                12 => "SDRAM",
                                                13 => "SGRAM",
                                                _ => "N/A"
                                            };
                                    }

                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Video Memory Type", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                CurrentResolutionList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) CurrentResolutionList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "CurrentHorizontalResolution", "CurrentVerticalResolution" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (CurrentResolutionList != null)
                                            CurrentResolutionList[i] = $"{returnValue.PropertiesResultList[i, 0]}x{returnValue.PropertiesResultList[i, 1]}";
                                    }
                                    Log.Info("Successful get Gpu Current Resolution");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Current Resolution", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                BitsPerPixelList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) BitsPerPixelList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "CurrentBitsPerPixel" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (BitsPerPixelList != null)
                                            BitsPerPixelList[i] = ((uint)returnValue.PropertiesResultList[i, 0]).ToString();
                                    }
                                    Log.Info("Successful get Gpu Bits Per Pixel");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Bits Per Pixel", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                CurrentNumberOfColorsList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) CurrentNumberOfColorsList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "CurrentNumberOfColors" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (CurrentNumberOfColorsList != null)
                                            CurrentNumberOfColorsList[i] = ((ulong)returnValue.PropertiesResultList[i, 0]).ToString();
                                    }
                                    Log.Info("Successful get Gpu Current Number Of Colors");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Current Number Of Colors", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                AdapterDacTypeList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) AdapterDacTypeList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "AdapterDACType" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (AdapterDacTypeList != null)
                                            AdapterDacTypeList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Adapter Dac Type");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Adapter Dac Type", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                InfFileNameList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) InfFileNameList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "InfFilename" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (InfFileNameList != null)
                                            InfFileNameList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Inf File Name");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Inf File Name", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                InfSectionList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) InfSectionList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "InfSection" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (InfSectionList != null)
                                            InfSectionList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Inf Section");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Inf Section", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                MinMaxFreshRateList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) MinMaxFreshRateList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "MaxRefreshRate", "MinRefreshRate" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (MinMaxFreshRateList != null)
                                            MinMaxFreshRateList[i] = $"{returnValue.PropertiesResultList[i, 1]}Hz - {returnValue.PropertiesResultList[i, 0]}Hz";
                                    }
                                    Log.Info("Successful get Gpu Min Max Fresh Rate");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Min Max Fresh Rate", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                PnpDeviceIdList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) PnpDeviceIdList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "PNPDeviceID" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (PnpDeviceIdList != null)
                                            PnpDeviceIdList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Pnp Device Id");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Pnp Device Id", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                DriverDateList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) DriverDateList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "DriverDate" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (DriverDateList != null)
                                        {
                                            var date = (string)returnValue.PropertiesResultList[i, 0];
                                            DriverDateList[i] = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);
                                        }
                                    }
                                    Log.Info("Successful get Gpu Driver Date");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Driver Date", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                DriverVersionList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) DriverVersionList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "DriverVersion" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (DriverVersionList != null)
                                            DriverVersionList[i] = (string)returnValue.PropertiesResultList[i, 0];
                                    }
                                    Log.Info("Successful get Gpu Driver Version");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Driver Version", e);
                                }
                            }),

                            Task.Run(() =>
                            {
                                InstalledDisplayDriversList = new List<string>();
                                for (var i = 0; i < nbGpu; i++) InstalledDisplayDriversList?.Add("N/A");

                                try
                                {
                                    // Query return
                                    var returnValue = wmiQueryManager.WmIquery("Win32_VideoController",
                                        new[] { "InstalledDisplayDrivers" });
                                    
                                    // Add result to list
                                    for (var i = 0; i < nbGpu; i++)
                                    {
                                        if (InstalledDisplayDriversList != null)
                                            InstalledDisplayDriversList[i] = (string)returnValue.PropertiesResultList[i, 0];

                                    }
                                    Log.Info("Successful get Gpu Installed Display Drivers");
                                }
                                catch (Exception e)
                                {
                                    Log.Error("Failed to get Gpu Installed Display Drivers", e);
                                }
                            })
                    };
                    Task.WaitAll(wmiTasks.ToArray());

                        // Add all result to GpuList
                        for (var i = 0; i < nbGpu; i++)
                    {
                        GpuList.Add(new GpuObj(
                            caption: CaptionList?[i],
                            companyName: CompanyList?[i],
                            name: NameList?[i],
                            videoProcessor: VideoProcessorList?[i],
                            videoModeDescription: VideoModeDescriptionList?[i],
                            memorySize: MemorySizeList?[i],
                            videoArchitecture: VideoArchitectureList?[i],
                            videoMemoryType: VideoMemoryTypeList?[i],
                            currentResolution: CurrentResolutionList?[i],
                            bitsPerPixel: BitsPerPixelList?[i],
                            currentNumberOfColors: CurrentNumberOfColorsList?[i],
                            adapterDacType: AdapterDacTypeList?[i],
                            infFileName: InfFileNameList?[i],
                            infSection: InfSectionList?[i],
                            minMaxRefreshRate: MinMaxFreshRateList?[i],
                            pnpDeviceId: PnpDeviceIdList?[i],
                            driverDate: DriverDateList?[i],
                            driverVersion: DriverVersionList?[i],
                            installedDisplayDrivers: InstalledDisplayDriversList?[i]));
                    }
                }
                catch (Exception e)
                {
                    Log.Error("Error get Gpu info with WMI", e);
                }
            }).Wait();

            //////////////// TEMP
            // Trace WriteLine all GpuList
            foreach (var gpu in GpuList)
            {
                Debug.WriteLine("Caption : " + gpu.Caption);
                Debug.WriteLine("Company : " + gpu.CompanyName);
                Debug.WriteLine("Name : " + gpu.Name);
                Debug.WriteLine("VideoProcessor : " + gpu.VideoProcessor);
                Debug.WriteLine("VideoModeDescription : " + gpu.VideoModeDescription);
                Debug.WriteLine("MemorySize : " + gpu.MemorySize);
                Debug.WriteLine("VideoArchitecture : " + gpu.VideoArchitecture);
                Debug.WriteLine("VideoMemoryType : " + gpu.VideoMemoryType);
                Debug.WriteLine("CurrentResolution : " + gpu.CurrentResolution);
                Debug.WriteLine("BitsPerPixel : " + gpu.BitsPerPixel);
                Debug.WriteLine("CurrentNumberOfColors : " + gpu.CurrentNumberOfColors);
                Debug.WriteLine("AdapterDacType : " + gpu.AdapterDacType);
                Debug.WriteLine("InfFileName : " + gpu.InfFileName);
                Debug.WriteLine("InfSection : " + gpu.InfSection);
                Debug.WriteLine("MinMaxRefreshRate : " + gpu.MinMaxRefreshRate);
                Debug.WriteLine("PnpDeviceId : " + gpu.PnpDeviceId);
                Debug.WriteLine("DriverDate : " + gpu.DriverDate);
                Debug.WriteLine("DriverVersion : " + gpu.DriverVersion);
                Debug.WriteLine("InstalledDisplayDrivers : " + gpu.InstalledDisplayDrivers);
            }
            /////////////// 

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

        private List<string> CaptionList { get; set; }
        private List<string> CompanyList { get; set; }
        private List<string> NameList { get; set; }
        private List<string> VideoProcessorList { get; set; }
        private List<string> VideoModeDescriptionList { get; set; }
        private List<string> MemorySizeList { get; set; }
        private List<string> VideoArchitectureList { get; set; }
        private List<string> VideoMemoryTypeList { get; set; }
        private List<string> CurrentResolutionList { get; set; }
        private List<string> BitsPerPixelList { get; set; }
        private List<string> CurrentNumberOfColorsList { get; set; }
        private List<string> AdapterDacTypeList { get; set; }
        private List<string> InfFileNameList { get; set; }
        private List<string> InfSectionList { get; set; }
        private List<string> MinMaxFreshRateList { get; set; }
        private List<string> PnpDeviceIdList { get; set; }
        private List<string> DriverDateList { get; set; }
        private List<string> DriverVersionList { get; set; }
        private List<string> InstalledDisplayDriversList { get; set; }

        #endregion
    }
}
