using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VKAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Auth();
        }
        public static string requestedUserId { get; set; }
        private void Auth()
        {
            AuthWindow window = new AuthWindow();
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            requestedUserId = UserIdTextBlock.Text;
            ListOfGroups.Text = Repository.GetGroups().Count().ToString();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            requestedUserId = UserIdTextBlock.Text;
            ListBox.ItemsSource = Repository.Compare_groups();
        }
    }
}
