using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using Application = System.Windows.Application;

namespace OnScreenReticle2.ViewModels
{
    public class MainWindow_ViewModel : ViewModelBase
    {
        private Settings settings;
        private SettingsWindow window;
        private MainWindow main;
        private NotifyIcon Noti;
        private bool allVisibility;

        public double DotDiameter { get => settings.DotDiameter; set { settings.DotDiameter = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double AngleThickness { get => settings.AngleThickness; set { settings.AngleThickness = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double AngleLength { get => settings.AngleLength; set { settings.AngleLength = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = Math.Round(value, 1); RaisePropertyChanged(); } }
        public double WindowTop { get => settings.WindowTop; set { settings.WindowTop = value; RaisePropertyChanged(); } }
        public double WindowLeft { get => settings.WindowLeft; set { settings.WindowLeft = value; RaisePropertyChanged(); } }
        public int ColorR { get => settings.ColorR; set { settings.ColorR = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); } }
        public int ColorG { get => settings.ColorG; set { settings.ColorG = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); } }
        public int ColorB { get => settings.ColorB; set { settings.ColorB = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); } }
        public int ColorA { get => settings.ColorA; set { settings.ColorA = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); } }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; RaisePropertyChanged(); } }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; RaisePropertyChanged(); } }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; RaisePropertyChanged(); } }
        public bool AllVisibility { get => allVisibility; set { allVisibility = value; RaisePropertyChanged(); } }

        public Brush ColorBrush
        {
            get => new SolidColorBrush(Color.FromArgb(byte.Parse(ColorA.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorR.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorG.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorB.ToString(), NumberStyles.Integer)));
            set { return; }
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(DotDiameter));
            RaisePropertyChanged(nameof(AngleThickness));
            RaisePropertyChanged(nameof(AngleLength));
            RaisePropertyChanged(nameof(CrossThickness));
            RaisePropertyChanged(nameof(CrossLength));
            RaisePropertyChanged(nameof(CrossOffset));
            RaisePropertyChanged(nameof(DotVisibility));
            RaisePropertyChanged(nameof(AngleVisibility));
            RaisePropertyChanged(nameof(CrossVisibility));
            RaisePropertyChanged(nameof(WindowTop));
            RaisePropertyChanged(nameof(WindowLeft));
            RaisePropertyChanged(nameof(ColorR));
            RaisePropertyChanged(nameof(ColorG));
            RaisePropertyChanged(nameof(ColorB));
            RaisePropertyChanged(nameof(ColorA));
            RaisePropertyChanged(nameof(ColorBrush));
        }

        public MainWindow_ViewModel(MainWindow main)
        {
            this.main = main;
            settings = ((App)Application.Current).Xml.settings;
            AllVisibility = true;
            GenerateNotifyIcon();
        }

        internal void SetVisibility()
        {
            AllVisibility = !AllVisibility;
        }

        internal void RotateProfiles()
        {
            if (settings.WindowTop == Screen.PrimaryScreen.Bounds.Height * 0.5 - 50 && settings.WindowLeft == Screen.PrimaryScreen.Bounds.Width * 0.5 - 50)
            {
                settings.WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.6 - 50;
                settings.WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            }
            else if (settings.WindowTop == Screen.PrimaryScreen.Bounds.Height * 0.6 - 50 && settings.WindowLeft == Screen.PrimaryScreen.Bounds.Width * 0.5 - 50)
            {
                settings.WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.5 - 50;
                settings.WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            }
            else
            {
                settings.WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.5 - 50;
                settings.WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
            }

            Refresh();
        }

        #region NotifyIcon
        private void GenerateNotifyIcon()
        {
            ContextMenu Menu = new ContextMenu();

            MenuItem OpenSettingsItem = new MenuItem()
            {
                Text = "Open Setting"
            };
            OpenSettingsItem.Click += (object o, EventArgs e) =>
            {
                OpenSettingWindow();
            };
            Menu.MenuItems.Add(OpenSettingsItem);

            MenuItem SetVisibilityItem = new MenuItem()
            {
                Text = "Set Visibility"
            };
            SetVisibilityItem.Click += (object o, EventArgs e) =>
            {
                SetVisibility();
                ((App)Application.Current).Xml.SaveSettings();
            };
            Menu.MenuItems.Add(SetVisibilityItem);

            MenuItem RotateProfilesItem = new MenuItem()
            {
                Text = "Rotate Profiles"
            };
            RotateProfilesItem.Click += (object o, EventArgs e) =>
            {
                RotateProfiles();
                ((App)Application.Current).Xml.SaveSettings();
            };
            Menu.MenuItems.Add(RotateProfilesItem);

            MenuItem ExitItem = new MenuItem()
            {
                Text = "Exit"
            };
            ExitItem.Click += (object o, EventArgs e) =>
            {
                if (window != null) window.Close();
                main.Close();
            };
            Menu.MenuItems.Add(ExitItem);

            Noti = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Visible = true,
                Text = "OnScreenReticle",
                ContextMenu = Menu
            };
        }

        internal void Window_Closing()
        {
            Noti.Visible = false;
            Noti.Icon = null;
        }
        #endregion

        public void OpenSettingWindow()
        {
            if (window != null)
            {
                window.Close();
                window = null;
            }
            else
            {
                window = new SettingsWindow(this);
                window.ShowDialog();
            }
        }
    }

    public class VisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = true;
            foreach (var o in values)
            {
                if (o is bool v)
                {
                    if (val != v) val = v;
                }
            }

            return val;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
