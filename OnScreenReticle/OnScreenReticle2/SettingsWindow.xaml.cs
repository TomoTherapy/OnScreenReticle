using OnScreenReticle2.ViewModels;
using System.Windows;
using System.Windows.Forms;

namespace OnScreenReticle2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(MainWindow_ViewModel main)
        {
            InitializeComponent();
            DataContext = new SettingsWindow_ViewModel(main);

            this.Top = Screen.PrimaryScreen.Bounds.Height * 0.5 - 171;
            this.Left = Screen.PrimaryScreen.Bounds.Width * 0.45 - 287;
        }

        private void CenterScreen_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).CenterScreen_button_Click();
        }

        private void HuntShowdown_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).HuntShowdown_button_Click();
        }

        private void Default_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Default_button_Click();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Window_Closing();
        }

        private void Up_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Up_button_Click();
        }

        private void Left_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Left_button_Click();
        }

        private void Right_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Right_button_Click();
        }

        private void Down_button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsWindow_ViewModel).Down_button_Click();
        }
    }
}
