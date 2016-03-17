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
            view.RowFilter = string.Format("Title LIKE '%{0}%'", search);
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            view.RowFilter = string.Empty;
        }
    }
}
