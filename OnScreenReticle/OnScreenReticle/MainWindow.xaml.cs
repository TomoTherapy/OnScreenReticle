using System.Windows;
using System.Windows.Input;

namespace OnScreenReticle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Center_menuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).Center_menuItem_Click();
        }

        private void Hunt_menuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).Hunt_menuItem_Click();
        }

        private void Exit_menuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).Center_menuItem_Click();
        }

        private void UpAdjust_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).UpAdjust_Click();
        }

        private void LeftAdjust_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).LeftAdjust_Click();
        }

        private void RightAdjust_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).RightAdjust_Click();
        }

        private void DownAdjust_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModel).DownAdjust_Click();
        }
    }
}
