using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Backlog
{
    class Login
    {
        private string connectionString;
        private string username;
        private string password;
        private MD5forPHP converter = new MD5forPHP();

        public Login(string username, string password)
        {
            this.username = username;
            this.password = this.converter.ConvertToPHP(password);
            this.connectionString = Properties.Settings.Default.Database;
        }

        private int CheckIfVerified(MySqlCommand command)
        {
            int verified = 0;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                verified = reader.GetInt32("verified");
            }
            return verified;
        }

        public bool CheckCredentials()
        {
            int verified = 1;
            MySqlConnection connection = new MySqlConnection(this.connectionString);
            try
            {
                connection.Open();
                string commandText = "SELECT * FROM user WHERE uid = @UID AND password = @PASSWORD";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", this.username);
                command.Parameters.AddWithValue("@PASSWORD", this.password);              
                command.ExecuteNonQuery();
                int account = CheckIfVerified(command);
                if (account == verified)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login: " + ex.Message);
                return false;
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
