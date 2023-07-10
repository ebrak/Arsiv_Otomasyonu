using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

namespace ArsivOtomasyonu
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        public bool yetki = false;
       
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (textEdit2.Text.Length < 1)
            {
                MessageBox.Show("Lütfen kullanıcı adı ve parola alanını boş bırakmayın.");
            }
            else if (textEdit2.Text.Length < 1)
            {
                MessageBox.Show("Lütfen parola alanını boş bırakmayın.");
            }
            else if (textEdit1.Text.Length < 1)
            {
                MessageBox.Show("Lütfen kullanıcı adı alanını boş bırakmayın.");
            }
            else
            {
                
                
                using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
                {
                    MySqlCommand command = new MySqlCommand("LoginProd", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("kullaniciadi", textEdit1.Text);
                    command.Parameters.AddWithValue("sifre", textEdit2.Text);
                    connection.Open();

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        
                        if (reader.GetString(3) == "1")
                            yetki = true;
                        else 
                            yetki = false;

                    }
                    reader.Close();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        Properties.Settings.Default.nick = textEdit1.Text;
                        Properties.Settings.Default.Save();
                        this.Hide();                        
                        Form1 fr1 = new Form1();                       
                        //fr1.labelControl1.Text = textEdit1.Text + " | ";
                        if (yetki == true)
                        {                           
                            fr1.yetki = true;
                            fr1.simpleButton4.Enabled = true;
                        }
                            
                        else
                        {
                            fr1.yetki = false;
                            fr1.simpleButton4.Enabled = false;
                        }                         
                        fr1.Show();
                    }
                }

            }

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Func.CikisYap();
            Application.Exit();
        }
    }
}