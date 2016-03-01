using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Backlog
{
    class Register
    {
        private string dbPass;
        private string connectionString;
        private string username;
        private string email;
        private string password;
        private string hash;

        public Register(string username, string password, string email)
        {
            this.username = username;
            this.password = MD5ForPHP(password);
            this.email = email;
            this.dbPass = GetDatabaseInfo();
            this.hash = GenerateVerificationHash();
            this.connectionString = "user=H3090;database=H3090_1;server=mysql.labranet.jamk.fi;password=" + this.dbPass + ";";
        }
        private string GenerateVerificationHash()
        {
            Random rand = new Random();
            string hash = MD5ForPHP(rand.Next(0, 1000).ToString());
            return hash;
        }

        private string MD5ForPHP(string textToHash)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] unhashed = Encoding.UTF8.GetBytes((textToHash).ToLower());
            byte[] hashed = md5.ComputeHash(unhashed);
            StringBuilder builder = new StringBuilder();

            foreach (var b in hashed)
            {
                builder.Append(b.ToString("x").ToLower());
            }
            return builder.ToString();
        }

        public bool RegisterUser()
        {
            MySqlConnection connection = new MySqlConnection(this.connectionString);
            try
            {
                connection.Open();
                string commandText = "INSERT INTO user (uid, password, email, hash) VALUES (@UID, @PASSWORD, @EMAIL, @HASH)";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", this.username);
                command.Parameters.AddWithValue("@PASSWORD", this.password);
                command.Parameters.AddWithValue("@EMAIL", this.email);
                command.Parameters.AddWithValue("@HASH", this.hash);
                int rowCount = command.ExecuteNonQuery();
                if (rowCount == 1)
                {
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
