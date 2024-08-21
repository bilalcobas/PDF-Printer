using System;
using System.Drawing.Printing;
using System.IO;
using PdfiumViewer;
using System.Windows.Forms;

public static class PrintControl
{
    public static void PrintPdf(string filePath, string printerName)
    {
        bool isSuccess = false;
        try
        {
            using (var document = PdfDocument.Load(filePath))
            {
                using (var printDocument = document.CreatePrintDocument())
                {
                    printDocument.PrinterSettings.PrinterName = printerName;
                    printDocument.PrintController = new StandardPrintController(); // Bu genellikle diyalogları devre dışı bırakır

                    // Diğer yazıcı ayarlarını buradan yapabilirsiniz
                    printDocument.Print();

                    isSuccess = true;
                }
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda mesajı kullanıcıya göster
            MessageBox.Show($"Printing error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (isSuccess)
        {
            // Başarı durumunda mesajı kullanıcıya göster
            MessageBox.Show($"Printing {filePath} complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
