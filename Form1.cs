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
using System.IO;


namespace Diplom_project
{
    public partial class Main: Form
    {
        private string selectedFilePath = ""; // Будет хранить путь к БД
        
        private string configFilePath = "C:/Users/isavr/OneDrive/Рабочий стол/Диплом/Diplom_project/tmp/Diplom_project.inc"; // Файл конфигурации
        public string ConnectionString { get; private set; }

        private string sortOrder = "FIO"; // По умолчанию сортировка по ФИО

        public string SortOrder
        {
            get { return sortOrder; }
            set
            {
                sortOrder = value;
                LoadClients(); // Автообновление списка при изменении сортировки
            }
        }


        public Main()
        {
            InitializeComponent();
        }

        private void LoadDatabasePath()
        {
            if (File.Exists(configFilePath))
            {
                string path = File.ReadAllText(configFilePath).Trim();

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    selectedFilePath = path;
                    ConnectionString = $"Data Source={selectedFilePath};Version=3;";

                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                        {
                            connection.Open();
                        }

                        LoadClients(); // Загружаем данные из базы
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Файл конфигурации найден, но путь к базе данных неверен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Файл конфигурации не найден. Выберите базу данных.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//ОТКРЫТИЕ ФОРМЫ РЕГИСТРАЦИЯ КЛИЕНТА
        {
            int selectedIndex = listBoxClients.SelectedIndex; // Запоминаем индекс
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Сначала выберите базу данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddClient form2 = new AddClient(selectedFilePath, this);
            form2.ShowDialog(); // Открываем форму модально
                                // После удаления восстанавливаем выделение
            if (listBoxClients.Items.Count > 0)
            {
                if (selectedIndex >= listBoxClients.Items.Count)
                {
                    selectedIndex = listBoxClients.Items.Count - 1; // Если удалили последний, выбираем предыдущий
                }

                listBoxClients.SelectedIndex = selectedIndex; // Восстанавливаем выделение
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void listBoxSamples_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
           // LoadClients();
        }

        private void buttonDellClient_Click(object sender, EventArgs e)//удаление клиента
        {
            if (listBoxClients.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedIndex = listBoxClients.SelectedIndex; // Запоминаем индекс
            string selectedClient = listBoxClients.SelectedItem.ToString();
            string[] parts = selectedClient.Split('-');

            if (parts.Length < 2)
            {
                MessageBox.Show("Ошибка в данных клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fio = parts[0].Trim();

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить {fio}?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Client WHERE FIO = @FIO";
                        using (SQLiteCommand command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FIO", fio);
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadClients(); // Обновляем список

                    // После удаления восстанавливаем выделение
                    if (listBoxClients.Items.Count > 0)
                    {
                        if (selectedIndex >= listBoxClients.Items.Count)
                        {
                            selectedIndex = listBoxClients.Items.Count - 1; // Если удалили последний, выбираем предыдущий
                        }

                        listBoxClients.SelectedIndex = selectedIndex; // Восстанавливаем выделение
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadDatabasePath();
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
                    ConnectionString = $"Data Source={selectedFilePath};Version=3;";

                    try
                    {
                        using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                        {
                            connection.Open();
                        }

                        // Сохраняем путь в конфигурационный файл
                        File.WriteAllText(configFilePath, selectedFilePath);

                        // Загружаем клиентов
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public void LoadClients()//выгрузка списка клиентов из базы в листбокс
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
                string query = $"SELECT FIO, Phone_num FROM Client ORDER BY {SortOrder} ASC";

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


        //должен быть установлен драйвер для ардуино 
        private void selectCOMPortToolStripMenuItem_Click(object sender, EventArgs e)//выбор компорта - сначала полдключаем контролллер
        {
            // Получаем список доступных COM-портов
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                MessageBox.Show("Нет доступных COM-портов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем всплывающее окно с выпадающим списком из подключенных ком портов
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

                  
        

        private void buttonChangeDataClient_Click(object sender, EventArgs e)//открываем форму для изменения данных клиента
        {
            if (listBoxClients.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем выбранного клиента
            int selectedIndex = listBoxClients.SelectedIndex; // Запоминаем индекс
            string selectedClient = listBoxClients.SelectedItem.ToString();
            string[] parts = selectedClient.Split('-');
            if (parts.Length < 2)
            {
                MessageBox.Show("Ошибка в данных клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fio = parts[0].Trim();
            string phone = parts[1].Trim();

            // Открываем ChangeClient (Form3), передавая данные фио и номер телефона для заполнения
            ChangeClient form3 = new ChangeClient(this, fio, phone, ConnectionString);
            form3.ShowDialog(); // Открываем форму
                                // После удаления восстанавливаем выделение
            if (listBoxClients.Items.Count > 0)
            {
                if (selectedIndex >= listBoxClients.Items.Count)
                {
                    selectedIndex = listBoxClients.Items.Count - 1; // Если удалили последний, выбираем предыдущий
                }

                listBoxClients.SelectedIndex = selectedIndex; // Восстанавливаем выделение
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sortedByFIOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortOrder = "FIO"; // Меняем сортировку на ФИО
        }

        private void sortedByPhoneNumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortOrder = "Phone_num"; // Меняем сортировку на телефон
        }
    }
}
