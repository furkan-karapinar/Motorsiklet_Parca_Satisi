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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Motorsiklet_Parca_Satisi
{
    public partial class Kategorii_Yonetimi : Form
    {
        public Kategorii_Yonetimi()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";

        private void button1_Click(object sender, EventArgs e)
        {
            Kategori_Ekle kategori_Ekle = new Kategori_Ekle();
            kategori_Ekle.ShowDialog();
            listele();
        }

        private void listele()
        {
            dataGridView1.DataSource = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"SELECT * FROM kategoriler";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Herhangi bir kategori eklenmemiş");
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

        private void Kategorii_Yonetimi_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kategori_Duzenle kategori_Duzenle = new Kategori_Duzenle();
            kategori_Duzenle.ShowDialog();
            listele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Kategori_Sil kategori_Sil = new Kategori_Sil();
            kategori_Sil.ShowDialog();
            listele();
        }
    }
}
