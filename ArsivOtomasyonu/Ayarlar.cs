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
using System.Collections;

namespace ArsivOtomasyonu
{
    public partial class Ayarlar : DevExpress.XtraEditors.XtraForm
    {
        public Ayarlar()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fr1 = new Form1();
            fr1.Show();
        }
        ArrayList indexx = new ArrayList();
        void comboListele()
        {
            comboBoxEdit4.Properties.Items.Clear();
            comboBoxEdit1.Properties.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM kullanici";
                MySqlCommand command = new MySqlCommand(sql, connection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        indexx.Add(id);
                        comboBoxEdit4.Properties.Items.Add(reader.GetString(1));
                        comboBoxEdit1.Properties.Items.Add(reader.GetString(1));

                    }
                }
                connection.Close();
            }
        }

        int id;
        private void Ayarlar_Load(object sender, EventArgs e)
        {
            comboListele();
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                connection.Open();

                //string sql = "SELECT * FROM kullanici WHERE kullaniciID=1";
                MySqlCommand command = new MySqlCommand("SelectAdminProd", connection);
                command.CommandType = CommandType.StoredProcedure;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        textEdit1.Text = reader.GetString(1);
                        textEdit2.Text = reader.GetString(2);
                    }
                }
                connection.Close();
            }
        }

        private void Ayarlar_FormClosed(object sender, FormClosedEventArgs e)
        {
            Func.CikisYap();
            Application.Exit();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEdit4_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                connection.Open();

                //string sql = "SELECT * FROM kullanici WHERE kullaniciID="+indexx[comboBoxEdit4.SelectedIndex];
                MySqlCommand command = new MySqlCommand("KullaniciSelectProd", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

              
                command.Parameters.AddWithValue("kullaniciID", indexx[comboBoxEdit4.SelectedIndex]);
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        textEdit6.Text = reader.GetString(1);
                        textEdit5.Text = reader.GetString(2);
                        if (reader.GetString(3) == "1")
                            comboBoxEdit3.SelectedIndex = 0;
                        if (reader.GetString(3) == "0")
                            comboBoxEdit3.SelectedIndex = 1;
                    }
                }
                connection.Close();
            }
        }
        string yetkiSecimi;
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit3.SelectedIndex == 0)
                yetkiSecimi = "1";
            if (comboBoxEdit3.SelectedIndex == 1)
                yetkiSecimi = "0";
            string query = "UPDATE kullanici SET kullaniciadi = @p1, sifre = @p2, yetki = @p3  WHERE kullaniciID = @id";
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@p1", textEdit6.Text);
                command.Parameters.AddWithValue("@p2", textEdit5.Text);
                command.Parameters.AddWithValue("@p3", yetkiSecimi);
                command.Parameters.AddWithValue("@id", id);


                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kayıt güncellendi.");
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("Güncellenecek kayıt bulunamadı.");
                }
            }
        }
        
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (comboBoxEdit2.SelectedIndex == 0)
                yetkiSecimi = "1";
            if (comboBoxEdit2.SelectedIndex == 1)
                yetkiSecimi = "0";
            string query = "INSERT INTO kullanici (kullaniciadi, sifre, yetki) VALUES (@Value1, @Value2, @Value3)";
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {

                MySqlCommand command = new MySqlCommand(query, connection);
                //command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Value1", textEdit8.Text);
                command.Parameters.AddWithValue("@Value2", textEdit7.Text);
                command.Parameters.AddWithValue("@Value3", yetkiSecimi);       

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Yeni kullanıcı başarıyla eklendi.");
                    textEdit7.Text = "";
                    textEdit8.Text = "";
                    comboBoxEdit2.SelectedIndex = -1;
                    comboListele();
                }

                else
                    MessageBox.Show("Kayıt sırasında bir hata oluştu.");
            }


        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM kullanici WHERE kullaniciID=" + indexx[comboBoxEdit1.SelectedIndex];
                MySqlCommand command = new MySqlCommand(sql, connection);
                //command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("kullaniciID", indexx[comboBoxEdit1.SelectedIndex]);
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                        string secim = "";
                        if (reader.GetString(3) == "1")
                            secim = "Yönetici";
                        if (reader.GetString(3) == "0")
                            secim = "Kullanıcı";
                        labelControl6.Text = secim;                        
                    }
                }
                connection.Close();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM kullanici WHERE kullaniciID = @p";
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@p", id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kayıt başarıyla silindi.");

                }
            }
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bu işlemi onaylıyor musunuz?.", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (textEdit3.Text == textEdit4.Text)
                {
                    string query = "UPDATE kullanici SET kullaniciadi = @param1, sifre = @param2 WHERE kullaniciID =1";
                    using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@param1", textEdit1.Text);
                        command.Parameters.AddWithValue("@param2", textEdit3.Text);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        DialogResult result2 = MessageBox.Show("Admin bilgileri güncellendi", "Bilgi");
                        if (result2 == DialogResult.OK)
                        {
                            Application.Restart();
                        }
                    }
                }
            }
        }
    }
}