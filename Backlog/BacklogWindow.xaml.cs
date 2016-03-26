using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Backlog
{
    /// <summary>
    /// Interaction logic for BacklogWindow.xaml
    /// </summary>
    public partial class BacklogWindow : Window
    {
        private DataTable table;
        private DataView view;
        private string user;

        public BacklogWindow(string user)
        {
            InitializeComponent();
            this.user = user;
            ListUsersGames(user);
            PopulateComboBoxWithGenres();
            tbTitle.Text = user + "'s Backlog";
        }

        private void ListUsersGames(string user)
        {
            table = BLController.GetAllUsersGames(user);
            view = table.DefaultView;
            dataGrid.ItemsSource = view;
        }

        private void PopulateComboBoxWithGenres()
        {
            cbGenres.ItemsSource = BLController.GetAllGenres();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {   
            view.RowFilter = BLController.GetSearchFilter(txtSearch.Text);
            sbStatus.Text = string.Format("Results filtered by title: '{0}'", txtSearch.Text);
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            view.RowFilter = string.Empty;
        }

        private void UpdateProgress()
        {
            int finished = BLController.GetProgress(table, "Finished");
            int inProgress = BLController.GetProgress(table, "In progress");
            int notStarted = BLController.GetProgress(table, "Not started");
            int mastered = BLController.GetProgress(table, "Mastered");
            int abandoned = BLController.GetProgress(table, "Abandoned");
            int total = table.Rows.Count;

            pbFinished.Value = BLController.GetPercentage(finished, total);
            pbInProgress.Value = BLController.GetPercentage(inProgress, total);
            pbNotStarted.Value = BLController.GetPercentage(notStarted, total);
            pbMastered.Value = BLController.GetPercentage(mastered, total);
            pbAbandoned.Value = BLController.GetPercentage(abandoned, total);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is TabControl)
            {
                TabControl tabControl = sender as TabControl;
                TabItem currentTab = tabControl.SelectedItem as TabItem;
                string current = currentTab.Header as string;
                sbStatus.Text = "";
                switch (current)
                {
                    case "My status":
                        UpdateProgress();
                        break;
                    case "My games":
                        ListUsersGames(user);
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text;
            string achievements = txtAchievementsGained.Text + "/" + txtAchievementsTotal.Text;
            RadioButton selected = spProgress.Children.OfType<RadioButton>()
                         .FirstOrDefault(button => button.IsChecked.HasValue && button.IsChecked.Value);
            string progress = selected.ToolTip as string;
            string user = this.user;
            string genre = txtGenre.Text;
            string comment = txtComment.Text;

            try
            {
                BLController.AddNewGenre(genre);
                BLController.AddNewGame(user, title, achievements, progress, comment, genre);
                sbStatus.Text = "Game added to database";
            }
            catch(Exception ex)
            {
                sbStatus.Text = ex.Message;
            }
            ClearForm();
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtAchievementsGained.Text = "";
            txtAchievementsTotal.Text = "";
            txtGenre.Text = "";
            txtComment.Text = ""; 
            rbNotStarted.IsChecked = true;
        }

        private void cbGenres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string genre = cbGenres.SelectedItem as string;
            if(genre != null)
            {
                view.RowFilter = BLController.GetGenreFilter(genre);
                sbStatus.Text = string.Format("Results filtered by genre: '{0}'", genre);
            }
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            string status = button.ToolTip as string;

            view.RowFilter = BLController.GetStatusFilter(status);
            sbStatus.Text = string.Format("Results filtered by status: '{0}'", status);
        }

        private void cm_Delete(object sender, RoutedEventArgs e)
        {

            try
            {
                DataRowView rowView = dataGrid.SelectedItem as DataRowView;
                string title = rowView.Row[1] as string;
                int id = (int)rowView.Row[0];
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + title + " from the database?", "Delete confirmation", MessageBoxButton.YesNo);
                switch (result.ToString())
                {
                    case "Yes":
                        try
                        {
                            BLController.DeleteGame(id);
                            ListUsersGames(user);
                            sbStatus.Text = title + " deleted from database";
                        }
                        catch (Exception ex)
                        {
                            sbStatus.Text = ex.Message;
                        }
                        break;
                    case "No":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                sbStatus.Text = ex.Message;
            }


        }

        private void cm_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView rowView = dataGrid.SelectedItem as DataRowView;
                string title = rowView["Name"].ToString(); ;
                int id = (int)rowView["idgame"];
                string status = rowView["Status"].ToString();
                string genre = rowView["Genre"].ToString();
                string achievements = rowView["Achievements"].ToString(); ;
                string comment = rowView["Comment"].ToString(); ;
                MessageBox.Show(genre);
                MessageBoxResult result = MessageBox.Show("Confirm data update for " + title, "Update confirmation", MessageBoxButton.YesNo);
                switch (result.ToString())
                {
                    case "Yes":
                        try
                        {
                            BLController.UpdateGame(id, user, title, status, achievements, genre, comment);
                            ListUsersGames(user);
                            sbStatus.Text = "Data updated for " + title;
                        }
                        catch (Exception ex)
                        {
                            sbStatus.Text = ex.Message;
                        }
                        break;
                    case "No":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                sbStatus.Text = ex.Message;
            }
        }
    }
}
