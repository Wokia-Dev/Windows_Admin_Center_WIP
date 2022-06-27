using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.TreeView;
using Syncfusion.UI.Xaml.TreeView.Engine;
using System_Information.MVVM.ViewModel;

namespace System_Information
{
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
            this.WindowState = WindowState.Minimized;
        }

        private void CloseBtn_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                SwitchWindowState();
                this.Top = 3;
            }
            this.DragMove();

            if (e.ClickCount == 2)
            {
                SwitchWindowState();
            }
        }


        private void TreeViewNav_OnSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            var selectedItem = (TreeViewNode)TreeViewNav.SelectedItem;
            var content = selectedItem.Content.ToString();


            switch (content)
            {
                case "Hardware":
                    DataContext = new HardwareViewModel();
                    break;
                case "Software":
                    DataContext = new SoftwareViewModel();
                    break;
                case "BIOS":
                    DataContext = new BiosViewModel();
                    break;
                case "Motherboard":
                    DataContext = new MotherboardViewModel();
                    break;
                case "CPU":
                    DataContext = new CpuViewModel();
                    break;
                case "Video":
                    DataContext = new VideoViewModel();
                    break;
                case "Memory":
                    DataContext = new MemoryViewModel();
                    break;
                case "Drive":
                    DataContext = new DriveViewModel();
                    break;
                case "Audio":
                    DataContext = new AudioViewMode();
                    break;
                case "Network":
                    DataContext = new NetworkViewModel();
                    break;
                case "External Device":
                    DataContext = new ExternalDeviceViewModel();
                    break;
                case "Battery":
                    DataContext = new BatteryViewModel();
                    break;
                case "Computer System":
                    DataContext = new ComputerSystemViewModel();
                    break;
                case "Operating System":
                    DataContext = new OperatingSystemViewModel();
                    break;
                case "Users":
                    DataContext = new UsersViewModel();
                    break;
                case "Environment":
                    DataContext = new EnvironmentViewModel();
                    break;
                case "Codecs":
                    DataContext = new CodecsViewModel();
                    break;
                case "System folders":
                    DataContext = new SystemFoldersViewModel();
                    break;

            }

        }


        private void MainWindow_OnSourceInitialized(object sender, EventArgs e)
        {
            var mWindowsHandle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(mWindowsHandle).AddHook(new HwndSourceHook(WindowProc));
        }

        private static System.IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            Point lMousePosition;
            GetCursorPos(out lMousePosition);

            var lPrimaryScreen = MonitorFromPoint(new Point(0, 0), MonitorOptions.MonitorDefaulttoprimary);
            var lPrimaryScreenInfo = new Monitorinfo();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
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
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        break;
                    }
            }
        }


        private void rctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if ((ResizeMode == ResizeMode.CanResize) || (ResizeMode == ResizeMode.CanResizeWithGrip))
                {
                    SwitchWindowState();
                }

                return;
            }

            else if (WindowState == WindowState.Maximized)
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
            if (_mRestoreIfMove)
            {
                _mRestoreIfMove = false;

                double percentHorizontal = e.GetPosition(this).X / ActualWidth;
                double targetHorizontal = RestoreBounds.Width * percentHorizontal;

                double percentVertical = e.GetPosition(this).Y / ActualHeight;
                double targetVertical = RestoreBounds.Height * percentVertical;

                WindowState = WindowState.Normal;

                Point lMousePosition;
                GetCursorPos(out lMousePosition);

                Left = lMousePosition.X - targetHorizontal;
                Top = lMousePosition.Y - targetVertical;

                DragMove();
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out Point lpPoint);


        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(Point pt, MonitorOptions dwFlags);

        enum MonitorOptions : uint
        {
            MonitorDefaulttonull = 0x00000000,
            MonitorDefaulttoprimary = 0x00000001,
            MonitorDefaulttonearest = 0x00000002
        }


        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, Monitorinfo lpmi);


        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Minmaxinfo
        {
            public Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            public Point ptMinTrackSize;
            public Point ptMaxTrackSize;
        };


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class Monitorinfo
        {
            public int cbSize = Marshal.SizeOf(typeof(Monitorinfo));
            public Rect rcMonitor = new Rect();
            public Rect rcWork = new Rect();
            public int dwFlags = 0;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left, Top, Right, Bottom;

            public Rect(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }
    }
}