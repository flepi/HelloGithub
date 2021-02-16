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
    public partial class add_doc : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;



        public add_doc()
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
            comboBox1.DisplayMember = "name_dolg";
            comboBox1.ValueMember = "id_dolg";
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT id_dolg,name_dolg FROM t_dolg";
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

        private void insert_user_Load(object sender, EventArgs e)
        {
            //Вызов методов для заполнения ComboBox
            GetListDolg();
        }

            private void button1_Click(object sender, EventArgs e)
         {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            string new_doc = textBox2.Text;
            string new_cab = textBox4.Text;
            int new_dolg = Convert.ToInt32(comboBox1.SelectedValue);
            string new_data = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string new_phone = textBox6.Text;

            string sql_add_doctor = "INSERT INTO `st_2_11`.`doctors` ( `fio_doc`, `dolg`, `cab`, `data`, `phone`)" +
            "VALUES ('" + new_doc + "', '" + new_dolg + "', '" + new_cab + "', '" + new_data + "', '" + new_phone + "')";
            //Посылаем запрос на добавление данных
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
