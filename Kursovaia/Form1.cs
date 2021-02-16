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
    public partial class Form1 : Form
    {
        //Строка подключения 
        string connStr = "server= osp74.ru; user = st_2_11; database= st_2_11; password = 53259459; port = 33333";
        MySqlConnection conn;
        private MySqlDataAdapter MyDA = new MySqlDataAdapter();
        private BindingSource bSource = new BindingSource();
        private DataSet ds = new DataSet();
        private DataTable table = new DataTable();
        string id_selected_rows;

        public Form1()
        {
            InitializeComponent();

        }


        //Метод наполнения DataGreed
        public void GetListUsers()
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            //Запрос для вывода строк в БД
            string commandStr = "SELECT * FROM journal";
            //Открываем соединение
            conn.Open();
            //Объявляем команду, которая выполнить запрос в соединении conn
            MyDA.SelectCommand = new MySqlCommand(commandStr, conn);
            //Заполняем таблицу записями из БД
            MyDA.Fill(table);
            //Указываем, что источником данных в bindingsource является заполненная выше таблица
            bSource.DataSource = table;
            //Указываем, что источником данных ДатаГрида является bindingsource 
            dataGridView1.DataSource = bSource;
            //Закрываем соединение
            conn.Close();       
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            conn.Open();
            conn = new MySqlConnection(connStr);
            //Вызываем метод для заполнение дата Грида
            GetListUsers();
            //Видимость полей в гриде
            dataGridView1.Columns[0].Visible = true;
            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            //Ширина полей
            dataGridView1.Columns[0].FillWeight = 10;
            dataGridView1.Columns[1].FillWeight = 50;
            dataGridView1.Columns[2].FillWeight = 20;
            dataGridView1.Columns[3].FillWeight = 20;
            //Растягивание полей грида
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //Убираем заголовки строк
            dataGridView1.RowHeadersVisible = false;
            //Показываем заголовки столбцов
            dataGridView1.ColumnHeadersVisible = true;
            conn.Close();
        }      
        public void reload_list()
        {
            //Чистим виртуальную таблицу
            table.Clear();
            //Вызываем метод получения записей, который вновь заполнит таблицу
            GetListUsers();
        }


        //Метод получения ID выделенной строки, для последующего вызова его в нужных методах
        public void GetSelectedIDString()
        {
            //Переменная для индекс выбранной строки в гриде
            string index_selected_rows;
            //Индекс выбранной строки
            index_selected_rows = dataGridView1.SelectedCells[0].RowIndex.ToString();
            //ID конкретной записи в Базе данных, на основании индекса строки
            id_selected_rows = dataGridView1.Rows[Convert.ToInt32(index_selected_rows)].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Инициализируем соединение с БД
            conn = new MySqlConnection(connStr);
            //Формируем строку запроса на добавление строк
            string sql_delete_user = "DELETE FROM journal WHERE id_talona='" + id_selected_rows + "'";
            //Посылаем запрос на обновление данных
            MySqlCommand delete_user = new MySqlCommand(sql_delete_user, conn);
            try
            {
                conn.Open();
                delete_user.ExecuteNonQuery();
                MessageBox.Show("Удаление прошло успешно", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления строки \n\n" + ex, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            finally
            {
                conn.Close();
                //Вызов метода обновления ДатаГрида
                reload_list();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            add_journal f2 = new add_journal();
            f2.Owner = this;
            f2.ShowDialog();
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void сотрудникиToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            doctors f6 = new doctors();
            f6.Owner = this;
            f6.ShowDialog();
        }

        private void пациентыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            patients f5 = new patients();
            f5.Owner = this;
            f5.ShowDialog();
        }


        private void обновитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reload_list();
        }

        private void выходToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void записатьсяНаПриёмToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            priem f9 = new priem();
            f9.Owner = this;
            f9.ShowDialog();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            reload_list();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            //Метод получения ID выделенной строки, для последующего вызова его в нужных методах
            GetSelectedIDString();
        }
        
        //Изменить журнал
        private void button2_Click(object sender, EventArgs e)
        {
            class_edit_user.id = id_selected_rows;
            edit_journal edit_user = new edit_journal();
            edit_user.ShowDialog();
            reload_list();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Объявляем и инициализируем построитель команд и привязываем его к Адаптеру
            MySqlCommandBuilder cb = new MySqlCommandBuilder(MyDA);
            //Обновляем таблицу (виртуальную)
            MyDA.Update(table);
            //Вызываем метод обновления грида
            reload_list();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
                if (toolStripTextBox1.Text != "")
                {
                    bSource.Filter = "id_talona='" + toolStripTextBox1.Text + "'";
                }
                else
                {
                    bSource.RemoveFilter();
                }

            
        }
    }
}