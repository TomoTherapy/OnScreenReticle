using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

        // Location
        public double Top { get => settings.Top; set { settings.Top = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public double Left { get => settings.Left; set { settings.Left = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); } }
        public string MarginString { get => $"{Left},{Top},0,0"; }

        // Dot
        public double DotDiameter { get => settings.DotDiameter; set { settings.DotDiameter = value; } }
        public int DotColorA { get => settings.DotColorA; set { settings.DotColorA = value; } }
        public int DotColorR { get => settings.DotColorR; set { settings.DotColorR = value; } }
        public int DotColorG { get => settings.DotColorG; set { settings.DotColorG = value; } }
        public int DotColorB { get => settings.DotColorB; set { settings.DotColorB = value; } }
        public string DotColorString { get => $"#{DotColorA:X2}{DotColorR:X2}{DotColorG:X2}{DotColorB:X2}"; }
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
        public string AngleColorString { get => $"#{AngleColorA:X2}{AngleColorR:X2}{AngleColorG:X2}{AngleColorB:X2}"; }
        public bool AngleVisibility { get => settings.AngleVisibility; set { settings.AngleVisibility = value; } }
        public string AngleVisibilityString { get => AngleVisibility ? "Visible" : "Collapsed"; }

        public string AnglePoints { get => $"0,0 {AngleLength},0 {AngleLength - AngleThickness * (90 - (double)AngleAngle) / 45},{AngleThickness} {AngleThickness - AngleThickness * (55 - (double)AngleAngle) / 45},{AngleThickness}"; }

        // Cross
        public double CrossThickness { get => settings.CrossThickness; set { settings.CrossThickness = value; } }
        public double CrossLength { get => settings.CrossLength; set { settings.CrossLength = value; } }
        public double CrossOffset { get => settings.CrossOffset; set { settings.CrossOffset = value; } }
        public string CrossOffsetString { get => $"0,{CrossOffset},0,0"; }
        public double CrossRotation { get => settings.CrossRotation; set { settings.CrossRotation = value; } }
        public int CrossColorA { get => settings.CrossColorA; set { settings.CrossColorA = value; } }
        public int CrossColorR { get => settings.CrossColorR; set { settings.CrossColorR = value; } }
        public int CrossColorG { get => settings.CrossColorG; set { settings.CrossColorG = value; } }
        public int CrossColorB { get => settings.CrossColorB; set { settings.CrossColorB = value; } }
        public string CrossColorString { get => $"#{CrossColorA:X2}{CrossColorR:X2}{CrossColorG:X2}{CrossColorB:X2}"; }
        public bool CrossVisibility { get => settings.CrossVisibility; set { settings.CrossVisibility = value; } }
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
            await widget.CenterWindowAsync();
        }
    }
}
