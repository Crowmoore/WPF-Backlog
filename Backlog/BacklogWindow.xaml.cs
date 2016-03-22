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

        public BacklogWindow(string user)
        {
            InitializeComponent();
            ListUserGames(user);
            tbTitle.Text = user + "'s Backlog";
        }

        private void ListUserGames(string user)
        {
            table = database.GetTestData();
            view = table.DefaultView;
            dataGrid.ItemsSource = view;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
          
            string search = txtSearch.Text;
            string category = cbCategory.SelectedValue.ToString();
            
            string filter = string.Format("Title LIKE '%{0}%'", search);
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
            return Math.Round((((double)count / (double)total) * 100), 2);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(tabStatus != null && tabStatus.IsSelected)
            {
                ShowProgress();
            }
        }
    }
}
