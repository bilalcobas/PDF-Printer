using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Printer
{
    public partial class Main2 : Form
    {
        private int userId;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip trayMenu;

        private UserControl1Profil panel1UserControl;
        private UserControl2Yazicilar panel2UserControl;
        private UserControl3Faturalar panel3UserControl;
        private UserControl4indirilenler panel4UserControl;
        private UserControl5Hakkinda panel5UserControl;

        public Main2(int userId)
        {
            InitializeComponent();
            InitializePictureBoxes();
            InitializeUserControls(userId);
            InitializeTrayIcon();
            this.FormClosing += Main2_FormClosing;
            this.userId = userId;
        }

        private void InitializeTrayIcon()
        {
            // Sistem tepsisi için NotifyIcon oluştur
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = new Icon("icon.ico"); // İkon dosyanızın yolunu belirtin
            notifyIcon.Text = "PrintSoft";
            notifyIcon.Visible = true;

            // Sağ tık menüsü ekler
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Göster", null, ShowApp);
            trayMenu.Items.Add("Çıkış", null, ExitApp);
            notifyIcon.ContextMenuStrip = trayMenu;

            // Çift tıklama ile uygulamayı geri getirir
            notifyIcon.DoubleClick += (sender, args) => ShowApp(sender, args);
        }

        private void InitializePictureBoxes()
        {
            pictureBox1.Click += pictureBox1_Click;
            pictureBox2.Click += pictureBox2_Click;
            pictureBox3.Click += pictureBox3_Click;
            pictureBox4.Click += pictureBox4_Click;
            pictureBox5.Click += pictureBox5_Click;
            pictureBox6.Click += pictureBox6_Click_1;
            pictureBox7.Click += pictureBox7_Click;

            pictureBox1.MouseEnter += PictureBox_MouseEnter;
            pictureBox2.MouseEnter += PictureBox_MouseEnter;
            pictureBox3.MouseEnter += PictureBox_MouseEnter;
            pictureBox4.MouseEnter += PictureBox_MouseEnter;
            pictureBox5.MouseEnter += PictureBox_MouseEnter;
            pictureBox6.MouseEnter += PictureBox_MouseEnter;
            pictureBox7.MouseEnter += PictureBox_MouseEnter;

            pictureBox1.MouseLeave += PictureBox_MouseLeave;
            pictureBox2.MouseLeave += PictureBox_MouseLeave;
            pictureBox3.MouseLeave += PictureBox_MouseLeave;
            pictureBox4.MouseLeave += PictureBox_MouseLeave;
            pictureBox5.MouseLeave += PictureBox_MouseLeave;
            pictureBox6.MouseLeave += PictureBox_MouseLeave;
            pictureBox7.MouseLeave += PictureBox_MouseLeave;
        }

        private void Main2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Form kapanırken sistemi tepsisine küçültür
                e.Cancel = true; // Kapatma işlemini iptal eder
                this.Hide();
                notifyIcon.Icon = new Icon(@"Resources\icon.ico");
                notifyIcon.BalloonTipTitle = "PrintSoft";
                notifyIcon.BalloonTipText = "Uygulama arka planda çalışmaya devam ediyor.";
                notifyIcon.ShowBalloonTip(1000);
            }
            else
            {
                // Uygulama başka bir nedenle kapanıyorsa (örneğin, sistem kapanıyor), normal şekilde kapan
                ExitApp(sender, e);
            }
        }

        private void ShowApp(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

        private void ExitApp(object sender, EventArgs e)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
                notifyIcon = null; // Nullify to avoid re-disposing
            }
            Application.Exit();
        }

        private void InitializeUserControls(int userId)
        {
            panel1UserControl = new UserControl1Profil(userId) { Dock = DockStyle.Fill };
            panel2UserControl = new UserControl2Yazicilar { Dock = DockStyle.Fill };
            panel3UserControl = new UserControl3Faturalar { Dock = DockStyle.Fill };
            panel4UserControl = new UserControl4indirilenler { Dock = DockStyle.Fill };
            panel5UserControl = new UserControl5Hakkinda { Dock = DockStyle.Fill };
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ShowUserControl(panel1UserControl);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowUserControl(panel2UserControl);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowUserControl(panel3UserControl);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ShowUserControl(panel4UserControl);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ShowUserControl(panel5UserControl);
        }

        private void ShowUserControl(UserControl userControl)
        {
            panel1.Controls.Clear(); // panel1 içinde diğer UserControl'leri temizler
            panel1.Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result1 == DialogResult.Yes)
            {
                Debug.WriteLine("Evet seçildi, Login formu açılacak");
                Login loginForm = new Login();

                // Mevcut formu gizler
                this.Hide();

                // Login formunu gösterir
                loginForm.Show();

                // Login formunun kapanma olayını dinler
                loginForm.FormClosed += (s, args) => this.Close();
            }
        }

        private void pictureBox6_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamayı kapatmak istediğinizden emin misiniz?", "Çıkış Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ExitApp(sender, e);
            }
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (sender is PictureBox pb)
            {
                pb.BackColor = Color.FromArgb(20, 255, 255, 255);
            }
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox pb)
            {
                pb.BackColor = Color.Transparent;
            }
        }
    }
}
