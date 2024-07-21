using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Motorsiklet_Parca_Satisi
{
    public partial class Urun_Yonetimi : Form
    {
        public Urun_Yonetimi()
        {
            InitializeComponent();
        }

        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";

        private void listele()
        {
            dataGridView1.DataSource = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"SELECT * FROM urunler";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Herhangi bir urun eklenmemiş");
                        }
                        else
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridView1.DataSource = dataTable;
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
        }


        private void Urun_Yonetimi_Load(object sender, EventArgs e)
        {
            listele();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Urun_Ekle urun_ekle = new Urun_Ekle();
            urun_ekle.ShowDialog();
            listele();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Urun_Duzenle urun_duzenle = new Urun_Duzenle();
            urun_duzenle.ShowDialog();
            listele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Urun_Sil urun_sil = new Urun_Sil();
            urun_sil.ShowDialog();
            listele();
        }



  
    }
}
