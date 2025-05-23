﻿using System;
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

    public partial class ChangeClient: Form
    {
        private Main mainForm;
        private string oldFIO;
        private string connectionString;
        public ChangeClient(Main form, string fio, string phone, string connString)
        {
            InitializeComponent();
            mainForm = form;
            oldFIO = fio;

            connectionString = connString; // Переданная строка подключения
            textBox1.Text = fio;   // Загружаем текущее ФИО
            textBox2.Text = phone; // Загружаем текущий номер
        }

        private void ChangeClient_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public string UpdatedFIO { get; private set; }
        public string UpdatedPhone { get; private set; }
        private void buttonChangeDataClient_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Fill in all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            UpdatedFIO = textBox1.Text.Trim();
            UpdatedPhone = textBox2.Text.Trim();
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Client SET FIO = @NewFIO, Phone_num = @NewPhone WHERE FIO = @OldFIO";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewFIO", textBox1.Text);
                        command.Parameters.AddWithValue("@NewPhone", textBox2.Text);
                        command.Parameters.AddWithValue("@OldFIO", oldFIO);
                        command.ExecuteNonQuery();
                    }
                }

                

                mainForm.LoadClients(); // Обновляем список клиентов
                this.DialogResult = DialogResult.OK;
                this.Close(); // Закрываем форму
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during the update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Esq_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
