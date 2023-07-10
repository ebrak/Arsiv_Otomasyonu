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
    public partial class Arsiv : DevExpress.XtraEditors.XtraForm
    {
        public Arsiv()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }
        Func func = new Func();
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fr1 = new Form1();          
            fr1.Show();
        }
        void GridDoldur()
        {          
            //string query = "SELECT * FROM evrak";         
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                MySqlCommand command = new MySqlCommand("EvrakListeleProd", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                gridControl1.DataSource = dataTable;
                
            }
        }
        void TureGoreGrid()
        {
            gridView1.ActiveFilter.Clear();
            gridView1.ActiveFilterString = "[tur] = '"+ comboBoxEdit1.Text+"'";

        }
        void TariheGoreGrid()
        {
            gridView1.ActiveFilter.Clear();
            gridView1.ActiveFilterString = "[tarih] = '" + dateEdit1.Text + "'";
        }
       
        #region baslik
        void Baslik()
        {
            this.gridView1.Columns[0].Caption = "Evrak No";
            this.gridView1.Columns[1].Caption = "Evrak Adı";
            this.gridView1.Columns[2].Caption = "Durumu";
            this.gridView1.Columns[3].Caption = "Açıklama";
            this.gridView1.Columns[4].Caption = "Tarih";
            this.gridView1.Columns[5].Caption = "Tür";
            this.gridView1.Columns[6].Caption = "Dosya Adı";

            object CellValue = gridView1.GetFocusedRowCellValue("evrakID");
            evrakID = int.Parse(CellValue.ToString());
            if (evrakID >= 1 && evrakID <= 1000 )
            gridView1.Columns[0].Width = 30;
            else if (evrakID >= 10001 && evrakID <= 100000)
                gridView1.Columns[0].Width = 50;

            gridView1.Columns[4].Width = 40;

        }
        #endregion
        void Listboxdoldur()
        {          
            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                MySqlCommand command = new MySqlCommand("SelectKullaniciProd", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string columnValue = reader.GetString(0);
                    listBoxControl1.Items.Add(columnValue);
                }
                reader.Close();
            }
        }
      
        private void Arsiv_Load(object sender, EventArgs e)
        {
            
            GridDoldur();
            Baslik();
            Listboxdoldur();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if(comboBoxEdit1.SelectedIndex != -1)
            TureGoreGrid();
            else
                MessageBox.Show("Lütfen listeden bir seçim yapın.");
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void gridControl1_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void gridControl1_FocusedViewChanged(object sender, DevExpress.XtraGrid.ViewFocusEventArgs e)
        {
            
        }

        private void gridControl1_Enter(object sender, EventArgs e)
        {
            
        }
        int evrakID;
        private void gridControl1_MouseCaptureChanged(object sender, EventArgs e)
        {
            if(gridView1.SelectedRowsCount == 1)//Boş datagrid'e tıklandığında exception fırlatmaması için
            {
                object CellValue = gridView1.GetFocusedRowCellValue("dosyaYolu");
                object CellValue2 = gridView1.GetFocusedRowCellValue("evrakID");
                evrakID = int.Parse(CellValue2.ToString());
                string yol = CellValue.ToString();
                pdfViewer1.DocumentFilePath = yol;
            }          
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (dateEdit1.SelectedText != "")
                TariheGoreGrid();
            else
                MessageBox.Show("Lütfen bir tarih seçin.");
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            GridDoldur();
        }
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Onizleme onizle = new Onizleme();
            onizle.pdfViewer1.DocumentFilePath = pdfViewer1.DocumentFilePath;
            onizle.Show();
        }
        private void Arsiv_FormClosed(object sender, FormClosedEventArgs e)
        {
            Func.CikisYap();
            Application.Exit();
        }
        void KayitGunelle(int id, string YeniDeger)
        {
            string query = "UPDATE evrak SET emanetVerildi = @YeniDeger WHERE evrakID = @id";

            using (MySqlConnection connection = new MySqlConnection(Func.connectionString))
            {
                connection.Open();              
                MySqlCommand command = new MySqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.Text;            
                command.Parameters.AddWithValue("@YeniDeger", YeniDeger);
                command.Parameters.AddWithValue("@id", id);
               
                int rowsAffected = command.ExecuteNonQuery();
                
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Kayıt güncellendi.");
                }
                else
                {
                    MessageBox.Show("Güncellenecek kayıt bulunamadı.");
                }
            }

        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            // update ve select gelicek
            KayitGunelle(evrakID, listBoxControl1.SelectedItem+" kullanıcısına teslim edildi.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}