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
    public partial class edit : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;

        public edit()
        {
            InitializeComponent();
            MySqlConnection conn = new MySqlConnection(connStr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edit_user(textBox1.Text);
        }

        public void edit_user(string edit)
        {
            MySqlConnection conn = new MySqlConnection(connStr);


            string sql_edit_doctors = "UPDATE doctors SET dolg = '" + textBox2.Text + "' WHERE id_doc = " + edit;

            MySqlCommand edit_user = new MySqlCommand(sql_edit_doctors, conn);

            try
            {
                conn.Open();
                edit_user.ExecuteNonQuery();
                MessageBox.Show(" Пользователь успешно изменен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменение  пользователя \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
