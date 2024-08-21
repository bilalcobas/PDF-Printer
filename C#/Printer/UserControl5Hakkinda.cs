using System.Drawing;
using System.Windows.Forms;
using System;

namespace Printer
{
    public partial class UserControl5Hakkinda : UserControl
    {
        public UserControl5Hakkinda()
        {
            InitializeComponent();

            // RichTextBox'a metin ekleme ve biçimlendirme
            richTextBox1.Clear(); // Var olan metni temizler

            richTextBox1.ReadOnly = true;

            AddBoldText2("Program Adı: PrintSoft\n\n");
            AddBoldText2("Nedir?\n\n");
            AddRegularText("PrintSoft, arkaplanda çalışan ve kullanıcıların PDF dosyalarını yazdırma işlemini otomatikleştiren bir yazılım çözümüdür. Kullanıcıların veritabanına manuel olarak bağlanıp PDF dosyalarını yazdırması gerekmez; PrintSoft bu işlemleri arkaplanda kendi başına gerçekleştirir. Kullanıcıların yapması gereken tek şey, programın yazıcılar panelinden bir varsayılan yazıcı seçmek ve faturalar bölümünden yazdırma işlemini başlatmaktır.\n\n\n");

            AddBoldText2("Nasıl Kullanılır?\n\n\n");
            AddBoldText("Yazıcı Seçimi:");
            AddRegularText("Programın yazıcılar panelinden varsayılan bir yazıcı seçin. Bu yazıcı, PDF dosyalarını yazdırmak için kullanılacaktır.\n\n");
            AddBoldText("Başlat:");
            AddRegularText("Yazıcı seçildikten sonra, faturalar bölümüne gidin ve yazdırma işlemini başlatın.\n\n");
            AddBoldText("Otomatik Yazdırma:");
            AddRegularText("Program, arkaplanda API üzerinden veritabanına bağlanarak PDF dosyalarını çeker ve seçilen yazıcıyla yazdırır. Bu işlemler kullanıcı müdahalesi gerektirmez.\n\n");
            AddBoldText("Takip Etme:");
            AddRegularText("Yazdırma işlemi devam ederken, kullanıcı programın arayüzü üzerinden ilerlemeyi takip edebilir.\n\n\n");

            AddBoldText2("Amaçları\n\n\n");
            AddBoldText("Zaman Tasarrufu:");
            AddRegularText("PrintSoft, manuel yazdırma işlemlerini otomatik hale getirerek kullanıcıların zamanını verimli kullanmasını sağlar.\n\n");
            AddBoldText("Kullanım Kolaylığı:");
            AddRegularText("Basit bir yazıcı seçimi ve başlatma işlemi ile kullanıcılar, karmaşık yazdırma süreçlerini kolaylıkla yönetebilir.\n\n");
            AddBoldText("Arkaplanda Çalışma:");
            AddRegularText("Yazılım, tüm işlemleri arkaplanda gerçekleştirerek kullanıcıların diğer işlerine odaklanmasını sağlar.\n\n");
            AddBoldText("Güvenilirlik:");
            AddRegularText("Veritabanındaki PDF dosyaları otomatik olarak çekilir ve yazdırılır, bu da kullanıcı hatalarını en aza indirir.\n\n");
            AddBoldText("Esneklik:");
            AddRegularText("Farklı yazıcıları destekleyen program, çeşitli çalışma ortamlarına kolayca entegre edilebilir.\n");
        }

        private void AddBoldText(string text)
        {
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            richTextBox1.AppendText(text);

        }
        private void AddBoldText2(string text)
        {
            richTextBox1.SelectionFont = new Font("Arial", 14, FontStyle.Bold);
            richTextBox1.AppendText(text);

        }

        private void AddRegularText(string text)
        {
            richTextBox1.SelectionFont = new Font("Arial", 12, FontStyle.Regular);
            richTextBox1.AppendText(text);

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Burada gerekirse metin değişikliklerini işleyebilirsiniz
        }
    }
}
