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

namespace Diplom_project
{
    public partial class Form6: Form
    {
        private SerialPort serialPort;
        public Form6(string portName)
        {
            InitializeComponent();
            try
            {
                serialPort = new SerialPort(portName, 115200);
                serialPort.DataReceived += serialPort_DataReceived;

                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия порта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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




        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePort(); // Закрываем порт при закрытии формы
        }

        private void buttonCheckInst_Click(object sender, EventArgs e)
        {
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
            SendCommand("0");
            ToggleButtons(false); // Разблокировка кнопок
            ExperimentFinished();  // Записываем данные и закрываем форму
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
                MessageBox.Show("Порт закрыт. Проверьте соединение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

                // Проверяем служебные коды завершения
                if (data == "8888" || data == "9999" || data == "7777")
                {
                    isExperimentRunning = false;
                    this.Invoke(new Action(() => ExperimentFinished()));
                    return;
                }

                // Разбираем пакет данных
                string[] values = data.Split(';');
                if (values.Length != 7) return;

                // Проверяем, идет ли эксперимент
                if (!isExperimentRunning) return;

                // Если начинается новое испытание, сохраняем предыдущее и очищаем данные
                if (testNumber != "" && testNumber != values[5])
                {
                    SaveCurrentExperiment();
                }

                // Записываем номер испытания (если не задан)
                testNumber = values[5];

                // Записываем номер ошибки (первое ненулевое значение)
                if (errorNumber == "0" && values[6] != "0")
                {
                    errorNumber = values[6];
                }

                // Записываем значения в списки
                timeData.Add(values[1]);
                temperatureData.Add(values[2]);
                weightData.Add(values[3]);
                rotationData.Add(values[4]);
            }
            catch (IOException ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"Ошибка при чтении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }));
            }
            catch (InvalidOperationException)
            {
                // Игнорируем ошибку, если порт закрыт
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




        private async void ExperimentFinished()
        {
            await Task.Run(() =>
            {
                SaveCurrentExperiment(); // Сохраняем последнее испытание

                List<string> fileData = new List<string>();
                foreach (var experiment in allExperiments)
                {
                    fileData.AddRange(experiment);
                }

                File.WriteAllLines("experiment_result.txt", fileData); // Запись в файл в фоне
            });

            this.Invoke(new Action(() =>
            {
                MessageBox.Show("Эксперимент завершен. Данные сохранены в файл.", "Эксперимент завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClosePort(); // Закрываем COM-порт в UI-потоке
                this.Close(); // Закрываем форму
            }));
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
                    MessageBox.Show($"Ошибка при закрытии порта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }










        private void ToggleButtons(bool isTransmitting)
        {
            buttonCheckInst.Enabled = !isTransmitting;
            buttonStartExp.Enabled = !isTransmitting;
            buttonStopExp.Enabled = isTransmitting; // Кнопка "Стоп" активна только во время передачи
        }



    }
}
