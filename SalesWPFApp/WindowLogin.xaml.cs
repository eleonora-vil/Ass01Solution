using Microsoft.Extensions.Configuration;
using SalesBusinessObjects;
using SalesRepositories;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        private readonly IMemberRepository? memberRepository = null;
        public WindowLogin()
        {
            InitializeComponent();
            if (memberRepository == null)
            {
                memberRepository = new MemberRepository();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUser.Text.Trim();
            string password = txtPass.Password.Trim();

            // Read default admin credentials from appsettings.json
            IConfiguration config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .Build();

            string defaultAdminEmail = config["DefaultAdmin:Email"];
            string defaultAdminPassword = config["DefaultAdmin:Password"];

            // Check against default admin credentials
            bool isAdminAuthenticated = email.Equals(defaultAdminEmail) && password.Equals(defaultAdminPassword);

            // Authenticate the member against the database
            Member authenticatedMember = memberRepository.GetAccount(email, password);

            if (authenticatedMember != null || isAdminAuthenticated)
            {
                MessageBox.Show("Login Successful!");
                MainWindow mainWindow = new();
                mainWindow.Show();
                Close();

            }
            else
            {
                // Authentication failed
                MessageBox.Show("Invalid email or password.");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
