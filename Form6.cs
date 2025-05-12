using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using System.Data.SQLite;

namespace Diplom_project
{
    public partial class Form6: Form
    {
        private SerialPort serialPort;
        private int experimentId;
        private string connectionString = "";
        public Form6(string portName, string dbPath, int ExperimentId)
        {
            InitializeComponent();
            experimentId = ExperimentId;
            connectionString = $"Data Source={dbPath};Version=3;";
            try
            {
                serialPort = new SerialPort(portName, 115200);
                serialPort.DataReceived += serialPort_DataReceived;

                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Port opening error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private List<List<string>> allExperiments = new List<List<string>>(); // Список всех испытаний

        private List<string> timeData = new List<string>();       // Время
        private List<string> temperatureData = new List<string>(); // Температура
        private List<string> weightData = new List<string>();      // Вес
        private List<string> rotationData = new List<string>();    // Скорость вращения

        private string testNumber = "";  // Номер испытания (одно значение)
        private string errorNumber = "0"; // Первое ненулевое значение ошибки (одно значение)

        private bool isExperimentRunning = false; // Флаг, идет ли эксперимент
        private bool isTestMode = false; // Если true, данные в БД не записываются
        private bool isSaving = false; // ✅ Флаг для защиты от двойного сохранения






        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePort(); // Закрываем порт при закрытии формы
        }

        private void buttonCheckInst_Click(object sender, EventArgs e)
        {
            isTestMode = true; // Включаем режим теста
            SendCommand("2");
        }

        private void buttonStartExp_Click(object sender, EventArgs e)
        {
            isExperimentRunning = true;
            allExperiments.Clear(); // Очистка перед новым экспериментом
            SendCommand("1");
        }

        private void buttonStopExp_Click(object sender, EventArgs e)
        {
            if (isExperimentRunning || isTestMode)
            {
                SendCommand("0");
                ToggleButtons(false); // Разблокировка кнопок
                ExperimentFinished();  // Записываем данные и закрываем форму
            }
            
        }




        private void SendCommand(string command)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                ToggleButtons(true); // Блокируем кнопки перед отправкой
                serialPort.WriteLine(command);
            }
            else
            {
                MessageBox.Show("The port is closed. Check the connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }





        
        private void SaveCurrentExperiment()
        {
            if (string.IsNullOrEmpty(testNumber)) return;

            List<string> experimentData = new List<string>
    {
        testNumber,                             // 1 строка: номер испытания
        errorNumber,                            // 2 строка: номер ошибки
        string.Join(";", timeData),             // 3 строка: Время
        string.Join(";", temperatureData),      // 4 строка: Температура
        string.Join(";", weightData),           // 5 строка: Вес
        string.Join(";", rotationData)          // 6 строка: Скорость вращения
    };

            allExperiments.Add(experimentData);

            // Очищаем данные для следующего испытания
            timeData.Clear();
            temperatureData.Clear();
            weightData.Clear();
            rotationData.Clear();
            errorNumber = "0";
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();

                this.Invoke(new Action(() =>
                {
                    listBoxExp.Items.Add(data);
                    listBoxExp.TopIndex = listBoxExp.Items.Count - 1;
                }));

                // Проверяем, не в тестовом ли режиме мы находимся
                if (isTestMode)
                {
                    if (data == "8888" || data == "7777") // Завершение проверки
                    {
                        this.Invoke(new Action(() => ToggleButtons(false))); // Разблокируем кнопки
                    }
                    return; // Дальше ничего не делаем
                }

                // Проверяем коды завершения эксперимента
                if ( data == "9999")
                {
                    isExperimentRunning = false;

                    // ✅ Проверяем, не идет ли уже сохранение
                    if (!isSaving)
                    {
                        this.Invoke(new Action(() => ExperimentFinished()));
                    }
                    return;
                }

                // Разбираем пакет данных (если не тестовый режим)
                string[] values = data.Split(';');
                if (values.Length != 7) return;

                if (!isExperimentRunning) return;

                if (testNumber != "" && testNumber != values[5] && !isSaving)
                {
                    SaveCurrentExperiment();
                }

                testNumber = values[5];

                if (errorNumber == "0" && values[6] != "0")
                {
                    errorNumber = values[6];
                }

                timeData.Add(values[1]);
                temperatureData.Add(values[2]);
                weightData.Add(values[3]);
                rotationData.Add(values[4]);
            }
            //catch (IOException ex)
            catch (IOException)
            {
                /*из за этого падает в ошибку
                this.Invoke(new Action(() =>
                {
                   MessageBox.Show($"Error in reading: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
                */
            }
            catch (InvalidOperationException)
            {
                // Игнорируем ошибку, если порт закрыт
            }
        }



        private void ClosePort()
        {
            if (serialPort != null)
            {
                try
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.DataReceived -= serialPort_DataReceived; // Отписываемся от события
                        serialPort.DiscardInBuffer(); // Очищаем буфер перед закрытием
                        serialPort.Close();
                    }
                    serialPort.Dispose();
                    serialPort = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error closing the port: {ex.Message}", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }



        private void ToggleButtons(bool isTransmitting)
        {
            buttonCheckInst.Enabled = !isTransmitting;
            buttonStartExp.Enabled = !isTransmitting;
            buttonStopExp.Enabled = isTransmitting; // Кнопка "Стоп" активна только во время передачи
        }

        private void UpdateExperimentError(SQLiteConnection connection, SQLiteTransaction transaction)
        {
            string queryCount = "SELECT COUNT(*) FROM Data_of_exp WHERE Experiment_FK = @ExperimentId";
            string queryErrors = "SELECT num_of_error FROM Data_of_exp WHERE Experiment_FK = @ExperimentId";

            int totalTests = 0;
            List<int> errors = new List<int>();

            using (SQLiteCommand cmd = new SQLiteCommand(queryCount, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@ExperimentId", experimentId);
                totalTests = Convert.ToInt32(cmd.ExecuteScalar()); // Количество испытаний
            }

            using (SQLiteCommand cmd = new SQLiteCommand(queryErrors, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@ExperimentId", experimentId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        errors.Add(reader.GetInt32(0)); // Список всех ошибок
                    }
                }
            }

            int finalError = 1; // По умолчанию, если все ошибки = 0 и >= 10 испытаний

            // Проверяем наличие ненулевых ошибок
            foreach (var err in errors)
            {
                if (err != 0)
                {
                    finalError = err; // Берем первое ненулевое значение ошибки
                    break;
                }
            }

            // Если ошибок не было, проверяем количество испытаний
            if (finalError == 1 && totalTests < 3)
            {
                finalError = 777; // Если испытаний < 10 и ошибок нет, ставим 777
            }

            string updateQuery = "UPDATE Experiment SET Error = @Error WHERE Experiment_PK = @ExperimentId";

            using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@Error", finalError);
                cmd.Parameters.AddWithValue("@ExperimentId", experimentId);
                cmd.ExecuteNonQuery();
            }
        }




        private async void ExperimentFinished()
        {
            if (isTestMode)
            {
                isTestMode = false;
                return;
            }
            if (isSaving) return; // ✅ Если уже идет сохранение, пропускаем
            isSaving = true; // ✅ Устанавливаем флаг, чтобы избежать дублирования
            await Task.Run(() =>
            {
                SaveCurrentExperiment(); // Сохраняем последнее испытание

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var experiment in allExperiments)
                            {
                                string query = @"
                            INSERT INTO Data_of_exp (run_of_test, num_of_error, Time, Temp, Power, Speed, Experiment_FK) 
                            VALUES (@run_of_test, @num_of_error, @Time, @Temp, @Power, @Speed, @ExperimentId)";

                                using (SQLiteCommand cmd = new SQLiteCommand(query, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@run_of_test", experiment[0]); // Номер испытания
                                    cmd.Parameters.AddWithValue("@num_of_error", experiment[1]); // Ошибка
                                    cmd.Parameters.AddWithValue("@Time", experiment[2]); // Время
                                    cmd.Parameters.AddWithValue("@Temp", experiment[3]); // Температура
                                    cmd.Parameters.AddWithValue("@Power", experiment[4]); // Вес
                                    cmd.Parameters.AddWithValue("@Speed", experiment[5]); // Скорость вращения
                                    cmd.Parameters.AddWithValue("@ExperimentId", experimentId);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            UpdateExperimentError(connection, transaction); // Обновляем поле Error в Experiment

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error when writing to the database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });

            this.Invoke(new Action(() =>
            {
                MessageBox.Show("The experiment is completed. The data is saved in the database.", "The experiment is completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClosePort();
                this.Close();
                isSaving = false; // ✅ Сбрасываем флаг после завершения
            }));
        }



    }
}
