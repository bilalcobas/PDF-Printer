using System;
using System.Drawing.Printing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Printer
{
    public partial class varsayilanyazici : Form
    {
        public string SelectedPrinter { get; private set; } // Seçilen yazıcıyı tutacak property

        public varsayilanyazici()
        {
            InitializeComponent();
            LoadPrintersToDataGridView();
        }

        private void varsayilanyazici_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPrintersToDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void LoadPrintersToDataGridView()
        {
            // DataGridView'da sütunları tanımla
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("PrinterName", "Yazıcı Adı");
            dataGridView1.Columns.Add("Port", "Port");
            dataGridView1.Columns.Add("LocalAddress", "Local Address");

            // Sütunların DataGridView'in genişliğini doldurmasını sağla
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Yazıcı bilgilerini DataGridView'a ekle
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                // Port ve Local Address bilgilerini almak için gerekli methodları çağır
                string port = GetPrinterPort(printer);
                string localAddress = GetPrinterIP(printer);

                // DataGridView'a veri ekle
                dataGridView1.Rows.Add(printer, port, localAddress);
                // ComboBox'a yazıcı adını ekle
                comboBox1.Items.Add(printer);
            }
        }

        private string GetPrinterPort(string printerName)
        {
            try
            {
                string query = $"SELECT * FROM Win32_Printer WHERE Name = '{printerName.Replace("\\", "\\\\")}'";
                using (var searcher = new System.Management.ManagementObjectSearcher(query))
                using (var results = searcher.Get())
                {
                    foreach (System.Management.ManagementObject printer in results)
                    {
                        return printer["PortName"]?.ToString() ?? "Unknown";
                    }
                }
            }
            catch
            {
                return "Unknown";
            }
            return "Unknown";
        }

        private string GetPrinterIP(string printerName)
        {
            try
            {
                string query = $"SELECT * FROM Win32_TCPIPPrinterPort WHERE Name = '{printerName.Replace("\\", "\\\\")}'";
                using (var searcher = new System.Management.ManagementObjectSearcher(query))
                using (var results = searcher.Get())
                {
                    foreach (System.Management.ManagementObject port in results)
                    {
                        return port["HostAddress"]?.ToString() ?? "Unknown";
                    }
                }
            }
            catch
            {
                return "Unknown";
            }
            return "Unknown";
        }

        private async Task SendPrinterDataToApiAsync(string printerName, string port)
        {
            string baseUrl = ConfigReader.GetValue("url").TrimEnd('/');
            string userId = ConfigReader.GetValue("userId").Trim('/');
            string apiKey = ConfigReader.GetValue("x-api-key");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                // API URL'sini oluşturun
                string requestUrl = $"{baseUrl}/api/users/{userId}/printer";

                var data = new
                {
                    printername = printerName,
                    ipaddress = port,
                    status = 1 // Durumu 1 olarak ayarla
                };

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(data),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                try
                {
                    var response = await httpClient.PostAsync(requestUrl, jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Yazıcı bilgileri başarıyla kaydedildi.");
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Yazıcı bilgileri kaydedilirken bir hata oluştu: {responseContent}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedPrinter = comboBox1.SelectedItem.ToString(); // Seçili yazıcıyı kaydet
                string port = GetPrinterPort(selectedPrinter);

                // API'ye yazıcı bilgilerini gönder
                await SendPrinterDataToApiAsync(selectedPrinter, port);

                this.DialogResult = DialogResult.OK; // Formu başarıyla kapat
                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen bir yazıcı seçin.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Formu kapatmak istediğinize emin misiniz?", "Emin misiniz?", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel; // Formu iptal ederek kapat
                this.Close();
            }
        }
    }
}
