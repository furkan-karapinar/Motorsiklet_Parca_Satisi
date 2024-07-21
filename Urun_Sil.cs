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
    public partial class Urun_Sil : Form
    {
        public Urun_Sil()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";
        int secilen_id = -1;
        DataTable dataTable;
        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(textBox1.Text) || secilen_id == -1)
                    {
                        MessageBox.Show("Lütfen bir ürün seçiniz");
                    }
                    else
                    {
                        conn.Open();

                        string sql = $"DELETE FROM urunler WHERE id = '{secilen_id}'";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
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

                    string sql = $"SELECT * FROM urunler";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Herhangi bir ürün eklenmemiş");
                        }
                        else
                        {
                            dataTable = new DataTable();
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

        private void Urun_Sil_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    secilen_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                    textBox1.Text = dataGridView1.CurrentRow.Cells["urun_adi"].Value.ToString();
                }

            }
            catch (NullReferenceException) { }

        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                dataTable.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                try
                {
                    // Filtreleme ifadesini oluşturma
                    string filterExpression = string.Format("CONVERT(id, 'System.String') LIKE '%{0}%'", textBox2.Text);

                    // DataView kullanarak filtreleme
                    DataView dv = dataTable.DefaultView;
                    dv.RowFilter = filterExpression;

                    // Filtrelenmiş DataView'i DataGridView'e bağlama
                    dataGridView1.DataSource = dv;
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesajı yazdırın
                    MessageBox.Show("Eksik ya da yanlış veri girdiniz veya veri mevcut değil");
                }
            }
        }
    }
}
