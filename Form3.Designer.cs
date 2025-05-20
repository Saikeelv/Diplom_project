namespace Diplom_project
{
    partial class ChangeClient
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonChangeDataClient = new System.Windows.Forms.Button();
            this.Esq = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client data:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(16, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "FIO:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(16, 136);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 30);
            this.label3.TabIndex = 2;
            this.label3.Text = "Phone number:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(174, 68);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(280, 30);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.Location = new System.Drawing.Point(174, 136);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(280, 30);
            this.textBox2.TabIndex = 4;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // buttonChangeDataClient
            // 
            this.buttonChangeDataClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeDataClient.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeDataClient.Location = new System.Drawing.Point(174, 258);
            this.buttonChangeDataClient.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonChangeDataClient.Name = "buttonChangeDataClient";
            this.buttonChangeDataClient.Size = new System.Drawing.Size(127, 38);
            this.buttonChangeDataClient.TabIndex = 5;
            this.buttonChangeDataClient.Text = "ACCEPT";
            this.buttonChangeDataClient.UseVisualStyleBackColor = true;
            this.buttonChangeDataClient.Click += new System.EventHandler(this.buttonChangeDataClient_Click);
            // 
            // Esq
            // 
            this.Esq.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Esq.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Esq.Location = new System.Drawing.Point(327, 258);
            this.Esq.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Esq.Name = "Esq";
            this.Esq.Size = new System.Drawing.Size(127, 38);
            this.Esq.TabIndex = 6;
            this.Esq.Text = "ESC";
            this.Esq.UseVisualStyleBackColor = true;
            this.Esq.Click += new System.EventHandler(this.Esq_Click);
            // 
            // ChangeClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 309);
            this.Controls.Add(this.Esq);
            this.Controls.Add(this.buttonChangeDataClient);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChangeClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change data of Client";
            this.Load += new System.EventHandler(this.ChangeClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonChangeDataClient;
        private System.Windows.Forms.Button Esq;
    }
}