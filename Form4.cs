using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom_project
{
    
    public partial class AddSample: Form
    {
        private string ConnectionString;
        private Main mainForm;
        
        public AddSample(string dbPath, Main form)
        {
            InitializeComponent();
            ConnectionString = $"Data Source={dbPath};Version=3;";
            mainForm = form;
           // LoadComboBoxes(); // Загружаем данные при открытии формы

            
        }

        
        private void EngineType_Click(object sender, EventArgs e)
        {

        }

        private void AddSample_Load(object sender, EventArgs e)
        {
            if (mainForm.GetSelectedClientId() == null)
            {
                MessageBox.Show("Select a client!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                
            }
            LoadEngineTypes();
            LoadGuides();
        }
        /*
        private void LoadComboBoxes()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Заполняем comboBoxAddGuide1 и comboBoxAddGuide2 из таблицы Guide
                string queryGuide = "SELECT Guide_PK, Unit_of_measure FROM Guide";
                using (SQLiteCommand cmd = new SQLiteCommand(queryGuide, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string displayValue = $"{reader["Unit_of_measure"]}";
                        comboBoxAddGuide1.Items.Add(displayValue);
                        comboBoxAddGuide2.Items.Add(displayValue);
                    }
                }
                
                // Заполняем comboBoxAddEngineType из таблицы Type_engine
                string queryTypeEngine = "SELECT Type_eng_PK, Type_eng FROM Type_engine";
                using (SQLiteCommand cmd = new SQLiteCommand(queryTypeEngine, connection))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string displayValue = $"{reader["Type_eng"]}";
                        comboBoxAddEngineType.Items.Add(displayValue);
                    }
                }
                
            }
        }
*/

        private void LoadGuides()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Guide_PK, unit_of_measure FROM Guide;";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Привязываем данные к обоим ComboBox
                    comboBoxAddGuide1.DataSource = dt.Copy(); // Используем Copy(), чтобы избежать конфликта привязки
                    comboBoxAddGuide1.DisplayMember = "unit_of_measure";
                    comboBoxAddGuide1.ValueMember = "Guide_PK";

                    comboBoxAddGuide2.DataSource = dt;
                    comboBoxAddGuide2.DisplayMember = "unit_of_measure";
                    comboBoxAddGuide2.ValueMember = "Guide_PK";
                }
            }
        }

        private void LoadEngineTypes()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT Type_eng_PK, Type_eng FROM Type_engine;";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Привязываем данные к comboBox
                    comboBoxAddEngineType.DataSource = dt;
                    comboBoxAddEngineType.DisplayMember = "Type_eng";  // Показываем название типа
                    comboBoxAddEngineType.ValueMember = "Type_eng_PK"; // Храним первичный ключ
                }
            }
        }



        private void ApplyAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNoteAdd.Text) ||
        string.IsNullOrWhiteSpace(textBoxEngineNomberAdd.Text) ||
        string.IsNullOrWhiteSpace(textBoxEngineMileageAdd.Text) ||
        string.IsNullOrWhiteSpace(textBoxOilMileageAdd.Text) ||
        comboBoxAddEngineType.SelectedIndex == -1 ||
        comboBoxAddEngineType.SelectedValue == null ||
        comboBoxAddGuide1.SelectedIndex == -1 ||
        comboBoxAddGuide1.SelectedValue == null ||
        comboBoxAddGuide2.SelectedIndex == -1 ||
        comboBoxAddGuide2.SelectedValue == null)
            {
                MessageBox.Show("Fill in all fields before adding!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Добавляем дату и время
                        string insertDatetime = "INSERT INTO Datetime (Date, Time) VALUES (@Date, @Time);";
                        int datetimeId;
                        using (SQLiteCommand cmd = new SQLiteCommand(insertDatetime, connection))
                        {
                            cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                            cmd.ExecuteNonQuery();
                            datetimeId = (int)connection.LastInsertRowId;
                        }

                        // 2. Проверяем, что выбраны значения в comboBoxAddEngineType
                        if (comboBoxAddEngineType.SelectedIndex == -1 || comboBoxAddEngineType.SelectedValue == null)
                        {
                            MessageBox.Show("Select the engine type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int typeEngineId = Convert.ToInt32(comboBoxAddEngineType.SelectedValue);

                        // 3. Проверяем, что марка двигателя введена
                        string engineMarka = textBoxEhgineBrandAdd.Text.Trim();
                        if (string.IsNullOrEmpty(engineMarka))
                        {
                            throw new Exception("Enter the engine brand!");
                        }

                        // 4. Проверяем существование Engine
                        int engineId;
                        string checkEngine = "SELECT Engine_PK FROM Engine WHERE Marka = @Marka AND Type_eng_FK = @TypeId;";
                        using (SQLiteCommand cmd = new SQLiteCommand(checkEngine, connection))
                        {
                            cmd.Parameters.AddWithValue("@Marka", engineMarka);
                            cmd.Parameters.AddWithValue("@TypeId", typeEngineId);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                engineId = Convert.ToInt32(result);
                            }
                            else
                            {
                                string insertEngine = "INSERT INTO Engine (Marka, Type_eng_FK) VALUES (@Marka, @TypeId);";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertEngine, connection))
                                {
                                    insertCmd.Parameters.AddWithValue("@Marka", engineMarka);
                                    insertCmd.Parameters.AddWithValue("@TypeId", typeEngineId);
                                    insertCmd.ExecuteNonQuery();
                                    engineId = (int)connection.LastInsertRowId;
                                }
                            }
                        }

                        // 5. Проверяем существование Number_engine
                        string engineNumber = textBoxEngineNomberAdd.Text.Trim();
                        if (string.IsNullOrEmpty(engineNumber))
                        {
                            throw new Exception("Enter the engine number!");
                        }

                        int engineNumberId;
                        string checkEngineNumber = "SELECT Number_eng_PK FROM Number_engine WHERE Number_eng = @Number;";
                        using (SQLiteCommand cmd = new SQLiteCommand(checkEngineNumber, connection))
                        {
                            cmd.Parameters.AddWithValue("@Number", engineNumber);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                engineNumberId = Convert.ToInt32(result);
                            }
                            else
                            {
                                string insertEngineNumber = "INSERT INTO Number_engine (Number_eng, Engine_FK) VALUES (@Number, @EngineId);";
                                using (SQLiteCommand insertCmd = new SQLiteCommand(insertEngineNumber, connection))
                                {
                                    insertCmd.Parameters.AddWithValue("@Number", engineNumber);
                                    insertCmd.Parameters.AddWithValue("@EngineId", engineId);
                                    insertCmd.ExecuteNonQuery();
                                    engineNumberId = (int)connection.LastInsertRowId;
                                }
                            }
                        }

                        // 6. Проверяем, что Guide выбраны корректно
                        if (comboBoxAddGuide1.SelectedIndex == -1 || comboBoxAddGuide1.SelectedValue == null)
                        {
                            MessageBox.Show("Select the unit of measurement for the engine mileage!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (comboBoxAddGuide2.SelectedIndex == -1 || comboBoxAddGuide2.SelectedValue == null)
                        {
                            MessageBox.Show("Select the unit of measurement for the oil mileage!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        int guideId1 = Convert.ToInt32(comboBoxAddGuide1.SelectedValue);
                        int guideId2 = Convert.ToInt32(comboBoxAddGuide2.SelectedValue);

                        // 7. Добавляем пробег двигателя
                        double engineMileage;
                        if (!double.TryParse(textBoxEngineMileageAdd.Text, out engineMileage))
                        {
                            throw new Exception("Enter the correct engine mileage!");
                        }

                        int engineMileageId;
                        string insertEngineMileage = "INSERT INTO Engine_mileage (Engine_mil, Guide_FK) VALUES (@Mileage, @Guide);";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertEngineMileage, connection))
                        {
                            cmd.Parameters.AddWithValue("@Mileage", engineMileage);
                            cmd.Parameters.AddWithValue("@Guide", guideId1);
                            cmd.ExecuteNonQuery();
                            engineMileageId = (int)connection.LastInsertRowId;
                        }

                        // 8. Добавляем пробег масла
                        double oilMileage;
                        if (!double.TryParse(textBoxOilMileageAdd.Text, out oilMileage))
                        {
                            throw new Exception("Enter the correct oil mileage!");
                        }

                        int oilMileageId;
                        string insertOilMileage = "INSERT INTO Oil_mileage (Oil_mil, Guide_FK) VALUES (@Mileage, @Guide);";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertOilMileage, connection))
                        {
                            cmd.Parameters.AddWithValue("@Mileage", oilMileage);
                            cmd.Parameters.AddWithValue("@Guide", guideId2);
                            cmd.ExecuteNonQuery();
                            oilMileageId = (int)connection.LastInsertRowId;
                        }

                        // 9. Проверяем, что выбран клиент
                        int? clientId = mainForm.GetSelectedClientId();
                        if (clientId == null)
                        {
                            MessageBox.Show("Select the client to delete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 10. Вставляем Sample
                        string insertSample = @"
                    INSERT INTO Sample (Note, Datetime_FK, Number_eng_FK, Engine_mileage_FK, Oil_mileage_FK, Client_FK)
                    VALUES (@Note, @Datetime, @EngineNumber, @EngineMileage, @OilMileage, @ClientId);";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertSample, connection))
                        {
                            cmd.Parameters.AddWithValue("@Note", textBoxNoteAdd.Text);
                            cmd.Parameters.AddWithValue("@Datetime", datetimeId);
                            cmd.Parameters.AddWithValue("@EngineNumber", engineNumberId);
                            cmd.Parameters.AddWithValue("@EngineMileage", engineMileageId);
                            cmd.Parameters.AddWithValue("@OilMileage", oilMileageId);
                            cmd.Parameters.AddWithValue("@ClientId", clientId);
                            cmd.ExecuteNonQuery();
                        }
                        
                        transaction.Commit();
                        //MessageBox.Show("Образец успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ESC_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
