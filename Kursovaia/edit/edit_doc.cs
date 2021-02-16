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
    public partial class edit_doc : Form
    {
        //Переменная, хранящая ID пользователя которого будут менять
        string id_user_for_edit;

        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;
        int id_current_dolg;


        public edit_doc()
        {
            InitializeComponent();
        }



        //Метод заполнения ComboBox с должностями
        public void GetListDolg()
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            //Формирование списка статусов
            DataTable list_dolg_table = new DataTable();
            MySqlCommand list_dolg_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка отделов
            list_dolg_table.Columns.Add(new DataColumn("id_dolg", System.Type.GetType("System.Int32")));
            list_dolg_table.Columns.Add(new DataColumn("name_dolg", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_dolg_table;
            comboBox1.DisplayMember = "name_dolg";    // столбец для отображения
            comboBox1.ValueMember = "id_dolg";       //  столбец с id
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT id_dolg, name_dolg FROM t_dolg";
            list_dolg_command.CommandText = sql_list_users;
            list_dolg_command.Connection = conn;
            //Формирование списка отделов для combobox'a
            MySqlDataReader reader_list_otdel;
            try
            {
                reader_list_otdel = list_dolg_command.ExecuteReader();

                while (reader_list_otdel.Read())
                {
                    DataRow rowToAdd = list_dolg_table.NewRow();
                    rowToAdd["id_dolg"] = Convert.ToInt32(reader_list_otdel[0]);
                    rowToAdd["name_dolg"] = reader_list_otdel[1].ToString();
                    list_dolg_table.Rows.Add(rowToAdd);
                }
                reader_list_otdel.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка чтения списка статусов \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
            }
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
            string get_current_string_sql = "SELECT * FROM doctors WHERE id_doc = " + id_user_for_edit;
            //Объявляем команду для запроса данных по текущему пользователю
            MySqlCommand current_user_command = new MySqlCommand(get_current_string_sql, conn);
            conn.Open();
            MySqlDataReader current_user_reader = current_user_command.ExecuteReader();
            //Получаем текущие значения полей пользователя
            while (current_user_reader.Read())
            {
                DateTime dt_db;
                dt_db = Convert.ToDateTime(current_user_reader[5].ToString());
                dateTimePicker1.Value = dt_db;
                textBox1.Text = current_user_reader[1].ToString();
                //Получение ID текущей должности пользователя
                id_current_dolg = Convert.ToInt32(current_user_reader[2]);
                //Указаний ComboBox что бы были выбраны те значения, которые указаны в таблице БД
                comboBox1.SelectedValue = id_current_dolg;
                textBox3.Text = current_user_reader[3].ToString();
                textBox5.Text = current_user_reader[4].ToString();

                //Изменение метки формы
                this.Text = "Редактирование пользователя: " + current_user_reader[1];
            }
            conn.Close();
        }



            //Метод обновления текущей записи для выбранного пользователя
            public void UpdateCurrentString(string fio, int  dolg, string cab, string phone, string data)
        {
                //Инициализируем соединение с БД
                 conn = new MySqlConnection(connStr);
                 //Формируем строку запроса на добавление строк
                 string sql_update_user = "UPDATE doctors " +
                "SET " +
                "fio_doc='" + fio + "', " +
                "dolg='" + dolg.ToString() + "', " +
                "cab='" + cab + "', " +
                "phone='" + phone + "', " +
                "data='" + data + "' " +
                "WHERE (id_doc='" + id_user_for_edit + "') " +
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

        //Загрузка формы
        private void edit_user_Load(object sender, EventArgs e)
        {

            //Вызов метода для загрузки текущих значений записи
            GetCurrentString();
            //Вызов методов для заполнения ComboBox1
            GetListDolg();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int id_new_dolg = Convert.ToInt32(comboBox1.SelectedValue);
            UpdateCurrentString(textBox1.Text, id_new_dolg, textBox3.Text,  textBox5.Text, dateTimePicker1.Value.ToString("yyyy-MM-dd"));
        }
    }
}
