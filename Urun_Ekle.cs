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
    public partial class Urun_Ekle : Form
    {
        public Urun_Ekle()
        {
            InitializeComponent();
        }

        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(maskedTextBox1.Text) || String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(comboBox1.Text))
                    {
                        MessageBox.Show("Lütfen tüm alanları doldurunuz");
                    }
                    else
                    {
                        conn.Open();

                        string sql = $"INSERT INTO urunler (id , urun_adi , kategori_id , adet , fiyat) VALUES('{maskedTextBox1.Text}' , '{textBox1.Text}' , '{comboBox1.SelectedValue.ToString()}' , '{numericUpDown1.Value.ToString()}' , '{numericUpDown2.Value.ToString()}')";
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
            maskedTextBox1.Clear();
            textBox1.Clear();
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
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
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            comboBox1.DataSource = dataTable;
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


        private void Urun_Ekle_Load(object sender, EventArgs e)
        {
            listele();
        }
    }
}
