using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Printing;
using PdfiumViewer;
using System.Linq;
using System.Text;

namespace Printer
{
    public partial class UserControl3Faturalar : UserControl
    {
        private Timer _timer;

        public UserControl3Faturalar()
        {
            InitializeComponent();
        }

        private async void UserControl3Faturalar_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();

            // Her 10 saniye de bir günceller
            _timer = new Timer();
            _timer.Interval = 10000; 
            _timer.Tick += async (s, args) =>
            {
                await LoadDataAsync();
                await PrintDataAsync();
            };

            _timer.Start();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                string baseUrl = ConfigReader.GetValue("url").TrimEnd('/');
                string userId = ConfigReader.GetValue("userId").Trim('/');
                string apiKey = ConfigReader.GetValue("x-api-key");

                string requestUrl = $"{baseUrl}/api/users/{userId}/pdfs";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON array into a list of FaturaData
                    var dataList = JsonConvert.DeserializeObject<List<FaturaData>>(responseData);

                    // Filter: Only load PDFs with value 0
                    dataList = dataList.Where(d => d.Value == 0).ToList();

                    dataGridView1.DataSource = dataList;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    await PrintDataAsync(); // Start printing immediately after data is loaded
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"HTTP Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task PrintDataAsync()
        {
            try
            {
                var dataList = dataGridView1.DataSource as List<FaturaData>;
                if (dataList != null)
                {
                    var updatedDataList = new List<FaturaData>();

                    foreach (var pdfData in dataList)
                    {
                        // Yazıcı adını API'den al
                        string printerName = await GetPrinterNameForUserAsync();
                        if (string.IsNullOrEmpty(printerName))
                        {
                            MessageBox.Show("Yazıcı adı alınamadı!");
                            continue;
                        }

                        // Dosyayı indirin ve indirme yolunu alın
                        string downloadedFilePath = await DownloadPdfAsync(pdfData.FilePath);

                        if (downloadedFilePath != null)
                        {
                            // İndirilen dosyayı yazdır
                            PrintControl.PrintPdf(downloadedFilePath, printerName);

                            // Value'yu güncelle ve başarılı olup olmadığını kontrol et
                            var success = await UpdateValueAsync(pdfData.ID);
                            if (success)
                            {
                                pdfData.Value = 1; // Güncelleme başarılıysa, local datayı da güncelle
                                updatedDataList.Add(pdfData); // Güncellenmiş veriyi listeye ekle
                            }
                            else
                            {
                                MessageBox.Show($"Failed to update value for PDF ID: {pdfData.ID}");
                            }
                        }
                    }

                    // DataGridView güncellenen verilerle yeniden bağla
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = updatedDataList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task<string> GetPrinterNameForUserAsync()
        {
            try
            {
                string baseUrl = ConfigReader.GetValue("url").TrimEnd('/');
                string userId = ConfigReader.GetValue("userId").Trim('/');
                string apiKey = ConfigReader.GetValue("x-api-key");

                string requestUrl = $"{baseUrl}/api/users/{userId}/printer";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();
                    string responseData = await response.Content.ReadAsStringAsync();

                    // JSON dizisini deserialize ederek sadece printerName döndür
                    var printerDataArray = JsonConvert.DeserializeObject<List<PrinterData>>(responseData);
                    return printerDataArray?.FirstOrDefault()?.printername; // İlk printerName'i döndür
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"HTTP Error: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return null;
            }
        }

        private async Task<string> DownloadPdfAsync(string filePath)
        {
            try
            {
                string downloadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads");

                // Downloads klasörü yoksa oluştur
                if (!Directory.Exists(downloadsFolder))
                {
                    Directory.CreateDirectory(downloadsFolder);
                }

                // Dosya yolu geçerliyse indirme işlemi gerçekleştir
                if (File.Exists(filePath))
                {
                    string destinationPath = Path.Combine(downloadsFolder, Path.GetFileName(filePath));
                    File.Copy(filePath, destinationPath, true); // Dosyayı kopyala
                    //MessageBox.Show($"Dosya başarıyla indirildi: {destinationPath}");
                    return destinationPath; // İndirilen dosyanın yolunu döndür
                }
                else
                {
                    MessageBox.Show($"Dosya bulunamadı: {filePath}");
                    return null; // Eğer dosya bulunamazsa null döndür
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dosya indirme hatası: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> UpdateValueAsync(int pdfId)
        {
            try
            {
                string baseUrl = ConfigReader.GetValue("url").TrimEnd('/');
                string apiKey = ConfigReader.GetValue("x-api-key");

                // URL'yi oluşturun
                string requestUrl = $"{baseUrl}/api/pdfs/{pdfId}/value";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                    // JSON içeriğinin doğru olduğundan emin olun
                    var content = new StringContent(JsonConvert.SerializeObject(new { value = 1 }), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PutAsync(requestUrl, content);

                    // Yanıt kodunu ve içeriğini kontrol edin
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Failed: {response.StatusCode} - {responseContent}");
                        return false;
                    }
                    return true;
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"HTTP Error: {httpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }
    }

    public class FaturaData
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string FilePath { get; set; }
        public int Pages { get; set; }
        public string Orientation { get; set; }
        public int Value { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

public class PrinterData
{
    public string printername { get; set; }
}
