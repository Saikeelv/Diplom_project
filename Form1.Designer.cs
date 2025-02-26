namespace Diplom_project
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.labelClients = new System.Windows.Forms.Label();
            this.buttonAddClient = new System.Windows.Forms.Button();
            this.buttonDellClient = new System.Windows.Forms.Button();
            this.labelSamples = new System.Windows.Forms.Label();
            this.buttonAddSamples = new System.Windows.Forms.Button();
            this.buttonDellSamples = new System.Windows.Forms.Button();
            this.buttonChangeSamples = new System.Windows.Forms.Button();
            this.Note = new System.Windows.Forms.Label();
            this.Date = new System.Windows.Forms.Label();
            this.EngineType = new System.Windows.Forms.Label();
            this.EngineBrand = new System.Windows.Forms.Label();
            this.EngineNomber = new System.Windows.Forms.Label();
            this.EngineMileage = new System.Windows.Forms.Label();
            this.OilMileage = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.textBoxEngineType = new System.Windows.Forms.TextBox();
            this.textBoxEhgineBrand = new System.Windows.Forms.TextBox();
            this.textBoxEngineNomber = new System.Windows.Forms.TextBox();
            this.textBoxEngineMileage = new System.Windows.Forms.TextBox();
            this.textBoxOilMileage = new System.Windows.Forms.TextBox();
            this.textBoxEngineDictionary = new System.Windows.Forms.TextBox();
            this.textBoxOilDictionary = new System.Windows.Forms.TextBox();
            this.buttonDataOfExp = new System.Windows.Forms.Button();
            this.buttonMakeExp = new System.Windows.Forms.Button();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectBDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortedByFIOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortedByPhoneNumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.samplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonChangeDataClient = new System.Windows.Forms.Button();
            this.listViewClients = new System.Windows.Forms.ListView();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelClients
            // 
            this.labelClients.AutoSize = true;
            this.labelClients.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClients.Location = new System.Drawing.Point(12, 37);
            this.labelClients.Name = "labelClients";
            this.labelClients.Size = new System.Drawing.Size(66, 22);
            this.labelClients.TabIndex = 3;
            this.labelClients.Text = "Clients";
            this.labelClients.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonAddClient
            // 
            this.buttonAddClient.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddClient.Location = new System.Drawing.Point(12, 384);
            this.buttonAddClient.Name = "buttonAddClient";
            this.buttonAddClient.Size = new System.Drawing.Size(112, 39);
            this.buttonAddClient.TabIndex = 4;
            this.buttonAddClient.Text = "ADD";
            this.buttonAddClient.UseVisualStyleBackColor = true;
            this.buttonAddClient.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonDellClient
            // 
            this.buttonDellClient.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDellClient.Location = new System.Drawing.Point(130, 384);
            this.buttonDellClient.Name = "buttonDellClient";
            this.buttonDellClient.Size = new System.Drawing.Size(112, 39);
            this.buttonDellClient.TabIndex = 5;
            this.buttonDellClient.Text = "DEL";
            this.buttonDellClient.UseVisualStyleBackColor = true;
            this.buttonDellClient.Click += new System.EventHandler(this.buttonDellClient_Click);
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSamples.Location = new System.Drawing.Point(471, 42);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(77, 22);
            this.labelSamples.TabIndex = 7;
            this.labelSamples.Text = "Samples";
            // 
            // buttonAddSamples
            // 
            this.buttonAddSamples.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddSamples.Location = new System.Drawing.Point(474, 384);
            this.buttonAddSamples.Name = "buttonAddSamples";
            this.buttonAddSamples.Size = new System.Drawing.Size(112, 39);
            this.buttonAddSamples.TabIndex = 9;
            this.buttonAddSamples.Text = "ADD";
            this.buttonAddSamples.UseVisualStyleBackColor = true;
            // 
            // buttonDellSamples
            // 
            this.buttonDellSamples.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDellSamples.Location = new System.Drawing.Point(592, 384);
            this.buttonDellSamples.Name = "buttonDellSamples";
            this.buttonDellSamples.Size = new System.Drawing.Size(112, 39);
            this.buttonDellSamples.TabIndex = 10;
            this.buttonDellSamples.Text = "DEL";
            this.buttonDellSamples.UseVisualStyleBackColor = true;
            // 
            // buttonChangeSamples
            // 
            this.buttonChangeSamples.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeSamples.Location = new System.Drawing.Point(710, 384);
            this.buttonChangeSamples.Name = "buttonChangeSamples";
            this.buttonChangeSamples.Size = new System.Drawing.Size(112, 39);
            this.buttonChangeSamples.TabIndex = 11;
            this.buttonChangeSamples.Text = "CHANGE";
            this.buttonChangeSamples.UseVisualStyleBackColor = true;
            // 
            // Note
            // 
            this.Note.AutoSize = true;
            this.Note.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Note.Location = new System.Drawing.Point(867, 68);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(54, 22);
            this.Note.TabIndex = 12;
            this.Note.Text = "Note:";
            this.Note.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Date.Location = new System.Drawing.Point(868, 110);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(53, 22);
            this.Date.TabIndex = 13;
            this.Date.Text = "Date:";
            // 
            // EngineType
            // 
            this.EngineType.AutoSize = true;
            this.EngineType.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineType.Location = new System.Drawing.Point(867, 152);
            this.EngineType.Name = "EngineType";
            this.EngineType.Size = new System.Drawing.Size(114, 22);
            this.EngineType.TabIndex = 14;
            this.EngineType.Text = "Engine Type:";
            // 
            // EngineBrand
            // 
            this.EngineBrand.AutoSize = true;
            this.EngineBrand.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineBrand.Location = new System.Drawing.Point(867, 194);
            this.EngineBrand.Name = "EngineBrand";
            this.EngineBrand.Size = new System.Drawing.Size(123, 22);
            this.EngineBrand.TabIndex = 15;
            this.EngineBrand.Text = "Engine Brand:";
            // 
            // EngineNomber
            // 
            this.EngineNomber.AutoSize = true;
            this.EngineNomber.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineNomber.Location = new System.Drawing.Point(868, 236);
            this.EngineNomber.Name = "EngineNomber";
            this.EngineNomber.Size = new System.Drawing.Size(139, 22);
            this.EngineNomber.TabIndex = 16;
            this.EngineNomber.Text = "Engine Nomber:";
            // 
            // EngineMileage
            // 
            this.EngineMileage.AutoSize = true;
            this.EngineMileage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineMileage.Location = new System.Drawing.Point(867, 278);
            this.EngineMileage.Name = "EngineMileage";
            this.EngineMileage.Size = new System.Drawing.Size(140, 22);
            this.EngineMileage.TabIndex = 17;
            this.EngineMileage.Text = "Engine Mileage:";
            // 
            // OilMileage
            // 
            this.OilMileage.AutoSize = true;
            this.OilMileage.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OilMileage.Location = new System.Drawing.Point(869, 320);
            this.OilMileage.Name = "OilMileage";
            this.OilMileage.Size = new System.Drawing.Size(112, 22);
            this.OilMileage.TabIndex = 18;
            this.OilMileage.Text = "Oil Mileage:";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNote.Location = new System.Drawing.Point(1058, 69);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ReadOnly = true;
            this.textBoxNote.Size = new System.Drawing.Size(220, 22);
            this.textBoxNote.TabIndex = 19;
            // 
            // textBoxData
            // 
            this.textBoxData.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxData.Location = new System.Drawing.Point(1058, 111);
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.ReadOnly = true;
            this.textBoxData.Size = new System.Drawing.Size(220, 22);
            this.textBoxData.TabIndex = 20;
            // 
            // textBoxEngineType
            // 
            this.textBoxEngineType.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineType.Location = new System.Drawing.Point(1058, 153);
            this.textBoxEngineType.Name = "textBoxEngineType";
            this.textBoxEngineType.ReadOnly = true;
            this.textBoxEngineType.Size = new System.Drawing.Size(220, 22);
            this.textBoxEngineType.TabIndex = 21;
            // 
            // textBoxEhgineBrand
            // 
            this.textBoxEhgineBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEhgineBrand.Location = new System.Drawing.Point(1058, 194);
            this.textBoxEhgineBrand.Name = "textBoxEhgineBrand";
            this.textBoxEhgineBrand.ReadOnly = true;
            this.textBoxEhgineBrand.Size = new System.Drawing.Size(220, 22);
            this.textBoxEhgineBrand.TabIndex = 22;
            // 
            // textBoxEngineNomber
            // 
            this.textBoxEngineNomber.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineNomber.Location = new System.Drawing.Point(1058, 237);
            this.textBoxEngineNomber.Name = "textBoxEngineNomber";
            this.textBoxEngineNomber.ReadOnly = true;
            this.textBoxEngineNomber.Size = new System.Drawing.Size(220, 22);
            this.textBoxEngineNomber.TabIndex = 23;
            // 
            // textBoxEngineMileage
            // 
            this.textBoxEngineMileage.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineMileage.Location = new System.Drawing.Point(1058, 279);
            this.textBoxEngineMileage.Name = "textBoxEngineMileage";
            this.textBoxEngineMileage.ReadOnly = true;
            this.textBoxEngineMileage.Size = new System.Drawing.Size(100, 22);
            this.textBoxEngineMileage.TabIndex = 24;
            // 
            // textBoxOilMileage
            // 
            this.textBoxOilMileage.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxOilMileage.Location = new System.Drawing.Point(1058, 321);
            this.textBoxOilMileage.Name = "textBoxOilMileage";
            this.textBoxOilMileage.ReadOnly = true;
            this.textBoxOilMileage.Size = new System.Drawing.Size(100, 22);
            this.textBoxOilMileage.TabIndex = 25;
            // 
            // textBoxEngineDictionary
            // 
            this.textBoxEngineDictionary.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineDictionary.Location = new System.Drawing.Point(1178, 278);
            this.textBoxEngineDictionary.Name = "textBoxEngineDictionary";
            this.textBoxEngineDictionary.ReadOnly = true;
            this.textBoxEngineDictionary.Size = new System.Drawing.Size(100, 22);
            this.textBoxEngineDictionary.TabIndex = 26;
            // 
            // textBoxOilDictionary
            // 
            this.textBoxOilDictionary.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxOilDictionary.Location = new System.Drawing.Point(1178, 321);
            this.textBoxOilDictionary.Name = "textBoxOilDictionary";
            this.textBoxOilDictionary.ReadOnly = true;
            this.textBoxOilDictionary.Size = new System.Drawing.Size(100, 22);
            this.textBoxOilDictionary.TabIndex = 27;
            // 
            // buttonDataOfExp
            // 
            this.buttonDataOfExp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDataOfExp.Location = new System.Drawing.Point(832, 384);
            this.buttonDataOfExp.Name = "buttonDataOfExp";
            this.buttonDataOfExp.Size = new System.Drawing.Size(220, 39);
            this.buttonDataOfExp.TabIndex = 28;
            this.buttonDataOfExp.Text = "DATA";
            this.buttonDataOfExp.UseVisualStyleBackColor = true;
            this.buttonDataOfExp.Click += new System.EventHandler(this.buttonDataOfExp_Click);
            // 
            // buttonMakeExp
            // 
            this.buttonMakeExp.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMakeExp.Location = new System.Drawing.Point(1058, 384);
            this.buttonMakeExp.Name = "buttonMakeExp";
            this.buttonMakeExp.Size = new System.Drawing.Size(220, 39);
            this.buttonMakeExp.TabIndex = 29;
            this.buttonMakeExp.Text = "RUN";
            this.buttonMakeExp.UseVisualStyleBackColor = true;
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1305, 28);
            this.menuStrip2.TabIndex = 31;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectBDToolStripMenuItem,
            this.selectCOMPortToolStripMenuItem,
            this.sortToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // selectBDToolStripMenuItem
            // 
            this.selectBDToolStripMenuItem.Name = "selectBDToolStripMenuItem";
            this.selectBDToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.selectBDToolStripMenuItem.Text = "Select BD";
            this.selectBDToolStripMenuItem.Click += new System.EventHandler(this.selectBDToolStripMenuItem_Click);
            // 
            // selectCOMPortToolStripMenuItem
            // 
            this.selectCOMPortToolStripMenuItem.Name = "selectCOMPortToolStripMenuItem";
            this.selectCOMPortToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.selectCOMPortToolStripMenuItem.Text = "Select COM port";
            this.selectCOMPortToolStripMenuItem.Click += new System.EventHandler(this.selectCOMPortToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientsToolStripMenuItem,
            this.samplesToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // clientsToolStripMenuItem
            // 
            this.clientsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortedByFIOToolStripMenuItem,
            this.sortedByPhoneNumToolStripMenuItem});
            this.clientsToolStripMenuItem.Name = "clientsToolStripMenuItem";
            this.clientsToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.clientsToolStripMenuItem.Text = "Clients";
            // 
            // sortedByFIOToolStripMenuItem
            // 
            this.sortedByFIOToolStripMenuItem.Name = "sortedByFIOToolStripMenuItem";
            this.sortedByFIOToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
            this.sortedByFIOToolStripMenuItem.Text = "Sorted by FIO";
            this.sortedByFIOToolStripMenuItem.Click += new System.EventHandler(this.sortedByFIOToolStripMenuItem_Click);
            // 
            // sortedByPhoneNumToolStripMenuItem
            // 
            this.sortedByPhoneNumToolStripMenuItem.Name = "sortedByPhoneNumToolStripMenuItem";
            this.sortedByPhoneNumToolStripMenuItem.Size = new System.Drawing.Size(235, 26);
            this.sortedByPhoneNumToolStripMenuItem.Text = "Sorted by phone num";
            this.sortedByPhoneNumToolStripMenuItem.Click += new System.EventHandler(this.sortedByPhoneNumToolStripMenuItem_Click);
            // 
            // samplesToolStripMenuItem
            // 
            this.samplesToolStripMenuItem.Name = "samplesToolStripMenuItem";
            this.samplesToolStripMenuItem.Size = new System.Drawing.Size(148, 26);
            this.samplesToolStripMenuItem.Text = "Samples";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // buttonChangeDataClient
            // 
            this.buttonChangeDataClient.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeDataClient.Location = new System.Drawing.Point(248, 384);
            this.buttonChangeDataClient.Name = "buttonChangeDataClient";
            this.buttonChangeDataClient.Size = new System.Drawing.Size(112, 39);
            this.buttonChangeDataClient.TabIndex = 34;
            this.buttonChangeDataClient.Text = "CHANGE";
            this.buttonChangeDataClient.UseVisualStyleBackColor = true;
            this.buttonChangeDataClient.Click += new System.EventHandler(this.buttonChangeDataClient_Click);
            // 
            // listViewClients
            // 
            this.listViewClients.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewClients.FullRowSelect = true;
            this.listViewClients.GridLines = true;
            this.listViewClients.HideSelection = false;
            this.listViewClients.Location = new System.Drawing.Point(12, 68);
            this.listViewClients.Name = "listViewClients";
            this.listViewClients.Size = new System.Drawing.Size(348, 308);
            this.listViewClients.TabIndex = 35;
            this.listViewClients.UseCompatibleStateImageBehavior = false;
            this.listViewClients.View = System.Windows.Forms.View.Details;
            this.listViewClients.SelectedIndexChanged += new System.EventHandler(this.listViewClients_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1305, 528);
            this.Controls.Add(this.listViewClients);
            this.Controls.Add(this.buttonChangeDataClient);
            this.Controls.Add(this.buttonMakeExp);
            this.Controls.Add(this.buttonDataOfExp);
            this.Controls.Add(this.textBoxOilDictionary);
            this.Controls.Add(this.textBoxEngineDictionary);
            this.Controls.Add(this.textBoxOilMileage);
            this.Controls.Add(this.textBoxEngineMileage);
            this.Controls.Add(this.textBoxEngineNomber);
            this.Controls.Add(this.textBoxEhgineBrand);
            this.Controls.Add(this.textBoxEngineType);
            this.Controls.Add(this.textBoxData);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.OilMileage);
            this.Controls.Add(this.EngineMileage);
            this.Controls.Add(this.EngineNomber);
            this.Controls.Add(this.EngineBrand);
            this.Controls.Add(this.EngineType);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.Note);
            this.Controls.Add(this.buttonChangeSamples);
            this.Controls.Add(this.buttonDellSamples);
            this.Controls.Add(this.buttonAddSamples);
            this.Controls.Add(this.labelSamples);
            this.Controls.Add(this.buttonDellClient);
            this.Controls.Add(this.buttonAddClient);
            this.Controls.Add(this.labelClients);
            this.Controls.Add(this.menuStrip2);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label labelClients;
        private System.Windows.Forms.Button buttonAddClient;
        private System.Windows.Forms.Button buttonDellClient;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.Button buttonAddSamples;
        private System.Windows.Forms.Button buttonDellSamples;
        private System.Windows.Forms.Button buttonChangeSamples;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.Label EngineType;
        private System.Windows.Forms.Label EngineBrand;
        private System.Windows.Forms.Label EngineNomber;
        private System.Windows.Forms.Label EngineMileage;
        private System.Windows.Forms.Label OilMileage;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.TextBox textBoxEngineType;
        private System.Windows.Forms.TextBox textBoxEhgineBrand;
        private System.Windows.Forms.TextBox textBoxEngineNomber;
        private System.Windows.Forms.TextBox textBoxEngineMileage;
        private System.Windows.Forms.TextBox textBoxOilMileage;
        private System.Windows.Forms.TextBox textBoxEngineDictionary;
        private System.Windows.Forms.TextBox textBoxOilDictionary;
        private System.Windows.Forms.Button buttonDataOfExp;
        private System.Windows.Forms.Button buttonMakeExp;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectBDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCOMPortToolStripMenuItem;
        private System.Windows.Forms.Button buttonChangeDataClient;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortedByFIOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortedByPhoneNumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem samplesToolStripMenuItem;
        private System.Windows.Forms.ListView listViewClients;
    }
}

