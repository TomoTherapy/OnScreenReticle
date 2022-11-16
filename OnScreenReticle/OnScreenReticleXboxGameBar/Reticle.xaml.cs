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

        public ObservableCollection<Settings> SettingsList { get => ((App)Application.Current).SettingsList.List; set => ((App)Application.Current).SettingsList.List = value; }
        public int SettingsListIndex
        {
            get
            {
                return ((App)Application.Current).SettingsList.ChosenOne;
            }
            set
            {
                ((App)Application.Current).SettingsList.ChosenOne = value;
                NotifyAllProperties();
            }
        }

        public Settings Settings { get => settings; set { settings = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public Color ThemeColor { get => Settings.ThemeColor; set { Settings.ThemeColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }

        // Location
        public double Top
        {
            get => Settings.Top - 300;
            set
            {
                double val = value + 300;
                if (val < 0) val = 0;
                if (val > 600) val = 600;
                Settings.Top = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(MarginString));
                Settings.NotifyAllProperties();
                Top_textBox.Text = Math.Round(val - 300, 1).ToString();
            }
        }
        public double Left
        {
            get => Settings.Left - 125;
            set
            {
                double val = value + 125;
                if (val < 0) val = 0;
                if (val > 250) val = 250;
                Settings.Left = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(MarginString));
                Settings.NotifyAllProperties();
                Left_textBox.Text = Math.Round(val - 125, 1).ToString();
            }
        }
        public string MarginString { get => $"{Left + 125},{Top + 300},0,0"; }

        // Dot
        public double DotDiameter
        {
            get => Settings.DotDiameter;
            set
            {
                double val = value;
                if (val < 1) val = 1;
                if (val > 30) val = 30;
                Settings.DotDiameter = val;
                NotifyPropertyChanged();
                Settings.NotifyAllProperties();
                DotDiameter_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public Color DotColor { get => Settings.DotColor; set { Settings.DotColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public bool DotVisibility { get => Settings.DotVisibility; set { Settings.DotVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(DotVisibilityString)); Settings.NotifyAllProperties(); } }
        public string DotVisibilityString { get => Settings.DotVisibilityString; }

        // Chevron
        public double ChevronThickness
        {
            get => Settings.ChevronThickness;
            set
            {
                double val = value;
                if (val < 1) val = 1;
                if (val > 10) val = 10;
                Settings.ChevronThickness = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChevronPoints));
                Settings.NotifyAllProperties();
                ChevronThickness_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public double ChevronLength
        {
            get => Settings.ChevronLength;
            set
            {
                double val = value;
                if (val < 5) val = 5;
                if (val > 50) val = 50;
                Settings.ChevronLength = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChevronPoints));
                Settings.NotifyAllProperties();
                ChevronLength_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public double ChevronAngle
        {
            get => Settings.ChevronAngle;
            set
            {
                double val = value;
                if (val < 35) val = 35;
                if (val > 70) val = 70;
                Settings.ChevronAngle = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(ChevronPoints));
                Settings.NotifyAllProperties();
                ChevronAngle_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public Color ChevronColor { get => Settings.ChevronColor; set { Settings.ChevronColor = value; NotifyPropertyChanged(); Settings.NotifyAllProperties(); } }
        public bool ChevronVisibility { get => Settings.ChevronVisibility; set { Settings.ChevronVisibility = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(ChevronVisibilityString)); Settings.NotifyAllProperties(); } }
        public string ChevronVisibilityString { get => Settings.ChevronVisibilityString; }
        public string ChevronPoints { get => Settings.ChevronPoints; }

        // Cross
        public double CrossThickness
        {
            get => Settings.CrossThickness;
            set
            {
                double val = value;
                if (val < 1) val = 1;
                if (val > 20) val = 20;
                Settings.CrossThickness = val;
                NotifyPropertyChanged();
                Settings.NotifyAllProperties();
                CrossThickness_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public double CrossLength
        {
            get => Settings.CrossLength;
            set
            {
                double val = value;
                if (val < 1) val = 1;
                if (val > 50) val = 50;
                Settings.CrossLength = val;
                NotifyPropertyChanged();
                Settings.NotifyAllProperties();
                CrossLength_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public double CrossOffset
        {
            get => Settings.CrossOffset;
            set
            {
                double val = value;
                if (val < 0) val = 0;
                if (val > 30) val = 30;
                Settings.CrossOffset = val;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CrossOffsetString));
                Settings.NotifyAllProperties();
                CrossOffset_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
        public string CrossOffsetString { get => Settings.CrossOffsetString; }
        public double CrossRotation
        {
            get => Settings.CrossRotation;
            set
            {
                double val = value;
                if (val < 0) val = 0;
                if (val > 90) val = 90;
                Settings.CrossRotation = val;
                NotifyPropertyChanged();
                Settings.NotifyAllProperties();
                CrossRotation_textBox.Text = Math.Round(val, 1).ToString();
            }
        }
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

            Settings temp = ((sender as Button).Parent as Grid).DataContext as Settings;
            SettingsList.Remove(temp);
        }

        private void SetDefault_button_Click(object sender, RoutedEventArgs e)
        {
            Top = 0;
            Left = 0;
            ThemeColor = new Color() { A = 0xB2, R = 0, G = 0xFF, B = 0xBF };

            DotDiameter = 6;
            DotColor = new Color() { A = 0xFF, R = 0xFA, G = 0xA, B = 0xA };
            DotVisibility = true;

            ChevronThickness = 3;
            ChevronLength = 13;
            ChevronAngle = 50;//35~70
            ChevronColor = new Color() { A = 0xFF, R = 0xFA, G = 0xA, B = 0xA };
            ChevronVisibility = true;

            CrossThickness = 3.5;
            CrossLength = 10;
            CrossOffset = 10;
            CrossRotation = 0;
            CrossColor = new Color() { A = 0xFF, R = 0xFA, G = 0xA, B = 0xA };
            CrossVisibility = true;
        }

        private void DotColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            DotColorButton.Flyout.Hide();
        }

        private void ChevronColorPickerClose_Click(object sender, RoutedEventArgs e)
        {
            ChevronColorButton.Flyout.Hide();
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

                NotifyPropertyChanged(nameof(DotDiameter));
                NotifyPropertyChanged(nameof(DotColor));
                NotifyPropertyChanged(nameof(DotVisibility));
                NotifyPropertyChanged(nameof(DotVisibilityString));

                NotifyPropertyChanged(nameof(ChevronThickness));
                NotifyPropertyChanged(nameof(ChevronLength));
                NotifyPropertyChanged(nameof(ChevronAngle));
                NotifyPropertyChanged(nameof(ChevronPoints));
                NotifyPropertyChanged(nameof(ChevronColor));
                NotifyPropertyChanged(nameof(ChevronVisibility));
                NotifyPropertyChanged(nameof(ChevronVisibilityString));

                NotifyPropertyChanged(nameof(CrossThickness));
                NotifyPropertyChanged(nameof(CrossLength));
                NotifyPropertyChanged(nameof(CrossOffset));
                NotifyPropertyChanged(nameof(CrossOffsetString));
                NotifyPropertyChanged(nameof(CrossRotation));
                NotifyPropertyChanged(nameof(CrossColor));
                NotifyPropertyChanged(nameof(CrossVisibility));
                NotifyPropertyChanged(nameof(CrossVisibilityString));
            });
        }
    }
}
