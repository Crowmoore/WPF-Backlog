using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.IO;

namespace Backlog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Validator validator = new Validator();

        public MainWindow()
        {
            InitializeComponent();
            LoadUserSettings();
        }

        private void LoadUserSettings()
        {
            Color color = (Color)ColorConverter.ConvertFromString(Properties.Settings.Default.Color);
            SolidColorBrush brush = new SolidColorBrush(color);

            this.Background = brush;
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            ShowRegisterScreen();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginScreen();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string email = txtEmail.Text;
            if(validator.Validate(username, password, email))
            {
                Register register = new Register(username, password, email);
                if (register.RegisterUser())
                {
                    ShowLoginScreen();
                    tbMessage.Text = "Registered successfully. Please login";
                }
            }
            
            else
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Something went wrong. Please try again later";
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(Login.CheckCredentials(txtUsername.Text, txtPassword.Password))
                {
                    tbMessage.Visibility = Visibility.Visible;
                    tbMessage.Text = "Logged in";
                    BacklogWindow window = new BacklogWindow(txtUsername.Text);
                    window.Show();
                    this.Close();
                }
                else
                {
                    tbMessage.Visibility = Visibility.Visible;
                    tbMessage.Text = "Wrong username or password";
                }          
            }
            catch (Exception ex)
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = ex.Message;
            }
              
        }
        private void ShowRegisterScreen()
        {
            txtEmail.Visibility = Visibility.Visible;
            txtPassword.Password = "";
            txtUsername.Text = "";
            txtUsername.Focus();
            tbEmail.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Collapsed;
            btnRegister.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Visible;
            btnSignup.Visibility = Visibility.Collapsed;
        }
        private void ShowLoginScreen()
        {
            tbMessage.Visibility = Visibility.Visible;
            txtEmail.Visibility = Visibility.Collapsed;
            txtPassword.Password = "";
            txtUsername.Text = "";
            txtUsername.Focus();
            tbEmail.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Visible;
            btnRegister.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Collapsed;
            btnSignup.Visibility = Visibility.Visible;
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(validator.CheckUsername(txtUsername.Text))
            {
                txtUsername.Foreground = Brushes.Green;
            }
            else
            {
                txtUsername.Foreground = Brushes.Red;
            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (validator.CheckEmail(txtEmail.Text))
            {
                txtEmail.Foreground = Brushes.Green;
            }
            else
            {
                txtEmail.Foreground = Brushes.Red;
            }
        }
    }
}
