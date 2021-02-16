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
    public partial class add_pat : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;

        public add_pat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            string new_pat = textBox2.Text;
            string new_adress = textBox3.Text;
            string new_polis = textBox4.Text;
            string new_phone = textBox5.Text;
            string new_pol = textBox6.Text;
            string new_roj = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string sql_add_pat = "INSERT INTO `st_2_11`.`patients` ( `fio_pat`, `adress`, `med_polis`, `phone`, `pol`, `data`)" +
            "VALUES ('" + new_pat + "', '" + new_adress + "', '" + new_polis + "', " +
            "'" + new_phone + "', '" + new_pol + "', '" + new_roj + "')";
            //Посылаем запрос на добавление данных
            MySqlCommand add_user = new MySqlCommand(sql_add_pat, conn);

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
            DialogResult vibor2 = MessageBox.Show("Закрыть форму без сохранения изменений?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            //Закрытие формы без сохранения изменений
            if (vibor2 == DialogResult.Yes)
            {
                this.Close();
            }
            //Закрытие диалога
            if (vibor2 == DialogResult.No)
            {
            }
        }
    }
}
