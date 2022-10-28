using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace OnScreenReticleXboxGameBar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReticleSettings : Page, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private Settings settings;
        private string newSettingName;

        public SettingsList SettingsList { get => ((App)Application.Current).SettingsList; set => ((App)Application.Current).SettingsList = value; }
        public int SettingsListIndex
        {
            get
            {
                settings = SettingsList.List[SettingsList.ChosenOne];
                return SettingsList.ChosenOne;
            }
            set
            {
                settings = SettingsList.List[SettingsList.ChosenOne];
                NotifyAllProperties();
                SettingsList.ChosenOne = value;
            }
        }
        public string NewSettingName { get => newSettingName; set { newSettingName = value; NotifyPropertyChanged(); } }


        public string SettingsName { get => settings.Name; set { settings.Name = value; NotifyPropertyChanged(); } }
        // Location
        public double Top { get => settings.Top; set { settings.Top = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public double Left { get => settings.Left; set { settings.Left = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public string MarginString { get => $"{Left},{Top},0,0"; }

        // Dot
        public double DotDiameter
        {
            get => settings.DotDiameter;
            set
            {
                if (value < 1) value = 1;
                if (value > 20) value = 20;
                value = Math.Round(value, 1);
                settings.DotDiameter = value; NotifyPropertyChanged();
            }
        }
        //public int DotColorA { get => settings.DotColorA; set { settings.DotColorA = value; NotifyPropertyChanged(); } }
        //public int DotColorR { get => settings.DotColorR; set { settings.DotColorR = value; NotifyPropertyChanged(); } }
        //public int DotColorG { get => settings.DotColorG; set { settings.DotColorG = value; NotifyPropertyChanged(); } }
        //public int DotColorB { get => settings.DotColorB; set { settings.DotColorB = value; NotifyPropertyChanged(); } }
        //public string DotColorString { get => $"#{DotColorA:X2}{DotColorR:X2}{DotColorG:X2}{DotColorB:X2}"; }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; NotifyPropertyChanged(); } }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Angle
        public double AngleThickness { get => settings.AngleThickness; set { settings.AngleThickness = value; NotifyPropertyChanged(); } }
        public double AngleLength { get => settings.AngleLength; set { settings.AngleLength = value; NotifyPropertyChanged(); } }
        public double AngleAngle { get => settings.AngleAngle; set { settings.AngleAngle = value; NotifyPropertyChanged(); } }
        //public int AngleColorA { get => settings.AngleColorA; set { settings.AngleColorA = value; NotifyPropertyChanged(); } }
        //public int AngleColorR { get => settings.AngleColorR; set { settings.AngleColorR = value; NotifyPropertyChanged(); } }
        //public int AngleColorG { get => settings.AngleColorG; set { settings.AngleColorG = value; NotifyPropertyChanged(); } }
        //public int AngleColorB { get => settings.AngleColorB; set { settings.AngleColorB = value; NotifyPropertyChanged(); } }
        //public string AngleColorString { get => $"#{AngleColorA:X2}{AngleColorR:X2}{AngleColorG:X2}{AngleColorB:X2}"; }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; NotifyPropertyChanged(); } }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }
        public string AnglePoints { get => $"0,0 {AngleLength},0 {AngleLength - AngleThickness * (90 - (double)AngleAngle) / 45 * (AngleAngle < 45 ? (1 + (45 - AngleAngle) * 0.02) : (1 + (45 - AngleAngle) * 0.008))},{AngleThickness} {AngleThickness - AngleThickness * (55 - (double)AngleAngle) / 45},{AngleThickness}"; }

        // Cross
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = value; NotifyPropertyChanged(); } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = value; NotifyPropertyChanged(); } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = value; NotifyPropertyChanged(); } }
        public string CrossOffsetString { get => $"0,{CrossOffset},0,0"; }
        public double CrossRotation { get => settings.CrossRotation; set { settings.CrossRotation = value; NotifyPropertyChanged(); } }
        //public int CrossColorA { get => settings.CrossColorA; set { settings.CrossColorA = value; NotifyPropertyChanged(); } }
        //public int CrossColorR { get => settings.CrossColorR; set { settings.CrossColorR = value; NotifyPropertyChanged(); } }
        //public int CrossColorG { get => settings.CrossColorG; set { settings.CrossColorG = value; NotifyPropertyChanged(); } }
        //public int CrossColorB { get => settings.CrossColorB; set { settings.CrossColorB = value; NotifyPropertyChanged(); } }
        //public string CrossColorString { get => $"#{CrossColorA:X2}{CrossColorR:X2}{CrossColorG:X2}{CrossColorB:X2}"; }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; NotifyPropertyChanged(); } }
        public string CrossVisibilityString { get => CrossVisibility ? "Visible" : "Collapsed"; }

        public ReticleSettings()
        {
            SettingsList = ((App)Application.Current).SettingsList;
            settings = SettingsList.List[SettingsList.ChosenOne];

            this.InitializeComponent();

            DataContext = this;
        }

        private void NotifyAllProperties()
        {
            //NotifyPropertyChanged(nameof(DotDiameter));
            //NotifyPropertyChanged(nameof(DotColorString));
            //NotifyPropertyChanged(nameof(DotVisibilityString));

            //NotifyPropertyChanged(nameof(AngleColorString));
            //NotifyPropertyChanged(nameof(AngleVisibilityString));
            //NotifyPropertyChanged(nameof(AnglePoints));

            //NotifyPropertyChanged(nameof(CrossThickness));
            //NotifyPropertyChanged(nameof(CrossLength));
            //NotifyPropertyChanged(nameof(CrossOffsetString));
            //NotifyPropertyChanged(nameof(CrossRotation));
            //NotifyPropertyChanged(nameof(CrossColorString));
            //NotifyPropertyChanged(nameof(CrossVisibilityString));
        }

        private void New_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetDefault_button_Click(object sender, RoutedEventArgs e)
        {
            //var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
            //    ((App)Application.Current).reticleUWP.NotifyAllProperties();
            //});
        }
    }
}
