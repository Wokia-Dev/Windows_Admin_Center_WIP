﻿using System;
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
using System_Information.Core.WmiObjects;

namespace System_Information.MVVM.View;

/// <summary>
///     Logique d'interaction pour MotherboardView.xaml
/// </summary>
public partial class MotherboardView
{
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

    public MotherboardView()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("MaterialLight"));

        var wmIqueryManager = new WmIqueryManagement();

        // Set all list to new list
        OnBoardDevicesList = new List<OnBoardDeviceObj>();
        IdeControllerList = new List<IdeControllerObj>();
        InterfaceList = new List<InterfaceObj>();
        SystemSlot = new List<string>();
        BusDeviceList = new List<BusObj>();
        UsbControllerList = new List<UsbControllerObj>();

        var wmiTasks = new List<Task>
        {
            // Get MbProductName
            Task.Run(() =>
            {
                Log.Info("Get Motherboard ProductName with WMI");
                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_BaseBoard", new[] { "Product" });

                // Set final string of MbProductName
                MbProductName = (string)returnValue.PropertiesResultList[0, 0];
            }),

            // Get MbManufacturer
            Task.Run(() =>
            {
                Log.Info("Get Motherboard Manufacturer with WMI");
                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_BaseBoard", new[] { "Manufacturer" });

                // Set final string of MbManufacturer
                MbManufacturer = (string)returnValue.PropertiesResultList[0, 0];
            }),

            // Get MbSerialNumber
            Task.Run(() =>
            {
                Log.Info("Get Motherboard SerialNumber with WMI");
                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_BaseBoard", new[] { "SerialNumber" });

                // Set final string of MbSerialNumber
                MbSerialNumber = (string)returnValue.PropertiesResultList[0, 0];
            }),

            // Get MbStatus
            Task.Run(() =>
            {
                Log.Info("Get Motherboard Status with WMI");
                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_BaseBoard", new[] { "Status" });

                // Set final string of MbStatus
                MbStatus = (string)returnValue.PropertiesResultList[0, 0];
            }),

            // Get MbBusType
            Task.Run(() =>
            {
                Log.Info("Get Motherboard BusType with WMI");
                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_MotherboardDevice",
                    new[] { "PrimaryBusType", "SecondaryBusType" });

                // Set final string of MbBusType
                MbBusType = (string)returnValue.PropertiesResultList[0, 0] + ", " +
                            (string)returnValue.PropertiesResultList[0, 1];
            }),

            // Get OnBoardDevices Info
            Task.Run(() =>
            {
                Log.Info("Get OnBoard Devices Info with WMI");
                // Query return
                var returnValue =
                    wmIqueryManager.WmIquery("Win32_OnBoardDevice", new[] { "Description", "DeviceType" });

                // Add each element of returnValue to OnBoardDevices list
                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    var deviceType = "N/A";
                    var description = "N/A";
                    if ((string)returnValue.PropertiesResultList[i, 0] != "")
                        description = (string)returnValue.PropertiesResultList[i, 0];

                    var deviceTypeResult = (ushort)returnValue.PropertiesResultList[i, 1];

                    // Set final string of DevicesType
                    switch (deviceTypeResult)
                    {
                        case 1:
                            deviceType = "Other";
                            break;
                        case 2:
                            deviceType = "Unknown";
                            break;
                        case 3:
                            deviceType = "Video";
                            break;
                        case 4:
                            deviceType = "SCSI Controller";
                            break;
                        case 5:
                            deviceType = "Ethernet";
                            break;
                        case 6:
                            deviceType = "Token Ring";
                            break;
                        case 7:
                            deviceType = "Sound";
                            break;
                    }

                    var onBoardDevice = new OnBoardDeviceObj(description, deviceType);

                    OnBoardDevicesList.Add(onBoardDevice);
                }
            }),

            // Get IdeController Info
            Task.Run(() =>
            {
                Log.Info("Get Ide Controller Info with WMI");

                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_IDEController",
                    new[] { "Caption", "Manufacturer", "ProtocolSupported", "Status" });

                // Add each element of returnValue to IdeControllerCaption list
                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    string protocolSupported;
                    var caption = (string)returnValue.PropertiesResultList[i, 0] != ""
                        ? (string)returnValue.PropertiesResultList[i, 0]
                        : "N/A";
                    var manufacturer = (string)returnValue.PropertiesResultList[i, 1] != ""
                        ? (string)returnValue.PropertiesResultList[i, 1]
                        : "N/A";
                    var status = (string)returnValue.PropertiesResultList[i, 3] != ""
                        ? (string)returnValue.PropertiesResultList[i, 3]
                        : "N/A";
                    var result = (ushort)returnValue.PropertiesResultList[i, 2];

                    switch (result)
                    {
                        case 1:
                            protocolSupported = "Other";
                            break;
                        case 2:
                            protocolSupported = "Unknown";
                            break;
                        case 3:
                            protocolSupported = "EISA";
                            break;
                        case 4:
                            protocolSupported = "ISA";
                            break;
                        case 5:
                            protocolSupported = "PCI";
                            break;
                        case 6:
                            protocolSupported = "ATA/ATAPI";
                            break;
                        case 7:
                            protocolSupported = "Flexible Diskette";
                            break;
                        case 8:
                            protocolSupported = "1496";
                            break;
                        case 9:
                            protocolSupported = "SCSI Parallel Interface";
                            break;
                        case 10:
                            protocolSupported = "SCSI Fibre Channel Protocol";
                            break;
                        case 11:
                            protocolSupported = "SCSI Serial Bus Protocol";
                            break;
                        case 12:
                            protocolSupported = "SCSI Serial Bus Protocol-2 (1394)";
                            break;
                        case 13:
                            protocolSupported = "SCSI Serial Storage Architecture";
                            break;
                        case 14:
                            protocolSupported = "VESA";
                            break;
                        case 15:
                            protocolSupported = "PCMCIA";
                            break;
                        case 16:
                            protocolSupported = "Universal Serial Bus";
                            break;
                        case 17:
                            protocolSupported = "Parallel Protocol";
                            break;
                        case 18:
                            protocolSupported = "ESCON";
                            break;
                        case 19:
                            protocolSupported = "Diagnostic";
                            break;
                        case 20:
                            protocolSupported = "I2C";
                            break;
                        case 21:
                            protocolSupported = "Power";
                            break;
                        case 22:
                            protocolSupported = "HIPPI";
                            break;
                        case 23:
                            protocolSupported = "MultiBus";
                            break;
                        case 24:
                            protocolSupported = "VME";
                            break;
                        case 25:
                            protocolSupported = "IPI";
                            break;
                        case 26:
                            protocolSupported = "IEEE-488";
                            break;
                        case 27:
                            protocolSupported = "RS232";
                            break;
                        case 28:
                            protocolSupported = "IEEE 802.3 10BASE5";
                            break;
                        case 29:
                            protocolSupported = "IEEE 802.3 10BASE2";
                            break;
                        case 30:
                            protocolSupported = "IEEE 802.3 1BASE5";
                            break;
                        case 31:
                            protocolSupported = "IEEE 802.3 10BROAD36";
                            break;
                        case 32:
                            protocolSupported = "IEEE 802.3 100BASEVG";
                            break;
                        case 33:
                            protocolSupported = "IEEE 802.5 Token-Ring";
                            break;
                        case 34:
                            protocolSupported = "ANSI X3T9.5 FDDI";
                            break;
                        case 35:
                            protocolSupported = "MCA";
                            break;
                        case 36:
                            protocolSupported = "ESDI";
                            break;
                        case 37:
                            protocolSupported = "IDE";
                            break;
                        case 38:
                            protocolSupported = "CMD";
                            break;
                        case 39:
                            protocolSupported = "ST506";
                            break;
                        case 40:
                            protocolSupported = "DSSI";
                            break;
                        case 41:
                            protocolSupported = "QIC2";
                            break;
                        case 42:
                            protocolSupported = "Enhanced ATA/IDE";
                            break;
                        case 43:
                            protocolSupported = "AGP";
                            break;
                        case 44:
                            protocolSupported = "TWIRP (two-way infrared)";
                            break;
                        case 45:
                            protocolSupported = "FIR (fast infrared)";
                            break;
                        case 46:
                            protocolSupported = "SIR (serial infrared)";
                            break;
                        case 47:
                            protocolSupported = "IrBus";
                            break;
                        default:
                            protocolSupported = "N/A";
                            break;
                    }

                    IdeControllerList.Add(new IdeControllerObj(
                        caption,
                        manufacturer,
                        protocolSupported,
                        status
                    ));
                }
            }),

            // Get Interfaces Info
            Task.Run(() =>
            {
                Log.Info("Get Interfaces Info with WMI");

                // Query return 
                // class docs link : https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-portconnector
                var returnValue = wmIqueryManager.WmIquery("Win32_PortConnector",
                    new[] { "ExternalReferenceDesignator", "ConnectorType", "PortType" });

                // Add each element of returnValue to InterfacesList list
                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    var connectorTypeList = new List<string>();
                    var connectorType = "";
                    string portType;
                    var connectorTypeResult = (ushort[])returnValue.PropertiesResultList[i, 1];
                    var portTypeResult = (ushort)returnValue.PropertiesResultList[i, 2];

                    foreach (var type in connectorTypeResult)
                        // switch the property "ConnectorType" from the WMI class "Win32_PortConnector"
                        switch (type)
                        {
                            case 0:
                                connectorTypeList.Add("Unknown");
                                break;
                            case 1:
                                connectorTypeList.Add("Other");
                                break;
                            case 2:
                                connectorTypeList.Add("Male");
                                break;
                            case 3:
                                connectorTypeList.Add("Female");
                                break;
                            case 4:
                                connectorTypeList.Add("Shielded ");
                                break;
                            case 5:
                                connectorTypeList.Add("Unshielded");
                                break;
                            case 6:
                                connectorTypeList.Add("SCSI (A) High-Density (50 pins)");
                                break;
                            case 7:
                                connectorTypeList.Add("SCSI (A) Low-Density (50 pins)");
                                break;
                            case 8:
                                connectorTypeList.Add("SCSI (P) High-Density (68 pins)");
                                break;
                            case 9:
                                connectorTypeList.Add("SCSI SCA-I (80 pins)");
                                break;
                            case 10:
                                connectorTypeList.Add("SCSI SCA-II (80 pins)");
                                break;
                            case 11:
                                connectorTypeList.Add("SCSI Fibre Channel (DB-9, Copper)");
                                break;
                            case 12:
                                connectorTypeList.Add("SCSI Fibre Channel (Fibre)");
                                break;
                            case 13:
                                connectorTypeList.Add("SCSI Fibre Channel SCA-II (40 pins)");
                                break;
                            case 14:
                                connectorTypeList.Add("SCSI Fibre Channel SCA-II (20 pins)");
                                break;
                            case 15:
                                connectorTypeList.Add("SCSI Fibre Channel BNC");
                                break;
                            case 16:
                                connectorTypeList.Add("ATA 3-1/2 Inch (40 pins)");
                                break;
                            case 17:
                                connectorTypeList.Add("ATA 2-1/2 Inch (44 pins)");
                                break;
                            case 18:
                                connectorTypeList.Add("ATA-2");
                                break;
                            case 19:
                                connectorTypeList.Add("ATA-3");
                                break;
                            case 20:
                                connectorTypeList.Add("ATA/66");
                                break;
                            case 21:
                                connectorTypeList.Add("DB-9");
                                break;
                            case 22:
                                connectorTypeList.Add("DB-15");
                                break;
                            case 23:
                                connectorTypeList.Add("DB-25");
                                break;
                            case 24:
                                connectorTypeList.Add("DB-36");
                                break;
                            case 25:
                                connectorTypeList.Add("RS-232C");
                                break;
                            case 26:
                                connectorTypeList.Add("RS-422");
                                break;
                            case 27:
                                connectorTypeList.Add("RS-423");
                                break;
                            case 28:
                                connectorTypeList.Add("RS-485");
                                break;
                            case 29:
                                connectorTypeList.Add("RS-449");
                                break;
                            case 30:
                                connectorTypeList.Add("V.35");
                                break;
                            case 31:
                                connectorTypeList.Add("X.21");
                                break;
                            case 32:
                                connectorTypeList.Add("IEEE-488");
                                break;
                            case 33:
                                connectorTypeList.Add("AUI");
                                break;
                            case 34:
                                connectorTypeList.Add("UTP Category 3");
                                break;
                            case 35:
                                connectorTypeList.Add("UTP Category 4");
                                break;
                            case 36:
                                connectorTypeList.Add("UTP Category 5");
                                break;
                            case 37:
                                connectorTypeList.Add("BNC");
                                break;
                            case 38:
                                connectorTypeList.Add("RJ11");
                                break;
                            case 39:
                                connectorTypeList.Add("RJ45");
                                break;
                            case 40:
                                connectorTypeList.Add("Fiber MIC");
                                break;
                            case 41:
                                connectorTypeList.Add("Apple AUI");
                                break;
                            case 42:
                                connectorTypeList.Add("Apple GeoPort");
                                break;
                            case 43:
                                connectorTypeList.Add("PCI");
                                break;
                            case 44:
                                connectorTypeList.Add("ISA");
                                break;
                            case 45:
                                connectorTypeList.Add("EISA");
                                break;
                            case 46:
                                connectorTypeList.Add("VESA");
                                break;
                            case 47:
                                connectorTypeList.Add("PCMCIA");
                                break;
                            case 48:
                                connectorTypeList.Add("PCMCIA Type I");
                                break;
                            case 49:
                                connectorTypeList.Add("PCMCIA Type II");
                                break;
                            case 50:
                                connectorTypeList.Add("PCMCIA Type III");
                                break;
                            case 51:
                                connectorTypeList.Add("Port ZV");
                                break;
                            case 52:
                                connectorTypeList.Add("Card Bus");
                                break;
                            case 53:
                                connectorTypeList.Add("USB");
                                break;
                            case 54:
                                connectorTypeList.Add("IEEE 1394");
                                break;
                            case 55:
                                connectorTypeList.Add("HIPPI");
                                break;
                            case 56:
                                connectorTypeList.Add("HSSDC (6 broches)");
                                break;
                            case 57:
                                connectorTypeList.Add("GBIC");
                                break;
                            case 58:
                                connectorTypeList.Add("DIN");
                                break;
                            case 59:
                                connectorTypeList.Add("Mini-DIN");
                                break;
                            case 60:
                                connectorTypeList.Add("Micro-DIN");
                                break;
                            case 61:
                                connectorTypeList.Add("PS/2");
                                break;
                            case 62:
                                connectorTypeList.Add("Infrared");
                                break;
                            case 63:
                                connectorTypeList.Add("HP-HIL");
                                break;
                            case 64:
                                connectorTypeList.Add("Access Bus");
                                break;
                            case 65:
                                connectorTypeList.Add("Nu Bus");
                                break;
                            case 66:
                                connectorTypeList.Add("Centronics");
                                break;
                            case 67:
                                connectorTypeList.Add("Mini-Centronics");
                                break;
                            case 68:
                                connectorTypeList.Add("Mini-Centronics Type-14");
                                break;
                            case 69:
                                connectorTypeList.Add("Mini-Centronics Type-20");
                                break;
                            case 70:
                                connectorTypeList.Add("Mini-Centronics Type-26");
                                break;
                            case 71:
                                connectorTypeList.Add("Bus Mouse");
                                break;
                            case 72:
                                connectorTypeList.Add("ADB");
                                break;
                            case 73:
                                connectorTypeList.Add("AGP");
                                break;
                            case 74:
                                connectorTypeList.Add("VME Bus");
                                break;
                            case 75:
                                connectorTypeList.Add("VME64");
                                break;
                            case 76:
                                connectorTypeList.Add("Proprietary");
                                break;
                            case 77:
                                connectorTypeList.Add("Proprietary Processor Card Slot");
                                break;
                            case 78:
                                connectorTypeList.Add("Proprietary Memory Card Slot");
                                break;
                            case 79:
                                connectorTypeList.Add("Proprietary I/O Riser Slot");
                                break;
                            case 80:
                                connectorTypeList.Add("PCI-66MHz");
                                break;
                            case 81:
                                connectorTypeList.Add("AGP 2x");
                                break;
                            case 82:
                                connectorTypeList.Add("AGP 4x");
                                break;
                            case 83:
                                connectorTypeList.Add("PC-98");
                                break;
                            case 84:
                                connectorTypeList.Add("PC-98 Hireso");
                                break;
                            case 85:
                                connectorTypeList.Add("PC-H98");
                                break;
                            case 86:
                                connectorTypeList.Add("PC-98 Note");
                                break;
                            case 87:
                                connectorTypeList.Add("PC-98 Full");
                                break;
                            case 88:
                                connectorTypeList.Add("Mini-Jack");
                                break;
                            case 89:
                                connectorTypeList.Add("On Board Floppy");
                                break;
                            case 90:
                                connectorTypeList.Add("9 Pin Dual Inline (pin 10 cut)");
                                break;
                            case 91:
                                connectorTypeList.Add("25 Pin Dual Inline (pin 26 cut)");
                                break;
                            case 92:
                                connectorTypeList.Add("50 Pin Dual Inline");
                                break;
                            case 93:
                                connectorTypeList.Add("68 Pin Dual Inline");
                                break;
                            case 94:
                                connectorTypeList.Add("On Board Sound Input from CD-ROM");
                                break;
                            case 95:
                                connectorTypeList.Add("68 Pin Dual Inline");
                                break;
                            case 96:
                                connectorTypeList.Add("On Board Sound Connector");
                                break;
                            case 97:
                                connectorTypeList.Add("Mini-Jack");
                                break;
                            case 98:
                                connectorTypeList.Add("PCI-X");
                                break;
                            case 99:
                                connectorTypeList.Add("Sbus IEEE 1396-1993 32 Bit");
                                break;
                            case 100:
                                connectorTypeList.Add("Sbus IEEE 1396-1993 64 Bit");
                                break;
                            case 101:
                                connectorTypeList.Add("MCA");
                                break;
                            case 102:
                                connectorTypeList.Add("GIO");
                                break;
                            case 103:
                                connectorTypeList.Add("XIO");
                                break;
                            case 104:
                                connectorTypeList.Add("HIO");
                                break;
                            case 105:
                                connectorTypeList.Add("NGIO");
                                break;
                            case 106:
                                connectorTypeList.Add("PMC");
                                break;
                            case 107:
                                connectorTypeList.Add("MTRJ");
                                break;
                            case 108:
                                connectorTypeList.Add("VF-45");
                                break;
                            case 109:
                                connectorTypeList.Add("Future I/O");
                                break;
                            case 110:
                                connectorTypeList.Add("SC");
                                break;
                            case 111:
                                connectorTypeList.Add("SG");
                                break;
                            case 112:
                                connectorTypeList.Add("Electrical");
                                break;
                            case 113:
                                connectorTypeList.Add("Optical");
                                break;
                            case 114:
                                connectorTypeList.Add("Ribbon");
                                break;
                            case 115:
                                connectorTypeList.Add("GLM");
                                break;
                            case 116:
                                connectorTypeList.Add("1x9");
                                break;
                            case 117:
                                connectorTypeList.Add("Mini SG");
                                break;
                            case 118:
                                connectorTypeList.Add("LC");
                                break;
                            case 119:
                                connectorTypeList.Add("HSSC");
                                break;
                            case 120:
                                connectorTypeList.Add("VHDCI Shielded (68 pins)");
                                break;
                            case 121:
                                connectorTypeList.Add("InfiniBand");
                                break;
                        }

                    // Get the connector type string
                    foreach (var str in connectorTypeList) connectorType += str + " ";

                    switch (portTypeResult)
                    {
                        case 0:
                            portType = "None";
                            break;
                        case 1:
                            portType = "Parallel Port XT/AT Compatible";
                            break;
                        case 2:
                            portType = "Parallel Port PS/2";
                            break;
                        case 3:
                            portType = "Parallel Port ECP";
                            break;
                        case 4:
                            portType = "Parallel Port EPP";
                            break;
                        case 5:
                            portType = "Parallel Port ECP/EPP";
                            break;
                        case 6:
                            portType = "Serial Port XT/AT Compatible";
                            break;
                        case 7:
                            portType = "Serial Port 16450 Compatible";
                            break;
                        case 8:
                            portType = "Serial Port 16550 Compatible";
                            break;
                        case 9:
                            portType = "Serial Port 16650A Compatible";
                            break;
                        case 10:
                            portType = "SCSI Port";
                            break;
                        case 11:
                            portType = "MIDI Port";
                            break;
                        case 12:
                            portType = "Joystick Port";
                            break;
                        case 13:
                            portType = "Keyboard Port";
                            break;
                        case 14:
                            portType = "Mouse Port";
                            break;
                        case 15:
                            portType = "SSA SCSI Port";
                            break;
                        case 16:
                            portType = "USB";
                            break;
                        case 17:
                            portType = "FireWire (IEEE 1394)";
                            break;
                        case 18:
                            portType = "PCMCIA Type II";
                            break;
                        case 19:
                            portType = "PCMCIA Type II";
                            break;
                        case 20:
                            portType = "PCMCIA Type III";
                            break;
                        case 21:
                            portType = "CardBus";
                            break;
                        case 22:
                            portType = "Access Bus Port";
                            break;
                        case 23:
                            portType = "SCSI II";
                            break;
                        case 24:
                            portType = "SCSI Wide";
                            break;
                        case 25:
                            portType = "PC-98";
                            break;
                        case 26:
                            portType = "PC-98-Hireso";
                            break;
                        case 27:
                            portType = "PC-H98";
                            break;
                        case 28:
                            portType = "Video Port";
                            break;
                        case 29:
                            portType = "Audio Port";
                            break;
                        case 30:
                            portType = "Modem Port";
                            break;
                        case 31:
                            portType = "Network Port";
                            break;
                        case 32:
                            portType = "8251 Compatible";
                            break;
                        case 33:
                            portType = "8251 FIFO Compatible";
                            break;
                        default:
                            portType = "N/A";
                            break;
                    }

                    InterfaceList.Add(new InterfaceObj(
                        (string)returnValue.PropertiesResultList[i, 0],
                        connectorType,
                        portType));
                }
            }),

            // Get System Slot Info
            Task.Run(() =>
            {
                Log.Info("Get System Slot Info");

                // Query return
                // class docs link : https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-systemslot
                var returnValue = wmIqueryManager.WmIquery("Win32_SystemSlot",
                    new[] { "SlotDesignation", "ConnectorType", "CurrentUsage" });

                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    var slotDesignation = (string)returnValue.PropertiesResultList[i, 0];
                    var connectorTypeList = new List<string?>();
                    string? connectorType = null;
                    string? currentUsage = null;

                    var connectorTypeResult = (ushort[])returnValue.PropertiesResultList[i, 1];
                    var currentUsageResult = (ushort)returnValue.PropertiesResultList[i, 2];

                    // Get Connector Type
                    foreach (var type in connectorTypeResult)
                        switch (type)
                        {
                            case 0:
                                connectorTypeList.Add("Unknown");
                                break;
                            case 1:
                                connectorTypeList.Add("Other");
                                break;
                            case 2:
                                connectorTypeList.Add("Male");
                                break;
                            case 3:
                                connectorTypeList.Add("Female");
                                break;
                            case 4:
                                connectorTypeList.Add("Shielded ");
                                break;
                            case 5:
                                connectorTypeList.Add("Unshielded");
                                break;
                            case 6:
                                connectorTypeList.Add("SCSI (A) High-Density (50 pins)");
                                break;
                            case 7:
                                connectorTypeList.Add("SCSI (A) Low-Density (50 pins)");
                                break;
                            case 8:
                                connectorTypeList.Add("SCSI (P) High-Density (68 pins)");
                                break;
                            case 9:
                                connectorTypeList.Add("SCSI SCA-I (80 pins)");
                                break;
                            case 10:
                                connectorTypeList.Add("SCSI SCA-II (80 pins)");
                                break;
                            case 11:
                                connectorTypeList.Add("SCSI Fibre Channel (DB-9, Copper)");
                                break;
                            case 12:
                                connectorTypeList.Add("SCSI Fibre Channel (Fibre)");
                                break;
                            case 13:
                                connectorTypeList.Add("SCSI Fibre Channel SCA-II (40 pins)");
                                break;
                            case 14:
                                connectorTypeList.Add("SCSI Fibre Channel SCA-II (20 pins)");
                                break;
                            case 15:
                                connectorTypeList.Add("SCSI Fibre Channel BNC");
                                break;
                            case 16:
                                connectorTypeList.Add("ATA 3-1/2 Inch (40 pins)");
                                break;
                            case 17:
                                connectorTypeList.Add("ATA 2-1/2 Inch (44 pins)");
                                break;
                            case 18:
                                connectorTypeList.Add("ATA-2");
                                break;
                            case 19:
                                connectorTypeList.Add("ATA-3");
                                break;
                            case 20:
                                connectorTypeList.Add("ATA/66");
                                break;
                            case 21:
                                connectorTypeList.Add("DB-9");
                                break;
                            case 22:
                                connectorTypeList.Add("DB-15");
                                break;
                            case 23:
                                connectorTypeList.Add("DB-25");
                                break;
                            case 24:
                                connectorTypeList.Add("DB-36");
                                break;
                            case 25:
                                connectorTypeList.Add("RS-232C");
                                break;
                            case 26:
                                connectorTypeList.Add("RS-422");
                                break;
                            case 27:
                                connectorTypeList.Add("RS-423");
                                break;
                            case 28:
                                connectorTypeList.Add("RS-485");
                                break;
                            case 29:
                                connectorTypeList.Add("RS-449");
                                break;
                            case 30:
                                connectorTypeList.Add("V.35");
                                break;
                            case 31:
                                connectorTypeList.Add("X.21");
                                break;
                            case 32:
                                connectorTypeList.Add("IEEE-488");
                                break;
                            case 33:
                                connectorTypeList.Add("AUI");
                                break;
                            case 34:
                                connectorTypeList.Add("UTP Category 3");
                                break;
                            case 35:
                                connectorTypeList.Add("UTP Category 4");
                                break;
                            case 36:
                                connectorTypeList.Add("UTP Category 5");
                                break;
                            case 37:
                                connectorTypeList.Add("BNC");
                                break;
                            case 38:
                                connectorTypeList.Add("RJ11");
                                break;
                            case 39:
                                connectorTypeList.Add("RJ45");
                                break;
                            case 40:
                                connectorTypeList.Add("Fiber MIC");
                                break;
                            case 41:
                                connectorTypeList.Add("Apple AUI");
                                break;
                            case 42:
                                connectorTypeList.Add("Apple GeoPort");
                                break;
                            case 43:
                                connectorTypeList.Add("PCI");
                                break;
                            case 44:
                                connectorTypeList.Add("ISA");
                                break;
                            case 45:
                                connectorTypeList.Add("EISA");
                                break;
                            case 46:
                                connectorTypeList.Add("VESA");
                                break;
                            case 47:
                                connectorTypeList.Add("PCMCIA");
                                break;
                            case 48:
                                connectorTypeList.Add("PCMCIA Type I");
                                break;
                            case 49:
                                connectorTypeList.Add("PCMCIA Type II");
                                break;
                            case 50:
                                connectorTypeList.Add("PCMCIA Type III");
                                break;
                            case 51:
                                connectorTypeList.Add("Port ZV");
                                break;
                            case 52:
                                connectorTypeList.Add("Card Bus");
                                break;
                            case 53:
                                connectorTypeList.Add("USB");
                                break;
                            case 54:
                                connectorTypeList.Add("IEEE 1394");
                                break;
                            case 55:
                                connectorTypeList.Add("HIPPI");
                                break;
                            case 56:
                                connectorTypeList.Add("HSSDC (6 broches)");
                                break;
                            case 57:
                                connectorTypeList.Add("GBIC");
                                break;
                            case 58:
                                connectorTypeList.Add("DIN");
                                break;
                            case 59:
                                connectorTypeList.Add("Mini-DIN");
                                break;
                            case 60:
                                connectorTypeList.Add("Micro-DIN");
                                break;
                            case 61:
                                connectorTypeList.Add("PS/2");
                                break;
                            case 62:
                                connectorTypeList.Add("Infrared");
                                break;
                            case 63:
                                connectorTypeList.Add("HP-HIL");
                                break;
                            case 64:
                                connectorTypeList.Add("Access Bus");
                                break;
                            case 65:
                                connectorTypeList.Add("Nu Bus");
                                break;
                            case 66:
                                connectorTypeList.Add("Centronics");
                                break;
                            case 67:
                                connectorTypeList.Add("Mini-Centronics");
                                break;
                            case 68:
                                connectorTypeList.Add("Mini-Centronics Type-14");
                                break;
                            case 69:
                                connectorTypeList.Add("Mini-Centronics Type-20");
                                break;
                            case 70:
                                connectorTypeList.Add("Mini-Centronics Type-26");
                                break;
                            case 71:
                                connectorTypeList.Add("Bus Mouse");
                                break;
                            case 72:
                                connectorTypeList.Add("ADB");
                                break;
                            case 73:
                                connectorTypeList.Add("AGP");
                                break;
                            case 74:
                                connectorTypeList.Add("VME Bus");
                                break;
                            case 75:
                                connectorTypeList.Add("VME64");
                                break;
                            case 76:
                                connectorTypeList.Add("Proprietary");
                                break;
                            case 77:
                                connectorTypeList.Add("Proprietary Processor Card Slot");
                                break;
                            case 78:
                                connectorTypeList.Add("Proprietary Memory Card Slot");
                                break;
                            case 79:
                                connectorTypeList.Add("Proprietary I/O Riser Slot");
                                break;
                            case 80:
                                connectorTypeList.Add("PCI-66MHz");
                                break;
                            case 81:
                                connectorTypeList.Add("AGP 2x");
                                break;
                            case 82:
                                connectorTypeList.Add("AGP 4x");
                                break;
                            case 83:
                                connectorTypeList.Add("PC-98");
                                break;
                            case 84:
                                connectorTypeList.Add("PC-98 Hireso");
                                break;
                            case 85:
                                connectorTypeList.Add("PC-H98");
                                break;
                            case 86:
                                connectorTypeList.Add("PC-98 Note");
                                break;
                            case 87:
                                connectorTypeList.Add("PC-98 Full");
                                break;
                            case 88:
                                connectorTypeList.Add("PCI-X");
                                break;
                            case 89:
                                connectorTypeList.Add("Sbus IEEE 1396-1993 32 bit");
                                break;
                            case 90:
                                connectorTypeList.Add("Sbus IEEE 1396-1993 64 bit");
                                break;
                            case 91:
                                connectorTypeList.Add("MCA");
                                break;
                            case 92:
                                connectorTypeList.Add("GIO");
                                break;
                            case 93:
                                connectorTypeList.Add("XIO");
                                break;
                            case 94:
                                connectorTypeList.Add("HIO");
                                break;
                            case 95:
                                connectorTypeList.Add("NGIO");
                                break;
                            case 96:
                                connectorTypeList.Add("PMC");
                                break;
                            case 97:
                                connectorTypeList.Add("Future I/O");
                                break;
                            case 98:
                                connectorTypeList.Add("InfiniBand");
                                break;
                            case 99:
                                connectorTypeList.Add("AGP 8X");
                                break;
                            case 100:
                                connectorTypeList.Add("PCI-E");
                                break;
                        }

                    // Set final string of Connector Type
                    if (connectorTypeList.Count > 1)
                        foreach (var type in connectorTypeList)
                            connectorType += type + ", ";
                    else
                        connectorType = connectorTypeList[0];

                    // Get Current Usage
                    switch (currentUsageResult)
                    {
                        case 0:
                            currentUsage = "Reserved";
                            break;
                        case 1:
                            currentUsage = "Other";
                            break;
                        case 2:
                            currentUsage = "Unknown";
                            break;
                        case 3:
                            currentUsage = "Available";
                            break;
                        case 4:
                            currentUsage = "In Use";
                            break;
                    }

                    // Add each slot to the list
                    SystemSlot.Add(slotDesignation + " - " + connectorType + "  (" + currentUsage + ")");
                }
            }),

            // Get Bus Info
            Task.Run(() =>
            {
                Log.Info("Get Bus Info");

                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_Bus", new[] { "DeviceID", "BusType", "BusNum" });

                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    string busType;

                    var busTypeResult = (uint)returnValue.PropertiesResultList[i, 1];

                    switch (busTypeResult)
                    {
                        case 0:
                            busType = "Internal";
                            break;
                        case 1:
                            busType = "Isa";
                            break;
                        case 2:
                            busType = "Eisa";
                            break;
                        case 3:
                            busType = "Micro Channel";
                            break;
                        case 4:
                            busType = "Turbo Channel";
                            break;
                        case 5:
                            busType = "PCI Bus";
                            break;
                        case 6:
                            busType = "VME Bus";
                            break;
                        case 7:
                            busType = "NuBus";
                            break;
                        case 8:
                            busType = "PCMCIA Bus";
                            break;
                        case 9:
                            busType = "CBus";
                            break;
                        case 10:
                            busType = "MPI Bus";
                            break;
                        case 11:
                            busType = "MPSA Bus";
                            break;
                        case 12:
                            busType = "Internal Processor";
                            break;
                        case 13:
                            busType = "Internal Power Bus";
                            break;
                        case 14:
                            busType = "PNP ISA Bus";
                            break;
                        case 15:
                            busType = "PNP Bus";
                            break;
                        case 16:
                            busType = "Maximum Interface Type";
                            break;
                        default:
                            busType = "N/A";
                            break;
                    }

                    BusDeviceList.Add(new BusObj(
                        (string)returnValue.PropertiesResultList[i, 0],
                        busType,
                        returnValue.PropertiesResultList[i, 2].ToString()));
                }
            }),

            // Get Usb Controller Info
            Task.Run(() =>
            {
                Log.Info("Get Usb Controller Info");

                // Query return
                var returnValue = wmIqueryManager.WmIquery("Win32_USBController",
                    new[] { "Name", "Manufacturer", "Description", "ProtocolSupported", "Status" });

                for (var i = 0; i < returnValue.NbResult; i++)
                {
                    var protocolSupported = "N/A";

                    var protocolSupportedResult = (ushort)returnValue.PropertiesResultList[i, 3];

                    switch (protocolSupportedResult)
                    {
                        case 1:
                            protocolSupported = "Other";
                            break;
                        case 2:
                            protocolSupported = "Unknown";
                            break;
                        case 3:
                            protocolSupported = "EISA";
                            break;
                        case 4:
                            protocolSupported = "ISA";
                            break;
                        case 5:
                            protocolSupported = "PCI";
                            break;
                        case 6:
                            protocolSupported = "ATA/ATAPI";
                            break;
                        case 7:
                            protocolSupported = "Flexible Diskette";
                            break;
                        case 8:
                            protocolSupported = "1496";
                            break;
                        case 9:
                            protocolSupported = "SCSI Parallel Interface";
                            break;
                        case 10:
                            protocolSupported = "SCSI Fibre Channel Protocol";
                            break;
                        case 11:
                            protocolSupported = "SCSI Serial Bus Protocol";
                            break;
                        case 12:
                            protocolSupported = "SCSI Serial Bus Protocol-2 (1394)";
                            break;
                        case 13:
                            protocolSupported = "SCSI Serial Storage Architecture";
                            break;
                        case 14:
                            protocolSupported = "VESA";
                            break;
                        case 15:
                            protocolSupported = "PCMCIA";
                            break;
                        case 16:
                            protocolSupported = "Universal Serial Bus";
                            break;
                        case 17:
                            protocolSupported = "Parallel Protocol";
                            break;
                        case 18:
                            protocolSupported = "ESCON";
                            break;
                        case 19:
                            protocolSupported = "Diagnostic";
                            break;
                        case 20:
                            protocolSupported = "I2C";
                            break;
                        case 21:
                            protocolSupported = "Power";
                            break;
                        case 22:
                            protocolSupported = "HIPPI";
                            break;
                        case 23:
                            protocolSupported = "Multi Bus";
                            break;
                        case 24:
                            protocolSupported = "VME";
                            break;
                        case 25:
                            protocolSupported = "IPI";
                            break;
                        case 26:
                            protocolSupported = "IEEE-488";
                            break;
                        case 27:
                            protocolSupported = "RS-232";
                            break;
                        case 28:
                            protocolSupported = "IEEE 802.3 10BASE5";
                            break;
                        case 29:
                            protocolSupported = "IEEE 802.3 10BASE2";
                            break;
                        case 30:
                            protocolSupported = "IEEE 802.3 1BASE5";
                            break;
                        case 31:
                            protocolSupported = "IEEE 802.3 10BROAD36";
                            break;
                        case 32:
                            protocolSupported = "IEEE 802.3 100BASEVG";
                            break;
                        case 33:
                            protocolSupported = "IEEE 802.5 Token Ring";
                            break;
                        case 34:
                            protocolSupported = "ANSI X3T9.5 FDDI";
                            break;
                        case 35:
                            protocolSupported = "MCA";
                            break;
                        case 36:
                            protocolSupported = "ESDI";
                            break;
                        case 37:
                            protocolSupported = "IDE";
                            break;
                        case 38:
                            protocolSupported = "CMD";
                            break;
                        case 39:
                            protocolSupported = "ST506";
                            break;
                        case 40:
                            protocolSupported = "DSSI";
                            break;
                        case 41:
                            protocolSupported = "QIC2";
                            break;
                        case 42:
                            protocolSupported = "Enhanced ATA/IDE";
                            break;
                        case 43:
                            protocolSupported = "AGP";
                            break;
                        case 44:
                            protocolSupported = "TWIRP (two-way infrared)";
                            break;
                        case 45:
                            protocolSupported = "FIR (fast infrared)";
                            break;
                        case 46:
                            protocolSupported = "SIR (serial infrared)";
                            break;
                        case 47:
                            protocolSupported = "IrBus";
                            break;
                    }

                    UsbControllerList.Add(new UsbControllerObj(
                        (string)returnValue.PropertiesResultList[i, 0],
                        (string)returnValue.PropertiesResultList[i, 1],
                        (string)returnValue.PropertiesResultList[i, 2],
                        protocolSupported,
                        (string)returnValue.PropertiesResultList[i, 4]));
                }
            })
        };

        // Wait for all tasks to complete
        Task.WaitAll(wmiTasks.ToArray());


        // Add Manufacturer to mainNode
        var manufacturerNode = new TreeViewNode();
        manufacturerNode.Content = $"Manufacturer : {MbManufacturer}";

        // Add Product to mainNode
        var productNode = new TreeViewNode();
        productNode.Content = $"Product : {MbProductName}";

        // Add Serial Number to mainNode
        var serialNumberNode = new TreeViewNode();
        serialNumberNode.Content = $"Serial Number : {MbSerialNumber}";

        // Add Status to mainNode
        var statusNode = new TreeViewNode();
        statusNode.Content = $"Status : {MbStatus}";

        // Add Bus Type to mainNode
        var busTypeNode = new TreeViewNode();
        busTypeNode.Content = $"Bus Type : {MbBusType}";

        // Add Onboard Device to parent node
        var onBoardDeviceNodeList = new List<TreeViewNode>();
        foreach (var onBoardDeviceObj in OnBoardDevicesList)
        {
            var onBoardDeviceNode = new TreeViewNode();
            onBoardDeviceNode.Content = onBoardDeviceObj.Description;
            onBoardDeviceNode.IsExpanded = true;

            var onBoardDeviceChildNode = new TreeViewNode();
            onBoardDeviceChildNode.Content = $"Type : {onBoardDeviceObj.DeviceType}";
            onBoardDeviceNode.ChildNodes.Add(onBoardDeviceChildNode);

            onBoardDeviceNodeList.Add(onBoardDeviceNode);
        }

        // Add On Board Devices Node to mainNode
        var onBoardDevicesNode = new TreeViewNode();
        onBoardDevicesNode.Content = OnBoardDevicesList.Count > 0 ? "On Board Devices" : "On Board Devices (None)";
        onBoardDevicesNode.IsExpanded = true;
        foreach (var onBoardDevice in onBoardDeviceNodeList) onBoardDevicesNode.ChildNodes.Add(onBoardDevice);

        // Add IDE Controller to parent node
        var ideControllerNodeList = new List<TreeViewNode>();
        foreach (var ideControllerObj in IdeControllerList)
        {
            var ideControllerNode = new TreeViewNode();
            ideControllerNode.Content = ideControllerObj.Caption;
            ideControllerNode.IsExpanded = true;

            var ideControllerManufacturerNode = new TreeViewNode();
            ideControllerManufacturerNode.Content = $"Manufacturer : {ideControllerObj.Manufacturer}";
            ideControllerNode.ChildNodes.Add(ideControllerManufacturerNode);

            var ideControllerProtocolNode = new TreeViewNode();
            ideControllerProtocolNode.Content = $"Protocol Supported : {ideControllerObj.ProtocolSupported}";
            ideControllerNode.ChildNodes.Add(ideControllerProtocolNode);

            var ideControllerStatusNode = new TreeViewNode();
            ideControllerStatusNode.Content = $"Status : {ideControllerObj.Status}";
            ideControllerNode.ChildNodes.Add(ideControllerStatusNode);

            ideControllerNodeList.Add(ideControllerNode);
        }

        // Add IDE Controller Node to mainNode
        var ideControllersNode = new TreeViewNode();
        ideControllersNode.Content = IdeControllerList.Count > 0 ? "IDE Controller" : "IDE Controller (None)";
        ideControllersNode.IsExpanded = true;
        foreach (var ideControllerDevice in ideControllerNodeList)
            ideControllersNode.ChildNodes.Add(ideControllerDevice);

        // Add Interface to parent node
        var interfaceNodeList = new List<TreeViewNode>();
        foreach (var interfaceObj in InterfaceList)
        {
            var interfaceNode = new TreeViewNode();
            interfaceNode.Content = interfaceObj.ExternalReferenceDesignator;

            var interfaceConnectorTypeNode = new TreeViewNode();
            interfaceConnectorTypeNode.Content = $"Connector Type : {interfaceObj.ConnectorType}";
            interfaceNode.ChildNodes.Add(interfaceConnectorTypeNode);

            var interfacePortTypeNode = new TreeViewNode();
            interfacePortTypeNode.Content = $"Port Type : {interfaceObj.PortType}";
            interfaceNode.ChildNodes.Add(interfacePortTypeNode);

            interfaceNodeList.Add(interfaceNode);
        }

        // Add Interfaces Node to mainNode
        var interfacesNode = new TreeViewNode();
        interfacesNode.Content = InterfaceList.Count > 0 ? "Interfaces" : "Interfaces (None)";
        interfacesNode.IsExpanded = true;
        foreach (var interfaceDevice in interfaceNodeList) interfacesNode.ChildNodes.Add(interfaceDevice);

        // Add System slot to parent node
        var systemSlotNodeList = new List<TreeViewNode>();
        foreach (var systemSlot in SystemSlot)
        {
            var systemSlotNode = new TreeViewNode();
            systemSlotNode.Content = systemSlot;

            systemSlotNodeList.Add(systemSlotNode);
        }

        // Add System Slot to mainNode
        var systemSlotsNode = new TreeViewNode();
        systemSlotsNode.Content = SystemSlot.Count > 0 ? "System Slots" : "System Slots (None)";
        systemSlotsNode.IsExpanded = true;
        foreach (var systemSlotDevice in systemSlotNodeList) systemSlotsNode.ChildNodes.Add(systemSlotDevice);

        // Add Bus Device to parent node
        var busDeviceNodeList = new List<TreeViewNode>();
        foreach (var busDeviceObj in BusDeviceList)
        {
            var busDeviceNode = new TreeViewNode();
            busDeviceNode.Content = busDeviceObj.DeviceId;

            var busDeviceTypeNode = new TreeViewNode();
            busDeviceTypeNode.Content = $"Bus Type : {busDeviceObj.BusType}";
            busDeviceNode.ChildNodes.Add(busDeviceTypeNode);

            var busDeviceNumNode = new TreeViewNode();
            busDeviceNumNode.Content = $"Bus Number : {busDeviceObj.BusNumber}";
            busDeviceNode.ChildNodes.Add(busDeviceNumNode);

            busDeviceNodeList.Add(busDeviceNode);
        }

        // Add Bus to mainNode
        var busNode = new TreeViewNode();
        busNode.Content = BusDeviceList.Count > 0 ? "Bus" : "Bus (None)";
        busNode.IsExpanded = true;
        foreach (var busDevice in busDeviceNodeList) busNode.ChildNodes.Add(busDevice);

        // Add USB Controller to parent node
        var usbControllerNodeList = new List<TreeViewNode>();
        foreach (var usbControllerObj in UsbControllerList)
        {
            var usbNode = new TreeViewNode();
            usbNode.Content = usbControllerObj.Name;
            usbNode.IsExpanded = true;

            var usbManufacturerNode = new TreeViewNode();
            usbManufacturerNode.Content = $"Manufacturer : {usbControllerObj.Manufacturer}";
            usbNode.ChildNodes.Add(usbManufacturerNode);

            var usbDescriptionNode = new TreeViewNode();
            usbDescriptionNode.Content = $"Description : {usbControllerObj.Description}";
            usbNode.ChildNodes.Add(usbDescriptionNode);

            var usbProtocolNode = new TreeViewNode();
            usbProtocolNode.Content = $"Protocol : {usbControllerObj.ProtocolSupported}";
            usbNode.ChildNodes.Add(usbProtocolNode);

            var usbStatusNode = new TreeViewNode();
            usbStatusNode.Content = $"Status : {usbControllerObj.Status}";
            usbNode.ChildNodes.Add(usbStatusNode);

            usbControllerNodeList.Add(usbNode);
        }

        // Add USB Controller to mainNode
        var usbControllerNode = new TreeViewNode();
        usbControllerNode.Content = UsbControllerList.Count > 0 ? "USB Controller" : "USB Controller (None)";
        usbControllerNode.IsExpanded = true;
        foreach (var usbControllerDevice in usbControllerNodeList)
            usbControllerNode.ChildNodes.Add(usbControllerDevice);

        // Main Node for Motherboard
        var mainNode = new TreeViewNode();
        mainNode.Content = MbProductName;
        mainNode.IsExpanded = true;
        mainNode.ChildNodes.Add(manufacturerNode);
        mainNode.ChildNodes.Add(productNode);
        mainNode.ChildNodes.Add(serialNumberNode);
        mainNode.ChildNodes.Add(statusNode);
        mainNode.ChildNodes.Add(busTypeNode);
        mainNode.ChildNodes.Add(onBoardDevicesNode);
        mainNode.ChildNodes.Add(ideControllersNode);
        mainNode.ChildNodes.Add(interfacesNode);
        mainNode.ChildNodes.Add(systemSlotsNode);
        mainNode.ChildNodes.Add(busNode);
        mainNode.ChildNodes.Add(usbControllerNode);

        // Main TreeView Nodes Collection
        var treeViewNodes = new TreeViewNodeCollection();
        treeViewNodes.Add(mainNode);

        // Main TreeView
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
                var onBoardDeviceString = "";
                foreach (var onBoardDeviceObj in OnBoardDevicesList)
                    onBoardDeviceString += $"<li><h3>{onBoardDeviceObj.Description}</h3><ul><li><h3>" +
                                           $"{onBoardDeviceObj.DeviceType}</h3></li></ul></li>";

                var ideControllerString = "";
                foreach (var ideControllerObj in IdeControllerList)
                    ideControllerString +=
                        $"<li><h3>{ideControllerObj.Caption}</h3><ul><li><h3>Manufacturer : " +
                        $"{ideControllerObj.Manufacturer}</h3></li><li><h3>Protocol Supported : " +
                        $"{ideControllerObj.ProtocolSupported}</h3></li><li><h3>Status : " +
                        $"{ideControllerObj.Status}</h3></li></ul></li>";

                var interfaceString = "";
                foreach (var interfaceObj in InterfaceList)
                    interfaceString += $"<li><h3>{interfaceObj.ExternalReferenceDesignator}</h3><ul><li><h3>" +
                                       $"Connector Type : {interfaceObj.ConnectorType}</h3></li><li><h3>Port Type" +
                                       $" : {interfaceObj.PortType}</h3></li></ul></li>";

                var systemSlotString = "";
                foreach (var systemSlot in SystemSlot) systemSlotString += $"<li><h3>{systemSlot}</h3></li>";

                var busString = "";
                foreach (var busObj in BusDeviceList)
                    busString += $"<li><h3>{busObj.DeviceId}</h3><ul><li><h3>Bus Type : {busObj.BusType}" +
                                 $"</h3></li><li><h3>Bus Num : {busObj.BusNumber}</h3></li></ul></li>";

                var usbControllerString = "";
                foreach (var usbControllerObj in UsbControllerList)
                    usbControllerString += $"<li><h3>{usbControllerObj.Name}</h3><ul><li><h3>Manufacturer :" +
                                           $" {usbControllerObj.Manufacturer}</h3></li><li><h3>Description :" +
                                           $" {usbControllerObj.Description}</h3></li><li><h3>Protocol Supported " +
                                           $": {usbControllerObj.ProtocolSupported}</h3></li><li><h3>Status : " +
                                           $"{usbControllerObj.Status}</h3></li></ul></li>";

                // html template
                var htmlString = "<!DOCTYPE html><head> <title>Data Export - Windows Admin Center</title></head>" +
                                 "<body style=\"font-family: monospace, sans-serif;\"> <h1 style=\"text-align: " +
                                 "center; margin: 50px 0;\"> Windows Admin Center - System Information " +
                                 $"(Motherboard) </h1> <ul> <li> <h2>{MbProductName}</h2> <ul> <li> <h3>" +
                                 $"Manufacturer : {MbManufacturer}</h3> </li> <li> <h3>Product :{MbProductName}" +
                                 $"</h3> </li> <li> <h3>Serial Number : {MbSerialNumber}</h3> </li> <li> <h3>Status" +
                                 $" : {MbStatus}</h3> </li> <li> <h3>Bus Type : {MbBusType}</h3> </li> <li> <h3>On " +
                                 $"Board Device</h3> <ul>{onBoardDeviceString} </ul> </li> <li> <h3>IDE " +
                                 $"Controller</h3> <ul>{ideControllerString}</ul> </li> <li> <h3>Interfaces</h3> " +
                                 $"<ul>{interfaceString}</ul> </li> <li> <h3>System Slot</h3> <ul>{systemSlotString}" +
                                 $"</ul> </li> <li> <h3>Bus</h3> <ul>{busString}</ul> </li> <li> <h3>USB Controller" +
                                 $"</h3> <ul>{usbControllerString}</ul> </li> </ul> </li> </ul></body></html>";

                // Create a file and ask user to save it
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "HTML File (*.html)|*.html",
                    Title = "Save HTML File",
                    FileName = "System Information(Motherboard)_" + DateTime.Now.ToString("yyyy-MM-dd"),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, htmlString);
                    Process.Start(saveFileDialog.FileName);
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

    private string MbProductName { get; set; } = "";
    private string MbManufacturer { get; set; } = "";
    private string MbSerialNumber { get; set; } = "";
    private string MbStatus { get; set; } = "";
    private string MbBusType { get; set; } = "";
    private List<OnBoardDeviceObj> OnBoardDevicesList { get; }
    private List<IdeControllerObj> IdeControllerList { get; }
    private List<InterfaceObj> InterfaceList { get; }
    private List<string> SystemSlot { get; }
    private List<BusObj> BusDeviceList { get; }
    private List<UsbControllerObj> UsbControllerList { get; }

    #endregion
}