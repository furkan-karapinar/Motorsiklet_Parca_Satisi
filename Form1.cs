using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Motorsiklet_Parca_Satisi
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(textBox2.Text))
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre boş bırakılamaz");
                    }
                    else
                    {
                        conn.Open();

                        string sql = $"SELECT id FROM kullanicilar WHERE kullanici_adi = '{textBox1.Text}' AND sifre = '{textBox2.Text}'";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show("Giriş Başarılı");
                                Anasayfa anasayfa = new Anasayfa();
                                this.Hide();
                                anasayfa.ShowDialog();
                                this.Show();
                            }
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
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
