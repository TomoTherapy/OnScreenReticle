using Microsoft.Gaming.XboxGameBar;
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
    public sealed partial class Reticle : Page, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private XboxGameBarWidget widget;
        private Settings settings;

        // Location
        public int Top { get => settings.Top; set { settings.Top = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public int Left { get => settings.Left; set { settings.Left = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public string MarginString { get => $"{Left},{Top},0,0"; }

        // Dot
        public double DotDiameter { get => settings.DotDiameter; set { settings.DotDiameter = value; } }
        public int DotColorA { get => settings.DotColorA; set { settings.DotColorA = value; } }
        public int DotColorR { get => settings.DotColorR; set { settings.DotColorR = value; } }
        public int DotColorG { get => settings.DotColorG; set { settings.DotColorG = value; } }
        public int DotColorB { get => settings.DotColorB; set { settings.DotColorB = value; } }
        public string DotColorString { get => $"#{DotColorA.ToString("X2")}{DotColorR.ToString("X2")}{DotColorG.ToString("X2")}{DotColorB.ToString("X2")}"; }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; } }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Angle
        public double AngleThickness { get => settings.AngleThickness; set { settings.AngleThickness = value; } }
        public double AngleLength { get => settings.AngleLength; set { settings.AngleLength = value; } }
        public double AngleAngle { get => settings.AngleAngle; set { settings.AngleAngle = value; } }
        public int AngleColorA { get => settings.AngleColorA; set { settings.AngleColorA = value; } }
        public int AngleColorR { get => settings.AngleColorR; set { settings.AngleColorR = value; } }
        public int AngleColorG { get => settings.AngleColorG; set { settings.AngleColorG = value; } }
        public int AngleColorB { get => settings.AngleColorB; set { settings.AngleColorB = value; } }
        public string AngleColorString { get => $"#{AngleColorA.ToString("X2")}{AngleColorR.ToString("X2")}{AngleColorG.ToString("X2")}{AngleColorB.ToString("X2")}"; }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; } }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }

        // Cross
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = value; } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = value; } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = value; } }
        public double CrossRotation { get => settings.CrossRotation; set { settings.CrossRotation = value; } }
        public int CrossColorA { get => settings.CrossColorA; set { settings.CrossColorA = value; } }
        public int CrossColorR { get => settings.CrossColorR; set { settings.CrossColorR = value; } }
        public int CrossColorG { get => settings.CrossColorG; set { settings.CrossColorG = value; } }
        public int CrossColorB { get => settings.CrossColorB; set { settings.CrossColorB = value; } }
        public string CrossColorString { get => $"#{CrossColorA.ToString("X2")}{CrossColorR.ToString("X2")}{CrossColorG.ToString("X2")}{CrossColorB.ToString("X2")}"; }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; } }
        public string CrossVisibilityString { get => CrossVisibility ? "Visible" : "Collapsed"; }


        public Reticle()
        {
            this.InitializeComponent();
            settings = ((App)Application.Current).Settings;

            DataContext = this;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            widget = e.Parameter as XboxGameBarWidget;

            widget.SettingsClicked += Widget_SettingsClicked;

            await widget.CenterWindowAsync();
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await sender.ActivateSettingsAsync();

            await sender.CenterWindowAsync();
        }
    }
}
