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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Visibility = Visibility.Visible;
            tbEmail.Visibility = Visibility.Visible;
            btnLogin.Visibility = Visibility.Collapsed;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            Login login = new Login(txtUsername.Text, txtPassword.Text);
            if(login.CheckCredentials())
            {
                MessageBox.Show("Successfull login");
            }
            else
            {
                MessageBox.Show("Login unsuccessfull");
            }
            
        }

        

    }
}
