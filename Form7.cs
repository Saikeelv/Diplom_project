using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
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
            this.checkBoxApprox.CheckedChanged += new System.EventHandler(this.checkBoxApprox_CheckedChanged);
           // this.checkBoxApprox2.CheckedChanged += new EventHandler(this.checkBoxApprox_CheckedChanged);
            this.checkBoxAvg.CheckedChanged += new EventHandler(this.checkBoxAvg_CheckedChanged);
            
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


        
        private void PlotGraph(Dictionary<int, List<(double, double)>> experimentsData, string xLabel, string yLabel)
        {
            chartExp.Series.Clear();

            chartExp.ChartAreas[0].AxisX.Title = xLabel;
            chartExp.ChartAreas[0].AxisY.Title = yLabel;

            chartExp.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartExp.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartExp.ChartAreas[0].BackColor = Color.White;

            List<(double x, double y)> allPoints = new List<(double, double)>();
            string[] fixedColors = { "Blue", "Green", "Red", "Orange", "Purple", "Brown", "Turquoise", "DarkCyan", "DarkSlateGray", "DarkMagenta" };
            int colorIndex = 0;

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (var experiment in experimentsData)
            {
                var points = experiment.Value
                    .Where(p => p.Item1 > 1 && p.Item2 > 1) // Удаление околонулевых и отрицательных
                    .ToList();

                if (xLabel == "Power" && yLabel == "Speed")
                {
                    points = points.Where(p => p.Item1 >= 1000).ToList(); // Отсечь Power < 1000
                }

                if (points.Count == 0)
                    continue;

                Series series = new Series($"Run {experiment.Key}")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    BorderWidth = 2,
                    Color = Color.FromName(fixedColors[colorIndex % fixedColors.Length])
                };
                colorIndex++;

                foreach (var point in points)
                {
                    series.Points.AddXY(point.Item1, point.Item2);
                    allPoints.Add(point);

                    if (point.Item1 < minX) minX = point.Item1;
                    if (point.Item1 > maxX) maxX = point.Item1;
                    if (point.Item2 < minY) minY = point.Item2;
                    if (point.Item2 > maxY) maxY = point.Item2;
                }

                chartExp.Series.Add(series);
            }

            // Установим границы осей только по отфильтрованным точкам
            chartExp.ChartAreas[0].AxisX.Minimum = minX;
            chartExp.ChartAreas[0].AxisX.Maximum = maxX;
            chartExp.ChartAreas[0].AxisY.Minimum = minY;
            chartExp.ChartAreas[0].AxisY.Maximum = maxY;

            
            // ✔ Усреднение и аппроксимация
            if (allPoints.Count > 2)
            {
                var filtered = allPoints
                    .Where(p => p.x > 0) // Убедимся, что x > 0
                    .ToList();

                if (filtered.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Нет точек после фильтра x > 0.");
                    MessageBox.Show("Нет точек после фильтра x > 0.", "Ошибка");
                    return;
                }

                // Усреднение (checkBoxAvg)
                if (checkBoxAvg.Checked)
                {
                    Series avgSeries = new Series("Average Line")
                    {
                        ChartType = SeriesChartType.Spline, // Используем сплайн для плавности
                        Color = Color.Red,
                        BorderWidth = 2,
                        MarkerStyle = MarkerStyle.None // Убираем маркеры
                    };

                    // Увеличиваем количество интервалов для более точного усреднения
                    int intervals = 50; // Было 10, теперь 50
                    double rangeX = maxX - minX;
                    double interval = rangeX / intervals;

                    // Собираем все точки средних значений
                    List<(double x, double y)> avgPoints = new List<(double, double)>();

                    for (int i = 0; i < intervals; i++)
                    {
                        double xStart = minX + i * interval;
                        double xEnd = xStart + interval;

                        // Находим точки в текущем интервале
                        var pointsInInterval = filtered
                            .Where(p => p.x >= xStart && p.x < xEnd)
                            .ToList();

                        if (pointsInInterval.Count > 0)
                        {
                            double avgY = pointsInInterval.Average(p => p.y);
                            double avgX = (xStart + xEnd) / 2.0; // Средняя точка интервала
                            avgPoints.Add((avgX, avgY));
                        }
                    }

                    // Сортируем точки по X для правильного построения
                    avgPoints = avgPoints.OrderBy(p => p.x).ToList();

                    // Добавляем точки в серию (ChartType.Spline автоматически создаст плавную кривую)
                    foreach (var point in avgPoints)
                    {
                        avgSeries.Points.AddXY(point.x, point.y);
                    }

                    chartExp.Series.Add(avgSeries);
                }


                //аппроксимация линейная
                // Аппроксимация (checkBoxApprox)
                if (checkBoxApprox.Checked)
                {
                    // Фильтруем точки
                    var validPoints = filtered
                        .Where(p => p.x > 0 && p.y > 0)
                        .OrderBy(p => p.x)
                        .ToList();

                    if (validPoints.Count < 2)
                    {
                        MessageBox.Show("Недостаточно точек для аппроксимации", "Ошибка");
                        return;
                    }

                    // Линейная регрессия для y = a * x + b методом наименьших квадратов
                    double sumX = validPoints.Sum(p => p.x);
                    double sumY = validPoints.Sum(p => p.y);
                    double sumXY = validPoints.Sum(p => p.x * p.y);
                    double sumX2 = validPoints.Sum(p => p.x * p.x);
                    int n = validPoints.Count;

                    // Коэффициенты
                    double a = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
                    double b = (sumY - a * sumX) / n;

                    // Формируем строку для легенды
                    string legendText = $"y = {a:F2} * x + {b:F2}";

                    // Создаем серию
                    Series approxSeries = new Series("Linear Approx")
                    {
                        ChartType = SeriesChartType.Line,
                        Color = Color.Black,
                        BorderWidth = 3,
                        MarkerStyle = MarkerStyle.None,
                        LegendText = legendText // Устанавливаем текст легенды
                    };

                    // Генерируем кривую
                    int steps = 100;
                    double stepSize = (maxX - minX) / steps;

                    for (int i = 0; i <= steps; i++)
                    {
                        double x = minX + i * stepSize;
                        double y = a * x + b;
                        approxSeries.Points.AddXY(x, y);
                    }

                    // Удаляем старую аппроксимацию, если есть
                    var oldSeries = chartExp.Series.FirstOrDefault(s => s.Name == "Linear Approx");
                    if (oldSeries != null) chartExp.Series.Remove(oldSeries);

                    chartExp.Series.Add(approxSeries);
                }

            }




            // ✔ Подпись крайней точки
            var lastPoint = allPoints.OrderByDescending(p => p.x).FirstOrDefault();
            if (lastPoint.x != 0 || lastPoint.y != 0)
            {
                chartExp.Series.Add(new Series("Last Point Label")
                {
                    ChartType = SeriesChartType.Point,
                    IsVisibleInLegend = false,
                    MarkerStyle = MarkerStyle.None,
                    LabelForeColor = Color.Gray
                });

                chartExp.Series["Last Point Label"].Points.AddXY(lastPoint.x, lastPoint.y);
                chartExp.Series["Last Point Label"].Points[0].Label = $"({lastPoint.x:F2}; {lastPoint.y:F2})";
            }

            
           
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
                MessageBox.Show("Choise both axes!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void checkBoxApprox_CheckedChanged(object sender, EventArgs e)
        {
            buttonMake_Click(null, null); // просто перерисовать график
        }

      

        private void checkBoxAvg_CheckedChanged(object sender, EventArgs e)
        {
            buttonMake_Click(null, null); // просто перерисовать график
        }

        



    }
}
