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
        private string sampleSortOrder = "Note"; // По умолчанию сортируем по Note


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
            listViewClients.ColumnClick += listViewClients_ColumnClick;
            listViewSamples.ColumnClick += listViewSamples_ColumnClick;
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
        
        private void button1_Click(object sender, EventArgs e)
        {
            // Открываем форму добавления клиента
            AddClient addForm = new AddClient(selectedFilePath, this);
            if (addForm.ShowDialog() == DialogResult.OK) // Ждем, пока пользователь добавит клиента
            {
                string newFIO = addForm.fio;
                string newPhone = addForm.phone;

                // Загружаем обновленный список клиентов
                LoadClients();

                // Ищем нового клиента в ListView
                foreach (ListViewItem item in listViewClients.Items)
                {
                    if (item.Text == newFIO && item.SubItems[1].Text == newPhone)
                    {
                        item.Selected = true; // Выделяем найденного клиента
                        listViewClients.Select();
                        break;
                    }
                }
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
        
        private void buttonDellClient_Click(object sender, EventArgs e)
        {
            int? clientId = GetSelectedClientId();
            if (clientId == null)
            {
                MessageBox.Show("Выберите клиента для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Client WHERE CLIENT_PK = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", clientId);
                    command.ExecuteNonQuery();
                }
            }

            // Запоминаем индекс
            int index = listViewClients.SelectedIndices[0];

            LoadClients(); // Обновляем список клиентов

            // Смещаем выделение вверх
            if (listViewClients.Items.Count > 0)
            {
                int newIndex = Math.Max(index - 1, 0);
                listViewClients.Items[newIndex].Selected = true;
                listViewClients.Select();
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            LoadDatabasePath();
            listViewClients.Columns.Add("ФИО", 200);
            listViewClients.Columns.Add("Телефон", 150);

            listViewClients.View = View.Details;
            listViewClients.SelectedIndexChanged += listViewClients_SelectedIndexChanged;//обработчик событий

            listViewSamples.Columns.Add("Note", 200);
            listViewSamples.Columns.Add("Дата и время", 150);

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
        
        public void LoadClients()
        {
            listViewClients.Items.Clear();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    string query = $"SELECT Client_PK, FIO, Phone_num FROM Client ORDER BY {SortOrder} ASC";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int clientId = Convert.ToInt32(reader["Client_PK"]);
                            string fio = reader["FIO"].ToString();
                            string phone = reader["Phone_num"].ToString();

                            ListViewItem item = new ListViewItem(fio);
                            item.Tag = clientId; // Сохраняем ID клиента в Tag
                            item.SubItems.Add(phone);
                            listViewClients.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        
        private void buttonChangeDataClient_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0) // Проверяем, есть ли выделенный элемент
            {
                MessageBox.Show("Выберите клиента для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем выделенный элемент
            ListViewItem selectedItem = listViewClients.SelectedItems[0];

            string fio = selectedItem.Text; // ФИО (первый столбец)
            string phone = selectedItem.SubItems[1].Text; // Телефон (второй столбец)

            // Открываем ChangeClient (Form3), передавая данные ФИО и номер телефона
            ChangeClient form3 = new ChangeClient(this, fio, phone, ConnectionString);

            if (form3.ShowDialog() == DialogResult.OK) // Ожидаем закрытия формы
            {
                string newFIO = form3.UpdatedFIO;
                string newPhone = form3.UpdatedPhone;

                // Обновляем список клиентов
                LoadClients();

                // Находим измененного клиента в ListView
                foreach (ListViewItem item in listViewClients.Items)
                {
                    if (item.Text == newFIO && item.SubItems[1].Text == newPhone)
                    {
                        item.Selected = true; // Выделяем измененного клиента
                        listViewClients.Select();
                        break;
                    }
                }
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
        private void toolStripMenuItemSortNote_Click(object sender, EventArgs e)
        {
            sampleSortOrder = "Note";
            LoadSamples();
        }
        private void toolStripMenuSortDatetime_Click(object sender, EventArgs e)
        {
            sampleSortOrder = "DateTime";
            LoadSamples();
        }


        private void listViewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count > 0) // Проверяем, есть ли выделенный элемент
            {
                LoadSamples(); // Загружаем образцы при выборе клиента
            }
        }


        // Метод для загрузки образцов в listViewSamples
        private void LoadSamples()
        {
            listViewSamples.Items.Clear(); // Очищаем список перед загрузкой новых данных

            int? clientId = GetSelectedClientId(); // Получаем ID выбранного клиента
            if (clientId == null)
                return; // Если клиент не выбран, выходим

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = $@"
SELECT s.Sample_PK, s.Note, d.Date, d.Time 
FROM Sample s
JOIN Datetime d ON s.Datetime_FK = d.Datetime_PK
WHERE s.Client_FK = @ClientId
ORDER BY 
    {(sampleSortOrder == "Note" ? "s.Note DESC" : "strftime('%Y-%m-%d %H:%M:%S', d.Date || ' ' || d.Time) DESC")}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sampleId = Convert.ToInt32(reader["Sample_PK"]); // Получаем первичный ключ
                            string note = reader["Note"].ToString();
                            string date = reader["Date"].ToString();
                            string time = reader["Time"].ToString();

                            ListViewItem item = new ListViewItem(note); // Первый столбец (Note)
                            item.SubItems.Add($"{date} {time}"); // Второй столбец (Дата + Время)
                            item.Tag = sampleId; // Сохраняем первичный ключ в Tag

                            listViewSamples.Items.Add(item);
                        }
                    }
                }
            }

            // Автоматически выделяем первый образец
            if (listViewSamples.Items.Count > 0)
            {
                listViewSamples.Items[0].Selected = true;
                listViewSamples.Select(); // Фокус на ListView
            }
        }







        public int? GetSelectedClientId()//получение id выбранного клиента
        {
            if (listViewClients.SelectedItems.Count == 0)
                return null;

            string selectedFIO = listViewClients.SelectedItems[0].Text;
            string selectedPhone = listViewClients.SelectedItems[0].SubItems[1].Text;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT CLIENT_PK FROM Client WHERE FIO = @FIO AND Phone_num = @Phone";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FIO", selectedFIO);
                    command.Parameters.AddWithValue("@Phone", selectedPhone);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }

        private int? GetSelectedSampleId()
        {
            if (listViewSamples.SelectedItems.Count == 0)
                return null;

            string selectedNote = listViewSamples.SelectedItems[0].Text;
            string selectedDateTime = listViewSamples.SelectedItems[0].SubItems[1].Text;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
        SELECT s.Sample_PK FROM Sample s
        JOIN Datetime d ON s.Datetime_FK = d.Datetime_PK
        WHERE s.Note = @Note AND d.Date || ' ' || d.Time = @DateTime";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Note", selectedNote);
                    command.Parameters.AddWithValue("@DateTime", selectedDateTime);

                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }
        //сортировка образцов
        private void listViewSamples_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            string columnName = listViewSamples.Columns[e.Column].Text;

            if (columnName == "Note")
            {
                sampleSortOrder = "Note"; // Сортируем по названию образца
            }
            else if (columnName == "Дата и время")
            {
                sampleSortOrder = "DateTime"; // Сортируем по дате и времени
            }
            else
            {
                return;
            }

            LoadSamples(); // Перезагружаем список с новым порядком сортировки
        }



        private void listViewClients_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Получаем заголовок колонки
            string columnName = listViewClients.Columns[e.Column].Text;

            //сортруем в зависимости от выбранной колонки            
            if (columnName == "ФИО")
            {
                SortOrder = "FIO"; // Меняем сортировку на ФИО
                LoadClients();
            }
            if (columnName == "Телефон")
            {
                SortOrder = "Phone_num"; // Меняем сортировку на телефон
                LoadClients();
            }
        }

        private void buttonDataOfExp_Click(object sender, EventArgs e)
        {

        }

        private void listViewSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSamples.SelectedItems.Count == 0)
                return;

            string selectedNote = listViewSamples.SelectedItems[0].Text;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = @"
            SELECT 
                s.Note, d.Date, d.Time, 
                e.Marka AS EngineBrand, t.Type_eng AS EngineType, ne.Number_eng AS EngineNumber,
                em.Engine_mil AS EngineMileage, om.Oil_mil AS OilMileage,
                ge.Unit_of_measure AS EngineDictionary, go.Unit_of_measure AS OilDictionary
            FROM Sample s
            JOIN Datetime d ON s.Datetime_FK = d.Datetime_PK
            LEFT JOIN Number_engine ne ON s.Number_eng_FK = ne.Number_eng_PK
            LEFT JOIN Engine e ON ne.Engine_FK = e.Engine_PK
            LEFT JOIN Type_engine t ON e.Type_eng_FK = t.Type_eng_PK
            LEFT JOIN Engine_mileage em ON s.Engine_mileage_FK = em.Engine_mileage_PK
            LEFT JOIN Oil_mileage om ON s.Oil_mileage_FK = om.Oil_mileage_PK
            LEFT JOIN Guide ge ON em.Guide_FK = ge.Guide_PK
            LEFT JOIN Guide go ON om.Guide_FK = go.Guide_PK
            WHERE s.Note = @Note;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Note", selectedNote);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxNote.Text = reader["Note"].ToString();
                            textBoxData.Text = $"{reader["Date"]} {reader["Time"]}";
                            textBoxEngineType.Text = reader["EngineType"].ToString();
                            textBoxEhgineBrand.Text = reader["EngineBrand"].ToString();
                            textBoxEngineNomber.Text = reader["EngineNumber"].ToString();
                            textBoxEngineMileage.Text = reader["EngineMileage"].ToString();
                            textBoxOilMileage.Text = reader["OilMileage"].ToString();
                            textBoxEngineDictionary.Text = reader["EngineDictionary"].ToString();
                            textBoxOilDictionary.Text = reader["OilDictionary"].ToString();
                        }
                    }
                }
            }
        }

        private void buttonAddSamples_Click(object sender, EventArgs e)
        {
            
            AddSample addSampleForm = new AddSample(selectedFilePath, this);
            addSampleForm.ShowDialog();

            // После закрытия формы обновляем список образцов
            LoadSamples();
        }

        private void buttonDellSamples_Click(object sender, EventArgs e)
        {
            

            int? sampleId = GetSelectedSampleId();
            if (sampleId == null)
            {
                MessageBox.Show("Выберите образец для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           

            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить этот образец и все связанные с ним данные?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                return;

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();

                    // Включаем поддержку внешних ключей
                    using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // 1. Удаляем записи из Data_of_exp
                    string deleteDataOfExpQuery = "DELETE FROM Data_of_exp WHERE Experiment_FK IN (SELECT Experiment_PK FROM Experiment WHERE Sample_FK = @SampleId)";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteDataOfExpQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SampleId", sampleId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Удаляем записи из Experiment
                    string deleteExperimentQuery = "DELETE FROM Experiment WHERE Sample_FK = @SampleId";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteExperimentQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SampleId", sampleId);
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Удаляем сам Sample
                    string deleteSampleQuery = "DELETE FROM Sample WHERE Sample_PK = @SampleId";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteSampleQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@SampleId", sampleId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                           // MessageBox.Show("Образец успешно удален!", "Удаление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка: образец не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                LoadSamples(); // Обновляем список
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonChangeSamples_Click(object sender, EventArgs e)
        {
            if (listViewSamples.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите образец для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем ID выбранного образца
            int? sampleId = GetSelectedSampleId();
            if (sampleId == null)
            {
                MessageBox.Show("Ошибка: Не удалось получить ID образца.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Открываем форму изменения
            ChangeSample form5 = new ChangeSample(sampleId.Value, ConnectionString);
            form5.ShowDialog();

            // После закрытия формы перезагружаем список
            LoadSamples();
        }

        
    }
}
