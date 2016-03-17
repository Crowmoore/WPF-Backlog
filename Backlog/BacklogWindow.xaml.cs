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

        public BacklogWindow(string username)
        {
            InitializeComponent();
            ListUserGames(username);
            tbTitle.Text = username + "'s Backlog";
        }

        private void ListUserGames(string username)
        {
            Database database = new Database();
            table = database.GetAllUsersGamesFromDatabase(username);
            view = table.DefaultView;
            dataGrid.ItemsSource = view;
        }
    }
}
