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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBoxX = new System.Windows.Forms.ComboBox();
            this.comboBoxY = new System.Windows.Forms.ComboBox();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.buttonMake = new System.Windows.Forms.Button();
            this.chartGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxX
            // 
            this.comboBoxX.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxX.FormattingEnabled = true;
            this.comboBoxX.Location = new System.Drawing.Point(61, 19);
            this.comboBoxX.Name = "comboBoxX";
            this.comboBoxX.Size = new System.Drawing.Size(121, 27);
            this.comboBoxX.TabIndex = 0;
            // 
            // comboBoxY
            // 
            this.comboBoxY.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxY.FormattingEnabled = true;
            this.comboBoxY.Location = new System.Drawing.Point(240, 18);
            this.comboBoxY.Name = "comboBoxY";
            this.comboBoxY.Size = new System.Drawing.Size(121, 27);
            this.comboBoxY.TabIndex = 1;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelX.Location = new System.Drawing.Point(21, 20);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(34, 26);
            this.labelX.TabIndex = 2;
            this.labelX.Text = "X:";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelY.Location = new System.Drawing.Point(202, 19);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(32, 26);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y:";
            this.labelY.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonMake
            // 
            this.buttonMake.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMake.Location = new System.Drawing.Point(26, 52);
            this.buttonMake.Name = "buttonMake";
            this.buttonMake.Size = new System.Drawing.Size(335, 45);
            this.buttonMake.TabIndex = 4;
            this.buttonMake.Text = "Построить";
            this.buttonMake.UseVisualStyleBackColor = true;
            this.buttonMake.Click += new System.EventHandler(this.buttonMake_Click);
            // 
            // chartGraph
            // 
            chartArea1.Name = "ChartArea1";
            this.chartGraph.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartGraph.Legends.Add(legend1);
            this.chartGraph.Location = new System.Drawing.Point(26, 145);
            this.chartGraph.Name = "chartGraph";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartGraph.Series.Add(series1);
            this.chartGraph.Size = new System.Drawing.Size(550, 409);
            this.chartGraph.TabIndex = 5;
            this.chartGraph.Text = "chart1";
            // 
            // Form7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 684);
            this.Controls.Add(this.chartGraph);
            this.Controls.Add(this.buttonMake);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.comboBoxY);
            this.Controls.Add(this.comboBoxX);
            this.Name = "Form7";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.Form7_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxX;
        private System.Windows.Forms.ComboBox comboBoxY;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Button buttonMake;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGraph;
    }
}