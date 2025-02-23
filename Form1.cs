using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SQLite;
using System.IO.Ports;

namespace Diplom_project
{
    public partial class Main: Form
    {
        private string selectedFilePath = ""; // Будет хранить путь к БД

        public Main()
        {
            InitializeComponent();
        }        

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//ОТКРЫТИЕ ФОРМЫ РЕГИСТРАЦИЯ КЛИЕНТА
        {
            
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Сначала выберите базу данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddClient form2 = new AddClient(selectedFilePath);
            form2.ShowDialog(); // Открываем форму модально
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void listBoxSamples_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClients();
        }

        private void buttonDellClient_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void selectBDToolStripMenuItem_Click(object sender, EventArgs e)//выбор файла базы данных
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл базы данных";
                openFileDialog.Filter = "SQLite Database (*.db;*.sqlite)|*.db;*.sqlite|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = openFileDialog.FileName;

                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection($"Data Source={selectedFilePath};Version=3;"))
                        {
                            connection.Open();
                            MessageBox.Show("Подключение к базе данных успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        // Загружаем клиентов после подключения к БД
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void LoadClients()//выгрузка списка клиентов из базы в листбокс
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Сначала выберите базу данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            listBoxClients.Items.Clear(); // Очищаем ListBox перед загрузкой

            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={selectedFilePath};Version=3;"))
            {
                connection.Open();
                string query = "SELECT FIO, Phone_num FROM Client";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string clientName = reader["FIO"].ToString();
                        string phoneNumber = reader["Phone_num"].ToString();
                        listBoxClients.Items.Add($"{clientName} - {phoneNumber}");
                    }
                }
            }
        }



        private void selectCOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Получаем список доступных COM-портов
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                MessageBox.Show("Нет доступных COM-портов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем всплывающее окно с выпадающим списком
            Form comPortForm = new Form
            {
                Text = "Выбор COM-порта",
                Size = new System.Drawing.Size(300, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            ComboBox comboBoxPorts = new ComboBox
            {
                DataSource = ports,
                Dock = DockStyle.Top
            };

            Button buttonOK = new Button
            {
                Text = "Выбрать",
                Dock = DockStyle.Bottom
            };

            buttonOK.Click += (s, args) =>
            {
                string selectedPort = comboBoxPorts.SelectedItem.ToString();
                MessageBox.Show($"Выбранный COM-порт: {selectedPort}", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comPortForm.Close();
            };

            comPortForm.Controls.Add(comboBoxPorts);
            comPortForm.Controls.Add(buttonOK);
            comPortForm.ShowDialog();
        }
    }
}
