using Microsoft.Gaming.XboxGameBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Settings> SettingsList { get => ((App)Application.Current).SettingsList.List; set => ((App)Application.Current).SettingsList.List = value; }
        public int SettingsListIndex
        {
            get
            {
                Settings = SettingsList[((App)Application.Current).SettingsList.ChosenOne];
                return ((App)Application.Current).SettingsList.ChosenOne;
            }
            set
            {
                ((App)Application.Current).SettingsList.ChosenOne = value;
                Settings = SettingsList[((App)Application.Current).SettingsList.ChosenOne];
                NotifyAllProperties();
            }
        }
        public Settings Settings { get => settings; set { settings = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public string NewSettingName { get => newSettingName; set { newSettingName = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }

        public Color ThemeColor { get => Settings.ThemeColor; set { Settings.ThemeColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }

        // Location
        public double Top { get => Settings.Top - 300; set { Settings.Top = value + 300; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); Settings.NotifyAllProperties(); } }
        public double Left { get => Settings.Left - 125; set { Settings.Left = value + 125; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(MarginString)); Settings.NotifyAllProperties(); } }
        public string MarginString { get => $"{Left + 125},{Top + 300},0,0"; }

        // Dot
        public double DotDiameter { get => Settings.DotDiameter; set { Settings.DotDiameter = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public Color DotColor { get => Settings.DotColor; set { Settings.DotColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public bool DotVisibility { get => Settings.DotVisibility; set { Settings.DotVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotVisibilityString)); Settings.NotifyAllProperties(); } }
        public string DotVisibilityString { get => Settings.DotVisibilityString; }

        // Angle
        public double AngleThickness { get => Settings.AngleThickness; set { Settings.AngleThickness = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); Settings.NotifyAllProperties(); } }
        public double AngleLength { get => Settings.AngleLength; set { Settings.AngleLength = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); Settings.NotifyAllProperties(); } }
        public double AngleAngle { get => Settings.AngleAngle; set { Settings.AngleAngle = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AnglePoints)); Settings.NotifyAllProperties(); } }
        public Color AngleColor { get => Settings.AngleColor; set { Settings.AngleColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public bool AngleVisibility { get => Settings.AngleVisibility; set { Settings.AngleVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(AngleVisibilityString)); Settings.NotifyAllProperties(); } }
        public string AngleVisibilityString { get => Settings.AngleVisibilityString; }
        public string AnglePoints { get => Settings.AnglePoints; }

        // Cross
        public double CrossThickness { get => Settings.CrossThickness; set { Settings.CrossThickness = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public double CrossLength { get => Settings.CrossLength; set { Settings.CrossLength = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public double CrossOffset { get => Settings.CrossOffset; set { Settings.CrossOffset = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossOffsetString)); Settings.NotifyAllProperties(); } }
        public string CrossOffsetString { get => Settings.CrossOffsetString; }
        public double CrossRotation { get => Settings.CrossRotation; set { Settings.CrossRotation = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public Color CrossColor { get => Settings.CrossColor; set { Settings.CrossColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public bool CrossVisibility { get => Settings.CrossVisibility; set { Settings.CrossVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(CrossVisibilityString)); Settings.NotifyAllProperties(); } }
        public string CrossVisibilityString { get => Settings.CrossVisibilityString; }

        public string GameBarState { get => widget.GameBarDisplayMode == 0 ? "Visible" : "Collapsed"; }
        public string ClickThroughEnabled { get => !widget.ClickThroughEnabled ? "Visible" : "Collapsed"; }
        public string Pinned { get => !widget.Pinned ? "Visible" : "Collapsed"; }

        public Reticle()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Settings = ((App)Application.Current).Settings;

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
            SettingsList.Add(new Settings());
        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsList.Count == 1) return;

            int index = SettingsListIndex;
            if (SettingsListIndex > 0) SettingsListIndex--;
            SettingsList.RemoveAt(index);
        }

        private void SetDefault_button_Click(object sender, RoutedEventArgs e)
        {
            Top = 300;
            Left = 125;
            ThemeColor = new Color() { A = 0xB2, R = 0, G = 0xFF, B = 0xBF };

            DotDiameter = 6;
            DotColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            DotVisibility = true;

            AngleThickness = 3;
            AngleLength = 13;
            AngleAngle = 50;//35~70
            AngleColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            AngleVisibility = true;

            CrossThickness = 3.5;
            CrossLength = 10;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColor = new Color() { A = 255, R = 250, G = 10, B = 10 };
            CrossVisibility = true;
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

        public void NotifyAllProperties()
        {
            var ignore = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(Top));
                NotifyPropertyChanged(nameof(Left));
                NotifyPropertyChanged(nameof(MarginString));

                NotifyPropertyChanged(nameof(DotColor));
                NotifyPropertyChanged(nameof(DotDiameter));
                NotifyPropertyChanged(nameof(DotVisibility));
                NotifyPropertyChanged(nameof(DotVisibilityString));

                NotifyPropertyChanged(nameof(AngleColor));
                NotifyPropertyChanged(nameof(AngleVisibility));
                NotifyPropertyChanged(nameof(AngleVisibilityString));
                NotifyPropertyChanged(nameof(AnglePoints));

                NotifyPropertyChanged(nameof(CrossColor));
                NotifyPropertyChanged(nameof(CrossThickness));
                NotifyPropertyChanged(nameof(CrossLength));
                NotifyPropertyChanged(nameof(CrossOffsetString));
                NotifyPropertyChanged(nameof(CrossRotation));
                NotifyPropertyChanged(nameof(CrossVisibility));
                NotifyPropertyChanged(nameof(CrossVisibilityString));
            });
        }

        private void ReticleList_dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
