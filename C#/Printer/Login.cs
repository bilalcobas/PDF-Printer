using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Printer
{
    public partial class Login : Form
    {
        public int UserId { get; private set; }

        public Login()
        {
            InitializeComponent();
            textBox1.KeyDown += TextBox_KeyDown;
            textBox2.KeyDown += TextBox_KeyDown;

            // Şifre karakterlerini gizlemek için
            textBox2.PasswordChar = '*';
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button_WOC1_Click(sender, e);
            }
        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz!");
                return;
            }

            string hashedPassword = HashPassword(password);
            string connectionString = "Server=localhost;Database=database1;Uid=root;Pwd=bc123;";
            var authenticationResult = AuthenticateUser(userName, hashedPassword, connectionString);

            if (authenticationResult == AuthenticationResult.Success)
            {
                int userId = GetUserId(userName); // Kullanıcı ID'sini alır

                // Main2 formunu oluşturur ve UserId'yi geçer
                Main2 mainForm = new Main2(userId);
                mainForm.Show();

                // Login formunu gizler
                this.Hide();
            }
            else if (authenticationResult == AuthenticationResult.WrongPassword)
            {
                MessageBox.Show("Şifre yanlış!");
            }
            else if (authenticationResult == AuthenticationResult.UserNotFound)
            {
                MessageBox.Show("Böyle bir kullanıcı bulunamadı!");
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
            }
        }

        private AuthenticationResult AuthenticateUser(string userName, string hashedPassword, string connectionString)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Kullanıcı adını kontrol eder
                string userQuery = "SELECT COUNT(*) FROM AuthUsers WHERE userName = @userName";
                using (var userCommand = new MySqlCommand(userQuery, connection))
                {
                    userCommand.Parameters.AddWithValue("@userName", userName);
                    int userCount = Convert.ToInt32(userCommand.ExecuteScalar());

                    if (userCount == 0)
                    {
                        return AuthenticationResult.UserNotFound;
                    }
                }

                // Şifreyi kontrol eder
                string passwordQuery = "SELECT COUNT(*) FROM AuthUsers WHERE userName = @userName AND password = @password";
                using (var passwordCommand = new MySqlCommand(passwordQuery, connection))
                {
                    passwordCommand.Parameters.AddWithValue("@userName", userName);
                    passwordCommand.Parameters.AddWithValue("@password", hashedPassword);
                    int passwordCount = Convert.ToInt32(passwordCommand.ExecuteScalar());

                    if (passwordCount == 0)
                    {
                        return AuthenticationResult.WrongPassword;
                    }
                }

                return AuthenticationResult.Success;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private int GetUserId(string userName)
        {
            string connectionString = "Server=localhost;Database=database1;Uid=root;Pwd=bc123;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id FROM AuthUsers WHERE userName = @userName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userName", userName);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private enum AuthenticationResult
        {
            Success,
            UserNotFound,
            WrongPassword
        }
    }
}