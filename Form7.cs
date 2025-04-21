using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;




namespace Diplom_project
{
    public partial class Form7: Form
    {
        private string connectionString;
        private int experimentId;
        private List<string> columnNames = new List<string> { "Time", "Temp", "Power", "Speed" };
        public Form7(int experimentId, string connectionString)
        {
            
            InitializeComponent();
            
            this.experimentId = experimentId;
            this.connectionString = connectionString;

            // Заполняем comboBoxX и comboBoxY
            comboBoxX.Items.AddRange(columnNames.ToArray());
            comboBoxY.Items.AddRange(columnNames.ToArray());

            comboBoxX.SelectedIndex = 0; // По умолчанию первая колонка
            comboBoxY.SelectedIndex = 1; // Вторая колонка
        }
        

        private void label1_Click(object sender, EventArgs e)
        {
        }


        /*
        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (comboBoxX.SelectedItem == null || comboBoxY.SelectedItem == null)
            {
                MessageBox.Show("Выберите обе оси!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string columnX = comboBoxX.SelectedItem.ToString();
            string columnY = comboBoxY.SelectedItem.ToString();

            // Словарь для хранения данных по испытаниям
            Dictionary<int, List<(string, string)>> experimentsData = new Dictionary<int, List<(string, string)>>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $@"SELECT run_of_test, {columnX}, {columnY} FROM Data_of_exp WHERE Experiment_FK = @ExperimentId ORDER BY run_of_test";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExperimentId", experimentId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int runOfTest = reader.GetInt32(0);

                            // Явное приведение типа для защиты от DBNull
                            string rawX = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string rawY = reader.IsDBNull(2) ? "" : reader.GetString(2);

                            string[] valuesX = rawX.Split(';');
                            string[] valuesY = rawY.Split(';');

                            int minLength = Math.Min(valuesX.Length, valuesY.Length); // Учитываем разную длину

                            if (!experimentsData.ContainsKey(runOfTest))
                            {
                                experimentsData[runOfTest] = new List<(string, string)>();
                            }

                            for (int i = 0; i < minLength; i++)
                            {
                                experimentsData[runOfTest].Add((valuesX[i], valuesY[i]));
                            }
                        }
                    }
                }
            }

            if (experimentsData.Count == 0)
            {
                MessageBox.Show("Нет данных для построения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем файл и записываем данные
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ExperimentData.txt");

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (var experiment in experimentsData)
                {
                    writer.WriteLine($"/ Испытание {experiment.Key} /");
                    writer.WriteLine($"{columnX} ///// {columnY}");

                    foreach (var (x, y) in experiment.Value)
                    {
                        writer.WriteLine($"{x} ///// {y}");
                    }

                    writer.WriteLine(); // Пустая строка между испытаниями
                }
            }

            MessageBox.Show($"Файл успешно сохранен на рабочем столе!\n{filePath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        */
        private void PlotGraph(Dictionary<int, List<(double, double)>> experimentsData, string axisX, string axisY)
        {
            chartExp.Series.Clear();
            chartExp.ChartAreas.Clear();

            // Добавляем область графика
            ChartArea chartArea = new ChartArea("MainArea");
            chartExp.ChartAreas.Add(chartArea);

            // Массив цветов (чтобы испытания различались)
            Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple,
                       Color.Cyan, Color.Magenta, Color.Brown, Color.DarkBlue, Color.DarkGreen };

            int colorIndex = 0;

            foreach (var experiment in experimentsData)
            {
                string seriesName = $"Test {experiment.Key}";
                Series series = new Series(seriesName)
                {
                    ChartType = SeriesChartType.Point, // Точки, без соединения линиями
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 7,
                    Color = colors[colorIndex % colors.Length] // Назначаем цвет
                };

                foreach (var (x, y) in experiment.Value)
                {
                    series.Points.AddXY(x, y);
                }

                chartExp.Series.Add(series);

                colorIndex++; // Меняем цвет для следующего испытания
            }

            // Настраиваем оси
            chartExp.ChartAreas["MainArea"].AxisX.Title = axisX;
            chartExp.ChartAreas["MainArea"].AxisY.Title = axisY;
            chartExp.ChartAreas["MainArea"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartExp.ChartAreas["MainArea"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
        }



        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (comboBoxX.SelectedItem == null || comboBoxY.SelectedItem == null)
            {
                MessageBox.Show("Select both axes!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string columnX = comboBoxX.SelectedItem.ToString();
            string columnY = comboBoxY.SelectedItem.ToString();

            Dictionary<int, List<(double, double)>> experimentsData = new Dictionary<int, List<(double, double)>>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = $@"SELECT run_of_test, {columnX}, {columnY} FROM Data_of_exp WHERE Experiment_FK = @ExperimentId ORDER BY run_of_test";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExperimentId", experimentId);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int runOfTest = reader.GetInt32(0);

                            string rawX = reader.IsDBNull(1) ? "" : reader.GetString(1).Replace(',', '.');
                            string rawY = reader.IsDBNull(2) ? "" : reader.GetString(2).Replace(',', '.');

                            string[] valuesX = rawX.Split(';');
                            string[] valuesY = rawY.Split(';');

                            int minLength = Math.Min(valuesX.Length, valuesY.Length);

                            if (!experimentsData.ContainsKey(runOfTest))
                            {
                                experimentsData[runOfTest] = new List<(double, double)>();
                            }

                            for (int i = 0; i < minLength; i++)
                            {
                                if (double.TryParse(valuesX[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double x) &&
                                    double.TryParse(valuesY[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double y))
                                {
                                    experimentsData[runOfTest].Add((x, y));
                                }
                            }
                        }
                    }
                }
            }

            if (experimentsData.Count == 0)
            {
                MessageBox.Show("There is no data to build!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Отображаем график с разными цветами для каждого испытания
            PlotGraph(experimentsData, columnX, columnY);
        }






        private void Form7_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            if (comboBoxX.SelectedItem == null || comboBoxY.SelectedItem == null)
            {
                MessageBox.Show("Выберите обе оси для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string columnX = comboBoxX.SelectedItem.ToString();
            string columnY = comboBoxY.SelectedItem.ToString();

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                Title = "Save data experiment"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            using (StreamWriter writer = new StreamWriter(saveDialog.FileName, false, Encoding.UTF8))
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = $@"
                SELECT run_of_test, {columnX}, {columnY} 
                FROM Data_of_exp 
                WHERE Experiment_FK = @ExperimentId 
                ORDER BY run_of_test";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExperimentId", experimentId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int runOfTest = reader.GetInt32(0);
                                string rawX = reader.IsDBNull(1) ? "" : reader.GetString(1).Replace(',', '.');
                                string rawY = reader.IsDBNull(2) ? "" : reader.GetString(2).Replace(',', '.');

                                string[] valuesX = rawX.Split(';');
                                string[] valuesY = rawY.Split(';');
                                int count = Math.Min(valuesX.Length, valuesY.Length);

                                writer.WriteLine($"Run {runOfTest}");
                                writer.WriteLine($"{columnX};{columnY}");

                                for (int i = 0; i < count; i++)
                                {
                                    writer.WriteLine($"{valuesX[i]};{valuesY[i]}");
                                }

                                writer.WriteLine(); // пустая строка между испытаниями
                            }
                        }
                    }
                }
            }

            MessageBox.Show("File saved successfully!", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
