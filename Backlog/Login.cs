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
        private string username;
        private string password;
        private MD5forPHP converter = new MD5forPHP();

        public Login(string username, string password)
        {
            this.username = username;
            this.password = this.converter.ConvertToPHP(password);
        }

        public bool CheckCredentials()
        {
            try
            {
                Database.CheckUsersCredentialsFromDatabase(this.username, this.password);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
