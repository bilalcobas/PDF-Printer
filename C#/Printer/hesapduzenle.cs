using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Printer
{
    public partial class hesapduzenle : Form
    {
        private int currentUserId;
        private string currentFirstName;
        private string currentLastName;
        private string currentUserName;
        private string currentRole;

        public hesapduzenle(int userId)
        {
            InitializeComponent();
            this.currentUserId = userId;
            LoadUserDetailsForEdit();
        }

        private void LoadUserDetailsForEdit()
        {
            using (var connection = new MySqlConnection("Server=localhost;Database=database1;Uid=root;Pwd=bc123;"))
            {
                connection.Open();
                string query = "SELECT * FROM AuthUsers WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", currentUserId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentFirstName = reader["FirstName"].ToString();
                            currentLastName = reader["LastName"].ToString();
                            currentUserName = reader["UserName"].ToString();
                            currentRole = reader["Role"].ToString();

                            SetPlaceholder(textBox1, currentFirstName);
                            SetPlaceholder(textBox2, currentLastName);
                            SetPlaceholder(textBox3, currentUserName);
                            SetPlaceholder(textBox4, currentRole);
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bilgileri bulunamadı!");
                        }
                    }
                }
            }
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.Black;
                }
            };

            textBox.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text == currentFirstName ? "" : textBox1.Text;
            string lastName = textBox2.Text == currentLastName ? "" : textBox2.Text;
            string userName = textBox3.Text == currentUserName ? "" : textBox3.Text;
            string role = textBox4.Text == currentRole ? "" : textBox4.Text;
            string password = textBox5.Text;
            string confirmPassword = textBox6.Text;

            if (!string.IsNullOrWhiteSpace(password) || !string.IsNullOrWhiteSpace(confirmPassword))
            {
                if (password != confirmPassword)
                {
                    MessageBox.Show("Şifreler uyuşmuyor!");
                    return;
                }

                password = HashPasswordForEdit(password);
            }

            using (var connection = new MySqlConnection("Server=localhost;Database=database1;Uid=root;Pwd=bc123;"))
            {
                connection.Open();
                string updateQuery = "UPDATE AuthUsers SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName, Role = @Role" +
                                     (string.IsNullOrWhiteSpace(password) ? "" : ", Password = @Password") +
                                     " WHERE Id = @Id";
                using (var command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Role", role);
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        command.Parameters.AddWithValue("@Password", password);
                    }
                    command.Parameters.AddWithValue("@Id", currentUserId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Bilgiler güncellenirken bir hata oluştu.");
                    }
                }
            }
        }

        private string HashPasswordForEdit(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
