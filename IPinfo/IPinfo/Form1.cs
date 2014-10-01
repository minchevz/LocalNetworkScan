using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace IPinfo
{
    public partial class Form1 : Form
    {
        //create a last scan informations and saving the previous scan informations in the list "LastInfo".
        List<IPInfo> LastInfo = new List<IPInfo>();

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(GetResults));
            t.Start();
            label1.ForeColor = System.Drawing.Color.Red;
            label1.Text = "Processing ...";
            label1.Visible = true;
            Application.DoEvents();
        }

        private void GetResults()
        {
            richTextBox1.Clear();

            try
            {
                // a List for ip informations 
                List<IPInfo> info = IPInfo.GetIPInfo();

                // scan and display ip-mac-host informations and what is the machine status is "on" or "off".
                // status "offline" is being checked with the current and previous scans.

                foreach (IPInfo item in info)
                {
                    Application.DoEvents();
                    richTextBox1.AppendText("IP: " + item.IPAddress + "\n");
                    richTextBox1.AppendText("Mac: " + item.MacAddress + "\n");
                    richTextBox1.AppendText("Host: " + item.HostName + "\n");
                    richTextBox1.AppendText("Status: Online\n");
                    richTextBox1.AppendText("-----------------------------------------------");
                    richTextBox1.AppendText("\n");
                }

                richTextBox1.AppendText("\n");

                foreach (IPInfo item in LastInfo)
                {
                    if (info.Where(i => i.IPAddress == item.IPAddress).Count() == 0)
                    {
                        Application.DoEvents();
                        richTextBox1.AppendText("IP: " + item.IPAddress + "\n");
                        richTextBox1.AppendText("Mac: " + item.MacAddress + "\n");
                        richTextBox1.AppendText("Host: " + item.HostName + "\n");
                        richTextBox1.AppendText("Status: Offline\n");
                        richTextBox1.AppendText("-----------------------------------------------");
                        richTextBox1.AppendText("\n");
                    }
                }

                LastInfo = new List<IPInfo>(info.ToArray());
            }
            catch (Exception)
            {
                MessageBox.Show("An error-Naptın ulen dumbass !");
            }

            label1.ForeColor = System.Drawing.Color.Green;
            label1.Text = "Completed ! Scan is over, wanna give a new try?";

        }
    }
}
