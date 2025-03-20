using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diplom_project
{
    public partial class ChangeSample: Form
    {
        private int sampleId;
        private string connectionString;
        public ChangeSample(int sampleId, string connectionString)
        {
            InitializeComponent();
            this.sampleId = sampleId;
            this.connectionString = connectionString;
            LoadSampleData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoadSampleData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
SELECT 
    s.Note, 
    te.Type_eng AS TypeName, 
    e.Marka AS EngineBrand, 
    ne.Number_eng AS EngineNumber, 
    em.Engine_mil AS EngineMileage,  
    om.Oil_mil AS OilMileage,        
    g1.Unit_of_measure AS EngineGuide, 
    g2.Unit_of_measure AS OilGuide
FROM Sample s
JOIN Engine_mileage em ON s.Engine_mileage_FK = em.Engine_mileage_PK
JOIN Oil_mileage om ON s.Oil_mileage_FK = om.Oil_mileage_PK
JOIN Number_engine ne ON s.Number_eng_FK = ne.Number_eng_PK
JOIN Engine e ON ne.Engine_FK = e.Engine_PK
JOIN Type_engine te ON e.Type_eng_FK = te.Type_eng_PK  
JOIN Guide g1 ON em.Guide_FK = g1.Guide_PK
JOIN Guide g2 ON om.Guide_FK = g2.Guide_PK
WHERE s.Sample_PK = @SampleId;";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SampleId", sampleId);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxNoteChange.Text = reader["Note"].ToString();
                            textBoxEhgineBrandChange.Text = reader["EngineBrand"].ToString();
                            textBoxEngineNomberChange.Text = reader["EngineNumber"].ToString();
                            textBoxEngineMileageChange.Text = reader["EngineMileage"].ToString();
                            textBoxOilMileageChange.Text = reader["OilMileage"].ToString();

                            // Загружаем возможные варианты в ComboBox
                            LoadComboBox(comboBoxEngineChange, "SELECT Type_eng FROM Type_engine", "Type_eng");
                            LoadComboBox(comboBoxChangeGuide1, "SELECT Unit_of_measure FROM Guide", "Unit_of_measure");
                            LoadComboBox(comboBoxChangeGuide2, "SELECT Unit_of_measure FROM Guide", "Unit_of_measure");

                            // Устанавливаем выбранные значения
                            comboBoxEngineChange.SelectedItem = reader["TypeName"].ToString();
                            comboBoxChangeGuide1.SelectedItem = reader["EngineGuide"].ToString();
                            comboBoxChangeGuide2.SelectedItem = reader["OilGuide"].ToString();
                        }
                    }
                }
            }
        }

        // Функция для загрузки данных в ComboBox
        private void LoadComboBox(ComboBox comboBox, string query, string columnName)
        {
            comboBox.Items.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox.Items.Add(reader[columnName].ToString());
                        }
                    }
                }
            }
        }


        //номер двигателя не найден, не получчается менять
        private void ApplyChange_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxNoteChange.Text) ||
                string.IsNullOrWhiteSpace(textBoxEhgineBrandChange.Text) ||
                string.IsNullOrWhiteSpace(textBoxEngineNomberChange.Text) ||
                string.IsNullOrWhiteSpace(textBoxEngineMileageChange.Text) ||
                string.IsNullOrWhiteSpace(textBoxOilMileageChange.Text) ||
                comboBoxEngineChange.SelectedItem == null ||
                comboBoxChangeGuide1.SelectedItem == null ||
                comboBoxChangeGuide2.SelectedItem == null)
            {
                MessageBox.Show("Fill in all fields!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 🔹 Получаем Number_eng_FK из Sample
                        string getNumberEngFKQuery = "SELECT Number_eng_FK FROM Sample WHERE Sample_PK = @SampleId";
                        int numberEngFK;
                        using (SQLiteCommand cmd = new SQLiteCommand(getNumberEngFKQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@SampleId", sampleId);
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                MessageBox.Show("Error: engine number could not be found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            numberEngFK = Convert.ToInt32(result);
                        }

                        // 🔹 Получаем Engine_FK из Number_engine
                        string getEngineFKQuery = "SELECT Engine_FK FROM Number_engine WHERE Number_eng_PK = @NumberEngFK";
                        int engineFK;
                        using (SQLiteCommand cmd = new SQLiteCommand(getEngineFKQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NumberEngFK", numberEngFK);
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                MessageBox.Show("Error: couldn't find the engine!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            engineFK = Convert.ToInt32(result);
                        }

                        // 🔹 Обновляем номер двигателя в Number_engine
                        string updateNumberEngineQuery = @"
                UPDATE Number_engine 
                SET Number_eng = @NewEngineNumber
                WHERE Number_eng_PK = @NumberEngFK";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateNumberEngineQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@NewEngineNumber", textBoxEngineNomberChange.Text);
                            cmd.Parameters.AddWithValue("@NumberEngFK", numberEngFK);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Обновляем данные в Engine
                        string updateEngineQuery = @"
                UPDATE Engine 
                SET Marka = @EngineBrand, 
                    Type_eng_FK = (SELECT Type_eng_PK FROM Type_engine WHERE Type_eng = @TypeEngine)
                WHERE Engine_PK = @EnginePK";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateEngineQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EngineBrand", textBoxEhgineBrandChange.Text);
                            cmd.Parameters.AddWithValue("@TypeEngine", comboBoxEngineChange.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@EnginePK", engineFK);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Обновляем пробег двигателя
                        string updateEngineMileageQuery = @"
                UPDATE Engine_mileage 
                SET Engine_mil = @EngineMileage, 
                    Guide_FK = (SELECT Guide_PK FROM Guide WHERE Unit_of_measure = @Guide1)
                WHERE Engine_mileage_PK = (SELECT Engine_mileage_FK FROM Sample WHERE Sample_PK = @SampleId)";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateEngineMileageQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EngineMileage", textBoxEngineMileageChange.Text);
                            cmd.Parameters.AddWithValue("@Guide1", comboBoxChangeGuide1.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@SampleId", sampleId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Обновляем пробег масла
                        string updateOilMileageQuery = @"
                UPDATE Oil_mileage 
                SET Oil_mil = @OilMileage, 
                    Guide_FK = (SELECT Guide_PK FROM Guide WHERE Unit_of_measure = @Guide2)
                WHERE Oil_mileage_PK = (SELECT Oil_mileage_FK FROM Sample WHERE Sample_PK = @SampleId)";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateOilMileageQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OilMileage", textBoxOilMileageChange.Text);
                            cmd.Parameters.AddWithValue("@Guide2", comboBoxChangeGuide2.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@SampleId", sampleId);
                            cmd.ExecuteNonQuery();
                        }

                        // 🔹 Обновляем образец, оставляя связь с клиентом и Number_eng_FK
                        string updateSampleQuery = @"
                UPDATE Sample 
                SET Note = @Note 
                WHERE Sample_PK = @SampleId";
                        using (SQLiteCommand cmd = new SQLiteCommand(updateSampleQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Note", textBoxNoteChange.Text);
                            cmd.Parameters.AddWithValue("@SampleId", sampleId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                       
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error when changing the sample: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


            //MessageBox.Show("Данные успешно изменены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        

        



        private void ESC_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
