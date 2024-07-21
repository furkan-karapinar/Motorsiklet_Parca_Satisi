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
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";
        decimal kdv = 1.18m;

        private void button1_Click(object sender, EventArgs e)
        {
            Kategorii_Yonetimi kategori_Yonetimi = new Kategorii_Yonetimi();
            kategori_Yonetimi.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Urun_Yonetimi urun_Yonetimi = new Urun_Yonetimi();
            urun_Yonetimi.ShowDialog();
        }

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"SELECT * FROM ayarlar";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Herhangi bir ayar eklenmemiş");
                            }
                            else
                            {
                                kdv = Convert.ToDecimal($"1,{reader["kdv_orani"]}");
                                textBox1.Text = "%" + reader["kdv_orani"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text = DateTime.Now.ToString("HH:mm  --  dd.MM.yyyy");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"SELECT u.id , u.urun_adi , k.kategori_adi as kategori, u.adet, u.fiyat FROM urunler u INNER JOIN kategoriler k ON k.id = u.kategori_id WHERE u.id = '{textBox2.Text}'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Ürün mevcut değil veya yanlış barkod numarası girdiniz");
                            }
                            else
                            {

                                if (Convert.ToDecimal(reader["adet"]) == 0 || (Convert.ToDecimal(reader["adet"]) - numericUpDown1.Value) < 0)
                                {
                                    MessageBox.Show($"Stokta yeterli ürün yok. Üründen {reader["adet"]} adet mevcut");
                                }
                                else
                                {
                                    dataGridView1.Rows.Add(reader["id"], reader["urun_adi"], reader["kategori"], numericUpDown1.Value, Convert.ToDecimal(reader["fiyat"]) * kdv * numericUpDown1.Value);
                                }


                            }
                        }


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
            numericUpDown1.Value = 1;
            textBox2.Text = "";
            toplam_hesapla();
        }

        private void toplam_hesapla()
        {
            decimal sum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Satırın geçerli olup olmadığını kontrol etme
                if (row.Cells[4].Value != null && row.Cells[4].Value != DBNull.Value)
                {
                    // Hücredeki değeri decimal türüne dönüştür ve toplama ekle
                    sum += Convert.ToDecimal(row.Cells[4].Value);
                }
            }

            textBox3.Text = sum.ToString();

            if (textBox4.Text != "")
            {
                textBox5.Text = (Convert.ToDecimal(textBox3.Text) - Convert.ToDecimal(textBox4.Text)).ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Sepetiniz boş");
            }
            else
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                toplam_hesapla();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Sepetiniz boş");
            }
            else
            {
                dataGridView1.Rows.Clear();
                textBox3.Text = "0";
                textBox4.Text = "";
                textBox5.Text = "0";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                textBox5.Text = (Convert.ToDecimal(textBox3.Text) - Convert.ToDecimal(textBox4.Text)).ToString();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int urun_id = Convert.ToInt32(row.Cells[0].Value);
                decimal adet = Convert.ToDecimal(row.Cells[3].Value);
                decimal toplam_urun_adedi = 0;

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string sql = $"SELECT * FROM urunler WHERE id = '{urun_id}'";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                toplam_urun_adedi = Convert.ToDecimal(reader["adet"]) - adet;
                            }
                        }

                        string update = $"UPDATE urunler SET adet = '{toplam_urun_adedi}' WHERE id = '{urun_id}'";
                        MySqlCommand cmd2 = new MySqlCommand(update, conn);
                        cmd2.ExecuteNonQuery();

                        MessageBox.Show("Satış işlemi başarılı");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Bir hata oluştu: " + ex.Message);
                    }
                }
            }

            dataGridView1.Rows.Clear();
            textBox3.Text = "0";
            textBox4.Text = "";
            textBox5.Text = "0";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            kdv_ayarla kdv_Ayarla = new kdv_ayarla();
            kdv_Ayarla.ShowDialog();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"SELECT * FROM ayarlar";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Herhangi bir ayar eklenmemiş");
                            }
                            else
                            {
                                kdv = Convert.ToDecimal($"1,{reader["kdv_orani"]}");
                                textBox1.Text = "%" + reader["kdv_orani"].ToString();
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
