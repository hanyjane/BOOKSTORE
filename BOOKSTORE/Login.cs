using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOOKSTORE
{
    public partial class Login : Form
    {

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\reyneil\Desktop\Database11.accdb;";
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(l.Text) ||
                string.IsNullOrWhiteSpace(pass.Text))
            {
                MessageBox.Show("Please enter both email and password", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // Query to check if credentials match
                    string loginQuery = "SELECT Username FROM Users WHERE Email = ? AND [Password] = ?";

                    using (OleDbCommand command = new OleDbCommand(loginQuery, connection))
                    {
                        command.Parameters.AddWithValue("?", txtLoginEmail.Text);
                        command.Parameters.AddWithValue("?", txtLoginPassword.Text);

                        // Execute the query
                        object result = command.ExecuteScalar();

                        if (result != null) // If we got a match
                        {
                            string username = result.ToString();
                            MessageBox.Show($"Welcome back, {username}!", "Login Successful",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Open main form or do something after successful login
                           mainform mainForm = new mainform();
                            mainForm.Show();
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Invalid email or password", "Login Failed",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
