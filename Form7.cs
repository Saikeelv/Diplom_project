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
            this.checkBoxAvg.CheckedChanged += new EventHandler(this.checkBoxAvg_CheckedChanged);
            this.checkBoxCutOff.CheckedChanged += new EventHandler(this.checkBoxCutOff_CheckedChanged);
            this.checkBoxLinear.CheckedChanged += new EventHandler(this.checkBoxLinear_CheckedChanged);

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
            chartExp.ChartAreas[0].AxisY.Title = yLabel == "Speed" && checkBoxLinear.Checked ? "Linear Speed (m/s)" : yLabel;

            chartExp.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartExp.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartExp.ChartAreas[0].BackColor = Color.White;

            // Установка формата подписей осей с 2 знаками после запятой
            chartExp.ChartAreas[0].AxisX.LabelStyle.Format = "{0:F2}";
            chartExp.ChartAreas[0].AxisY.LabelStyle.Format = "{0:F2}";

            List<(double x, double y)> allPoints = new List<(double, double)>();
            string[] fixedColors = { "Blue", "Green", "Red", "Orange", "Purple", "Brown", "Turquoise", "DarkCyan", "DarkSlateGray", "DarkMagenta" };
            int colorIndex = 0;

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            // Конверсия скорости в линейную, если нужно (радиус = 0.01 м)
            Dictionary<int, List<(double x, double y)>> processedData = new Dictionary<int, List<(double x, double y)>>();
            foreach (var experiment in experimentsData)
            {
                processedData[experiment.Key] = new List<(double x, double y)>();
                foreach (var point in experiment.Value)
                {
                    double x = point.Item1;
                    double y = point.Item2;
                    if (yLabel == "Speed" && checkBoxLinear.Checked)
                    {
                        y = (2 * Math.PI * 0.01 * y) / 60; // Конверсия RPM в м/с
                        if (double.IsNaN(y) || double.IsInfinity(y) || y < 0) continue; // Пропуск недопустимых значений
                    }
                    processedData[experiment.Key].Add((x, y));
                }
            }

            foreach (var experiment in processedData)
            {
                var points = experiment.Value
                    .Where(p => p.x > 1 && p.y > 0) // Убедимся, что y положительное для линейной скорости
                    .ToList();

                if (xLabel == "Power" && yLabel == "Speed" && checkBoxCutOff.Checked)
                {
                    points = points.Where(p => p.x >= 5000).ToList(); // Отсечение Power < 5000
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
                    series.Points.AddXY(point.x, point.y); // Используем полную точность для точек
                    allPoints.Add(point);

                    if (point.x < minX) minX = point.x;
                    if (point.x > maxX) maxX = point.x;
                    if (point.y < minY) minY = point.y;
                    if (point.y > maxY) maxY = point.y;
                }

                chartExp.Series.Add(series);
            }

            // Установка границ осей
            if (xLabel == "Power" && yLabel == "Speed" && checkBoxCutOff.Checked)
            {
                chartExp.ChartAreas[0].AxisX.Minimum = 5000;
                chartExp.ChartAreas[0].AxisX.Maximum = 15500;
                chartExp.ChartAreas[0].AxisY.Minimum = checkBoxLinear.Checked ? 0.5 : 500; // Нижняя граница 0.5 для линейной скорости
                chartExp.ChartAreas[0].AxisY.Maximum = checkBoxLinear.Checked ? maxY : (maxY > 500 ? maxY : 1000);
            }
            else if (xLabel == "Power" && yLabel == "Speed")
            {
                chartExp.ChartAreas[0].AxisX.Minimum = minX;
                chartExp.ChartAreas[0].AxisX.Maximum = 15500;
                chartExp.ChartAreas[0].AxisY.Minimum = checkBoxLinear.Checked ? 0.5 : 500; // Нижняя граница 0.5 для линейной скорости
                chartExp.ChartAreas[0].AxisY.Maximum = checkBoxLinear.Checked ? maxY : (maxY > 500 ? maxY : 1000);
            }
            else
            {
                chartExp.ChartAreas[0].AxisX.Minimum = minX;
                chartExp.ChartAreas[0].AxisX.Maximum = maxX;
                chartExp.ChartAreas[0].AxisY.Minimum = minY > 0 ? minY : 0;
                chartExp.ChartAreas[0].AxisY.Maximum = maxY > minY ? maxY : minY + 1;
            }

            // Усреднение и аппроксимация
            if (allPoints.Count > 2)
            {
                var filtered = allPoints
                    .Where(p => p.x > 0)
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
                        ChartType = SeriesChartType.Spline,
                        Color = Color.Red,
                        BorderWidth = 2,
                        MarkerStyle = MarkerStyle.None
                    };

                    int intervals = 50;
                    double rangeX = maxX - minX;
                    if (rangeX <= 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Диапазон X некорректен для усреднения.");
                        return;
                    }
                    double interval = rangeX / intervals;

                    List<(double x, double y)> avgPoints = new List<(double, double)>();

                    for (int i = 0; i < intervals; i++)
                    {
                        double xStart = minX + i * interval;
                        double xEnd = xStart + interval;

                        var pointsInInterval = filtered
                            .Where(p => p.x >= xStart && p.x < xEnd)
                            .ToList();

                        if (pointsInInterval.Count > 0)
                        {
                            double avgY = pointsInInterval.Average(p => p.y); // Полная точность
                            double avgX = (xStart + xEnd) / 2.0;
                            avgPoints.Add((avgX, avgY));
                        }
                    }

                    avgPoints = avgPoints.OrderBy(p => p.x).ToList();

                    foreach (var point in avgPoints)
                    {
                        avgSeries.Points.AddXY(point.x, point.y); // Полная точность
                    }

                    chartExp.Series.Add(avgSeries);
                }

                // Аппроксимация (checkBoxApprox)
                if (checkBoxApprox.Checked)
                {
                    var validPoints = filtered
                        .Where(p => p.x > 0 && p.y > 0)
                        .Where(p => !checkBoxCutOff.Checked || (checkBoxCutOff.Checked && p.x >= 5000))
                        .OrderBy(p => p.x)
                        .ToList();

                    if (validPoints.Count < 2)
                    {
                        MessageBox.Show("Недостаточно точек для аппроксимации", "Ошибка");
                        return;
                    }

                    Series approxSeries;
                    double rSquared = 0;
                    double c = (yLabel == "Speed" && checkBoxLinear.Checked) ? 0.7 : (yLabel == "Speed" ? 670 : 0);

                    if (xLabel == "Power" && yLabel == "Speed")
                    {
                        // Квадратичная аппроксимация: speed = a*power^2 + b*power + c
                        double sumX = validPoints.Sum(p => p.x);
                        double sumX2 = validPoints.Sum(p => p.x * p.x);
                        double sumX3 = validPoints.Sum(p => p.x * p.x * p.x);
                        double sumX4 = validPoints.Sum(p => p.x * p.x * p.x * p.x);
                        double sumY = validPoints.Sum(p => p.y - c);
                        double sumXY = validPoints.Sum(p => p.x * (p.y - c));
                        double sumX2Y = validPoints.Sum(p => (p.x * p.x) * (p.y - c));
                        int n = validPoints.Count;

                        double[,] A = new double[,] {
            { sumX4, sumX3 },
            { sumX3, sumX2 }
        };
                        double[] B = new double[] { sumX2Y, sumXY };

                        double detA = A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];
                        if (Math.Abs(detA) < 1e-10)
                        {
                            MessageBox.Show("Матрица системы вырожденна. Невозможно выполнить аппроксимацию.", "Ошибка");
                            return;
                        }

                        double a = (B[0] * A[1, 1] - B[1] * A[0, 1]) / detA;
                        double b = (A[0, 0] * B[1] - A[1, 0] * B[0]) / detA;

                        double meanY = validPoints.Average(p => p.y);
                        double sst = validPoints.Sum(p => Math.Pow(p.y - meanY, 2));
                        double ssr = validPoints.Sum(p => Math.Pow(p.y - (a * p.x * p.x + b * p.x + c), 2));
                        rSquared = sst > 0 ? 1 - (ssr / sst) : 0;

                        // Отображение коэффициентов в экспоненциальной форме в скобках
                        string legendText = $"speed = ({a:E2})*power^2 + ({b:E2})*power + {c} (R² = {rSquared:F2})";
                        approxSeries = new Series("Quadratic Approx")
                        {
                            ChartType = SeriesChartType.Line,
                            Color = Color.Magenta,
                            BorderWidth = 3,
                            MarkerStyle = MarkerStyle.None,
                            LegendText = legendText
                        };

                        int steps = 100;
                        double startX = checkBoxCutOff.Checked ? 5000 : minX;
                        double endX = 15500;
                        double stepSize = (endX - startX) / steps;

                        for (int i = 0; i <= steps; i++)
                        {
                            double x = startX + i * stepSize;
                            double y = a * x * x + b * x + c;
                            if (y >= 0.5 && checkBoxLinear.Checked) approxSeries.Points.AddXY(x, y);
                            else if (!checkBoxLinear.Checked) approxSeries.Points.AddXY(x, y);
                        }
                    }
                    else
                    {
                        // Линейная аппроксимация для остальных случаев
                        double sumX = validPoints.Sum(p => p.x);
                        double sumY = validPoints.Sum(p => p.y);
                        double sumXY = validPoints.Sum(p => p.x * p.y);
                        double sumX2 = validPoints.Sum(p => p.x * p.x);
                        int n = validPoints.Count;

                        double a = (n * sumXY - sumX * sumY) / (n * sumX2 - Math.Pow(sumX, 2));
                        double b = (sumY - a * sumX) / n;

                        double meanY = validPoints.Average(p => p.y);
                        double sst = validPoints.Sum(p => Math.Pow(p.y - meanY, 2));
                        double ssr = validPoints.Sum(p => Math.Pow(p.y - (a * p.x + b), 2));
                        rSquared = sst > 0 ? 1 - (ssr / sst) : 0;

                        // Отображение коэффициентов в экспоненциальной форме в скобках
                        string legendText = $"y = ({a:E2})*x + ({b:E2}) (R² = {rSquared:F2})";
                        approxSeries = new Series("Linear Approx")
                        {
                            ChartType = SeriesChartType.Line,
                            Color = Color.Black,
                            BorderWidth = 3,
                            MarkerStyle = MarkerStyle.None,
                            LegendText = legendText
                        };

                        int steps = 100;
                        double startX = (!checkBoxCutOff.Checked && xLabel == "Power" && yLabel == "Speed") ? minX : (checkBoxCutOff.Checked ? 5000 : minX);
                        double endX = (xLabel == "Power" && yLabel == "Speed") ? 15500 : maxX;
                        double stepSize = (endX - startX) / steps;

                        for (int i = 0; i <= steps; i++)
                        {
                            double x = startX + i * stepSize;
                            double y = a * x + b;
                            approxSeries.Points.AddXY(x, y);
                        }
                    }

                    var oldSeries = chartExp.Series.FirstOrDefault(s => s.Name == "Linear Approx" || s.Name == "Quadratic Approx");
                    if (oldSeries != null) chartExp.Series.Remove(oldSeries);

                    chartExp.Series.Add(approxSeries);
                }
            }

            // Подпись первой и крайней точек с 2 знаками после запятой
            var firstPoint = allPoints.OrderBy(p => p.x).FirstOrDefault();
            if (firstPoint.x != 0 || firstPoint.y != 0)
            {
                chartExp.Series.Add(new Series("First Point Label")
                {
                    ChartType = SeriesChartType.Point,
                    IsVisibleInLegend = false,
                    MarkerStyle = MarkerStyle.None,
                    LabelForeColor = Color.Gray
                });

                chartExp.Series["First Point Label"].Points.AddXY(firstPoint.x, firstPoint.y);
                chartExp.Series["First Point Label"].Points[0].Label = $"({firstPoint.x:F2}; {firstPoint.y:F2})";
            }

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

        private void checkBoxCutOff_CheckedChanged(object sender, EventArgs e)
        {
            buttonMake_Click(null, null); // просто перерисовать график
        }

        private void checkBoxLinear_CheckedChanged(object sender, EventArgs e)
        {
            buttonMake_Click(null, null); // Redraw the graph
        }
    }
}
