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
        public BacklogWindow(string username)
        {
            InitializeComponent();
            ListUserGames(username);
            tbTitle.Text = username + "'s Backlog";
        }

        private void ListUserGames(string username)
        {
            string dbPass = GetDatabaseInfo();
            MySqlConnection connection = new MySqlConnection("user=H3090;database=H3090_1;server=mysql.labranet.jamk.fi;password=" + dbPass + ";");
            try
            {
                connection.Open();
                string commandText = "SELECT name AS Title, progress_idprogress AS Status FROM game WHERE user = '" + username + "'";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.ExecuteNonQuery();
                DataTable table = new DataTable("game");
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table);
                dataGrid.ItemsSource = table.DefaultView;
                adapter.Update(table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        private string GetDatabaseInfo()
        {
            string dbPassword = "";
            try
            {
                using (StreamReader reader = new StreamReader("db.txt"))
                {
                    dbPassword = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dbPassword;
        }
    }
}
