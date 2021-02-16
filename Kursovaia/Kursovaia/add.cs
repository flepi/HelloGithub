using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Kursovaia
{
    public partial class add : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;
        

        public add()
        {
            InitializeComponent();
            MySqlConnection conn = new MySqlConnection(connStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_user(textBox1.Text);
        }
        public void add_user(string add)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            string new_id = textBox1.Text;
            string new_doc = textBox2.Text;
            string new_pos = textBox3.Text;
            string new_cab = textBox4.Text;
            string new_data = textBox5.Text;
            string new_phone = textBox6.Text;

            string sql_add_doctor = "INSERT INTO `st_2_11`.`doctors` (`id_doc`, `fio_doc`, `dolg`, `cab`, `data`, `phone`)" +
            "VALUES ('" + new_id + "' , '" + new_doc + "', '" + new_pos + "', '" + new_cab + "', '" + new_data + "', '" + new_phone + "')";
            MySqlCommand add_user = new MySqlCommand(sql_add_doctor, conn);

            try
            {
                conn.Open();
                add_user.ExecuteNonQuery();
                MessageBox.Show(" Пользователь успешно добавлен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавление  пользователя \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                

            }


        }



        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
