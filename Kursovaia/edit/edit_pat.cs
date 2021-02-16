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
    public partial class edit_pat : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;
        //Переменная, хранящая ID пользователя которого будут менять
        string id_user_for_edit;

        public edit_pat()
        {
            InitializeComponent();
        }

        //Метод загрузки текущих значений для выбранного пользователя
        public void GetCurrentString()
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            //Передача ID пользователя из глобальной переменной
            id_user_for_edit = class_edit_user.id;
            //Вывод этой переменной в заголовок формы
            this.Text = "ID пользователя для изменения: " + id_user_for_edit;
            //Формируем запрос, на заполнения всех текущих полей формы, для того, что бы пользователь подправил что ему нужно
            string get_current_string_sql = "SELECT * FROM patients WHERE id_pat = " + id_user_for_edit;
            //Объявляем команду для запроса данных по текущему пользователю
            MySqlCommand current_user_command = new MySqlCommand(get_current_string_sql, conn);
            conn.Open();
            MySqlDataReader current_user_reader = current_user_command.ExecuteReader();
            //Получаем текущие значения полей пользователя
            while (current_user_reader.Read())
            {
                DateTime dt_db;
                dt_db = Convert.ToDateTime(current_user_reader[6].ToString());
                dateTimePicker1.Value = dt_db;
                textBox1.Text = current_user_reader[1].ToString();
                textBox2.Text = current_user_reader[2].ToString();
                textBox3.Text = current_user_reader[3].ToString();
                textBox4.Text = current_user_reader[4].ToString();
                textBox5.Text = current_user_reader[5].ToString();
                //Изменение метки формы
                this.Text = "Редактирование пользователя: " + current_user_reader[1];
            }
            conn.Close();
        }


        //Метод обновления текущей записи для выбранного пользователя
        public void UpdateCurrentString(string fio, string adress, string polis, string phone, string pol, string dt)
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);

            //Формируем строку запроса на добавление строк
            string sql_update_user = "UPDATE patients " +
          "SET " +
          "fio_pat='" + fio + "', " +
          "adress='" + adress + "', " +
          "med_polis='" + polis + "', " +
          "phone='" + phone + "', " +
          "pol='" + pol + "', " +
          "data='" + dt + "' " +
          "WHERE (id_pat='" + id_user_for_edit + "') " +
          "LIMIT 1";

            //Посылаем запрос на обновление данных
            MySqlCommand update_user = new MySqlCommand(sql_update_user, conn);
            try
            {
                conn.Open();
                update_user.ExecuteNonQuery();
                MessageBox.Show("Изменение прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка изменения строки \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
        }

        //Загрузка формы
        private void edit_user_Load(object sender, EventArgs e)
        {

            //Вызов метода для загрузки текущих значений записи
            GetCurrentString();
        }
        //Кнопка изменить
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateCurrentString(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
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
