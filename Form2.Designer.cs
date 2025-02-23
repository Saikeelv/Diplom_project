namespace Diplom_project
{
    partial class AddClient
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ESC = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFIO = new System.Windows.Forms.TextBox();
            this.textBoxNomber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ESC
            // 
            this.ESC.Location = new System.Drawing.Point(308, 257);
            this.ESC.Name = "ESC";
            this.ESC.Size = new System.Drawing.Size(126, 36);
            this.ESC.TabIndex = 0;
            this.ESC.Text = "Отмена";
            this.ESC.UseVisualStyleBackColor = true;
            this.ESC.Click += new System.EventHandler(this.ESC_Click);
            // 
            // Apply
            // 
            this.Apply.Location = new System.Drawing.Point(176, 258);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(126, 36);
            this.Apply.TabIndex = 1;
            this.Apply.Text = "Применить";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Добавить клиента:";
            // 
            // textBoxFIO
            // 
            this.textBoxFIO.Location = new System.Drawing.Point(193, 58);
            this.textBoxFIO.Name = "textBoxFIO";
            this.textBoxFIO.Size = new System.Drawing.Size(241, 26);
            this.textBoxFIO.TabIndex = 3;
            this.textBoxFIO.TextChanged += new System.EventHandler(this.textBoxFIO_TextChanged);
            // 
            // textBoxNomber
            // 
            this.textBoxNomber.Location = new System.Drawing.Point(193, 121);
            this.textBoxNomber.Name = "textBoxNomber";
            this.textBoxNomber.Size = new System.Drawing.Size(241, 26);
            this.textBoxNomber.TabIndex = 4;
            this.textBoxNomber.TextChanged += new System.EventHandler(this.textBoxNomber_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = "ФИО:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Номер телефона:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // AddClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 306);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxNomber);
            this.Controls.Add(this.textBoxFIO);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.ESC);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add client";
            this.Load += new System.EventHandler(this.AddClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ESC;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFIO;
        private System.Windows.Forms.TextBox textBoxNomber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}