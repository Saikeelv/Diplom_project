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
using System.IO;

namespace Diplom_project
{
    public partial class AddClient: Form
    {
        private string connectionString;
        private Main mainForm;
        public AddClient(string dbPath, Main form)
        {
            InitializeComponent();
            connectionString = $"Data Source={dbPath};Version=3;";
            mainForm = form;
        }

        private void AddClient_Load(object sender, EventArgs e)
        {

        }

        private void ESC_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрывает текущую форму
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFIO_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNomber_TextChanged(object sender, EventArgs e)
        {

        }

        private void Apply_Click(object sender, EventArgs e)
        {
            string fio = textBoxFIO.Text.Trim();
            string phone = textBoxNomber.Text.Trim();

            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Client (FIO, Phone_num) VALUES (@FIO, @Phone)";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FIO", fio);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);



                mainForm.LoadClients(); // Обновляем список клиентов
                this.Close(); // Закрываем форму после успешного добавления
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
