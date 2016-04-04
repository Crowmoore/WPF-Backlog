using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows;

namespace Backlog
{
    class Register
    {
        private string connectionString;
        private string username;
        private string email;
        private string password;
        private string hash;
        private MD5forPHP converter = new MD5forPHP();

        public Register(string username, string password, string email)
        {
            this.username = username;
            this.password = this.converter.ConvertToPHP(password);
            this.email = email;
            this.hash = GenerateVerificationHash();
            this.connectionString = Properties.Settings.Default.Database;
        }
        private string GenerateVerificationHash()
        {
            Random rand = new Random();
            string hash = this.converter.ConvertToPHP(rand.Next(0, 1000).ToString());
            return hash;
        }

        public bool RegisterUser()
        {
            MySqlConnection connection = new MySqlConnection(this.connectionString);
            try
            {
                connection.Open();
                string commandText = "INSERT INTO user (uid, password, email, hash, verified) VALUES (@UID, @PASSWORD, @EMAIL, @HASH, 1)";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", this.username);
                command.Parameters.AddWithValue("@PASSWORD", this.password);
                command.Parameters.AddWithValue("@EMAIL", this.email);
                command.Parameters.AddWithValue("@HASH", this.hash);
                int rowCount = command.ExecuteNonQuery();
                if (rowCount == 1)
                {
                    //Verification verificate = new Verification();
                    //verificate.SendVerification(this.username, this.email, this.hash);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("connection: " + ex.Message);
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

        public bool CheckPassword(string password)
        {
            int specialChars = password.Count(c => !char.IsLetterOrDigit(c));
            if (password.Length > 0 && password.Length < 20 && specialChars == 0)
            {
                return true;
            }
            return false;
        }

        public bool CheckUsername()
        {
            int specialChars = this.username.Count(c => !char.IsLetterOrDigit(c));
            if (this.username.Length > 0 && this.username.Length <= 10 && specialChars == 0) 
            {
                return true;
            }
            return false;
        }

        public bool CheckEmail()
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                    + "@"
                                    + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            Match match = regex.Match(this.email);
            if(match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
