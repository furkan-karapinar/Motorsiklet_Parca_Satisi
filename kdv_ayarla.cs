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
    public partial class kdv_ayarla : Form
    {
        public kdv_ayarla()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;Database=motorsiklet;Uid=root;Pwd=;";

        private void kdv_ayarla_Load(object sender, EventArgs e)
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
                        while (reader.Read()) { 
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Herhangi bir ayar eklenmemiş");
                        }
                        else
                        {
                            numericUpDown1.Value = Convert.ToDecimal(reader["kdv_orani"]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {

                    conn.Open();

                    string sql = $"UPDATE ayarlar SET kdv_orani = '{numericUpDown1.Value}'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("KDV oranı güncellendi");
                    this.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bir hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
