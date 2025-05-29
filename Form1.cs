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
using System.Windows.Forms.DataVisualization.Charting;


namespace Diplom_project
{
    public partial class Main: Form
    {
        private string selectedFilePath = ""; // Будет хранить путь к БД
        public string selectedPort; // Будет хранить выбранный COM-порт
        private string configFilePath = "C:/Users/isavr/OneDrive/Рабочий стол/Диплом/Diplom_project/tmp/Diplom_project.inc"; // Файл конфигурации
        public string ConnectionString { get; private set; }

        private string sortOrder = "FIO"; // По умолчанию сортировка по ФИО
        private string sampleSortOrder = "Note"; // По умолчанию сортируем по Note
        private string experimentSortOrder = "Number"; // По умолчанию сортируем по номеру эксперимента

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
            listViewExperiments.ColumnClick += listViewExperiments_ColumnClick;



        }

        private void LoadDatabasePath()
        {
            if (File.Exists(configFilePath))
            {
                string[] lines = File.ReadAllLines(configFilePath);

                if (lines.Length > 0 && !string.IsNullOrEmpty(lines[0]) && File.Exists(lines[0]))
                {
                    selectedFilePath = lines[0].Trim();
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
                        MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The config file was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Загружаем последний выбранный COM-порт, если он есть
                if (lines.Length > 1 && !string.IsNullOrEmpty(lines[1]))
                {
                    selectedPort = lines[1].Trim();
                }
            }
            else
            {
                MessageBox.Show("The config file was not found. Choose a database.", "Attation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }

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

        private void label1_Click_1(object sender, EventArgs e) { }

        private void listBoxSamples_SelectedIndexChanged(object sender, EventArgs e){ }

        private void listBoxClients_SelectedIndexChanged(object sender, EventArgs e)
        {
           // LoadClients();
        }
        
        private void buttonDellClient_Click(object sender, EventArgs e)
        {
            int? clientId = GetSelectedClientId();
            if (clientId == null)
            {
                MessageBox.Show("Select the client to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete the client and all related data?",
                "Confirmation of deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                return;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Включаем поддержку внешних ключей
                        using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA foreign_keys = ON;", connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        // 🔹 Удаляем все данные из Data_of_experiment, связанные с экспериментами клиента
                        string deleteDataOfExpQuery = @"
                    DELETE FROM Data_of_exp 
                    WHERE Experiment_FK IN (SELECT Experiment_PK FROM Experiment WHERE Sample_FK IN 
                        (SELECT Sample_PK FROM Sample WHERE Client_FK = @ClientId))";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteDataOfExpQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ClientId", clientId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Удаляем все эксперименты, связанные с образцами клиента
                        string deleteExperimentQuery = @"
                    DELETE FROM Experiment 
                    WHERE Sample_FK IN (SELECT Sample_PK FROM Sample WHERE Client_FK = @ClientId)";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteExperimentQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ClientId", clientId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Удаляем все образцы клиента
                        string deleteSamplesQuery = "DELETE FROM Sample WHERE Client_FK = @ClientId";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteSamplesQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ClientId", clientId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Удаляем самого клиента
                        string deleteClientQuery = "DELETE FROM Client WHERE CLIENT_PK = @ClientId";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteClientQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ClientId", clientId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                       
                        int index = listViewClients.SelectedIndices[0];
                        LoadClients(); // Перезагружаем список клиентов
                        listViewSamples.Items.Clear(); // Очищаем список образцов
                        if (listViewClients.Items.Count > 0)
                        {
                            int newIndex = Math.Max(index - 1, 0);
                            listViewClients.Items[newIndex].Selected = true;
                            listViewClients.Select();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error when deleting a client: {ex.Message}", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadDatabasePath();
            listViewClients.Columns.Add("FIO", 230);
            listViewClients.Columns.Add("Phone number").Width = -2;

            listViewClients.View = View.Details;
            listViewClients.SelectedIndexChanged += listViewClients_SelectedIndexChanged;//обработчик событий

            listViewSamples.Columns.Add("Note", 230);
            listViewSamples.Columns.Add("Date time").Width = -2;

            listViewExperiments.Columns.Clear();
            listViewExperiments.Columns.Add("№", 100);
            listViewExperiments.Columns.Add("Registration date", 150);
            listViewExperiments.Columns.Add("Condition").Width = -2; // Автоматическая ширина


            //Выбор первого элемента в списке
            SalectFirstsElement();
        }

        private void SalectFirstsElement()
        {
            if (listViewSamples.Items.Count > 0)
            {
                listViewSamples.Items[0].Selected = true;
                listViewSamples.Select();
                listViewSamples.Focus();
            }

            if (listViewClients.Items.Count > 0)
            {
                listViewClients.Items[0].Selected = true;
                listViewClients.Select();
                listViewClients.Focus();
            }
            if (listViewExperiments.Items.Count > 0)
            {
                listViewExperiments.Items[0].Selected = true;
                listViewExperiments.Select();
                listViewExperiments.Focus();
            }
        }

        private void selectBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearSamples();
            listViewSamples.Items.Clear(); 
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select the database file";
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
                        // Сохраняем новый путь к БД
                        SaveConfigFile();

                        // Загружаем клиентов
                        LoadClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            SalectFirstsElement();
        }

        private void SaveConfigFile()
        {
            string[] lines = new string[2];

            // Первая строка — путь к базе данных
            lines[0] = selectedFilePath ?? "";

            // Вторая строка — последний выбранный COM-порт
            lines[1] = selectedPort ?? "";

            File.WriteAllLines(configFilePath, lines);
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
                MessageBox.Show($"Error loading clients: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        //должен быть установлен драйвер для ардуино 
        private void selectCOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                MessageBox.Show("There are no COM ports available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form comPortForm = new Form
            {
                Text = "Choosing a COM port",
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
                Text = "Accept",
                Dock = DockStyle.Bottom
            };

            buttonOK.Click += (s, args) =>
            {
                selectedPort = comboBoxPorts.SelectedItem.ToString();
                MessageBox.Show($"Selected COM port: {selectedPort}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comPortForm.Close();

                // Сохраняем COM-порт во вторую строку файла конфигурации
                SaveConfigFile();
            };

            comPortForm.Controls.Add(comboBoxPorts);
            comPortForm.Controls.Add(buttonOK);
            comPortForm.ShowDialog();
        }

        private void buttonChangeDataClient_Click(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count == 0) // Проверяем, есть ли выделенный элемент
            {
                MessageBox.Show("Select the client to change!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void sortedByFIOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? selectedClientId = GetSelectedClientId();
            SortOrder = "FIO"; // Меняем сортировку на ФИО
            if (selectedClientId != null)
            {
                foreach (ListViewItem item in listViewClients.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedClientId)
                    {
                        item.Selected = true;
                        listViewClients.Select();
                        listViewClients.Focus();
                        break;
                    }
                }
            }
        }

        private void sortedByPhoneNumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? selectedClientId = GetSelectedClientId();
            SortOrder = "Phone_num"; // Меняем сортировку на телефон
            if (selectedClientId != null)
            {
                foreach (ListViewItem item in listViewClients.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedClientId)
                    {
                        item.Selected = true;
                        listViewClients.Select();
                        listViewClients.Focus();
                        break;
                    }
                }
            }
        }

        private void toolStripMenuItemSortNote_Click(object sender, EventArgs e)
        {
            int? selectedSampleId = GetSelectedSampleId(); // Запоминаем выделенный образец
            sampleSortOrder = "Note";
            LoadSamples();
            if (selectedSampleId != null)
            {
                foreach (ListViewItem item in listViewSamples.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedSampleId)
                    {
                        item.Selected = true;
                        listViewSamples.Select();
                        break;
                    }
                }
            }
        }

        private void toolStripMenuSortDatetime_Click(object sender, EventArgs e)
        {
            int? selectedSampleId = GetSelectedSampleId(); // Запоминаем выделенный образец
            sampleSortOrder = "DateTime";
            LoadSamples();
            if (selectedSampleId != null)
            {
                foreach (ListViewItem item in listViewSamples.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedSampleId)
                    {
                        item.Selected = true;
                        listViewSamples.Select();
                        break;
                    }
                }
            }
        }

        private void listViewClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewClients.SelectedItems.Count > 0) // Проверяем, есть ли выделенный элемент
            {
                LoadSamples(); // Загружаем образцы при выборе клиента
                if (listViewSamples.Items.Count > 0)
                {
                    listViewSamples.Items[0].Selected = true;
                    listViewSamples.Select();
                    listViewSamples.Focus();
                }
            }
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void sortByToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? selectedExp = GetSelectedExperimentId();     
            experimentSortOrder = "Number";
           
            LoadExperimentsForSelectedSample(); // Перезагружаем список

            // 🔹 Восстанавливаем выделение после сортировки 
            if (selectedExp != null)
            {
                foreach (ListViewItem item in listViewExperiments.Items)
                {
                    if (item.Tag is int expId && expId == selectedExp.Value)
                    {
                        item.Selected = true;
                        listViewExperiments.Select();
                        listViewExperiments.Focus();
                        break;
                    }
                }
            }
        }

        private void sortByDateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? selectedExp = GetSelectedExperimentId();
            
            experimentSortOrder = "DateTime";
           
            LoadExperimentsForSelectedSample(); // Перезагружаем список

            // 🔹 Восстанавливаем выделение после сортировки 
            if (selectedExp != null)
            {
                foreach (ListViewItem item in listViewExperiments.Items)
                {
                    if (item.Tag is int expId && expId == selectedExp.Value)
                    {
                        item.Selected = true;
                        listViewExperiments.Select();
                        listViewExperiments.Focus();
                        break;
                    }
                }
            }
        }

        private void sortByErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int? selectedExp = GetSelectedExperimentId();

            experimentSortOrder = "Error";

            LoadExperimentsForSelectedSample(); // Перезагружаем список

            // 🔹 Восстанавливаем выделение после сортировки 
            if (selectedExp != null)
            {
                foreach (ListViewItem item in listViewExperiments.Items)
                {
                    if (item.Tag is int expId && expId == selectedExp.Value)
                    {
                        item.Selected = true;
                        listViewExperiments.Select();
                        listViewExperiments.Focus();
                        break;
                    }
                }
            }
        }

        // Метод для загрузки образцов в listViewSample
        private void LoadSamples(int? selectedSampleId = null)
        {
            ClearSamples();
            listViewSamples.Items.Clear(); // Очищаем список
            listViewExperiments.Items.Clear();

            int? clientId = GetSelectedClientId(); // Получаем ID клиента
            if (clientId == null) return;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string query = $@"
SELECT s.Sample_PK, s.Note, d.Date, d.Time 
FROM Sample s
JOIN Datetime d ON s.Datetime_FK = d.Datetime_PK
WHERE s.Client_FK = @ClientId
ORDER BY {(sampleSortOrder == "Note" ? "s.Note ASC" : "strftime('%Y-%m-%d %H:%M:%S', d.Date || ' ' || d.Time) ASC")}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int sampleId = Convert.ToInt32(reader["Sample_PK"]);
                            string note = reader["Note"].ToString();
                            string date = reader["Date"].ToString();
                            string time = reader["Time"].ToString();

                            ListViewItem item = new ListViewItem(note);
                            item.SubItems.Add($"{date} {time}");
                            item.Tag = sampleId;

                            listViewSamples.Items.Add(item);
                        }
                    }
                }
            }

            // 🔹 Восстанавливаем выделение, если передан `selectedSampleId`
            if (selectedSampleId != null)
            {
                foreach (ListViewItem item in listViewSamples.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedSampleId)
                    {
                        item.Selected = true;
                        listViewSamples.Select();
                        return; // Выделяем только один элемент, предотвращаем двойное выделение
                    }
                }
            }

            
        }

        public void ClearSamples()
        {
            textBoxNote.Text = null;
            textBoxData.Text = null;
            textBoxEngineType.Text = null;
            textBoxEhgineBrand.Text = null;
            textBoxEngineNomber.Text = null;
            textBoxEngineMileage.Text = null;
            textBoxOilMileage.Text = null;
            textBoxEngineDictionary.Text = null;
            textBoxOilDictionary.Text = null;
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
            int? selectedSampleId = GetSelectedSampleId(); // Запоминаем выделенный образец
            string columnName = listViewSamples.Columns[e.Column].Text;

            if (columnName == "Note")
            {
                sampleSortOrder = "Note"; // Сортируем по названию образца
            }
            else if (columnName == "Date time")
            {
                sampleSortOrder = "DateTime"; // Сортируем по дате и времени
            }
            else
            {
                return;
            }
            
            LoadSamples(); // Перезагружаем список с новым порядком сортировки
                           // 🔹 Восстанавливаем выделение после сортировки
            if (selectedSampleId != null)
            {
                foreach (ListViewItem item in listViewSamples.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedSampleId)
                    {
                        item.Selected = true;
                        listViewSamples.Select();
                        break;
                    }
                }
            }
        }

        private void listViewClients_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int? selectedClientId = GetSelectedClientId();
            // Получаем заголовок колонки
            string columnName = listViewClients.Columns[e.Column].Text;

            //сортруем в зависимости от выбранной колонки            
            if (columnName == "FIO")
            {
                SortOrder = "FIO"; // Меняем сортировку на ФИО
                
            }
            if (columnName == "Phone number")
            {
                SortOrder = "Phone_num"; // Меняем сортировку на телефон
                
            }
            LoadClients();

            // 🔹 После загрузки клиентов восстанавливаем выделение
            if (selectedClientId != null)
            {
                foreach (ListViewItem item in listViewClients.Items)
                {
                    if (Convert.ToInt32(item.Tag) == selectedClientId)
                    {
                        item.Selected = true;
                        listViewClients.Select();
                        listViewClients.Focus();
                        break;
                    }
                }
            }
        }

        private void listViewSamples_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            LoadExperimentsForSelectedSample();
            

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
            // 🔹 Автоматически выбираем первый эксперимент после загрузки
            if (listViewExperiments.Items.Count > 0)
            {
                listViewExperiments.Items[0].Selected = true;
                listViewExperiments.Select();
                listViewExperiments.Focus();
            }
        }

        private void buttonAddSamples_Click(object sender, EventArgs e)
        {
            int? clientId = GetSelectedClientId();
            if (clientId == null)
            {
                MessageBox.Show("Select the client before adding the sample!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            AddSample addSampleForm = new AddSample(selectedFilePath, this);
            addSampleForm.ShowDialog();
            int? newSampleId = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT MAX(Sample_PK) FROM Sample WHERE Client_FK = @ClientId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        newSampleId = Convert.ToInt32(result);
                    }
                }
            }
            // После закрытия формы обновляем список образцов
            LoadSamples(newSampleId);
        }

        private void buttonDellSamples_Click(object sender, EventArgs e)
        {

            int? selectedSampleId = GetSelectedSampleId();
            int? sampleId = GetSelectedSampleId();
            if (sampleId == null)
            {
                MessageBox.Show("Select a sample to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this sample and all related data??",
                "Confirmation of deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
                return;

            int selectedIndex = -1;
            for (int i = 0; i < listViewSamples.Items.Count; i++)
            {
                if (Convert.ToInt32(listViewSamples.Items[i].Tag) == selectedSampleId)
                {
                    selectedIndex = i;
                    break;
                }
            }

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
                            MessageBox.Show("Error: Sample not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                LoadSamples(); // Обновляем список

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error when deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // 🔹 Выбираем элемент выше удаленного (если он есть)
            if (listViewSamples.Items.Count > 0)
            {
                int newIndex = Math.Max(0, selectedIndex - 1); // Если удалили первый элемент, выбираем новый первый
                listViewSamples.Items[newIndex].Selected = true;
                listViewSamples.Select();
            }
        }

        private void buttonChangeSamples_Click(object sender, EventArgs e)
        {

            int? selectedSampleId = GetSelectedSampleId();
            if (listViewSamples.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a sample to change!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем ID выбранного образца
            int? sampleId = GetSelectedSampleId();
            if (sampleId == null)
            {
                MessageBox.Show("Error: Couldn't get sample ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Открываем форму изменения
            ChangeSample form5 = new ChangeSample(sampleId.Value, ConnectionString);
            form5.ShowDialog();

            // После закрытия формы перезагружаем список
            LoadSamples();
            foreach (ListViewItem item in listViewSamples.Items)
            {
                if (Convert.ToInt32(item.Tag) == selectedSampleId)
                {
                    item.Selected = true;
                    listViewSamples.Select();
                    return; // Выделяем только один элемент, предотвращаем двойное выделение
                }
            }
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        //загрузка экспериментов для выбранного образца
        private void LoadExperimentsForSelectedSample()
        {
            int? selectedSampleId = GetSelectedSampleId();
            if (selectedSampleId == null) return;

            listViewExperiments.Items.Clear();

            

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                
                // Запрос с сортировкой в зависимости от выбранного столбца
                string query = $@"
SELECT e.Experiment_PK, e.Number, d.Date, d.Time, e.Error
FROM Experiment e
JOIN Datetime d ON e.Datetime_FK = d.Datetime_PK
WHERE e.Sample_FK = @SampleId
ORDER BY 
    {(experimentSortOrder == "Number" ? "e.Number ASC" :
       experimentSortOrder == "DateTime" ? "strftime('%Y-%m-%d %H:%M:%S', d.Date || ' ' || d.Time) ASC" :
        experimentSortOrder == "Error" ? "e.Error ASC" : "e.Error ASC")}"; // По умолчанию сортируем по ошибке

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SampleId", selectedSampleId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int experimentId = reader.GetInt32(0);
                            int number = reader.GetInt32(1);
                            string date = reader.GetString(2);
                            string time = reader.GetString(3);
                            int numberError = reader.GetInt32(4);
                            string fullDate = $"{date} {time}";

                            
                            Color stateColor;

                            if (numberError == 0)
                            {
                                
                                stateColor = Color.Orange;
                            }
                            else if (numberError == 1)
                            {
                                
                                stateColor = Color.Green;
                            }
                            else
                            {
                                
                                stateColor = Color.Red;
                            }

                            ListViewItem item = new ListViewItem(number.ToString());
                            item.SubItems.Add(fullDate);
                            item.SubItems.Add(numberError.ToString());
                            item.BackColor = stateColor;
                            item.Tag = experimentId;  //  Сохраняем ID эксперимента в `Tag`
                            listViewExperiments.Items.Add(item);
                        }
                    }
                }
            }
            


        }

        //форма добавления эксперимента(маленькая форма)
        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label label = new Label() { Left = 10, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 260 };
            Button confirmation = new Button() { Text = "OK", Left = 180, Width = 90, Top = 80, DialogResult = DialogResult.OK };

            prompt.Controls.Add(label);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        //кнопка добавления эксперимента
        private void buttonAddExp_Click(object sender, EventArgs e)
        {
            int? selectedSampleId = GetSelectedSampleId();
            if (selectedSampleId == null)
            {
                MessageBox.Show("Select a sample before adding an experiment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Открываем диалог для ввода номера эксперимента
            string input = ShowInputDialog("Enter the experiment number:", "Adding an experiment");

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("The experiment number cannot be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(input, out int experimentNumber))
            {
                MessageBox.Show("Enter the correct numeric value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 🔹 Проверяем, существует ли уже такой номер
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Experiment WHERE Number = @Number";
                using (SQLiteCommand cmd = new SQLiteCommand(checkQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Number", experimentNumber);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show($"The experiment with the {experimentNumber} already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // ❌ Не добавляем дубликат
                    }
                }
            }

            int newExperimentId = 0;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 🔹 1. Вставляем дату и время в таблицу Datetime
                        string insertDatetimeQuery = "INSERT INTO Datetime (Date, Time) VALUES (DATE('now', 'localtime'), TIME('now', 'localtime'));";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertDatetimeQuery, connection, transaction))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 2. Получаем ID добавленной даты/времени
                        string getDatetimeIdQuery = "SELECT last_insert_rowid();";
                        int datetimeId;
                        using (SQLiteCommand cmd = new SQLiteCommand(getDatetimeIdQuery, connection, transaction))
                        {
                            datetimeId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 🔹 3. Добавляем новый эксперимент
                        string insertExperimentQuery = @"
                    INSERT INTO Experiment (Number, Datetime_FK, Sample_FK, Error)
                    VALUES (@Number, @DatetimeId, @SampleId, 0);";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertExperimentQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Number", experimentNumber);
                            cmd.Parameters.AddWithValue("@DatetimeId", datetimeId);
                            cmd.Parameters.AddWithValue("@SampleId", selectedSampleId);
                            cmd.ExecuteNonQuery();
                        }
                        // 🔹 Получаем ID добавленного эксперимента
                        using (SQLiteCommand cmd = new SQLiteCommand(getDatetimeIdQuery, connection, transaction))
                        {
                            newExperimentId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        transaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error when adding an experiment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // 🔹 Обновляем список экспериментов
            LoadExperimentsForSelectedSample();

            // 🔹 Автоматически выделяем добавленный эксперимент
            foreach (ListViewItem item in listViewExperiments.Items)
            {
                if (item.Tag != null && Convert.ToInt32(item.Tag) == newExperimentId)
                {
                    item.Selected = true;
                    listViewExperiments.Select();
                    listViewExperiments.Focus();
                    return; // Останавливаем поиск после выделения
                }
            }
        }

        private int? GetSelectedExperimentId()
        {
            if (listViewExperiments.SelectedItems.Count == 0)
                return null;

            int selectedNumber = Convert.ToInt32(listViewExperiments.SelectedItems[0].Text);

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Experiment_PK FROM Experiment WHERE Number = @Number";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Number", selectedNumber);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }

        private void buttonDelExp_Click(object sender, EventArgs e)
        {
            int? selectedExperimentId = GetSelectedExperimentId();
            if (selectedExperimentId == null)
            {
                MessageBox.Show("Select an experiment to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this experiment and all related data??",
                "Confirmation of deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No) return;

            // 🔹 Запоминаем индекс выделенного элемента перед удалением
            int selectedIndex = listViewExperiments.SelectedIndices[0];

            


            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 🔹 Удаляем **все** записи в Data_of_exp, которые ссылаются на этот эксперимент
                        string deleteDataOfExpQuery = "DELETE FROM Data_of_exp WHERE Experiment_FK = @ExperimentId";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteDataOfExpQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ExperimentId", selectedExperimentId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Теперь удаляем сам эксперимент
                        string deleteExperimentQuery = "DELETE FROM Experiment WHERE Experiment_PK = @ExperimentId";
                        using (SQLiteCommand cmd = new SQLiteCommand(deleteExperimentQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ExperimentId", selectedExperimentId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error deleting an experiment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // 🔹 Обновляем список экспериментов
            LoadExperimentsForSelectedSample();

            // 🔹 Восстанавливаем выделение на строку выше
            if (listViewExperiments.Items.Count > 0)
            {
                if (selectedIndex > 0)
                {
                    selectedIndex--; // Перемещаем выделение вверх
                }

                listViewExperiments.Items[selectedIndex].Selected = true;
                listViewExperiments.Select();
                listViewExperiments.Focus();
            }
        }

        private void listViewExperiments_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listViewExperiments_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int? selectedExp = GetSelectedExperimentId();
            string columnName = listViewExperiments.Columns[e.Column].Text;

            if (columnName == "№")
            {
                experimentSortOrder = "Number";
            }
            else if (columnName == "Registration date")
            {
                experimentSortOrder = "DateTime";
            }
            else if (columnName == "Condition")
            {
                experimentSortOrder = "Error";
            }
            else
            {
                return;
            }

            LoadExperimentsForSelectedSample(); // Перезагружаем список

            // 🔹 Восстанавливаем выделение после сортировки 
            if (selectedExp != null)
            {
                foreach (ListViewItem item in listViewExperiments.Items)
                {
                    if (item.Tag is int expId && expId == selectedExp.Value)
                    {
                        item.Selected = true;
                        listViewExperiments.Select();
                        listViewExperiments.Focus();
                        break;
                    }
                }
            }
        }

        private void buttonStartExperiment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedPort))
            {
                MessageBox.Show("Select the COM port before starting the experiment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 🔹 Проверяем, доступен ли COM-порт
            string[] availablePorts = SerialPort.GetPortNames();
            if (!availablePorts.Contains(selectedPort))
            {
                MessageBox.Show($"The COM port {selectedPort} was not found! Connect the device or select another port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // ❌ Не открываем `Form6`, если порта нет
            }
            int? experimentId = GetSelectedExperimentId(); // Получаем ID выделенного эксперимента
            if (experimentId == null)
            {
                MessageBox.Show("Select an experiment to test!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            // 🔹 Проверяем, проводился ли уже этот эксперимент
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Error FROM Experiment WHERE Experiment_PK = @ExperimentId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExperimentId", experimentId);
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int errorCode) && errorCode != 0)
                    {
                        string errorDescription = DecodeError(errorCode);
                        MessageBox.Show($"The experiment has already been conducted: {errorCode} - {errorDescription}",
                                        "Attation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // ❌ Не запускаем эксперимент, если уже есть данные
                    }
                }
            }

            // 🔹 Если ошибки нет, запускаем эксперимент
            try
            {
                Form6 experimentForm = new Form6(selectedPort, selectedFilePath, experimentId.Value);

                // Подписываемся на закрытие формы, чтобы обновить список экспериментов
                experimentForm.FormClosed += (s, args) => LoadExperimentsForSelectedSample();
                experimentForm.FormClosed += (s, args) => HighlightExperiment(experimentId.Value);
                experimentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when opening the experiment window: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private string DecodeError(int errorCode)
        {
            switch (errorCode)
            {
                case 1: return "The experiment was carried out successfully.";
                case 777: return "The experiment Was Stopped by the user.";
                case 11: return "The temperature sensor is not responding.";
                case 12: return "The temperature sensor gives incorrect readings.";
                case 13: return "Overheating of the oil.";
                case 21: return "The weight sensor is not responding.";
                case 22: return "The weight sensor gives incorrect readings.";
                case 23: return "Breakage of the tightening mechanism.";
                case 31: return "The speed sensor is not responding.";
                case 32: return "The speed sensor gives incorrect readings.";
                case 33: return "The gap in the speed sensor.";
                case 41: return "The motor current sensor is not responding.";
                case 42: return "The motor current sensor gives incorrect readings.";
                case 43: return "Overcurrent of the motor.";
                case 51: return "The current sensor of the clamping mechanism is not responding.";
                case 52: return "The current sensor of the clamping mechanism outputs incorrect values.";
                case 53: return "Overcurrent of the clamping mechanism.";
                default: return "Unknown error.";
            }
        }

        //выделение после завкрытия формы6
        public void HighlightExperiment(int experimentId)
        {
            foreach (ListViewItem item in listViewExperiments.Items)
            {
                if (item.Tag != null && Convert.ToInt32(item.Tag) == experimentId)
                {
                    item.Selected = true;
                    listViewExperiments.Select();
                    listViewExperiments.Focus();
                    break;
                }
            }
        }

        private void buttonDataExp_Click(object sender, EventArgs e)
        {
            int? experimentId = GetSelectedExperimentId();
            if (experimentId == null)
            {
                MessageBox.Show("Choose an experiment!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Открываем `Form7`, передавая `experimentId`
            Form7 graphForm = new Form7(experimentId.Value, ConnectionString);
            graphForm.Show();
        }

        private void creatNewDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Data Base SQLite (*.db)|*.db",
                Title = "Create new DB"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string dbPath = saveFileDialog.FileName;

                try
                {
                    SQLiteConnection.CreateFile(dbPath);
                    string connectionString = $"Data Source={dbPath};Version=3;";

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        // Создаем таблицы
                        string createScript = GetCreateDatabaseScript();
                        using (SQLiteCommand command = new SQLiteCommand(createScript, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        // Заполняем справочники
                        string insertGuide = @"
INSERT INTO Guide (Unit_of_measure) VALUES 
('Km'),
('Ml'),
('MCh');";

                        string insertEngineTypes = @"
INSERT INTO Type_engine (Type_eng) VALUES 
('gasoline'),
('diesel');";

                        using (SQLiteCommand command = new SQLiteCommand(insertGuide, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        using (SQLiteCommand command = new SQLiteCommand(insertEngineTypes, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    // Сохраняем путь в конфиг
                    // Сохраняем или создаём файл конфигурации
                    if (!File.Exists(configFilePath))
                    {
                        // Создаем новый файл с одной строкой — путь к БД
                        File.WriteAllLines(configFilePath, new[] { dbPath });
                    }
                    else
                    {
                        // Файл есть — читаем все строки
                        var lines = File.ReadAllLines(configFilePath).ToList();

                        if (lines.Count == 0)
                        {
                            lines.Add(dbPath);
                        }
                        else
                        {
                            lines[0] = dbPath; // Заменяем первую строку
                        }

                        File.WriteAllLines(configFilePath, lines);
                    }



                    // Обновляем активный путь и подключение в приложении
                    selectedFilePath = dbPath;
                    ConnectionString = $"Data Source={selectedFilePath};Version=3;";
                    LoadClients(); // загружаем сразу клиентов

                    MessageBox.Show("New database successfully created!", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating db: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private string GetCreateDatabaseScript()
        {
            return @"
CREATE TABLE Client (
    Client_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    FIO TEXT,
    Phone_num TEXT UNIQUE
);

CREATE TABLE Datetime (
    Datetime_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Time TEXT,
    Date TEXT
);

CREATE TABLE Type_engine (
    Type_eng_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Type_eng TEXT UNIQUE
);

CREATE TABLE Engine (
    Engine_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Marka TEXT,
    Type_eng_FK INTEGER,
    FOREIGN KEY (Type_eng_FK) REFERENCES Type_engine(Type_eng_PK)
);

CREATE TABLE Number_engine (
    Number_eng_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Number_eng TEXT UNIQUE,
    Engine_FK INTEGER,
    FOREIGN KEY (Engine_FK) REFERENCES Engine(Engine_PK)
);

CREATE TABLE Guide (
    Guide_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Unit_of_measure TEXT UNIQUE
);

CREATE TABLE Engine_mileage (
    Engine_mileage_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Engine_mil REAL,
    Guide_FK INTEGER,
    FOREIGN KEY (Guide_FK) REFERENCES Guide(Guide_PK)
);

CREATE TABLE Oil_mileage (
    Oil_mileage_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Oil_mil REAL,
    Guide_FK INTEGER,
    FOREIGN KEY (Guide_FK) REFERENCES Guide(Guide_PK)
);

CREATE TABLE Sample (
    Sample_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Note TEXT,
    Datetime_FK INTEGER,
    Number_eng_FK INTEGER,
    Engine_mileage_FK INTEGER,
    Oil_mileage_FK INTEGER,
    Client_FK INTEGER,
    FOREIGN KEY (Datetime_FK) REFERENCES Datetime(Datetime_PK),
    FOREIGN KEY (Number_eng_FK) REFERENCES Number_engine(Number_eng_PK),
    FOREIGN KEY (Engine_mileage_FK) REFERENCES Engine_mileage(Engine_mileage_PK),
    FOREIGN KEY (Oil_mileage_FK) REFERENCES Oil_mileage(Oil_mileage_PK),
    FOREIGN KEY (Client_FK) REFERENCES Client(Client_PK)
);

CREATE TABLE Experiment (
    Experiment_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    Number INTEGER,
    Error INTEGER,
    Sample_FK INTEGER,
    Datetime_FK INTEGER,
    FOREIGN KEY (Sample_FK) REFERENCES Sample(Sample_PK),
    FOREIGN KEY (Datetime_FK) REFERENCES Datetime(Datetime_PK)
);

CREATE TABLE Data_of_exp (
    Data_of_exp_PK INTEGER PRIMARY KEY AUTOINCREMENT,
    run_of_test INTEGER,
    num_of_error INTEGER,
    Time TEXT,
    Temp REAL,
    Power REAL,
    Speed REAL,
    Experiment_FK INTEGER,
    FOREIGN KEY (Experiment_FK) REFERENCES Experiment(Experiment_PK)
);
";
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            int? currentExperimentId = GetSelectedExperimentId();
            if (currentExperimentId == null)
            {
                MessageBox.Show("Please select an experiment to compare!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем форму выбора эксперимента для сравнения
            Form compareForm = new Form
            {
                Text = "Compare Experiments",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            // Компоненты формы
            Label labelExperiment = new Label
            {
                Text = "Select experiment to compare with:",
                Location = new Point(10, 10),
                AutoSize = true
            };

            ComboBox comboBoxExperiments = new ComboBox
            {
                Location = new Point(10, 35),
                Width = 350,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label labelXAxis = new Label
            {
                Text = "X Axis:",
                Location = new Point(10, 70),
                AutoSize = true
            };

            ComboBox comboBoxXAxis = new ComboBox
            {
                Location = new Point(10, 95),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Time", "Temp", "Power", "Speed" }
            };

            Label labelYAxis = new Label
            {
                Text = "Y Axis:",
                Location = new Point(190, 70),
                AutoSize = true
            };

            ComboBox comboBoxYAxis = new ComboBox
            {
                Location = new Point(190, 95),
                Width = 170,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Time", "Temp", "Power", "Speed" }
            };

            Button buttonCompare = new Button
            {
                Text = "Compare",
                Location = new Point(270, 140),
                DialogResult = DialogResult.OK
            };

            Button buttonCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(180, 140),
                DialogResult = DialogResult.Cancel
            };

            // Заполняем ComboBox экспериментами (кроме текущего)
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Experiment_PK, Number FROM Experiment WHERE Experiment_PK != @CurrentId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CurrentId", currentExperimentId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int expId = reader.GetInt32(0);
                            int expNumber = reader.GetInt32(1);
                            comboBoxExperiments.Items.Add(new { Id = expId, Number = expNumber });
                        }
                    }
                }
            }

            // Настраиваем отображение в ComboBox
            comboBoxExperiments.DisplayMember = "Number";
            comboBoxExperiments.ValueMember = "Id";

            // Устанавливаем значения по умолчанию для осей
            comboBoxXAxis.SelectedIndex = 0; // Time
            comboBoxYAxis.SelectedIndex = 1; // Temp

            // Добавляем компоненты на форму
            compareForm.Controls.Add(labelExperiment);
            compareForm.Controls.Add(comboBoxExperiments);
            compareForm.Controls.Add(labelXAxis);
            compareForm.Controls.Add(comboBoxXAxis);
            compareForm.Controls.Add(labelYAxis);
            compareForm.Controls.Add(comboBoxYAxis);
            compareForm.Controls.Add(buttonCompare);
            compareForm.Controls.Add(buttonCancel);

            // Показываем форму выбора
            if (compareForm.ShowDialog() != DialogResult.OK || comboBoxExperiments.SelectedItem == null)
            {
                compareForm.Dispose();
                return;
            }

            // Получаем выбранный эксперимент и оси
            int compareExperimentId = (comboBoxExperiments.SelectedItem as dynamic).Id;
            string xAxis = comboBoxXAxis.SelectedItem.ToString();
            string yAxis = comboBoxYAxis.SelectedItem.ToString();
            int currentExpNumber = 0;
            int compareExpNumber = (comboBoxExperiments.SelectedItem as dynamic).Number;

            // Получаем номер текущего эксперимента
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Number FROM Experiment WHERE Experiment_PK = @ExperimentId";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExperimentId", currentExperimentId);
                    currentExpNumber = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            // Загружаем данные для обоих экспериментов
            Dictionary<int, List<(double x, double y)>> experimentsData = new Dictionary<int, List<(double x, double y)>>();
            experimentsData[currentExperimentId.Value] = new List<(double x, double y)>();
            experimentsData[compareExperimentId] = new List<(double x, double y)>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT run_of_test, " + xAxis + ", " + yAxis + " FROM Data_of_exp WHERE Experiment_FK = @ExperimentId";

                foreach (var expId in new[] { currentExperimentId.Value, compareExperimentId })
                {
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExperimentId", expId);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string rawX = reader.IsDBNull(1) ? "" : reader.GetString(1).Replace(',', '.');
                                string rawY = reader.IsDBNull(2) ? "" : reader.GetString(2).Replace(',', '.');

                                string[] valuesX = rawX.Split(';');
                                string[] valuesY = rawY.Split(';');
                                int minLength = Math.Min(valuesX.Length, valuesY.Length);

                                for (int i = 0; i < minLength; i++)
                                {
                                    if (double.TryParse(valuesX[i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double x) &&
                                        double.TryParse(valuesY[i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double y))
                                    {
                                        experimentsData[expId].Add((x, y));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Создаем форму для графика
            Form graphForm = new Form
            {
                Text = $"Comparison: Exp {currentExpNumber} vs Exp {compareExpNumber}",
                Size = new Size(1200, 650), // Ширина увеличена в 1.5 раза: 800 * 1.5 = 1200
                StartPosition = FormStartPosition.CenterParent
            };

            Chart chartComparison = new Chart
            {
                Location = new Point(0, 50),
                Size = new Size(1200, 550), // Увеличиваем ширину графика
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Панель для CheckBox
            Panel checkBoxPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 50), // Увеличиваем ширину панели
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            CheckBox checkBoxApprox = new CheckBox
            {
                Text = "Show Linear Approximation",
                Location = new Point(10, 15),
                Checked = false
            };

            CheckBox checkBoxAvg = new CheckBox
            {
                Text = "Show Average Line",
                Location = new Point(200, 15),
                Checked = false
            };

            checkBoxPanel.Controls.Add(checkBoxApprox);
            checkBoxPanel.Controls.Add(checkBoxAvg);

            ChartArea chartArea = new ChartArea
            {
                Name = "ChartArea1"
            };
            chartArea.AxisX.Title = xAxis;
            chartArea.AxisY.Title = yAxis;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.BackColor = Color.White;
            chartComparison.ChartAreas.Add(chartArea);

            // Добавляем легенду
            Legend legend = new Legend
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Center,
                BackColor = Color.White,
                BorderColor = Color.Black,
                IsDockedInsideChartArea = false
            };
            chartComparison.Legends.Add(legend);

            // Определяем границы осей
            double minX = double.MaxValue, maxX = double.MinValue;
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (var expData in experimentsData.Values)
            {
                foreach (var point in expData)
                {
                    if (point.x < minX) minX = point.x;
                    if (point.x > maxX) maxX = point.x;
                    if (point.y < minY) minY = point.y;
                    if (point.y > maxY) maxY = point.y;
                }
            }

            // Специфические ограничения для Power-Speed
            if (xAxis == "Power" && yAxis == "Speed")
            {
                chartComparison.ChartAreas[0].AxisX.Minimum = 1000;
                chartComparison.ChartAreas[0].AxisX.Maximum = 15500;
                chartComparison.ChartAreas[0].AxisY.Minimum = 500;
            }
            else
            {
                chartComparison.ChartAreas[0].AxisX.Minimum = minX;
                chartComparison.ChartAreas[0].AxisX.Maximum = maxX;
                chartComparison.ChartAreas[0].AxisY.Minimum = minY;
                chartComparison.ChartAreas[0].AxisY.Maximum = maxY;
            }

            // Функция для обновления графика
            void UpdateGraph()
            {
                chartComparison.Series.Clear();

                // Добавляем серии точек
                Series currentSeries = new Series($"Exp {currentExpNumber} (Red)")
                {
                    ChartType = SeriesChartType.Point,
                    Color = Color.Red,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6
                };

                Series compareSeries = new Series($"Exp {compareExpNumber} (Blue)")
                {
                    ChartType = SeriesChartType.Point,
                    Color = Color.Blue,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6
                };

                // Добавляем точки
                foreach (var point in experimentsData[currentExperimentId.Value])
                {
                    currentSeries.Points.AddXY(point.x, point.y);
                }

                foreach (var point in experimentsData[compareExperimentId])
                {
                    compareSeries.Points.AddXY(point.x, point.y);
                }

                chartComparison.Series.Add(currentSeries);
                chartComparison.Series.Add(compareSeries);

                // Линейная аппроксимация
                if (checkBoxApprox.Checked)
                {
                    foreach (var expId in new[] { currentExperimentId.Value, compareExperimentId })
                    {
                        var validPoints = experimentsData[expId]
                            .Where(p => p.x > 0 && p.y > 0)
                            .OrderBy(p => p.x)
                            .ToList();

                        if (validPoints.Count < 2) continue;

                        double sumX = validPoints.Sum(p => p.x);
                        double sumY = validPoints.Sum(p => p.y);
                        double sumXY = validPoints.Sum(p => p.x * p.y);
                        double sumX2 = validPoints.Sum(p => p.x * p.x);
                        int n = validPoints.Count;

                        double a = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
                        double b = (sumY - a * sumX) / n;

                        Series approxSeries = new Series($"Exp {(expId == currentExperimentId.Value ? currentExpNumber : compareExpNumber)} Approx")
                        {
                            ChartType = SeriesChartType.Line,
                            Color = expId == currentExperimentId.Value ? Color.DarkRed : Color.DarkBlue,
                            BorderWidth = 2,
                            MarkerStyle = MarkerStyle.None
                        };

                        double startX = (xAxis == "Power" && yAxis == "Speed") ? 1000 : minX;
                        double endX = (xAxis == "Power" && yAxis == "Speed") ? 15500 : maxX;
                        int steps = 100;
                        double stepSize = (endX - startX) / steps;

                        for (int i = 0; i <= steps; i++)
                        {
                            double x = startX + i * stepSize;
                            double y = a * x + b;
                            approxSeries.Points.AddXY(x, y);
                        }

                        chartComparison.Series.Add(approxSeries);
                    }
                }

                // График среднего
                if (checkBoxAvg.Checked)
                {
                    foreach (var expId in new[] { currentExperimentId.Value, compareExperimentId })
                    {
                        var filtered = experimentsData[expId].Where(p => p.x > 0).ToList();
                        if (filtered.Count == 0) continue;

                        Series avgSeries = new Series($"Exp {(expId == currentExperimentId.Value ? currentExpNumber : compareExpNumber)} Avg")
                        {
                            ChartType = SeriesChartType.Spline,
                            Color = expId == currentExperimentId.Value ? Color.LightCoral : Color.LightBlue,
                            BorderWidth = 2,
                            MarkerStyle = MarkerStyle.None
                        };

                        int intervals = 50;
                        double minXExp = filtered.Min(p => p.x);
                        double maxXExp = filtered.Max(p => p.x);
                        double rangeX = maxXExp - minXExp;
                        double intervalSize = rangeX / intervals;

                        List<(double x, double y)> avgPoints = new List<(double x, double y)>();
                        for (int i = 0; i < intervals; i++)
                        {
                            double xStart = minXExp + i * intervalSize;
                            double xEnd = xStart + intervalSize;
                            var pointsInInterval = filtered.Where(p => p.x >= xStart && p.x < xEnd).ToList();
                            if (pointsInInterval.Count > 0)
                            {
                                double avgY = pointsInInterval.Average(p => p.y);
                                double avgX = (xStart + xEnd) / 2.0;
                                avgPoints.Add((avgX, avgY));
                            }
                        }

                        avgPoints = avgPoints.OrderBy(p => p.x).ToList();
                        foreach (var point in avgPoints)
                        {
                            avgSeries.Points.AddXY(point.x, point.y);
                        }

                        chartComparison.Series.Add(avgSeries);
                    }
                }
            }

            // Добавляем обработчики для CheckBox
            checkBoxApprox.CheckedChanged += (s, args) => UpdateGraph();
            checkBoxAvg.CheckedChanged += (s, args) => UpdateGraph();

            // Первоначальное построение графика
            UpdateGraph();

            graphForm.Controls.Add(checkBoxPanel);
            graphForm.Controls.Add(chartComparison);
            graphForm.ShowDialog();

            compareForm.Dispose();
            graphForm.Dispose();
        }


        ////////
    }

}

