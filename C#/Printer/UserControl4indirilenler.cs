using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Printer
{
    public partial class UserControl4indirilenler : UserControl
    {
        private DataGridView dataGridView10;

        public UserControl4indirilenler()
        {
            InitializeComponent();
            InitializeDataGridView();
            LoadDownloadedPdfs();
        }

        private void InitializeDataGridView()
        {
            dataGridView10 = new DataGridView();
            dataGridView10.Dock = DockStyle.Fill;

            // Sütunları ekler
            dataGridView10.Columns.Add("fileName", "PDF Adı");
            dataGridView10.Columns.Add("fileSize", "Boyut");
            dataGridView10.Columns.Add("downloadDate", "İndirilme Tarihi");
            dataGridView10.Columns.Add("filePath", "Dosya Yolu");
            dataGridView10.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.Controls.Add(dataGridView10);
        }

        private void LoadDownloadedPdfs()
        {
            string downloadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloads");

            if (!Directory.Exists(downloadsFolder))
            {
                MessageBox.Show("Downloads klasörü bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pdfFiles = Directory.GetFiles(downloadsFolder, "*.pdf")
                                    .Select(filePath => new
                                    {
                                        FileName = Path.GetFileName(filePath),
                                        FileSize = new FileInfo(filePath).Length,
                                        DownloadDate = File.GetCreationTime(filePath),
                                        FilePath = filePath
                                    });

            foreach (var pdf in pdfFiles)
            {
                dataGridView10.Rows.Add(pdf.FileName, FormatFileSize(pdf.FileSize), pdf.DownloadDate, pdf.FilePath);
            }
        }

        // Dosya boyutunu uygun bir formatta göstermek için yardımcı fonksiyon
        private string FormatFileSize(long fileSize)
        {
            string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;

            while (fileSize >= 1024 && order < sizeSuffixes.Length - 1)
            {
                order++;
                fileSize = fileSize / 1024;
            }

            return String.Format("{0:0.##} {1}", fileSize, sizeSuffixes[order]);
        }
    }
}
