namespace Diplom_project
{
    partial class Form7
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBoxX = new System.Windows.Forms.ComboBox();
            this.comboBoxY = new System.Windows.Forms.ComboBox();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.buttonMake = new System.Windows.Forms.Button();
            this.chartExp = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.buttonSaveData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chartExp)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxX
            // 
            this.comboBoxX.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxX.FormattingEnabled = true;
            this.comboBoxX.Location = new System.Drawing.Point(46, 15);
            this.comboBoxX.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxX.Name = "comboBoxX";
            this.comboBoxX.Size = new System.Drawing.Size(92, 30);
            this.comboBoxX.TabIndex = 0;
            // 
            // comboBoxY
            // 
            this.comboBoxY.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxY.FormattingEnabled = true;
            this.comboBoxY.Location = new System.Drawing.Point(180, 15);
            this.comboBoxY.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxY.Name = "comboBoxY";
            this.comboBoxY.Size = new System.Drawing.Size(92, 30);
            this.comboBoxY.TabIndex = 1;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX.Location = new System.Drawing.Point(16, 16);
            this.labelX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(30, 27);
            this.labelX.TabIndex = 2;
            this.labelX.Text = "X:";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelY.Location = new System.Drawing.Point(152, 15);
            this.labelY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(30, 27);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y:";
            this.labelY.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonMake
            // 
            this.buttonMake.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMake.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMake.Location = new System.Drawing.Point(20, 42);
            this.buttonMake.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMake.Name = "buttonMake";
            this.buttonMake.Size = new System.Drawing.Size(251, 37);
            this.buttonMake.TabIndex = 4;
            this.buttonMake.Text = "MAKE GRAPH";
            this.buttonMake.UseVisualStyleBackColor = true;
            this.buttonMake.Click += new System.EventHandler(this.buttonMake_Click);
            // 
            // chartExp
            // 
            chartArea2.Name = "ChartArea1";
            this.chartExp.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartExp.Legends.Add(legend2);
            this.chartExp.Location = new System.Drawing.Point(20, 84);
            this.chartExp.Margin = new System.Windows.Forms.Padding(2);
            this.chartExp.Name = "chartExp";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartExp.Series.Add(series2);
            this.chartExp.Size = new System.Drawing.Size(882, 454);
            this.chartExp.TabIndex = 5;
            this.chartExp.Text = "chart1";
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSaveData.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSaveData.Location = new System.Drawing.Point(290, 43);
            this.buttonSaveData.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(251, 37);
            this.buttonSaveData.TabIndex = 6;
            this.buttonSaveData.Text = "SAVE DATA";
            this.buttonSaveData.UseVisualStyleBackColor = true;
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // Form7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 556);
            this.Controls.Add(this.buttonSaveData);
            this.Controls.Add(this.chartExp);
            this.Controls.Add(this.buttonMake);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.comboBoxY);
            this.Controls.Add(this.comboBoxX);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form7";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Form7_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartExp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxX;
        private System.Windows.Forms.ComboBox comboBoxY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Button buttonMake;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartExp;
        private System.Windows.Forms.Button buttonSaveData;
    }
}