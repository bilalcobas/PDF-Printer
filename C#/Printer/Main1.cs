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
    public partial class Main1 : Form
    {
        public Main1()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            // Login formunu oluştur
            Login loginForm = new Login();

            // Login formunun kapanma olayını dinler
            loginForm.FormClosed += (s, args) => this.Close();

            // Mevcut formu gizler
            this.Hide();

            // Login formunu gösterir
            loginForm.Show();
        }

        private void button_WOC2_Click(object sender, EventArgs e)
        {
            // Register formunu oluşturur
            SignUp signupForm = new SignUp();

            // Register formunun kapanma olayını dinler
            signupForm.FormClosed += (s, args) => this.Show();

            // Mevcut formu gizler
            this.Hide();

            // Register formunu gösterir
            signupForm.Show();
        }

    }
}
