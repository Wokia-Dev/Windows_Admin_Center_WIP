using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.MVVM.ViewModel;

namespace System_Information;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private bool _mRestoreIfMove;

    public MainWindow()
    {
        InitializeComponent();
        SfSkinManager.SetTheme(this, new Theme("FluentLight"));

        DataContext = new HardwareViewModel();
    }

    private void MinimizeBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void TreeViewNav_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
    {
        var selectedItem = (TreeViewNode)TreeViewNav.SelectedItem;
        var content = selectedItem.Content.ToString();

        DataContext = content switch
        {
            "Hardware" => new HardwareViewModel(),
            "Software" => new SoftwareViewModel(),
            "BIOS" => new BiosViewModel(),
            "Motherboard" => new MotherboardViewModel(),
            "CPU" => new CpuViewModel(),
            "Video" => new VideoViewModel(),
            "Memory" => new MemoryViewModel(),
            "Drive" => new DriveViewModel(),
            "Audio" => new AudioViewMode(),
            "Network" => new NetworkViewModel(),
            "External Device" => new ExternalDeviceViewModel(),
            "Battery" => new BatteryViewModel(),
            "Computer System" => new ComputerSystemViewModel(),
            "Operating System" => new OperatingSystemViewModel(),
            "Users" => new UsersViewModel(),
            "Environment" => new EnvironmentViewModel(),
            "Codecs" => new CodecsViewModel(),
            "System folders" => new SystemFoldersViewModel(),
            _ => DataContext
        };
    }


    private void MainWindow_OnSourceInitialized(object sender, EventArgs e)
    {
        var mWindowsHandle = (new WindowInteropHelper(this)).Handle;
        HwndSource.FromHwnd(mWindowsHandle)?.AddHook(WindowProc);
    }

    private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        switch (msg)
        {
            case 0x0024:
                WmGetMinMaxInfo(lParam);
                break;
            default:
                return IntPtr.Zero;
        }

        return IntPtr.Zero;
    }

    private static void WmGetMinMaxInfo(IntPtr lParam)
    {
        GetCursorPos(out var lMousePosition);

        var lPrimaryScreen = MonitorFromPoint(new Point(0, 0), MonitorOptions.MonitorDefaulttoprimary);
        var lPrimaryScreenInfo = new Monitorinfo();
        if (!GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo))
        {
            return;
        }

        var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MonitorDefaulttonearest);

        var lMmi = (Minmaxinfo)Marshal.PtrToStructure(lParam, typeof(Minmaxinfo))!;

        if (lPrimaryScreen.Equals(lCurrentScreen))
        {
            lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
            lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
            lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
            lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
        }
        else
        {
            lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
            lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
            lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
            lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
        }

        Marshal.StructureToPtr(lMmi, lParam, true);
    }

    private void SwitchWindowState()
    {
        WindowState = WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => WindowState
        };
    }


    private void rctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            if (ResizeMode is ResizeMode.CanResize or ResizeMode.CanResizeWithGrip)
            {
                SwitchWindowState();
            }

            return;
        }

        if (WindowState == WindowState.Maximized)
        {
            _mRestoreIfMove = true;
            return;
        }

        DragMove();
    }


    private void rctHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _mRestoreIfMove = false;
    }


    private void rctHeader_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (!_mRestoreIfMove) return;
        _mRestoreIfMove = false;

        var percentHorizontal = e.GetPosition(this).X / ActualWidth;
        var targetHorizontal = RestoreBounds.Width * percentHorizontal;

        var percentVertical = e.GetPosition(this).Y / ActualHeight;
        var targetVertical = RestoreBounds.Height * percentVertical;

        WindowState = WindowState.Normal;

        GetCursorPos(out var lMousePosition);

        Left = lMousePosition.X - targetHorizontal;
        Top = lMousePosition.Y - targetVertical;

        DragMove();
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out Point lpPoint);


    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr MonitorFromPoint(Point pt, MonitorOptions dwFlags);

    private enum MonitorOptions : uint
    {
        MonitorDefaulttoprimary = 0x00000001,
        MonitorDefaulttonearest = 0x00000002
    }


    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, Monitorinfo lpmi);


    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Minmaxinfo
    {
        private readonly Point ptReserved;
        public Point ptMaxSize;
        public Point ptMaxPosition;
        private readonly Point ptMinTrackSize;
        private readonly Point ptMaxTrackSize;
    };


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class Monitorinfo
    {
        public Rect rcMonitor = new();
        public Rect rcWork = new();
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public readonly int Left, Top, Right, Bottom;

        public Rect(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}