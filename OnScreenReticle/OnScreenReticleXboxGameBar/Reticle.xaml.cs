using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        // Location
        public double Top { get => settings.Top - 300; set { settings.Top = value + 300; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public double Left { get => settings.Left - 125; set { settings.Left = value + 125; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public string MarginString { get => $"{Left + 125},{Top + 300},0,0"; }

        // Dot
        public double DotDiameter { get => settings.DotDiameter; set { settings.DotDiameter = value; NotifyPropertyChanged(); } }
        public int DotColorA { get => settings.DotColorA; set { settings.DotColorA = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotColorString)); } }
        public int DotColorR { get => settings.DotColorR; set { settings.DotColorR = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotColorString)); } }
        public int DotColorG { get => settings.DotColorG; set { settings.DotColorG = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotColorString)); } }
        public int DotColorB { get => settings.DotColorB; set { settings.DotColorB = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotColorString)); } }
        public string DotColorString { get => $"#{DotColorA:X2}{DotColorR:X2}{DotColorG:X2}{DotColorB:X2}"; }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotVisibilityString)); } }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Angle
        public double AngleThickness { get => settings.AngleThickness; set { settings.AngleThickness = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public double AngleLength { get => settings.AngleLength; set { settings.AngleLength = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public double AngleAngle { get => settings.AngleAngle; set { settings.AngleAngle = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public int AngleColorA { get => settings.AngleColorA; set { settings.AngleColorA = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleColorString)); } }
        public int AngleColorR { get => settings.AngleColorR; set { settings.AngleColorR = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleColorString)); } }
        public int AngleColorG { get => settings.AngleColorG; set { settings.AngleColorG = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleColorString)); } }
        public int AngleColorB { get => settings.AngleColorB; set { settings.AngleColorB = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleColorString)); } }
        public string AngleColorString { get => $"#{AngleColorA:X2}{AngleColorR:X2}{AngleColorG:X2}{AngleColorB:X2}"; }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleVisibilityString)); } }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }
        public string AnglePoints { get => $"0,0 {AngleLength},0 {AngleLength - AngleThickness * (90 - (double)AngleAngle) / 45 * (AngleAngle < 45 ? (1 + (45 - AngleAngle) * 0.02) : (1 + (45 - AngleAngle) * 0.008))},{AngleThickness} {AngleThickness - AngleThickness * (55 - (double)AngleAngle) / 45},{AngleThickness}"; }

        // Cross
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = value; NotifyPropertyChanged(); } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = value; NotifyPropertyChanged(); } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossOffsetString)); } }
        public string CrossOffsetString { get => $"0,{CrossOffset},0,0"; }
        public double CrossRotation { get => settings.CrossRotation; set { settings.CrossRotation = value; NotifyPropertyChanged(); } }
        public int CrossColorA { get => settings.CrossColorA; set { settings.CrossColorA = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossColorString)); } }
        public int CrossColorR { get => settings.CrossColorR; set { settings.CrossColorR = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossColorString)); } }
        public int CrossColorG { get => settings.CrossColorG; set { settings.CrossColorG = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossColorString)); } }
        public int CrossColorB { get => settings.CrossColorB; set { settings.CrossColorB = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossColorString)); } }
        public string CrossColorString { get => $"#{CrossColorA:X2}{CrossColorR:X2}{CrossColorG:X2}{CrossColorB:X2}"; }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossVisibilityString)); } }
        public string CrossVisibilityString { get => CrossVisibility ? "Visible" : "Collapsed"; }

        public string GameBarState { get => widget.GameBarDisplayMode == 0 ? "Visible" : "Collapsed"; }
        public string ClickThroughEnabled { get => !widget.ClickThroughEnabled ? "Visible" : "Collapsed"; }
        public string Pinned { get => !widget.Pinned ? "Visible" : "Collapsed"; }

        public Reticle()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            settings = ((App)Application.Current).Settings;

            widget = e.Parameter as XboxGameBarWidget;
            widget.SettingsClicked += Widget_SettingsClicked;
            widget.GameBarDisplayModeChanged += NotifyGameBarDisplayModeChanged;
            widget.ClickThroughEnabledChanged += NotifyClickThroughEnabledChanged;
            widget.PinnedChanged += NotifyPinnedChanged;

            widget.HorizontalResizeSupported = true;
            widget.VerticalResizeSupported = true;
            Windows.Foundation.Size size;
            size.Width = 1000;
            size.Height = 700;
            await widget.TryResizeWindowAsync(size);
            widget.HorizontalResizeSupported = false;
            widget.VerticalResizeSupported = false;

            await widget.CenterWindowAsync();

            DataContext = this;
        }

        private async void Widget_SettingsClicked(XboxGameBarWidget sender, object args)
        {
            await sender.ActivateSettingsAsync();
        }

        public void NotifyGameBarDisplayModeChanged(XboxGameBarWidget sender, object args)
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { 
                NotifyPropertyChanged(nameof(GameBarState));
            });
        }

        public void NotifyClickThroughEnabledChanged(XboxGameBarWidget sender, object args)
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                NotifyPropertyChanged(nameof(ClickThroughEnabled));
            });
        }

        public void NotifyPinnedChanged(XboxGameBarWidget sender, object args)
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                NotifyPropertyChanged(nameof(Pinned));
            });
        }

        private async void Center_button_Click(object sender, RoutedEventArgs e)
        {
            widget.HorizontalResizeSupported = true;
            widget.VerticalResizeSupported = true;
            Windows.Foundation.Size size;
            size.Width = 1000;
            size.Height = 700;
            await widget.TryResizeWindowAsync(size);
            widget.HorizontalResizeSupported = false;
            widget.VerticalResizeSupported = false;

            await widget.CenterWindowAsync();
        }

        private void New_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetDefault_button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void NotifyAllProperties()
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                NotifyPropertyChanged(nameof(MarginString));

                NotifyPropertyChanged(nameof(DotDiameter));
                NotifyPropertyChanged(nameof(DotColorString));
                NotifyPropertyChanged(nameof(DotVisibilityString));

                NotifyPropertyChanged(nameof(AngleColorString));
                NotifyPropertyChanged(nameof(AngleVisibilityString));
                NotifyPropertyChanged(nameof(AnglePoints));

                NotifyPropertyChanged(nameof(CrossThickness));
                NotifyPropertyChanged(nameof(CrossLength));
                NotifyPropertyChanged(nameof(CrossOffsetString));
                NotifyPropertyChanged(nameof(CrossRotation));
                NotifyPropertyChanged(nameof(CrossColorString));
                NotifyPropertyChanged(nameof(CrossVisibilityString));
            });
        }
    }
}
