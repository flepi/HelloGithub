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
    public partial class add_journal : Form
    {
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;
        

        public add_journal()
        {
            InitializeComponent();
        }

        //Метод заполнения ComboBox с докторами
        public void GetListDoc()
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            //Формирование списка статусов
            DataTable list_dolg_table = new DataTable();
            MySqlCommand list_dolg_command = new MySqlCommand();
            //Открываем соединение
            conn.Open();
            //Формируем столбцы для комбобокса списка отделов
            list_dolg_table.Columns.Add(new DataColumn("id_doc", System.Type.GetType("System.Int32")));
            list_dolg_table.Columns.Add(new DataColumn("fio_doc", System.Type.GetType("System.String")));
            //Настройка видимости полей комбобокса
            comboBox1.DataSource = list_dolg_table;
            comboBox1.DisplayMember = "fio_doc";    // столбец для отображения
            comboBox1.ValueMember = "id_doc";       //  столбец с id
            //Формируем строку запроса на отображение списка статусов прав пользователя
            string sql_list_users = "SELECT id_doc, fio_doc FROM doctors";
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
                    rowToAdd["id_doc"] = Convert.ToInt32(reader_list_otdel[0]);
                    rowToAdd["fio_doc"] = reader_list_otdel[1].ToString();
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
        private void button1_Click(object sender, EventArgs e)
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            conn = new MySqlConnection(connStr);
            int new_doc = Convert.ToInt32(comboBox1.SelectedValue);
            string new_pat = textBox3.Text;
            string new_data = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string new_time = textBox5.Text;
            string new_cab = textBox6.Text;


            string sql_add_journal = "INSERT INTO `st_2_11`.`journal` (`fio_doc`, `fio_pat`, `data`, `time`, `cab` )" +
            "VALUES ('" + new_doc + "', '" + new_pat + "', '" + new_data + "', '" + new_cab + "', '" + new_time + "')";
            //Посылаем запрос на добавление данных
            MySqlCommand add_user = new MySqlCommand(sql_add_journal, conn);

            try
            {
                conn.Open();
                add_user.ExecuteNonQuery();
                MessageBox.Show("Добавление прошло успешно ", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void insert_user_Load(object sender, EventArgs e)
        {
            //Вызов методов для заполнения ComboBox
            GetListDoc();
        }




        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
