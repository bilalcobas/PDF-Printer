using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Printer
{
    public partial class UserControl2Yazicilar : UserControl
    {
        private string selectedPrinter = null; // Varsayılan yazıcıyı tutacak değişken

        public UserControl2Yazicilar()
        {
            InitializeComponent();
            LoadPrintersToDataGridView();
        }

        private void LoadPrintersToDataGridView()
        {
            // DataGridView'da sütunları tanımla
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("PrinterName", "Yazıcı Adı");
            dataGridView1.Columns.Add("Port", "Ip Adresi");
            dataGridView1.Columns.Add("LocalAddress", "Port");

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

        // Yazıcının IP adresini al
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

        private async Task<string> GetDefaultPrinterNameAsync()
        {
            string baseUrl = ConfigReader.GetValue("url").TrimEnd('/');
            string userId = ConfigReader.GetValue("userId").Trim('/');
            string apiKey = ConfigReader.GetValue("x-api-key");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                // API URL'sini oluşturun
                string requestUrl = $"{baseUrl}/api/users/{userId}/printer";

                try
                {
                    var response = await httpClient.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var printers = JsonConvert.DeserializeObject<List<dynamic>>(responseContent);

                        // Status değeri 1 olan yazıcıyı bul
                        var defaultPrinter = printers.FirstOrDefault(p => p.status == 1);
                        if (defaultPrinter != null)
                        {
                            return defaultPrinter.printername;
                        }
                        else
                        {
                            MessageBox.Show("Status değeri 1 olan bir yazıcı bulunamadı.");
                            return null;
                        }
                    }
                    else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Yazıcı bilgileri alınırken bir hata oluştu: {responseContent}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}");
                    return null;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            varsayilanyazici vy = new varsayilanyazici();
            vy.Show();
        }

        private async void pictureBox2_Click(object sender, EventArgs e)
        {
            string selectedPrinter = await GetDefaultPrinterNameAsync();

            if (!string.IsNullOrEmpty(selectedPrinter))
            {
                MessageBox.Show($"Seçilen varsayılan yazıcı: {selectedPrinter}");
            }
            else
            {
                MessageBox.Show("Henüz varsayılan bir yazıcı seçilmedi veya bir hata oluştu.");
            }
        }

    }
}
