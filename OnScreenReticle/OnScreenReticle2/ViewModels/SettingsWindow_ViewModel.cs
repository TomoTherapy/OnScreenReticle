using System;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Media;
using Application = System.Windows.Application;

namespace OnScreenReticle2.ViewModels
{
    public class SettingsWindow_ViewModel : ViewModelBase
    {
        private Settings settings;
        private MainWindow_ViewModel main;

        public double DotDiameter { get => settings.DotDiameter; set { if (value >= 2 && value <= 15) { settings.DotDiameter = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public double AngleThickness { get => settings.AngleThickness; set { if (value >= 1 && value <= 10) { settings.AngleThickness = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public double AngleLength { get => settings.AngleLength; set { if (value >= 1 && value <= 20) { settings.AngleLength = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public double CrossThickness { get => settings.CrossThickness; set { if (value >= 1 && value <= 10) { settings.CrossThickness = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public double CrossLength { get => settings.CrossLength; set { if (value >= 1 && value <= 20) { settings.CrossLength = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public double CrossOffset { get => settings.CrossOffset; set { if (value >= 1 && value <= 20) { settings.CrossOffset = Math.Round(value, 1); RaisePropertyChanged(); main.Refresh(); } } }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; RaisePropertyChanged(); main.Refresh(); } }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; RaisePropertyChanged(); main.Refresh(); } }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; RaisePropertyChanged(); main.Refresh(); } }
        public double WindowTop { get => settings.WindowTop; set { settings.WindowTop = value; RaisePropertyChanged(); main.Refresh(); } }
        public double WindowLeft { get => settings.WindowLeft; set { settings.WindowLeft = value; RaisePropertyChanged(); main.Refresh(); } }
        public int ColorR { get => settings.ColorR; set { if (value >= 0 && value <= 255) { settings.ColorR = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); main.Refresh(); } } }
        public int ColorG { get => settings.ColorG; set { if (value >= 0 && value <= 255) { settings.ColorG = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); main.Refresh(); } } }
        public int ColorB { get => settings.ColorB; set { if (value >= 0 && value <= 255) { settings.ColorB = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); main.Refresh(); } } }
        public int ColorA { get => settings.ColorA; set { if (value >= 0 && value <= 255) { settings.ColorA = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(ColorBrush)); main.Refresh(); } } }
        public Brush ColorBrush
        {
            get => new SolidColorBrush(Color.FromArgb(byte.Parse(ColorA.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorR.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorG.ToString(), NumberStyles.Integer)
                , byte.Parse(ColorB.ToString(), NumberStyles.Integer)));
            set { return; }
        }

        public SettingsWindow_ViewModel(MainWindow_ViewModel main)
        {
            settings = ((App)Application.Current).Xml.settings;
            this.main = main;
        }

        internal void Window_Closing()
        {
            ((App)Application.Current).Xml.SaveSettings();
        }

        internal void CenterScreen_button_Click()
        {
            WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.5 - 50;
            WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
        }

        internal void HuntShowdown_button_Click()
        {
            WindowTop = Screen.PrimaryScreen.Bounds.Height * 0.6 - 50;
            WindowLeft = Screen.PrimaryScreen.Bounds.Width * 0.5 - 50;
        }

        internal void Default_button_Click()
        {
            ((App)Application.Current).Xml.DefaultSettings();
            main.Refresh();
            Refresh();
        }

        internal void Up_button_Click()
        {
            WindowTop = WindowTop - 1;
        }

        internal void Left_button_Click()
        {
            WindowLeft = WindowLeft - 1;
        }

        internal void Right_button_Click()
        {
            WindowLeft = WindowLeft + 1;
        }

        internal void Down_button_Click()
        {
            WindowTop = WindowTop + 1;
        }

        private void Refresh()
        {
            RaisePropertyChanged(nameof(DotDiameter));
            RaisePropertyChanged(nameof(AngleThickness));
            RaisePropertyChanged(nameof(AngleLength));
            //RaisePropertyChanged(nameof(CrossThickness));
            //RaisePropertyChanged(nameof(CrossLength));
            //RaisePropertyChanged(nameof(CrossOffset));
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

    }
}
