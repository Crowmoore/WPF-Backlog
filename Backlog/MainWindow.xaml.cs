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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Visibility = Visibility.Visible;
            txtPassword.Password = "";
            txtUsername.Text = "";
            tbEmail.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Collapsed;
            btnRegister.Visibility = Visibility.Visible;
            btnSignup.Visibility = Visibility.Collapsed;
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register(txtUsername.Text, txtPassword.Password, txtEmail.Text);
            if(!register.CheckUsername())
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Username max size is 10 and it cannot contain any special chars";
            }
            if(!register.CheckEmail())
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Not a valid E-mail address";
            }
            if (!register.CheckPassword(txtPassword.Password))
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Password must be between 1-20 characters long\n and it cannot contain special characters";
            }
            if (register.CheckUsername() && register.CheckEmail() && register.CheckPassword(txtPassword.Password))
            {
                if(register.RegisterUser())
                {
                    tbMessage.Visibility = Visibility.Visible;
                    tbMessage.Text = "Registered successfully. Please login";
                    txtEmail.Visibility = Visibility.Collapsed;
                    txtPassword.Password = "";
                    txtUsername.Text = "";
                    tbEmail.Visibility = Visibility.Collapsed;
                    btnLogin.Visibility = Visibility.Visible;
                    btnRegister.Visibility = Visibility.Collapsed;
                    btnSignup.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                MessageBox.Show("Something went wrong");
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            Login login = new Login(txtUsername.Text, txtPassword.Password);
            if(login.CheckCredentials())
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Logging in";
            }
            else
            {
                tbMessage.Visibility = Visibility.Visible;
                tbMessage.Text = "Wrong username or password";
            }
            
        }

        

    }
}
