namespace Remwave.Client
{
    partial class SettingsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBoxLanguage = new System.Windows.Forms.GroupBox();
            this.radioButtonItalian = new System.Windows.Forms.RadioButton();
            this.radioButtonEspanol = new System.Windows.Forms.RadioButton();
            this.radioButtonDeutsch = new System.Windows.Forms.RadioButton();
            this.radioButtonEnglish = new System.Windows.Forms.RadioButton();
            this.groupBoxContactsStores = new System.Windows.Forms.GroupBox();
            this.checkBoxContactsServer = new System.Windows.Forms.CheckBox();
            this.checkBoxContactsLocal = new System.Windows.Forms.CheckBox();
            this.checkBoxContactsOutlook = new System.Windows.Forms.CheckBox();
            this.groupBoxBasicSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableQA = new System.Windows.Forms.CheckBox();
            this.checkBoxConfirmClosing = new System.Windows.Forms.CheckBox();
            this.checkBoxStartup = new System.Windows.Forms.CheckBox();
            this.tabPagePhone = new System.Windows.Forms.TabPage();
            this.checkBoxEnableSipDiagnosticLogging = new System.Windows.Forms.CheckBox();
            this.groupBoxRingTone = new System.Windows.Forms.GroupBox();
            this.buttonRingToneSelect = new System.Windows.Forms.Button();
            this.textBoxRingTonePath = new System.Windows.Forms.TextBox();
            this.radioButtonRingToneCustom = new System.Windows.Forms.RadioButton();
            this.radioButtonRingToneDefault = new System.Windows.Forms.RadioButton();
            this.groupBoxAudioCodecs = new System.Windows.Forms.GroupBox();
            this.buttonRemoveCodec = new System.Windows.Forms.Button();
            this.buttonAddCodec = new System.Windows.Forms.Button();
            this.labelEnabledCodecs = new System.Windows.Forms.Label();
            this.listBoxEnabledCodecs = new System.Windows.Forms.ListBox();
            this.labelAvailableCodecs = new System.Windows.Forms.Label();
            this.listBoxAvailableCodecs = new System.Windows.Forms.ListBox();
            this.groupBoxAudioVolume = new System.Windows.Forms.GroupBox();
            this.labelSpeaker = new System.Windows.Forms.Label();
            this.labelMicrophone = new System.Windows.Forms.Label();
            this.trackBarSpeaker = new System.Windows.Forms.TrackBar();
            this.trackBarMicrophone = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanelBottomButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.openFileDialogRingTone = new System.Windows.Forms.OpenFileDialog();
            this.tabControlMain.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.groupBoxLanguage.SuspendLayout();
            this.groupBoxContactsStores.SuspendLayout();
            this.groupBoxBasicSettings.SuspendLayout();
            this.tabPagePhone.SuspendLayout();
            this.groupBoxRingTone.SuspendLayout();
            this.groupBoxAudioCodecs.SuspendLayout();
            this.groupBoxAudioVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeaker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMicrophone)).BeginInit();
            this.tableLayoutPanelBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGeneral);
            this.tabControlMain.Controls.Add(this.tabPagePhone);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(5, 5);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(382, 416);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.groupBoxLanguage);
            this.tabPageGeneral.Controls.Add(this.groupBoxContactsStores);
            this.tabPageGeneral.Controls.Add(this.groupBoxBasicSettings);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(374, 390);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBoxLanguage
            // 
            this.groupBoxLanguage.Controls.Add(this.radioButtonItalian);
            this.groupBoxLanguage.Controls.Add(this.radioButtonEspanol);
            this.groupBoxLanguage.Controls.Add(this.radioButtonDeutsch);
            this.groupBoxLanguage.Controls.Add(this.radioButtonEnglish);
            this.groupBoxLanguage.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxLanguage.Location = new System.Drawing.Point(3, 183);
            this.groupBoxLanguage.Name = "groupBoxLanguage";
            this.groupBoxLanguage.Size = new System.Drawing.Size(368, 45);
            this.groupBoxLanguage.TabIndex = 2;
            this.groupBoxLanguage.TabStop = false;
            this.groupBoxLanguage.Text = "Language";
            // 
            // radioButtonItalian
            // 
            this.radioButtonItalian.AutoSize = true;
            this.radioButtonItalian.Location = new System.Drawing.Point(212, 19);
            this.radioButtonItalian.Name = "radioButtonItalian";
            this.radioButtonItalian.Size = new System.Drawing.Size(59, 17);
            this.radioButtonItalian.TabIndex = 3;
            this.radioButtonItalian.TabStop = true;
            this.radioButtonItalian.Text = "Italiano";
            this.radioButtonItalian.UseVisualStyleBackColor = true;
            this.radioButtonItalian.CheckedChanged += new System.EventHandler(this.radioButtonItalian_CheckedChanged);
            // 
            // radioButtonEspanol
            // 
            this.radioButtonEspanol.AutoSize = true;
            this.radioButtonEspanol.Location = new System.Drawing.Point(143, 19);
            this.radioButtonEspanol.Name = "radioButtonEspanol";
            this.radioButtonEspanol.Size = new System.Drawing.Size(63, 17);
            this.radioButtonEspanol.TabIndex = 2;
            this.radioButtonEspanol.TabStop = true;
            this.radioButtonEspanol.Text = "Espanol";
            this.radioButtonEspanol.UseVisualStyleBackColor = true;
            this.radioButtonEspanol.CheckedChanged += new System.EventHandler(this.radioButtonEspanol_CheckedChanged);
            // 
            // radioButtonDeutsch
            // 
            this.radioButtonDeutsch.AutoSize = true;
            this.radioButtonDeutsch.Location = new System.Drawing.Point(72, 19);
            this.radioButtonDeutsch.Name = "radioButtonDeutsch";
            this.radioButtonDeutsch.Size = new System.Drawing.Size(65, 17);
            this.radioButtonDeutsch.TabIndex = 1;
            this.radioButtonDeutsch.TabStop = true;
            this.radioButtonDeutsch.Text = "Deutsch";
            this.radioButtonDeutsch.UseVisualStyleBackColor = true;
            this.radioButtonDeutsch.CheckedChanged += new System.EventHandler(this.radioButtonDeutsch_CheckedChanged);
            // 
            // radioButtonEnglish
            // 
            this.radioButtonEnglish.AutoSize = true;
            this.radioButtonEnglish.Location = new System.Drawing.Point(7, 19);
            this.radioButtonEnglish.Name = "radioButtonEnglish";
            this.radioButtonEnglish.Size = new System.Drawing.Size(59, 17);
            this.radioButtonEnglish.TabIndex = 0;
            this.radioButtonEnglish.TabStop = true;
            this.radioButtonEnglish.Text = "English";
            this.radioButtonEnglish.UseVisualStyleBackColor = true;
            this.radioButtonEnglish.CheckedChanged += new System.EventHandler(this.radioButtonEnglish_CheckedChanged);
            // 
            // groupBoxContactsStores
            // 
            this.groupBoxContactsStores.Controls.Add(this.checkBoxContactsServer);
            this.groupBoxContactsStores.Controls.Add(this.checkBoxContactsLocal);
            this.groupBoxContactsStores.Controls.Add(this.checkBoxContactsOutlook);
            this.groupBoxContactsStores.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxContactsStores.Location = new System.Drawing.Point(3, 93);
            this.groupBoxContactsStores.Name = "groupBoxContactsStores";
            this.groupBoxContactsStores.Size = new System.Drawing.Size(368, 90);
            this.groupBoxContactsStores.TabIndex = 1;
            this.groupBoxContactsStores.TabStop = false;
            this.groupBoxContactsStores.Text = "Contacts stores";
            // 
            // checkBoxContactsServer
            // 
            this.checkBoxContactsServer.AutoSize = true;
            this.checkBoxContactsServer.Location = new System.Drawing.Point(7, 42);
            this.checkBoxContactsServer.Name = "checkBoxContactsServer";
            this.checkBoxContactsServer.Size = new System.Drawing.Size(150, 17);
            this.checkBoxContactsServer.TabIndex = 1;
            this.checkBoxContactsServer.Text = "Use remote server storage";
            this.checkBoxContactsServer.UseVisualStyleBackColor = true;
            // 
            // checkBoxContactsLocal
            // 
            this.checkBoxContactsLocal.AutoSize = true;
            this.checkBoxContactsLocal.Checked = true;
            this.checkBoxContactsLocal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxContactsLocal.Enabled = false;
            this.checkBoxContactsLocal.Location = new System.Drawing.Point(7, 65);
            this.checkBoxContactsLocal.Name = "checkBoxContactsLocal";
            this.checkBoxContactsLocal.Size = new System.Drawing.Size(155, 17);
            this.checkBoxContactsLocal.TabIndex = 2;
            this.checkBoxContactsLocal.Text = "Use local computer storage";
            this.checkBoxContactsLocal.UseVisualStyleBackColor = true;
            // 
            // checkBoxContactsOutlook
            // 
            this.checkBoxContactsOutlook.AutoSize = true;
            this.checkBoxContactsOutlook.Location = new System.Drawing.Point(7, 19);
            this.checkBoxContactsOutlook.Name = "checkBoxContactsOutlook";
            this.checkBoxContactsOutlook.Size = new System.Drawing.Size(129, 17);
            this.checkBoxContactsOutlook.TabIndex = 0;
            this.checkBoxContactsOutlook.Text = "Use Outlook contacts";
            this.checkBoxContactsOutlook.UseVisualStyleBackColor = true;
            // 
            // groupBoxBasicSettings
            // 
            this.groupBoxBasicSettings.Controls.Add(this.checkBoxEnableQA);
            this.groupBoxBasicSettings.Controls.Add(this.checkBoxConfirmClosing);
            this.groupBoxBasicSettings.Controls.Add(this.checkBoxStartup);
            this.groupBoxBasicSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxBasicSettings.Location = new System.Drawing.Point(3, 3);
            this.groupBoxBasicSettings.Name = "groupBoxBasicSettings";
            this.groupBoxBasicSettings.Size = new System.Drawing.Size(368, 90);
            this.groupBoxBasicSettings.TabIndex = 0;
            this.groupBoxBasicSettings.TabStop = false;
            this.groupBoxBasicSettings.Text = "Basic settings";
            // 
            // checkBoxEnableQA
            // 
            this.checkBoxEnableQA.AutoSize = true;
            this.checkBoxEnableQA.Location = new System.Drawing.Point(7, 66);
            this.checkBoxEnableQA.Name = "checkBoxEnableQA";
            this.checkBoxEnableQA.Size = new System.Drawing.Size(169, 17);
            this.checkBoxEnableQA.TabIndex = 2;
            this.checkBoxEnableQA.Text = "Enable Quality Agent reporting";
            this.checkBoxEnableQA.UseVisualStyleBackColor = true;
            // 
            // checkBoxConfirmClosing
            // 
            this.checkBoxConfirmClosing.AutoSize = true;
            this.checkBoxConfirmClosing.Location = new System.Drawing.Point(7, 43);
            this.checkBoxConfirmClosing.Name = "checkBoxConfirmClosing";
            this.checkBoxConfirmClosing.Size = new System.Drawing.Size(130, 17);
            this.checkBoxConfirmClosing.TabIndex = 1;
            this.checkBoxConfirmClosing.Text = "Confirm before closing";
            this.checkBoxConfirmClosing.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartup
            // 
            this.checkBoxStartup.AutoSize = true;
            this.checkBoxStartup.Location = new System.Drawing.Point(7, 20);
            this.checkBoxStartup.Name = "checkBoxStartup";
            this.checkBoxStartup.Size = new System.Drawing.Size(166, 17);
            this.checkBoxStartup.TabIndex = 0;
            this.checkBoxStartup.Text = "Launch when Windows starts";
            this.checkBoxStartup.UseVisualStyleBackColor = true;
            // 
            // tabPagePhone
            // 
            this.tabPagePhone.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPagePhone.Controls.Add(this.checkBoxEnableSipDiagnosticLogging);
            this.tabPagePhone.Controls.Add(this.groupBoxRingTone);
            this.tabPagePhone.Controls.Add(this.groupBoxAudioCodecs);
            this.tabPagePhone.Controls.Add(this.groupBoxAudioVolume);
            this.tabPagePhone.Location = new System.Drawing.Point(4, 22);
            this.tabPagePhone.Name = "tabPagePhone";
            this.tabPagePhone.Padding = new System.Windows.Forms.Padding(10);
            this.tabPagePhone.Size = new System.Drawing.Size(374, 390);
            this.tabPagePhone.TabIndex = 1;
            this.tabPagePhone.Text = "Phone";
            // 
            // checkBoxEnableSipDiagnosticLogging
            // 
            this.checkBoxEnableSipDiagnosticLogging.AutoSize = true;
            this.checkBoxEnableSipDiagnosticLogging.Location = new System.Drawing.Point(18, 331);
            this.checkBoxEnableSipDiagnosticLogging.Name = "checkBoxEnableSipDiagnosticLogging";
            this.checkBoxEnableSipDiagnosticLogging.Size = new System.Drawing.Size(167, 17);
            this.checkBoxEnableSipDiagnosticLogging.TabIndex = 0;
            this.checkBoxEnableSipDiagnosticLogging.Text = "Enable SIP diagnostic logging";
            this.checkBoxEnableSipDiagnosticLogging.UseVisualStyleBackColor = true;
            // 
            // groupBoxRingTone
            // 
            this.groupBoxRingTone.Controls.Add(this.buttonRingToneSelect);
            this.groupBoxRingTone.Controls.Add(this.textBoxRingTonePath);
            this.groupBoxRingTone.Controls.Add(this.radioButtonRingToneCustom);
            this.groupBoxRingTone.Controls.Add(this.radioButtonRingToneDefault);
            this.groupBoxRingTone.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxRingTone.Location = new System.Drawing.Point(10, 210);
            this.groupBoxRingTone.Name = "groupBoxRingTone";
            this.groupBoxRingTone.Size = new System.Drawing.Size(354, 80);
            this.groupBoxRingTone.TabIndex = 2;
            this.groupBoxRingTone.TabStop = false;
            this.groupBoxRingTone.Text = "Ring Tone";
            // 
            // buttonRingToneSelect
            // 
            this.buttonRingToneSelect.Location = new System.Drawing.Point(243, 50);
            this.buttonRingToneSelect.Name = "buttonRingToneSelect";
            this.buttonRingToneSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonRingToneSelect.TabIndex = 1;
            this.buttonRingToneSelect.Text = "Select";
            this.buttonRingToneSelect.UseVisualStyleBackColor = true;
            this.buttonRingToneSelect.Click += new System.EventHandler(this.buttonRingToneSelect_Click);
            // 
            // textBoxRingTonePath
            // 
            this.textBoxRingTonePath.Location = new System.Drawing.Point(86, 52);
            this.textBoxRingTonePath.Name = "textBoxRingTonePath";
            this.textBoxRingTonePath.ReadOnly = true;
            this.textBoxRingTonePath.Size = new System.Drawing.Size(151, 20);
            this.textBoxRingTonePath.TabIndex = 0;
            // 
            // radioButtonRingToneCustom
            // 
            this.radioButtonRingToneCustom.AutoSize = true;
            this.radioButtonRingToneCustom.Location = new System.Drawing.Point(19, 52);
            this.radioButtonRingToneCustom.Name = "radioButtonRingToneCustom";
            this.radioButtonRingToneCustom.Size = new System.Drawing.Size(60, 17);
            this.radioButtonRingToneCustom.TabIndex = 1;
            this.radioButtonRingToneCustom.TabStop = true;
            this.radioButtonRingToneCustom.Text = "Custom";
            this.radioButtonRingToneCustom.UseVisualStyleBackColor = true;
            this.radioButtonRingToneCustom.CheckedChanged += new System.EventHandler(this.radioButtonRingToneCustom_CheckedChanged);
            // 
            // radioButtonRingToneDefault
            // 
            this.radioButtonRingToneDefault.AutoSize = true;
            this.radioButtonRingToneDefault.Location = new System.Drawing.Point(19, 29);
            this.radioButtonRingToneDefault.Name = "radioButtonRingToneDefault";
            this.radioButtonRingToneDefault.Size = new System.Drawing.Size(59, 17);
            this.radioButtonRingToneDefault.TabIndex = 0;
            this.radioButtonRingToneDefault.TabStop = true;
            this.radioButtonRingToneDefault.Text = "Default";
            this.radioButtonRingToneDefault.UseVisualStyleBackColor = true;
            this.radioButtonRingToneDefault.CheckedChanged += new System.EventHandler(this.radioButtonRingToneDefault_CheckedChanged);
            // 
            // groupBoxAudioCodecs
            // 
            this.groupBoxAudioCodecs.Controls.Add(this.buttonRemoveCodec);
            this.groupBoxAudioCodecs.Controls.Add(this.buttonAddCodec);
            this.groupBoxAudioCodecs.Controls.Add(this.labelEnabledCodecs);
            this.groupBoxAudioCodecs.Controls.Add(this.listBoxEnabledCodecs);
            this.groupBoxAudioCodecs.Controls.Add(this.labelAvailableCodecs);
            this.groupBoxAudioCodecs.Controls.Add(this.listBoxAvailableCodecs);
            this.groupBoxAudioCodecs.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAudioCodecs.Location = new System.Drawing.Point(10, 110);
            this.groupBoxAudioCodecs.Name = "groupBoxAudioCodecs";
            this.groupBoxAudioCodecs.Size = new System.Drawing.Size(354, 100);
            this.groupBoxAudioCodecs.TabIndex = 1;
            this.groupBoxAudioCodecs.TabStop = false;
            this.groupBoxAudioCodecs.Text = "Audio Codecs";
            // 
            // buttonRemoveCodec
            // 
            this.buttonRemoveCodec.Location = new System.Drawing.Point(154, 71);
            this.buttonRemoveCodec.Name = "buttonRemoveCodec";
            this.buttonRemoveCodec.Size = new System.Drawing.Size(23, 23);
            this.buttonRemoveCodec.TabIndex = 3;
            this.buttonRemoveCodec.Text = "<";
            this.buttonRemoveCodec.UseVisualStyleBackColor = true;
            this.buttonRemoveCodec.Click += new System.EventHandler(this.buttonRemoveCodec_Click);
            // 
            // buttonAddCodec
            // 
            this.buttonAddCodec.Location = new System.Drawing.Point(154, 38);
            this.buttonAddCodec.Name = "buttonAddCodec";
            this.buttonAddCodec.Size = new System.Drawing.Size(23, 23);
            this.buttonAddCodec.TabIndex = 1;
            this.buttonAddCodec.Text = ">";
            this.buttonAddCodec.UseVisualStyleBackColor = true;
            this.buttonAddCodec.Click += new System.EventHandler(this.buttonAddCodec_Click);
            // 
            // labelEnabledCodecs
            // 
            this.labelEnabledCodecs.AutoSize = true;
            this.labelEnabledCodecs.Location = new System.Drawing.Point(192, 22);
            this.labelEnabledCodecs.Name = "labelEnabledCodecs";
            this.labelEnabledCodecs.Size = new System.Drawing.Size(87, 13);
            this.labelEnabledCodecs.TabIndex = 5;
            this.labelEnabledCodecs.Text = "Enabled codecs:";
            // 
            // listBoxEnabledCodecs
            // 
            this.listBoxEnabledCodecs.FormattingEnabled = true;
            this.listBoxEnabledCodecs.Items.AddRange(new object[] {
            "g729",
            "iLBC 30ms",
            "uLaw 8k",
            "aLaw 8k"});
            this.listBoxEnabledCodecs.Location = new System.Drawing.Point(194, 38);
            this.listBoxEnabledCodecs.Name = "listBoxEnabledCodecs";
            this.listBoxEnabledCodecs.Size = new System.Drawing.Size(124, 56);
            this.listBoxEnabledCodecs.TabIndex = 1;
            // 
            // labelAvailableCodecs
            // 
            this.labelAvailableCodecs.AutoSize = true;
            this.labelAvailableCodecs.Location = new System.Drawing.Point(16, 22);
            this.labelAvailableCodecs.Name = "labelAvailableCodecs";
            this.labelAvailableCodecs.Size = new System.Drawing.Size(91, 13);
            this.labelAvailableCodecs.TabIndex = 3;
            this.labelAvailableCodecs.Text = "Available codecs:";
            // 
            // listBoxAvailableCodecs
            // 
            this.listBoxAvailableCodecs.FormattingEnabled = true;
            this.listBoxAvailableCodecs.Items.AddRange(new object[] {
            "uLaw 8k",
            "aLaw 8k",
            "g729",
            "iLBC 30ms"});
            this.listBoxAvailableCodecs.Location = new System.Drawing.Point(19, 38);
            this.listBoxAvailableCodecs.Name = "listBoxAvailableCodecs";
            this.listBoxAvailableCodecs.Size = new System.Drawing.Size(124, 56);
            this.listBoxAvailableCodecs.TabIndex = 0;
            // 
            // groupBoxAudioVolume
            // 
            this.groupBoxAudioVolume.Controls.Add(this.labelSpeaker);
            this.groupBoxAudioVolume.Controls.Add(this.labelMicrophone);
            this.groupBoxAudioVolume.Controls.Add(this.trackBarSpeaker);
            this.groupBoxAudioVolume.Controls.Add(this.trackBarMicrophone);
            this.groupBoxAudioVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxAudioVolume.Location = new System.Drawing.Point(10, 10);
            this.groupBoxAudioVolume.Name = "groupBoxAudioVolume";
            this.groupBoxAudioVolume.Size = new System.Drawing.Size(354, 100);
            this.groupBoxAudioVolume.TabIndex = 0;
            this.groupBoxAudioVolume.TabStop = false;
            this.groupBoxAudioVolume.Text = "Audio Volume";
            // 
            // labelSpeaker
            // 
            this.labelSpeaker.AutoSize = true;
            this.labelSpeaker.Location = new System.Drawing.Point(192, 30);
            this.labelSpeaker.Name = "labelSpeaker";
            this.labelSpeaker.Size = new System.Drawing.Size(47, 13);
            this.labelSpeaker.TabIndex = 3;
            this.labelSpeaker.Text = "Speaker";
            // 
            // labelMicrophone
            // 
            this.labelMicrophone.AutoSize = true;
            this.labelMicrophone.Location = new System.Drawing.Point(16, 30);
            this.labelMicrophone.Name = "labelMicrophone";
            this.labelMicrophone.Size = new System.Drawing.Size(63, 13);
            this.labelMicrophone.TabIndex = 2;
            this.labelMicrophone.Text = "Microphone";
            // 
            // trackBarSpeaker
            // 
            this.trackBarSpeaker.LargeChange = 32;
            this.trackBarSpeaker.Location = new System.Drawing.Point(194, 49);
            this.trackBarSpeaker.Maximum = 1024;
            this.trackBarSpeaker.Name = "trackBarSpeaker";
            this.trackBarSpeaker.Size = new System.Drawing.Size(124, 45);
            this.trackBarSpeaker.SmallChange = 16;
            this.trackBarSpeaker.TabIndex = 1;
            this.trackBarSpeaker.TickFrequency = 64;
            this.trackBarSpeaker.Value = 512;
            // 
            // trackBarMicrophone
            // 
            this.trackBarMicrophone.LargeChange = 32;
            this.trackBarMicrophone.Location = new System.Drawing.Point(19, 49);
            this.trackBarMicrophone.Maximum = 1024;
            this.trackBarMicrophone.Name = "trackBarMicrophone";
            this.trackBarMicrophone.Size = new System.Drawing.Size(124, 45);
            this.trackBarMicrophone.SmallChange = 16;
            this.trackBarMicrophone.TabIndex = 0;
            this.trackBarMicrophone.TickFrequency = 64;
            this.trackBarMicrophone.Value = 512;
            // 
            // tableLayoutPanelBottomButtons
            // 
            this.tableLayoutPanelBottomButtons.ColumnCount = 4;
            this.tableLayoutPanelBottomButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelBottomButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelBottomButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelBottomButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanelBottomButtons.Controls.Add(this.buttonOK, 1, 0);
            this.tableLayoutPanelBottomButtons.Controls.Add(this.buttonCancel, 2, 0);
            this.tableLayoutPanelBottomButtons.Controls.Add(this.buttonApply, 3, 0);
            this.tableLayoutPanelBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanelBottomButtons.Location = new System.Drawing.Point(5, 381);
            this.tableLayoutPanelBottomButtons.Name = "tableLayoutPanelBottomButtons";
            this.tableLayoutPanelBottomButtons.RowCount = 1;
            this.tableLayoutPanelBottomButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelBottomButtons.Size = new System.Drawing.Size(382, 40);
            this.tableLayoutPanelBottomButtons.TabIndex = 1;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOK.Location = new System.Drawing.Point(119, 8);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.Location = new System.Drawing.Point(209, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonApply.Location = new System.Drawing.Point(299, 8);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // openFileDialogRingTone
            // 
            this.openFileDialogRingTone.DefaultExt = "mp3";
            this.openFileDialogRingTone.FileName = "*.mp3";
            this.openFileDialogRingTone.Filter = "*.mp3|";
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 426);
            this.Controls.Add(this.tableLayoutPanelBottomButtons);
            this.Controls.Add(this.tabControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Settings";
            this.tabControlMain.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBoxLanguage.ResumeLayout(false);
            this.groupBoxLanguage.PerformLayout();
            this.groupBoxContactsStores.ResumeLayout(false);
            this.groupBoxContactsStores.PerformLayout();
            this.groupBoxBasicSettings.ResumeLayout(false);
            this.groupBoxBasicSettings.PerformLayout();
            this.tabPagePhone.ResumeLayout(false);
            this.tabPagePhone.PerformLayout();
            this.groupBoxRingTone.ResumeLayout(false);
            this.groupBoxRingTone.PerformLayout();
            this.groupBoxAudioCodecs.ResumeLayout(false);
            this.groupBoxAudioCodecs.PerformLayout();
            this.groupBoxAudioVolume.ResumeLayout(false);
            this.groupBoxAudioVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeaker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMicrophone)).EndInit();
            this.tableLayoutPanelBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPagePhone;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottomButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBoxAudioVolume;
        private System.Windows.Forms.TrackBar trackBarSpeaker;
        private System.Windows.Forms.TrackBar trackBarMicrophone;
        private System.Windows.Forms.Label labelSpeaker;
        private System.Windows.Forms.Label labelMicrophone;
        private System.Windows.Forms.GroupBox groupBoxAudioCodecs;
        private System.Windows.Forms.Label labelEnabledCodecs;
        private System.Windows.Forms.ListBox listBoxEnabledCodecs;
        private System.Windows.Forms.Label labelAvailableCodecs;
        private System.Windows.Forms.ListBox listBoxAvailableCodecs;
        private System.Windows.Forms.Button buttonRemoveCodec;
        private System.Windows.Forms.Button buttonAddCodec;
        private System.Windows.Forms.GroupBox groupBoxRingTone;
        private System.Windows.Forms.GroupBox groupBoxBasicSettings;
        private System.Windows.Forms.CheckBox checkBoxStartup;
        private System.Windows.Forms.CheckBox checkBoxConfirmClosing;
        private System.Windows.Forms.CheckBox checkBoxEnableQA;
        private System.Windows.Forms.Button buttonRingToneSelect;
        private System.Windows.Forms.TextBox textBoxRingTonePath;
        private System.Windows.Forms.RadioButton radioButtonRingToneCustom;
        private System.Windows.Forms.RadioButton radioButtonRingToneDefault;
        private System.Windows.Forms.OpenFileDialog openFileDialogRingTone;
        private System.Windows.Forms.CheckBox checkBoxEnableSipDiagnosticLogging;
        private System.Windows.Forms.GroupBox groupBoxContactsStores;
        private System.Windows.Forms.CheckBox checkBoxContactsServer;
        private System.Windows.Forms.CheckBox checkBoxContactsLocal;
        private System.Windows.Forms.CheckBox checkBoxContactsOutlook;
        private System.Windows.Forms.GroupBox groupBoxLanguage;
        private System.Windows.Forms.RadioButton radioButtonEspanol;
        private System.Windows.Forms.RadioButton radioButtonDeutsch;
        private System.Windows.Forms.RadioButton radioButtonEnglish;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.RadioButton radioButtonItalian;
    }
}