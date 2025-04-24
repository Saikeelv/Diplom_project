namespace Diplom_project
{
    partial class Form6
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
            this.listBoxExp = new System.Windows.Forms.ListBox();
            this.buttonStopExp = new System.Windows.Forms.Button();
            this.buttonCheckInst = new System.Windows.Forms.Button();
            this.buttonStartExp = new System.Windows.Forms.Button();
            this.Experiment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxExp
            // 
            this.listBoxExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxExp.FormattingEnabled = true;
            this.listBoxExp.ItemHeight = 24;
            this.listBoxExp.Location = new System.Drawing.Point(680, 69);
            this.listBoxExp.Margin = new System.Windows.Forms.Padding(4);
            this.listBoxExp.Name = "listBoxExp";
            this.listBoxExp.Size = new System.Drawing.Size(547, 412);
            this.listBoxExp.TabIndex = 53;
            // 
            // buttonStopExp
            // 
            this.buttonStopExp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStopExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStopExp.Location = new System.Drawing.Point(43, 229);
            this.buttonStopExp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStopExp.Name = "buttonStopExp";
            this.buttonStopExp.Size = new System.Drawing.Size(560, 252);
            this.buttonStopExp.TabIndex = 52;
            this.buttonStopExp.TabStop = false;
            this.buttonStopExp.Text = "STOP";
            this.buttonStopExp.UseVisualStyleBackColor = true;
            this.buttonStopExp.Click += new System.EventHandler(this.buttonStopExp_Click);
            // 
            // buttonCheckInst
            // 
            this.buttonCheckInst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCheckInst.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheckInst.Location = new System.Drawing.Point(43, 69);
            this.buttonCheckInst.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCheckInst.Name = "buttonCheckInst";
            this.buttonCheckInst.Size = new System.Drawing.Size(560, 48);
            this.buttonCheckInst.TabIndex = 51;
            this.buttonCheckInst.TabStop = false;
            this.buttonCheckInst.Text = "CHEKING INSTALATION";
            this.buttonCheckInst.UseVisualStyleBackColor = true;
            this.buttonCheckInst.Click += new System.EventHandler(this.buttonCheckInst_Click);
            // 
            // buttonStartExp
            // 
            this.buttonStartExp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartExp.Location = new System.Drawing.Point(43, 150);
            this.buttonStartExp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStartExp.Name = "buttonStartExp";
            this.buttonStartExp.Size = new System.Drawing.Size(560, 48);
            this.buttonStartExp.TabIndex = 50;
            this.buttonStartExp.TabStop = false;
            this.buttonStartExp.Text = "START EXPERIMENT";
            this.buttonStartExp.UseVisualStyleBackColor = true;
            this.buttonStartExp.Click += new System.EventHandler(this.buttonStartExp_Click);
            // 
            // Experiment
            // 
            this.Experiment.AutoSize = true;
            this.Experiment.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Experiment.Location = new System.Drawing.Point(36, 11);
            this.Experiment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Experiment.Name = "Experiment";
            this.Experiment.Size = new System.Drawing.Size(105, 27);
            this.Experiment.TabIndex = 55;
            this.Experiment.Text = "Experiment";
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 548);
            this.Controls.Add(this.Experiment);
            this.Controls.Add(this.listBoxExp);
            this.Controls.Add(this.buttonStopExp);
            this.Controls.Add(this.buttonCheckInst);
            this.Controls.Add(this.buttonStartExp);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form6";
            this.Text = "Form6";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxExp;
        private System.Windows.Forms.Button buttonStopExp;
        private System.Windows.Forms.Button buttonCheckInst;
        private System.Windows.Forms.Button buttonStartExp;
        private System.Windows.Forms.Label Experiment;
    }
}