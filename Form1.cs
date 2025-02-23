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
        private string connectionString = ""; //строка для хранения полного пути к бд 


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
           // LoadClients();
        }

        private void buttonDellClient_Click(object sender, EventArgs e)//удаление клиента
        {
            if (listBoxClients.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем выбранного клиента
            string selectedClient = listBoxClients.SelectedItem.ToString();
            string[] parts = selectedClient.Split('-');
            if (parts.Length < 2)
            {
                MessageBox.Show("Ошибка в данных клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string clientFIO = parts[0].Trim(); // ФИО клиента

            // Подтверждение удаления
            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить клиента {clientFIO} и все связанные данные?",
                                                  "Подтверждение удаления",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                connectionString = $"Data Source={selectedFilePath};Version=3;";
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Получаем ID клиента перед удалением
                    string clientID = "";
                    string queryGetID = "SELECT Client_PK FROM Client WHERE FIO = @FIO";
                    using (SQLiteCommand cmd = new SQLiteCommand(queryGetID, connection))
                    {
                        cmd.Parameters.AddWithValue("@FIO", clientFIO);
                        object idResult = cmd.ExecuteScalar();
                        if (idResult != null)
                        {
                            clientID = idResult.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Клиент не найден в базе данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Удаляем зависимые данные вручную (если нет каскадного удаления)
                    string queryDeleteData = @"
                DELETE FROM Data_of_exp WHERE Experiment_FK IN (SELECT Experiment_PK FROM Experiment WHERE Sample_FK IN (SELECT Sample_PK FROM Sample WHERE Client_FK = @ClientID));
                DELETE FROM Experiment WHERE Sample_FK IN (SELECT Sample_PK FROM Sample WHERE Client_FK = @ClientID);
                DELETE FROM Sample WHERE Client_FK = @ClientID;
                DELETE FROM Client WHERE Client_PK = @ClientID;
            ";

                    using (SQLiteCommand deleteCmd = new SQLiteCommand(queryDeleteData, connection))
                    {
                        deleteCmd.Parameters.AddWithValue("@ClientID", clientID);
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Клиент и его данные успешно удалены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Удаляем клиента из listBox
                listBoxClients.Items.Remove(listBoxClients.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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


        private void buttonUpdate_Click_1(object sender, EventArgs e)//кнопка обновления интерфейса
        {
            LoadClients();
        }

        private void buttonCloseMainForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonChangeDataClient_Click(object sender, EventArgs e)
        {

        }
    }
}
