using OnScreenReticle2.ViewModels;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;

namespace OnScreenReticle2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindow_ViewModel viewmodel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindow_ViewModel(this);
            viewmodel = DataContext as MainWindow_ViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //공통
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;

            //EX Transparent
            SetWindowExTransparent(hwnd);

            //Hotkey
            _source = HwndSource.FromHwnd(hwnd);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        #region EX Transparent
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
        #endregion

        #region Hotkey
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private HwndSource _source;
        private const int HOTKEY_ID1 = 9000;
        private const int HOTKEY_ID2 = 9001;
        private const int HOTKEY_ID3 = 9002;

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            RegisterHotKey(helper.Handle, HOTKEY_ID1, Constants.CTRL + Constants.SHIFT + Constants.ALT, (int)Keys.A);
            RegisterHotKey(helper.Handle, HOTKEY_ID2, Constants.CTRL + Constants.SHIFT + Constants.ALT, (int)Keys.S);
            RegisterHotKey(helper.Handle, HOTKEY_ID3, Constants.CTRL + Constants.SHIFT + Constants.ALT, (int)Keys.D);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID1:
                            OnHotKeyPressed1();
                            handled = true;
                            break;
                        case HOTKEY_ID2:
                            OnHotKeyPressed2();
                            handled = true;
                            break;
                        case HOTKEY_ID3:
                            OnHotKeyPressed3();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void OnHotKeyPressed1()
        {
            viewmodel.OpenSettingWindow();
        }
        private void OnHotKeyPressed2()
        {
            viewmodel.SetVisibility();
            ((App)System.Windows.Application.Current).Xml.SaveSettings();
        }
        private void OnHotKeyPressed3()
        {
            viewmodel.RotateProfiles();
            ((App)System.Windows.Application.Current).Xml.SaveSettings();
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((App)System.Windows.Application.Current).Xml.SaveSettings();
            viewmodel.Window_Closing();
        }
    }

    public static class Constants
    {
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }
}
