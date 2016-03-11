﻿using System;
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
using VKAnalyzer.DTO;

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
        
        private void Auth()
        {
            AuthWindow window = new AuthWindow();
            window.Show();
        }

        private void GetGroupsButton_Click(object sender, RoutedEventArgs e)
        {
            Repository.Instance.RequestedUserID = UserIdTextBlock.Text;
            ListOfGroups.Text = Repository.GetGroups().Count().ToString();
        }

        private void CompareGroupsButton_Click(object sender, RoutedEventArgs e)
        {
            Repository.Instance.RequestedUserID = UserIdTextBlock.Text;
            ListBox.ItemsSource = Repository.Compare_groups();
        }

        private void FriendsComboBox_DropDownOpened(object sender, EventArgs e)
        {
            if (FriendsComboBox.HasItems == false)
                FriendsComboBox.ItemsSource = Repository.GetFriends();
        }

        


        private void FriendsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var u = (User)FriendsComboBox.SelectedItem;
            Repository.Instance.RequestedUserID = u.Uid;
            ListBox.ItemsSource = Repository.Compare_groups();
            ListBox1.ItemsSource = Repository.UG_info();

            // statistics below
            Statistics.Items.Clear();
            Statistics.Items.Add(String.Format("Total: {0} groups", Repository.total));
            double match_percent = Repository.pluses / Repository.total * 100;
            Statistics.Items.Add(String.Format("{0} groups matched ({1:0.#}%)", Repository.pluses, match_percent));
            
            double match_percent1 = Repository.small / Repository.total * 100;
            Statistics.Items.Add(String.Format("{0} small groups ({1:0.#}%)", Repository.small, match_percent1));

            double error_percent = 100 - (match_percent + match_percent1);
            Statistics.Items.Add(String.Format("\n Mismatch percent: {0:0.#}%", error_percent));
        }

      
    }
}
