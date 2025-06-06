﻿using System;
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
        public string fio { get; private set; }
        public string phone { get; private set; }
        private void Apply_Click(object sender, EventArgs e)
        {
            fio = textBoxFIO.Text.Trim();
            phone = textBoxNomber.Text.Trim();

            if (string.IsNullOrWhiteSpace(fio) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please fill in all fields!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                



                mainForm.LoadClients(); // Обновляем список клиентов
                this.DialogResult = DialogResult.OK;// Закрываем форму и передаем результат
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding a client: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
