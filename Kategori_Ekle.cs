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
    public partial class Kategori_Ekle : Form
    {
        public Kategori_Ekle()
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
                    if (String.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("Kategori adı boş bırakılamaz");
                    }
                    else
                    {
                        conn.Open();

                        string sql = $"INSERT INTO kategoriler (kategori_adi) VALUES('{textBox1.Text}')";
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

        private void Kategori_Ekle_Load(object sender, EventArgs e)
        {
            listele();
        }
    }
}
