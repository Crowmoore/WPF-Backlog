using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Backlog
{
    class Database
    {
        public DataTable GetTestData()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Achievements", typeof(string));
            table.Columns.Add("Status", typeof(string));
            table.Columns.Add("Comments", typeof(string));

            table.Rows.Add("Jazzpunk", "10/24", "Finished", "");
            table.Rows.Add("Cities: Skylines", "15/30", "In progress", "");
            table.Rows.Add("Testgame", "15/30", "Finished", "Remove later");
            table.Rows.Add("Darkest Dungeon", "20/40", "Abandoned", "Too hard");
            table.Rows.Add("Testgame2", "0/30", "Not started", "");
            table.Rows.Add("Testgame3", "30/30", "Mastered", "");

            return table;
        }

        public DataTable GetAllUsersGamesFromDatabase(string user)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {     
                connection.Open();
                string query = "SELECT game.name, progress.name AS status, achievements, comment " +
                               "FROM game " +
                               "INNER JOIN progress ON game.progress_idprogress = progress.idprogress " +
                               "WHERE user_uid = @UID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", user);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet set = new DataSet();
                adapter.Fill(set, "game");
                return set.Tables["game"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        public DataTable GetAllGenresFromDatabase()
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string query = "SELECT name FROM genre";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet set = new DataSet();
                adapter.Fill(set, "genre");
                return set.Tables["genre"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
