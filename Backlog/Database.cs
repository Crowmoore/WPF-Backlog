﻿using System;
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
        public static DataTable GetTestData()
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

        public static bool CheckUsersCredentialsFromDatabase(string user, string password)
        {
            int verified = 1;
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string commandText = "SELECT * FROM user WHERE uid = @UID AND password = @PASSWORD";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", user);
                command.Parameters.AddWithValue("@PASSWORD", password);
                command.ExecuteNonQuery();
                int account = CheckIfUserIsVerified(command);
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

        public static int CheckIfUserIsVerified(MySqlCommand command)
        {
            int verified = 0;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                verified = reader.GetInt32("verified");
            }
            return verified;
        }

        public static bool AddUserToDatabase(string uid, string password, string email, string hash)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string commandText = "INSERT INTO user (uid, password, email, hash, verified) VALUES (@UID, @PASSWORD, @EMAIL, @HASH, 1)";
                MySqlCommand command = new MySqlCommand(commandText, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", uid);
                command.Parameters.AddWithValue("@PASSWORD", password);
                command.Parameters.AddWithValue("@EMAIL", email);
                command.Parameters.AddWithValue("@HASH", hash);
                int rowCount = command.ExecuteNonQuery();
                if (rowCount == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
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

        public static DataTable GetAllUsersGamesFromDatabase(string user)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {     
                connection.Open();
                string query = "SELECT idgame, game.name, progress.name AS status, genre.name AS genre, achievements, comment " +
                               "FROM game " +
                               "LEFT JOIN progress ON game.progress_idprogress = progress.idprogress " +
                               "LEFT JOIN genre ON game.genre_idgenre = genre.idgenre " +
                               "WHERE user_uid = @UID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@UID", user);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataSet set = new DataSet();
                adapter.Fill(set, "game");
                return set.Tables["game"];
            }
            catch (Exception)
            {
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

        public static void AddNewGame(string user, string title, string achievements, string progress, string comment, string genre)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string query = "INSERT INTO game (name, achievements, progress_idprogress, comment, user_uid, genre_idgenre) " +
                                "VALUES (@TITLE, @ACHIEVEMENTS, (SELECT idprogress FROM progress WHERE name = @PROGRESS), " +
                                "@COMMENT, (SELECT uid FROM user WHERE uid = @USER), (SELECT idgenre FROM genre WHERE name = @GENRE))";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@TITLE", title);
                command.Parameters.AddWithValue("@ACHIEVEMENTS", achievements);
                command.Parameters.AddWithValue("@PROGRESS", progress);
                command.Parameters.AddWithValue("@COMMENT", comment);
                command.Parameters.AddWithValue("@USER", user);
                command.Parameters.AddWithValue("@GENRE", genre);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
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

        public static void AddGenre(string genre)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string query = "REPLACE INTO genre (name) VALUES (@genre)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@GENRE", genre);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
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

        public static DataTable GetAllGenresFromDatabase()
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
            catch (Exception)
            {
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

        public static void DeleteGameFromDatabase(int id)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string query = "DELETE FROM game WHERE idgame = @ID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
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

        public static void UpdateGameInfoToDatabase(int id, string user, string title, string status, string achievements, string genre, string comment)
        {
            MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.Database);
            try
            {
                connection.Open();
                string query = "UPDATE game SET name = @TITLE, progress_idprogress = (SELECT idprogress FROM progress WHERE name = @STATUS), " +
                                "achievements = @ACHIEVEMENTS, comment = @COMMENT, " +
                                "genre_idgenre = (SELECT idgenre FROM genre WHERE name = @GENRE) WHERE idgame = @ID";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Prepare();
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@TITLE", title);
                command.Parameters.AddWithValue("@ACHIEVEMENTS", achievements);
                command.Parameters.AddWithValue("@STATUS", status);
                command.Parameters.AddWithValue("@COMMENT", comment);
                command.Parameters.AddWithValue("@GENRE", genre);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
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
