﻿namespace Diplom_project
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
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
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectBDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCOMPortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortedByFIOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortedByPhoneNumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.samplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSortNote = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuSortDatetime = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByDateTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByErrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonChangeDataClient = new System.Windows.Forms.Button();
            this.listViewClients = new System.Windows.Forms.ListView();
            this.listViewSamples = new System.Windows.Forms.ListView();
            this.listViewExperiments = new System.Windows.Forms.ListView();
            this.buttonDelExp = new System.Windows.Forms.Button();
            this.buttonAddExp = new System.Windows.Forms.Button();
            this.buttonStartExperiment = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDataExp = new System.Windows.Forms.Button();
            this.creatNewDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelClients
            // 
            this.labelClients.AutoSize = true;
            this.labelClients.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelClients.Location = new System.Drawing.Point(12, 39);
            this.labelClients.Name = "labelClients";
            this.labelClients.Size = new System.Drawing.Size(68, 27);
            this.labelClients.TabIndex = 3;
            this.labelClients.Text = "Clients";
            this.labelClients.Click += new System.EventHandler(this.label1_Click);
            // 
            // buttonAddClient
            // 
            this.buttonAddClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddClient.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddClient.Location = new System.Drawing.Point(12, 745);
            this.buttonAddClient.Name = "buttonAddClient";
            this.buttonAddClient.Size = new System.Drawing.Size(136, 39);
            this.buttonAddClient.TabIndex = 4;
            this.buttonAddClient.TabStop = false;
            this.buttonAddClient.Text = "ADD";
            this.buttonAddClient.UseVisualStyleBackColor = true;
            this.buttonAddClient.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonDellClient
            // 
            this.buttonDellClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDellClient.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDellClient.Location = new System.Drawing.Point(296, 744);
            this.buttonDellClient.Name = "buttonDellClient";
            this.buttonDellClient.Size = new System.Drawing.Size(136, 39);
            this.buttonDellClient.TabIndex = 5;
            this.buttonDellClient.TabStop = false;
            this.buttonDellClient.Text = "DEL";
            this.buttonDellClient.UseVisualStyleBackColor = true;
            this.buttonDellClient.Click += new System.EventHandler(this.buttonDellClient_Click);
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSamples.Location = new System.Drawing.Point(448, 39);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(84, 27);
            this.labelSamples.TabIndex = 7;
            this.labelSamples.Text = "Samples";
            // 
            // buttonAddSamples
            // 
            this.buttonAddSamples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddSamples.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddSamples.Location = new System.Drawing.Point(453, 364);
            this.buttonAddSamples.Name = "buttonAddSamples";
            this.buttonAddSamples.Size = new System.Drawing.Size(136, 39);
            this.buttonAddSamples.TabIndex = 9;
            this.buttonAddSamples.TabStop = false;
            this.buttonAddSamples.Text = "ADD";
            this.buttonAddSamples.UseVisualStyleBackColor = true;
            this.buttonAddSamples.Click += new System.EventHandler(this.buttonAddSamples_Click);
            // 
            // buttonDellSamples
            // 
            this.buttonDellSamples.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonDellSamples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDellSamples.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDellSamples.Location = new System.Drawing.Point(737, 364);
            this.buttonDellSamples.Name = "buttonDellSamples";
            this.buttonDellSamples.Size = new System.Drawing.Size(136, 39);
            this.buttonDellSamples.TabIndex = 10;
            this.buttonDellSamples.TabStop = false;
            this.buttonDellSamples.Text = "DEL";
            this.buttonDellSamples.UseVisualStyleBackColor = true;
            this.buttonDellSamples.Click += new System.EventHandler(this.buttonDellSamples_Click);
            // 
            // buttonChangeSamples
            // 
            this.buttonChangeSamples.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeSamples.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeSamples.Location = new System.Drawing.Point(595, 364);
            this.buttonChangeSamples.Name = "buttonChangeSamples";
            this.buttonChangeSamples.Size = new System.Drawing.Size(136, 39);
            this.buttonChangeSamples.TabIndex = 11;
            this.buttonChangeSamples.TabStop = false;
            this.buttonChangeSamples.Text = "CHANGE";
            this.buttonChangeSamples.UseVisualStyleBackColor = true;
            this.buttonChangeSamples.Click += new System.EventHandler(this.buttonChangeSamples_Click);
            // 
            // Note
            // 
            this.Note.AutoSize = true;
            this.Note.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Note.Location = new System.Drawing.Point(909, 68);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(56, 27);
            this.Note.TabIndex = 12;
            this.Note.Text = "Note:";
            this.Note.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // Date
            // 
            this.Date.AutoSize = true;
            this.Date.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Date.Location = new System.Drawing.Point(909, 141);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(205, 27);
            this.Date.TabIndex = 13;
            this.Date.Text = "Date of register sample:";
            // 
            // EngineType
            // 
            this.EngineType.AutoSize = true;
            this.EngineType.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineType.Location = new System.Drawing.Point(909, 213);
            this.EngineType.Name = "EngineType";
            this.EngineType.Size = new System.Drawing.Size(120, 27);
            this.EngineType.TabIndex = 14;
            this.EngineType.Text = "Engine Type:";
            // 
            // EngineBrand
            // 
            this.EngineBrand.AutoSize = true;
            this.EngineBrand.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineBrand.Location = new System.Drawing.Point(912, 291);
            this.EngineBrand.Name = "EngineBrand";
            this.EngineBrand.Size = new System.Drawing.Size(128, 27);
            this.EngineBrand.TabIndex = 15;
            this.EngineBrand.Text = "Engine Brand:";
            // 
            // EngineNomber
            // 
            this.EngineNomber.AutoSize = true;
            this.EngineNomber.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineNomber.Location = new System.Drawing.Point(913, 376);
            this.EngineNomber.Name = "EngineNomber";
            this.EngineNomber.Size = new System.Drawing.Size(145, 27);
            this.EngineNomber.TabIndex = 16;
            this.EngineNomber.Text = "Engine Nomber:";
            // 
            // EngineMileage
            // 
            this.EngineMileage.AutoSize = true;
            this.EngineMileage.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EngineMileage.Location = new System.Drawing.Point(913, 448);
            this.EngineMileage.Name = "EngineMileage";
            this.EngineMileage.Size = new System.Drawing.Size(144, 27);
            this.EngineMileage.TabIndex = 17;
            this.EngineMileage.Text = "Engine Mileage:";
            // 
            // OilMileage
            // 
            this.OilMileage.AutoSize = true;
            this.OilMileage.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OilMileage.Location = new System.Drawing.Point(913, 525);
            this.OilMileage.Name = "OilMileage";
            this.OilMileage.Size = new System.Drawing.Size(110, 27);
            this.OilMileage.TabIndex = 18;
            this.OilMileage.Text = "Oil Mileage:";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxNote.Location = new System.Drawing.Point(915, 97);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ReadOnly = true;
            this.textBoxNote.Size = new System.Drawing.Size(282, 27);
            this.textBoxNote.TabIndex = 19;
            this.textBoxNote.TabStop = false;
            // 
            // textBoxData
            // 
            this.textBoxData.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxData.Location = new System.Drawing.Point(915, 171);
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.ReadOnly = true;
            this.textBoxData.Size = new System.Drawing.Size(282, 27);
            this.textBoxData.TabIndex = 20;
            this.textBoxData.TabStop = false;
            // 
            // textBoxEngineType
            // 
            this.textBoxEngineType.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineType.Location = new System.Drawing.Point(914, 243);
            this.textBoxEngineType.Name = "textBoxEngineType";
            this.textBoxEngineType.ReadOnly = true;
            this.textBoxEngineType.Size = new System.Drawing.Size(282, 27);
            this.textBoxEngineType.TabIndex = 21;
            this.textBoxEngineType.TabStop = false;
            // 
            // textBoxEhgineBrand
            // 
            this.textBoxEhgineBrand.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEhgineBrand.Location = new System.Drawing.Point(917, 320);
            this.textBoxEhgineBrand.Name = "textBoxEhgineBrand";
            this.textBoxEhgineBrand.ReadOnly = true;
            this.textBoxEhgineBrand.Size = new System.Drawing.Size(282, 27);
            this.textBoxEhgineBrand.TabIndex = 22;
            this.textBoxEhgineBrand.TabStop = false;
            // 
            // textBoxEngineNomber
            // 
            this.textBoxEngineNomber.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineNomber.Location = new System.Drawing.Point(918, 406);
            this.textBoxEngineNomber.Name = "textBoxEngineNomber";
            this.textBoxEngineNomber.ReadOnly = true;
            this.textBoxEngineNomber.Size = new System.Drawing.Size(282, 27);
            this.textBoxEngineNomber.TabIndex = 23;
            this.textBoxEngineNomber.TabStop = false;
            // 
            // textBoxEngineMileage
            // 
            this.textBoxEngineMileage.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineMileage.Location = new System.Drawing.Point(918, 480);
            this.textBoxEngineMileage.Name = "textBoxEngineMileage";
            this.textBoxEngineMileage.ReadOnly = true;
            this.textBoxEngineMileage.Size = new System.Drawing.Size(169, 27);
            this.textBoxEngineMileage.TabIndex = 24;
            this.textBoxEngineMileage.TabStop = false;
            // 
            // textBoxOilMileage
            // 
            this.textBoxOilMileage.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxOilMileage.Location = new System.Drawing.Point(918, 555);
            this.textBoxOilMileage.Name = "textBoxOilMileage";
            this.textBoxOilMileage.ReadOnly = true;
            this.textBoxOilMileage.Size = new System.Drawing.Size(169, 27);
            this.textBoxOilMileage.TabIndex = 25;
            this.textBoxOilMileage.TabStop = false;
            // 
            // textBoxEngineDictionary
            // 
            this.textBoxEngineDictionary.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxEngineDictionary.Location = new System.Drawing.Point(1106, 480);
            this.textBoxEngineDictionary.Name = "textBoxEngineDictionary";
            this.textBoxEngineDictionary.ReadOnly = true;
            this.textBoxEngineDictionary.Size = new System.Drawing.Size(94, 27);
            this.textBoxEngineDictionary.TabIndex = 26;
            this.textBoxEngineDictionary.TabStop = false;
            // 
            // textBoxOilDictionary
            // 
            this.textBoxOilDictionary.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxOilDictionary.Location = new System.Drawing.Point(1106, 555);
            this.textBoxOilDictionary.Name = "textBoxOilDictionary";
            this.textBoxOilDictionary.ReadOnly = true;
            this.textBoxOilDictionary.Size = new System.Drawing.Size(94, 27);
            this.textBoxOilDictionary.TabIndex = 27;
            this.textBoxOilDictionary.TabStop = false;
            // 
            // menuStrip2
            // 
            this.menuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1286, 28);
            this.menuStrip2.TabIndex = 31;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.creatNewDBToolStripMenuItem,
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
            this.selectBDToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.selectBDToolStripMenuItem.Text = "Select BD";
            this.selectBDToolStripMenuItem.Click += new System.EventHandler(this.selectBDToolStripMenuItem_Click);
            // 
            // selectCOMPortToolStripMenuItem
            // 
            this.selectCOMPortToolStripMenuItem.Name = "selectCOMPortToolStripMenuItem";
            this.selectCOMPortToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.selectCOMPortToolStripMenuItem.Text = "Select COM port";
            this.selectCOMPortToolStripMenuItem.Click += new System.EventHandler(this.selectCOMPortToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientsToolStripMenuItem,
            this.samplesToolStripMenuItem,
            this.experimentsToolStripMenuItem});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.sortToolStripMenuItem.Text = "Sort";
            this.sortToolStripMenuItem.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
            // 
            // clientsToolStripMenuItem
            // 
            this.clientsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortedByFIOToolStripMenuItem,
            this.sortedByPhoneNumToolStripMenuItem});
            this.clientsToolStripMenuItem.Name = "clientsToolStripMenuItem";
            this.clientsToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.clientsToolStripMenuItem.Text = "Clients";
            this.clientsToolStripMenuItem.Click += new System.EventHandler(this.clientsToolStripMenuItem_Click);
            // 
            // sortedByFIOToolStripMenuItem
            // 
            this.sortedByFIOToolStripMenuItem.Name = "sortedByFIOToolStripMenuItem";
            this.sortedByFIOToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.sortedByFIOToolStripMenuItem.Text = "Sorted by FIO";
            this.sortedByFIOToolStripMenuItem.Click += new System.EventHandler(this.sortedByFIOToolStripMenuItem_Click);
            // 
            // sortedByPhoneNumToolStripMenuItem
            // 
            this.sortedByPhoneNumToolStripMenuItem.Name = "sortedByPhoneNumToolStripMenuItem";
            this.sortedByPhoneNumToolStripMenuItem.Size = new System.Drawing.Size(234, 26);
            this.sortedByPhoneNumToolStripMenuItem.Text = "Sorted by Phone num";
            this.sortedByPhoneNumToolStripMenuItem.Click += new System.EventHandler(this.sortedByPhoneNumToolStripMenuItem_Click);
            // 
            // samplesToolStripMenuItem
            // 
            this.samplesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSortNote,
            this.toolStripMenuSortDatetime});
            this.samplesToolStripMenuItem.Name = "samplesToolStripMenuItem";
            this.samplesToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.samplesToolStripMenuItem.Text = "Samples";
            // 
            // toolStripMenuItemSortNote
            // 
            this.toolStripMenuItemSortNote.Name = "toolStripMenuItemSortNote";
            this.toolStripMenuItemSortNote.Size = new System.Drawing.Size(214, 26);
            this.toolStripMenuItemSortNote.Text = "Sort by Note";
            this.toolStripMenuItemSortNote.Click += new System.EventHandler(this.toolStripMenuItemSortNote_Click);
            // 
            // toolStripMenuSortDatetime
            // 
            this.toolStripMenuSortDatetime.Name = "toolStripMenuSortDatetime";
            this.toolStripMenuSortDatetime.Size = new System.Drawing.Size(214, 26);
            this.toolStripMenuSortDatetime.Text = "Sort by Date/Time";
            this.toolStripMenuSortDatetime.Click += new System.EventHandler(this.toolStripMenuSortDatetime_Click);
            // 
            // experimentsToolStripMenuItem
            // 
            this.experimentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortByToolStripMenuItem,
            this.sortByDateTimeToolStripMenuItem,
            this.sortByErrorToolStripMenuItem});
            this.experimentsToolStripMenuItem.Name = "experimentsToolStripMenuItem";
            this.experimentsToolStripMenuItem.Size = new System.Drawing.Size(173, 26);
            this.experimentsToolStripMenuItem.Text = "Experiments";
            // 
            // sortByToolStripMenuItem
            // 
            this.sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
            this.sortByToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.sortByToolStripMenuItem.Text = "Sort by Num. Exp";
            this.sortByToolStripMenuItem.Click += new System.EventHandler(this.sortByToolStripMenuItem_Click);
            // 
            // sortByDateTimeToolStripMenuItem
            // 
            this.sortByDateTimeToolStripMenuItem.Name = "sortByDateTimeToolStripMenuItem";
            this.sortByDateTimeToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.sortByDateTimeToolStripMenuItem.Text = "Sort by Date/Time";
            this.sortByDateTimeToolStripMenuItem.Click += new System.EventHandler(this.sortByDateTimeToolStripMenuItem_Click);
            // 
            // sortByErrorToolStripMenuItem
            // 
            this.sortByErrorToolStripMenuItem.Name = "sortByErrorToolStripMenuItem";
            this.sortByErrorToolStripMenuItem.Size = new System.Drawing.Size(214, 26);
            this.sortByErrorToolStripMenuItem.Text = "Sort by Error";
            this.sortByErrorToolStripMenuItem.Click += new System.EventHandler(this.sortByErrorToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // buttonChangeDataClient
            // 
            this.buttonChangeDataClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChangeDataClient.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonChangeDataClient.Location = new System.Drawing.Point(154, 744);
            this.buttonChangeDataClient.Name = "buttonChangeDataClient";
            this.buttonChangeDataClient.Size = new System.Drawing.Size(136, 39);
            this.buttonChangeDataClient.TabIndex = 34;
            this.buttonChangeDataClient.TabStop = false;
            this.buttonChangeDataClient.Text = "CHANGE";
            this.buttonChangeDataClient.UseVisualStyleBackColor = true;
            this.buttonChangeDataClient.Click += new System.EventHandler(this.buttonChangeDataClient_Click);
            // 
            // listViewClients
            // 
            this.listViewClients.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewClients.FullRowSelect = true;
            this.listViewClients.GridLines = true;
            this.listViewClients.HideSelection = false;
            this.listViewClients.Location = new System.Drawing.Point(12, 68);
            this.listViewClients.Name = "listViewClients";
            this.listViewClients.Size = new System.Drawing.Size(420, 670);
            this.listViewClients.TabIndex = 35;
            this.listViewClients.UseCompatibleStateImageBehavior = false;
            this.listViewClients.View = System.Windows.Forms.View.Details;
            this.listViewClients.SelectedIndexChanged += new System.EventHandler(this.listViewClients_SelectedIndexChanged);
            // 
            // listViewSamples
            // 
            this.listViewSamples.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewSamples.FullRowSelect = true;
            this.listViewSamples.GridLines = true;
            this.listViewSamples.HideSelection = false;
            this.listViewSamples.Location = new System.Drawing.Point(453, 68);
            this.listViewSamples.Name = "listViewSamples";
            this.listViewSamples.Size = new System.Drawing.Size(420, 290);
            this.listViewSamples.TabIndex = 36;
            this.listViewSamples.UseCompatibleStateImageBehavior = false;
            this.listViewSamples.View = System.Windows.Forms.View.Details;
            this.listViewSamples.SelectedIndexChanged += new System.EventHandler(this.listViewSamples_SelectedIndexChanged);
            // 
            // listViewExperiments
            // 
            this.listViewExperiments.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewExperiments.FullRowSelect = true;
            this.listViewExperiments.GridLines = true;
            this.listViewExperiments.HideSelection = false;
            this.listViewExperiments.Location = new System.Drawing.Point(453, 448);
            this.listViewExperiments.Name = "listViewExperiments";
            this.listViewExperiments.Size = new System.Drawing.Size(420, 290);
            this.listViewExperiments.TabIndex = 37;
            this.listViewExperiments.UseCompatibleStateImageBehavior = false;
            this.listViewExperiments.View = System.Windows.Forms.View.Details;
            this.listViewExperiments.SelectedIndexChanged += new System.EventHandler(this.listViewExperiments_SelectedIndexChanged);
            // 
            // buttonDelExp
            // 
            this.buttonDelExp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDelExp.Location = new System.Drawing.Point(664, 744);
            this.buttonDelExp.Name = "buttonDelExp";
            this.buttonDelExp.Size = new System.Drawing.Size(209, 39);
            this.buttonDelExp.TabIndex = 39;
            this.buttonDelExp.TabStop = false;
            this.buttonDelExp.Text = "DEL";
            this.buttonDelExp.UseVisualStyleBackColor = true;
            this.buttonDelExp.Click += new System.EventHandler(this.buttonDelExp_Click);
            // 
            // buttonAddExp
            // 
            this.buttonAddExp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddExp.Location = new System.Drawing.Point(453, 744);
            this.buttonAddExp.Name = "buttonAddExp";
            this.buttonAddExp.Size = new System.Drawing.Size(205, 39);
            this.buttonAddExp.TabIndex = 38;
            this.buttonAddExp.TabStop = false;
            this.buttonAddExp.Text = "ADD";
            this.buttonAddExp.UseVisualStyleBackColor = true;
            this.buttonAddExp.Click += new System.EventHandler(this.buttonAddExp_Click);
            // 
            // buttonStartExperiment
            // 
            this.buttonStartExperiment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStartExperiment.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartExperiment.Location = new System.Drawing.Point(914, 676);
            this.buttonStartExperiment.Name = "buttonStartExperiment";
            this.buttonStartExperiment.Size = new System.Drawing.Size(286, 45);
            this.buttonStartExperiment.TabIndex = 43;
            this.buttonStartExperiment.TabStop = false;
            this.buttonStartExperiment.Text = "START EXPERIMENT";
            this.buttonStartExperiment.UseVisualStyleBackColor = true;
            this.buttonStartExperiment.Click += new System.EventHandler(this.buttonStartExperiment_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(448, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 27);
            this.label1.TabIndex = 44;
            this.label1.Text = "Experiments";
            this.label1.Click += new System.EventHandler(this.label1_Click_2);
            // 
            // buttonDataExp
            // 
            this.buttonDataExp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDataExp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDataExp.Location = new System.Drawing.Point(914, 744);
            this.buttonDataExp.Name = "buttonDataExp";
            this.buttonDataExp.Size = new System.Drawing.Size(286, 39);
            this.buttonDataExp.TabIndex = 45;
            this.buttonDataExp.TabStop = false;
            this.buttonDataExp.Text = "EXP DATA";
            this.buttonDataExp.UseVisualStyleBackColor = true;
            this.buttonDataExp.Click += new System.EventHandler(this.buttonDataExp_Click);
            // 
            // creatNewDBToolStripMenuItem
            // 
            this.creatNewDBToolStripMenuItem.Name = "creatNewDBToolStripMenuItem";
            this.creatNewDBToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.creatNewDBToolStripMenuItem.Text = "Create new DB";
            this.creatNewDBToolStripMenuItem.Click += new System.EventHandler(this.creatNewDBToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1286, 819);
            this.Controls.Add(this.buttonDataExp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStartExperiment);
            this.Controls.Add(this.buttonDelExp);
            this.Controls.Add(this.buttonAddExp);
            this.Controls.Add(this.listViewExperiments);
            this.Controls.Add(this.listViewSamples);
            this.Controls.Add(this.listViewClients);
            this.Controls.Add(this.buttonChangeDataClient);
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
            this.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project";
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
        private System.Windows.Forms.ListView listViewSamples;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSortNote;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuSortDatetime;
        private System.Windows.Forms.ListView listViewExperiments;
        private System.Windows.Forms.Button buttonDelExp;
        private System.Windows.Forms.Button buttonAddExp;
        private System.Windows.Forms.Button buttonStartExperiment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem experimentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByDateTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortByErrorToolStripMenuItem;
        private System.Windows.Forms.Button buttonDataExp;
        private System.Windows.Forms.ToolStripMenuItem creatNewDBToolStripMenuItem;
    }
}

