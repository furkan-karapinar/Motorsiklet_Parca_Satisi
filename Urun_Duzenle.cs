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
    public partial class Urun_Duzenle : Form
    {
        public Urun_Duzenle()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";
        int secilen_id = -1;
        DataTable dataTable;

        private void Urun_Duzenle_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(comboBox1.Text))
                    {
                        MessageBox.Show("Lütfen boş alanları doldurunuz");
                    }
                    else
                    {
                        conn.Open();

                        string sql = $"UPDATE urunler SET urun_adi = '{textBox1.Text}' , kategori_id = '{comboBox1.SelectedValue.ToString()}' , adet = {numericUpDown1.Value.ToString()} , fiyat = '{numericUpDown2.Value.ToString()}' WHERE id = '{secilen_id}'";
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

                    string sql = $"SELECT u.id , u.urun_adi , k.kategori_adi , u.adet , u.fiyat FROM urunler u INNER JOIN kategoriler k ON k.id = u.kategori_id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Herhangi bir kategori eklenmemiş");
                        }
                        else
                        {
                            secilen_id = -1;
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
            kategori_listele();
        }
        private void kategori_listele()
        {
            comboBox1.DataSource = null;
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
                            DataTable combodataTable = new DataTable();
                            combodataTable.Load(reader);
                            comboBox1.DataSource = combodataTable;
                            comboBox1.DisplayMember = "kategori_adi";
                            comboBox1.ValueMember = "id";
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    secilen_id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value);
                    textBox1.Text = dataGridView1.CurrentRow.Cells["urun_adi"].Value.ToString();
                    comboBox1.Text = dataGridView1.CurrentRow.Cells["kategori_adi"].Value.ToString();
                    numericUpDown1.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["adet"].Value);
                    numericUpDown2.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["fiyat"].Value);
                }
            }
            catch (NullReferenceException) { }
            catch (Exception) { }
            
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
