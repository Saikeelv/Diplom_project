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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.listBoxExp = new System.Windows.Forms.ListBox();
            this.buttonStopExp = new System.Windows.Forms.Button();
            this.buttonCheckInst = new System.Windows.Forms.Button();
            this.buttonStartExp = new System.Windows.Forms.Button();
            this.Experiment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(511, 352);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(419, 39);
            this.progressBar1.TabIndex = 54;
            // 
            // listBoxExp
            // 
            this.listBoxExp.FormattingEnabled = true;
            this.listBoxExp.Location = new System.Drawing.Point(510, 56);
            this.listBoxExp.Name = "listBoxExp";
            this.listBoxExp.Size = new System.Drawing.Size(420, 277);
            this.listBoxExp.TabIndex = 53;
            // 
            // buttonStopExp
            // 
            this.buttonStopExp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStopExp.Location = new System.Drawing.Point(32, 186);
            this.buttonStopExp.Name = "buttonStopExp";
            this.buttonStopExp.Size = new System.Drawing.Size(420, 205);
            this.buttonStopExp.TabIndex = 52;
            this.buttonStopExp.TabStop = false;
            this.buttonStopExp.Text = "STOP";
            this.buttonStopExp.UseVisualStyleBackColor = true;
            this.buttonStopExp.Click += new System.EventHandler(this.buttonStopExp_Click);
            // 
            // buttonCheckInst
            // 
            this.buttonCheckInst.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCheckInst.Location = new System.Drawing.Point(32, 56);
            this.buttonCheckInst.Name = "buttonCheckInst";
            this.buttonCheckInst.Size = new System.Drawing.Size(420, 39);
            this.buttonCheckInst.TabIndex = 51;
            this.buttonCheckInst.TabStop = false;
            this.buttonCheckInst.Text = "CHEKING INSTALATION";
            this.buttonCheckInst.UseVisualStyleBackColor = true;
            this.buttonCheckInst.Click += new System.EventHandler(this.buttonCheckInst_Click);
            // 
            // buttonStartExp
            // 
            this.buttonStartExp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartExp.Location = new System.Drawing.Point(32, 122);
            this.buttonStartExp.Name = "buttonStartExp";
            this.buttonStartExp.Size = new System.Drawing.Size(420, 39);
            this.buttonStartExp.TabIndex = 50;
            this.buttonStartExp.TabStop = false;
            this.buttonStartExp.Text = "START EXPERIMENT";
            this.buttonStartExp.UseVisualStyleBackColor = true;
            this.buttonStartExp.Click += new System.EventHandler(this.buttonStartExp_Click);
            // 
            // Experiment
            // 
            this.Experiment.AutoSize = true;
            this.Experiment.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Experiment.Location = new System.Drawing.Point(27, 9);
            this.Experiment.Name = "Experiment";
            this.Experiment.Size = new System.Drawing.Size(118, 26);
            this.Experiment.TabIndex = 55;
            this.Experiment.Text = "Experiment";
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 450);
            this.Controls.Add(this.Experiment);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listBoxExp);
            this.Controls.Add(this.buttonStopExp);
            this.Controls.Add(this.buttonCheckInst);
            this.Controls.Add(this.buttonStartExp);
            this.Name = "Form6";
            this.Text = "Form6";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListBox listBoxExp;
        private System.Windows.Forms.Button buttonStopExp;
        private System.Windows.Forms.Button buttonCheckInst;
        private System.Windows.Forms.Button buttonStartExp;
        private System.Windows.Forms.Label Experiment;
    }
}