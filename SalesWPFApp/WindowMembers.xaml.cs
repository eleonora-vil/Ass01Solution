using SalesBusinessObjects;
using SalesRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for WindowMembers.xaml
    /// </summary>
    public partial class WindowMembers : Window
    {
        private readonly IMemberRepository? memberRepository = null;

        public WindowMembers()
        {
            InitializeComponent();
            if (memberRepository == null)
            {
                memberRepository = new MemberRepository();
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate Email
                string simpleEmailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";
                Regex emailRegex = new Regex(simpleEmailPattern);
                if (!emailRegex.IsMatch(txtEmail.Text))
                {
                    MessageBox.Show("Invalid email format");
                    return;
                }
                if (
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtCountry.Text) ||
                    string.IsNullOrWhiteSpace(txtCompany.Text) ||
                    string.IsNullOrWhiteSpace(txtPassword.Text) ||
                    string.IsNullOrWhiteSpace(txtCity.Text))
                {
                    MessageBox.Show("All fields must be filled");
                    return;
                }
                  // Check if the email already exists
        var members = memberRepository.GetMembers(); // Assuming this method gets all members
        foreach (var eachmember in members)
        {
            if (eachmember.Email.Equals(txtEmail.Text, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Email already exists. Please choose a different email.");
                return;
            }
        }

                Member member = new Member
                {
                    Email = txtEmail.Text,
                    CompanyName = txtCompany.Text,
                    City = txtCity.Text,
                    Country = txtCountry.Text,
                    Password = txtPassword.Text
                };

                memberRepository.AddMember(member);
                MessageBox.Show("Added successfully!");
                ClearFields();
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add member: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string simpleEmailPattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";
                Regex emailRegex = new Regex(simpleEmailPattern);

                if (!emailRegex.IsMatch(txtEmail.Text))
                {
                    MessageBox.Show("Invalid email format");
                    return;
                }

                if (dgData.SelectedItem == null)
                {
                    MessageBox.Show("No member selected");
                    return;
                }

                Member selectedMember = (Member)dgData.SelectedItem;
                if (selectedMember != null)
                {
                    // Check if email is being changed
                    if (txtEmail.Text != selectedMember.Email)
                    {
                        MessageBox.Show("Cannot change email address.");
                        return; // Don't proceed with update if email is being changed
                    }

                    // Update member details
                    selectedMember.Email = txtEmail.Text;
                    selectedMember.Country = txtCountry.Text;
                    selectedMember.City = txtCity.Text;
                    selectedMember.Password = txtPassword.Text;
                    selectedMember.CompanyName = txtCompany.Text;

                    memberRepository.UpdateMember(selectedMember.MemberId, selectedMember);
                    MessageBox.Show("Update successful!");
                    ClearFields();
                    LoadDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update member: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMemberID.Text.Trim(), out int memberID))
                {
                    MessageBox.Show("Invalid Member ID. Please enter a valid number.");
                    return;
                }
                memberRepository.DeleteMember(memberID);
                MessageBox.Show("Deleted successfully!");
                ClearFields();
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? $"{ex.Message} See the inner exception for details." : ex.Message;
                MessageBox.Show($"Error deleting member: {errorMessage}");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            mainWindow.Show();
            Close();
        }

        // Grid view
        private void dgData_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem != null)
            {
                Member selectedMember = (Member)dgData.SelectedItem;
                txtMemberID.Text = selectedMember.MemberId.ToString();
                txtCity.Text = selectedMember.City;
                txtCompany.Text = selectedMember.CompanyName;
                txtEmail.Text = selectedMember.Email;
                txtPassword.Text = selectedMember.Password;
                txtCountry.Text = selectedMember.Country;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            dgData.ItemsSource = memberRepository.GetMembers();
            dgData.Items.Refresh();
        }

        private void ClearFields()
        {
            txtMemberID.Clear();
            txtCity.Clear();
            txtCompany.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtCountry.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
    }
}

