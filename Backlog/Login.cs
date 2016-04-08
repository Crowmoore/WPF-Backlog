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
        public static bool CheckCredentials(string username, string password)
        {
            MD5forPHP converter = new MD5forPHP();
            string hashedPassword = converter.ConvertToPHP(password);
            try
            {
                if(Database.CheckUsersCredentialsFromDatabase(username, hashedPassword))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
