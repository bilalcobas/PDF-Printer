using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Printer
{
    public partial class UserControl1Profil : UserControl
    {
        private int userId;

        // Kullanıcı ID'sini alarak UserControl'u oluştur
        public UserControl1Profil(int userId)
        {
            InitializeComponent();
            this.userId = userId; // UserId doğru şekilde set ediliyor
            LoadUserProfile(); // Kullanıcı bilgilerini yükleyin
        }

        private void LoadUserProfile()
        {
            // Kullanıcı bilgilerini veritabanından çekme
            using (var connection = new MySqlConnection("Server=localhost;Database=database1;Uid=root;Pwd=bc123;"))
            {
                connection.Open();
                string query = "SELECT * FROM AuthUsers WHERE Id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label5.Text = reader["FirstName"].ToString();
                            label6.Text = reader["LastName"].ToString();
                            label7.Text = reader["Role"].ToString();
                            label9.Text = reader["UserName"].ToString();
                            //label10.Text = reader["Password"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bilgileri bulunamadı!");
                        }
                    }
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Kullanıcıya onay mesajı göster
            DialogResult result = MessageBox.Show("Hesabınızı silmek istediğinizden emin misiniz?", "Hesap Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Kullanıcı veritabanından sil
                using (var connection = new MySqlConnection("Server=localhost;Database=database1;Uid=root;Pwd=bc123;"))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM AuthUsers WHERE Id = @Id";
                    using (var command = new MySqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", userId);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Hesabınız başarıyla silindi.");

                            // Login formunu göster
                            Login loginForm = new Login();
                            loginForm.Show();

                            // Mevcut formu kapat
                            Form mainForm = this.FindForm();
                            mainForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Hesap silinirken bir hata oluştu.");
                        }
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            hesapduzenle hd = new hesapduzenle(userId);
            hd.ShowDialog();
        }
    }
}
