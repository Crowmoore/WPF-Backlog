using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace Backlog
{
    class BLController
    {
        public static DataTable GetAllUsersGames(string user)
        {
            try
            {
                DataTable table = Database.GetAllUsersGamesFromDatabase(user);
                return table;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static List<string> GetAllGenres()
        {
            try
            {
                DataTable genres = Database.GetAllGenresFromDatabase();
                List<string> names = new List<string>();
                foreach (DataRow row in genres.Rows)
                {
                    string name = row[0].ToString();
                    names.Add(name);
                }
                return names;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetSearchFilter(string search)
        {
            return string.Format("name LIKE '%{0}%'", search);
        }

        public static string GetGenreFilter(string genre)
        {
            return string.Format("genre = '{0}'", genre);
        }

        public static string GetStatusFilter(string status)
        {
            return string.Format("status = '{0}'", status);
        }

        public static int GetProgress(DataTable table, string status)
        {
            return table.AsEnumerable().Where(x => x["Status"].ToString() == status).ToList().Count;
        }

        public static double GetPercentage(int count, int total)
        {
            return Math.Round(((double)count / (double)total) * 100, 2);
        }

        public static bool AddNewGame(string user, string title, string achievements, string progress, string comment, string genre)
        {
            try
            {
                Database.AddNewGame(user, title, achievements, progress, comment, genre);
                return true;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public static bool AddNewGenre(string genre)
        {
            try
            {
                Database.AddGenre(genre);
                return true;
            }
            catch(Exception)
            {
                throw;
            }
            
        }

        public static bool DeleteGame(int id)
        {
            try
            {
                Database.DeleteGameFromDatabase(id);
                return true;
            }
            catch(Exception)
            {
                throw;
            }           
        }

        public static bool UpdateGame(int id, string user, string title, string status, string achievements, string genre, string comment)
        {
            try
            {
                Database.UpdateGameInfoToDatabase(id, user, title, status, achievements, genre, comment);
                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
