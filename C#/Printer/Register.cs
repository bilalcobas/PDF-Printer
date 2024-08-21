using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Printer
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text;
            string lastName = textBox2.Text;
            string userName = textBox3.Text;
            string password = textBox4.Text;
            string role = comboBox1.SelectedItem.ToString();

            string hashedPassword = HashPassword(password);

            string connectionString = "Server=localhost;Database=database1;Uid=root;Pwd=bc123;";

            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO AuthUsers (firstName, lastName, userName, password, role) VALUES (@firstName, @lastName, @userName, @password, @role)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@lastName", lastName);
                    command.Parameters.AddWithValue("@userName", userName);
                    command.Parameters.AddWithValue("@password", hashedPassword);
                    command.Parameters.AddWithValue("@role", role);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Kullanıcı başarıyla kaydedildi!");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
