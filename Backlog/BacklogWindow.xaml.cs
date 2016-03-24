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
        private Database database = new Database();
        private string user;

        public BacklogWindow(string user)
        {
            InitializeComponent();
            this.user = user;
            ListUserGames(user);
            PopulateComboBox();
            tbTitle.Text = user + "'s Backlog";
        }

        private void ListUserGames(string user)
        {
            table = database.GetAllUsersGamesFromDatabase(user);
            view = table.DefaultView;
            dataGrid.ItemsSource = view;
        }

        private void PopulateComboBox()
        {
            DataTable genres = database.GetAllGenresFromDatabase();
            List<string> names = new List<string>();
            foreach (DataRow row in genres.Rows)
            {
                string name = row[0].ToString();
                names.Add(name);
            }
            cbGenres.ItemsSource = names;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {   
            string search = txtSearch.Text;
            
            string filter = string.Format("name LIKE '%{0}%'", search);
            view.RowFilter = filter;
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            view.RowFilter = string.Empty;
        }

        private void ShowProgress()
        {
            int finished = table.AsEnumerable().Where(x => x["Status"].ToString() == "Finished").ToList().Count;
            int inProgress = table.AsEnumerable().Where(x => x["Status"].ToString() == "In progress").ToList().Count;
            int notStarted = table.AsEnumerable().Where(x => x["Status"].ToString() == "Not started").ToList().Count;
            int mastered = table.AsEnumerable().Where(x => x["Status"].ToString() == "Mastered").ToList().Count;
            int abandoned = table.AsEnumerable().Where(x => x["Status"].ToString() == "Abandoned").ToList().Count;
            int total = table.Rows.Count;
            pbFinished.Value = GetPercentage(finished, total);
            pbInProgress.Value = GetPercentage(inProgress, total);
            pbNotStarted.Value = GetPercentage(notStarted, total);
            pbMastered.Value = GetPercentage(mastered, total);
            pbAbandoned.Value = GetPercentage(abandoned, total);

        }

        private double GetPercentage(int count, int total)
        {
            return Math.Round(((double)count / (double)total) * 100, 2);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabItem tab = tabControl.SelectedItem as TabItem;
            if (tab != null)
            {
                switch (tab.Header.ToString())
                {
                    case "My status":
                        ShowProgress();
                        break;
                    case "My games":
                        ListUserGames(user);
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
            var selected = spProgress.Children.OfType<RadioButton>()
                         .FirstOrDefault(button => button.IsChecked.HasValue && button.IsChecked.Value);
            string progress = selected.ToolTip.ToString();
            string user = this.user;
            string genre = txtGenre.Text;
            string comment = txtComment.Text;

            database.AddGenre(genre);
            database.AddNewGame(user, title, achievements, progress, comment, genre);
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
                string filter = string.Format("genre = '{0}'", genre);
                view.RowFilter = filter;
            }
        }

        private void rb_Checked(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            string status = button.ToolTip as string;

            string filter = string.Format("status = '{0}'", status);
            view.RowFilter = filter;
        }
    }
}
