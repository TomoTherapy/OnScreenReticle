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
using Windows.UI;
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

        public Color ThemeColor { get => settings.ThemeColor; set { settings.ThemeColor = value; NotifyPropertyChanged(); } }

        // Location
        public double Top { get => settings.Top - 300; set { settings.Top = value + 300; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public double Left { get => settings.Left - 125; set { settings.Left = value + 125; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public string MarginString { get => $"{Left + 125},{Top + 300},0,0"; }

        // Dot
        public double DotDiameter { get => settings.DotDiameter; set { settings.DotDiameter = value; NotifyPropertyChanged(); } }
        public Color DotColor { get => settings.DotColor; set { settings.DotColor = value; NotifyPropertyChanged(); } }
        public bool DotVisibility { get => settings.DotVisibility; set { settings.DotVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotVisibilityString)); } }
        public string DotVisibilityString { get => DotVisibility ? "Visible" : "Collapsed"; }

        // Angle
        public double AngleThickness { get => settings.AngleThickness; set { settings.AngleThickness = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public double AngleLength { get => settings.AngleLength; set { settings.AngleLength = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public double AngleAngle { get => settings.AngleAngle; set { settings.AngleAngle = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); } }
        public Color AngleColor { get => settings.AngleColor; set { settings.AngleColor = value; NotifyPropertyChanged(); } }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleVisibilityString)); } }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }
        public string AnglePoints { get => $"0,0 {AngleLength},0 {AngleLength - AngleThickness * (90 - (double)AngleAngle) / 45 * (AngleAngle < 45 ? (1 + (45 - AngleAngle) * 0.02) : (1 + (45 - AngleAngle) * 0.008))},{AngleThickness} {AngleThickness - AngleThickness * (55 - (double)AngleAngle) / 45},{AngleThickness}"; }

        // Cross
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = value; NotifyPropertyChanged(); } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = value; NotifyPropertyChanged(); } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossOffsetString)); } }
        public string CrossOffsetString { get => $"0,{CrossOffset},0,0"; }
        public double CrossRotation { get => settings.CrossRotation; set { settings.CrossRotation = value; NotifyPropertyChanged(); } }
        public Color CrossColor { get => settings.CrossColor; set { settings.CrossColor = value; NotifyPropertyChanged(); } }
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
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(GameBarState));
            });

            ((App)Application.Current).JsonParser.SerializeSettings();
        }

        public void NotifyClickThroughEnabledChanged(XboxGameBarWidget sender, object args)
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(ClickThroughEnabled));
            });
        }

        public void NotifyPinnedChanged(XboxGameBarWidget sender, object args)
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
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
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(MarginString));

                NotifyPropertyChanged(nameof(DotDiameter));
                NotifyPropertyChanged(nameof(DotVisibilityString));

                NotifyPropertyChanged(nameof(AngleVisibilityString));
                NotifyPropertyChanged(nameof(AnglePoints));

                NotifyPropertyChanged(nameof(CrossThickness));
                NotifyPropertyChanged(nameof(CrossLength));
                NotifyPropertyChanged(nameof(CrossOffsetString));
                NotifyPropertyChanged(nameof(CrossRotation));
                NotifyPropertyChanged(nameof(CrossVisibilityString));
            });
        }


        private void DotColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            DotColorButton.Flyout.Hide();
        }

        private void AngleColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            AngleColorButton.Flyout.Hide();
        }

        private void CrossColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            CrossColorButton.Flyout.Hide();
        }

        private void ThemeColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            ThemeColorButton.Flyout.Hide();
        }
    }
}
