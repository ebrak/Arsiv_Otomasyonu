using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArsivOtomasyonu
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        public bool yetki = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            labelControl1.Text = Properties.Settings.Default.nick+" | ";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            YeniEvrak yeniEvrak = new YeniEvrak();
            yeniEvrak.Show();
        }
        
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Arsiv arsiv = new Arsiv();
            if (yetki == true)
                arsiv.simpleButton6.Enabled = true;
            else
                arsiv.simpleButton6.Enabled = false;

            arsiv.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Ayarlar ayarlar = new Ayarlar();
            ayarlar.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.nick = "";
            Properties.Settings.Default.Save();
            this.Hide();
            Login login = new Login();
            login.Show();
        }
    }
}
