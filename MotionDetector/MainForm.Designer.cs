namespace MotionDetectorN
{
    partial class MainForm
    {
        
        
        
        private System.ComponentModel.IContainer components = null;

        
        
        
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        
        
        
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ChangeVideoSourceLabel = new System.Windows.Forms.Label();
            this.VideoSourceChanger = new System.Windows.Forms.ComboBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.FPSLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.SoundNotificationCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GridMotionProcessingRadioButton = new System.Windows.Forms.RadioButton();
            this.BorderHighlightMotionProcessingRadioButton = new System.Windows.Forms.RadioButton();
            this.AreaHighlightMotionProcessingRadioButton = new System.Windows.Forms.RadioButton();
            this.NoMotionProcessingRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BackgroundModelingMotionDetectorRadioButton = new System.Windows.Forms.RadioButton();
            this.FrameDifferenceMotionDetectorRadioButton = new System.Windows.Forms.RadioButton();
            this.NoMotionDetectorRadioButton = new System.Windows.Forms.RadioButton();
            this.MotionHistoryCheckbox = new System.Windows.Forms.CheckBox();
            this.StopDetectionButton = new System.Windows.Forms.Button();
            this.StartDetectionButton = new System.Windows.Forms.Button();
            this.VideoPlayer = new MotionDetector.Controls.VideoSourcePlayer();
            this.FPSTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            
            
            
            this.ChangeVideoSourceLabel.AutoSize = true;
            this.ChangeVideoSourceLabel.ForeColor = System.Drawing.Color.Black;
            this.ChangeVideoSourceLabel.Location = new System.Drawing.Point(6, 17);
            this.ChangeVideoSourceLabel.Name = "ChangeVideoSourceLabel";
            this.ChangeVideoSourceLabel.Size = new System.Drawing.Size(145, 13);
            this.ChangeVideoSourceLabel.TabIndex = 1;
            this.ChangeVideoSourceLabel.Text = "Выберите источник видео: ";
            
            
            
            this.VideoSourceChanger.FormattingEnabled = true;
            this.VideoSourceChanger.Location = new System.Drawing.Point(9, 33);
            this.VideoSourceChanger.Name = "VideoSourceChanger";
            this.VideoSourceChanger.Size = new System.Drawing.Size(283, 21);
            this.VideoSourceChanger.TabIndex = 2;
            
            
            
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusProgressBar,
            this.FPSLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 379);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(688, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            
            
            
            this.StatusProgressBar.Name = "StatusProgressBar";
            this.StatusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.StatusProgressBar.Step = 1000;
            
            
            
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(26, 17);
            this.FPSLabel.Text = "FPS";
            
            
            
            this.SettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsGroupBox.Controls.Add(this.SoundNotificationCheckBox);
            this.SettingsGroupBox.Controls.Add(this.groupBox2);
            this.SettingsGroupBox.Controls.Add(this.groupBox1);
            this.SettingsGroupBox.Controls.Add(this.MotionHistoryCheckbox);
            this.SettingsGroupBox.Controls.Add(this.StopDetectionButton);
            this.SettingsGroupBox.Controls.Add(this.StartDetectionButton);
            this.SettingsGroupBox.Controls.Add(this.ChangeVideoSourceLabel);
            this.SettingsGroupBox.Controls.Add(this.VideoSourceChanger);
            this.SettingsGroupBox.ForeColor = System.Drawing.Color.Blue;
            this.SettingsGroupBox.Location = new System.Drawing.Point(378, 12);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(298, 356);
            this.SettingsGroupBox.TabIndex = 4;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "Настройки";
            
            
            
            this.SoundNotificationCheckBox.AutoSize = true;
            this.SoundNotificationCheckBox.ForeColor = System.Drawing.Color.Black;
            this.SoundNotificationCheckBox.Location = new System.Drawing.Point(10, 82);
            this.SoundNotificationCheckBox.Name = "SoundNotificationCheckBox";
            this.SoundNotificationCheckBox.Size = new System.Drawing.Size(140, 17);
            this.SoundNotificationCheckBox.TabIndex = 8;
            this.SoundNotificationCheckBox.Text = "Звуковое оповещение";
            this.SoundNotificationCheckBox.UseVisualStyleBackColor = true;
            
            
            
            this.groupBox2.Controls.Add(this.GridMotionProcessingRadioButton);
            this.groupBox2.Controls.Add(this.BorderHighlightMotionProcessingRadioButton);
            this.groupBox2.Controls.Add(this.AreaHighlightMotionProcessingRadioButton);
            this.groupBox2.Controls.Add(this.NoMotionProcessingRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(10, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 114);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Алгоритм обрабоки движений";
            
            
            
            this.GridMotionProcessingRadioButton.AutoSize = true;
            this.GridMotionProcessingRadioButton.ForeColor = System.Drawing.Color.Black;
            this.GridMotionProcessingRadioButton.Location = new System.Drawing.Point(6, 88);
            this.GridMotionProcessingRadioButton.Name = "GridMotionProcessingRadioButton";
            this.GridMotionProcessingRadioButton.Size = new System.Drawing.Size(120, 17);
            this.GridMotionProcessingRadioButton.TabIndex = 6;
            this.GridMotionProcessingRadioButton.Text = "Выделение сеткой";
            this.GridMotionProcessingRadioButton.UseVisualStyleBackColor = true;
            this.GridMotionProcessingRadioButton.CheckedChanged += new System.EventHandler(this.GridMotionProcessingRadioButton_CheckedChanged);
            
            
            
            this.BorderHighlightMotionProcessingRadioButton.AutoSize = true;
            this.BorderHighlightMotionProcessingRadioButton.ForeColor = System.Drawing.Color.Black;
            this.BorderHighlightMotionProcessingRadioButton.Location = new System.Drawing.Point(6, 65);
            this.BorderHighlightMotionProcessingRadioButton.Name = "BorderHighlightMotionProcessingRadioButton";
            this.BorderHighlightMotionProcessingRadioButton.Size = new System.Drawing.Size(228, 17);
            this.BorderHighlightMotionProcessingRadioButton.TabIndex = 5;
            this.BorderHighlightMotionProcessingRadioButton.Text = "Подстветка границы области движения";
            this.BorderHighlightMotionProcessingRadioButton.UseVisualStyleBackColor = true;
            this.BorderHighlightMotionProcessingRadioButton.CheckedChanged += new System.EventHandler(this.BorderHighlightMotionProcessingRadioButton_CheckedChanged);
            
            
            
            this.AreaHighlightMotionProcessingRadioButton.AutoSize = true;
            this.AreaHighlightMotionProcessingRadioButton.Checked = true;
            this.AreaHighlightMotionProcessingRadioButton.ForeColor = System.Drawing.Color.Black;
            this.AreaHighlightMotionProcessingRadioButton.Location = new System.Drawing.Point(6, 42);
            this.AreaHighlightMotionProcessingRadioButton.Name = "AreaHighlightMotionProcessingRadioButton";
            this.AreaHighlightMotionProcessingRadioButton.Size = new System.Drawing.Size(182, 17);
            this.AreaHighlightMotionProcessingRadioButton.TabIndex = 4;
            this.AreaHighlightMotionProcessingRadioButton.TabStop = true;
            this.AreaHighlightMotionProcessingRadioButton.Text = "Подстветка области движения";
            this.AreaHighlightMotionProcessingRadioButton.UseVisualStyleBackColor = true;
            this.AreaHighlightMotionProcessingRadioButton.CheckedChanged += new System.EventHandler(this.AreaHighlightMotionProcessingRadioButton_CheckedChanged);
            
            
            
            this.NoMotionProcessingRadioButton.AutoSize = true;
            this.NoMotionProcessingRadioButton.ForeColor = System.Drawing.Color.Black;
            this.NoMotionProcessingRadioButton.Location = new System.Drawing.Point(6, 19);
            this.NoMotionProcessingRadioButton.Name = "NoMotionProcessingRadioButton";
            this.NoMotionProcessingRadioButton.Size = new System.Drawing.Size(87, 17);
            this.NoMotionProcessingRadioButton.TabIndex = 3;
            this.NoMotionProcessingRadioButton.Text = "Отсутствует";
            this.NoMotionProcessingRadioButton.UseVisualStyleBackColor = true;
            this.NoMotionProcessingRadioButton.CheckedChanged += new System.EventHandler(this.NoMotionProcessingRadioButton_CheckedChanged);
            
            
            
            this.groupBox1.Controls.Add(this.BackgroundModelingMotionDetectorRadioButton);
            this.groupBox1.Controls.Add(this.FrameDifferenceMotionDetectorRadioButton);
            this.groupBox1.Controls.Add(this.NoMotionDetectorRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(10, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 90);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Алгоритм поиска движений: ";
            
            
            
            this.BackgroundModelingMotionDetectorRadioButton.AutoSize = true;
            this.BackgroundModelingMotionDetectorRadioButton.ForeColor = System.Drawing.Color.Black;
            this.BackgroundModelingMotionDetectorRadioButton.Location = new System.Drawing.Point(6, 65);
            this.BackgroundModelingMotionDetectorRadioButton.Name = "BackgroundModelingMotionDetectorRadioButton";
            this.BackgroundModelingMotionDetectorRadioButton.Size = new System.Drawing.Size(135, 17);
            this.BackgroundModelingMotionDetectorRadioButton.TabIndex = 2;
            this.BackgroundModelingMotionDetectorRadioButton.Text = "Моделирование фона";
            this.BackgroundModelingMotionDetectorRadioButton.UseVisualStyleBackColor = true;
            this.BackgroundModelingMotionDetectorRadioButton.CheckedChanged += new System.EventHandler(this.BackgroundModelingMotionDetectorRadioButton_CheckedChanged);
            
            
            
            this.FrameDifferenceMotionDetectorRadioButton.AutoSize = true;
            this.FrameDifferenceMotionDetectorRadioButton.Checked = true;
            this.FrameDifferenceMotionDetectorRadioButton.ForeColor = System.Drawing.Color.Black;
            this.FrameDifferenceMotionDetectorRadioButton.Location = new System.Drawing.Point(6, 42);
            this.FrameDifferenceMotionDetectorRadioButton.Name = "FrameDifferenceMotionDetectorRadioButton";
            this.FrameDifferenceMotionDetectorRadioButton.Size = new System.Drawing.Size(107, 17);
            this.FrameDifferenceMotionDetectorRadioButton.TabIndex = 1;
            this.FrameDifferenceMotionDetectorRadioButton.TabStop = true;
            this.FrameDifferenceMotionDetectorRadioButton.Text = "Разница кадров";
            this.FrameDifferenceMotionDetectorRadioButton.UseVisualStyleBackColor = true;
            this.FrameDifferenceMotionDetectorRadioButton.CheckedChanged += new System.EventHandler(this.FrameDifferenceMotionDetectorRadioButton_CheckedChanged);
            
            
            
            this.NoMotionDetectorRadioButton.AutoSize = true;
            this.NoMotionDetectorRadioButton.ForeColor = System.Drawing.Color.Black;
            this.NoMotionDetectorRadioButton.Location = new System.Drawing.Point(6, 19);
            this.NoMotionDetectorRadioButton.Name = "NoMotionDetectorRadioButton";
            this.NoMotionDetectorRadioButton.Size = new System.Drawing.Size(87, 17);
            this.NoMotionDetectorRadioButton.TabIndex = 0;
            this.NoMotionDetectorRadioButton.Text = "Отсутствует";
            this.NoMotionDetectorRadioButton.UseVisualStyleBackColor = true;
            this.NoMotionDetectorRadioButton.CheckedChanged += new System.EventHandler(this.NoMotionDetectorRadioButton_CheckedChanged);
            
            
            
            this.MotionHistoryCheckbox.AutoSize = true;
            this.MotionHistoryCheckbox.ForeColor = System.Drawing.Color.Black;
            this.MotionHistoryCheckbox.Location = new System.Drawing.Point(10, 59);
            this.MotionHistoryCheckbox.Name = "MotionHistoryCheckbox";
            this.MotionHistoryCheckbox.Size = new System.Drawing.Size(216, 17);
            this.MotionHistoryCheckbox.TabIndex = 5;
            this.MotionHistoryCheckbox.Text = "Показывать ли статистику движений";
            this.MotionHistoryCheckbox.UseVisualStyleBackColor = true;
            
            
            
            this.StopDetectionButton.ForeColor = System.Drawing.Color.Black;
            this.StopDetectionButton.Location = new System.Drawing.Point(156, 328);
            this.StopDetectionButton.Name = "StopDetectionButton";
            this.StopDetectionButton.Size = new System.Drawing.Size(136, 22);
            this.StopDetectionButton.TabIndex = 4;
            this.StopDetectionButton.Text = "Стоп";
            this.StopDetectionButton.UseVisualStyleBackColor = true;
            this.StopDetectionButton.Click += new System.EventHandler(this.StopDetectionButton_Click);
            
            
            
            this.StartDetectionButton.ForeColor = System.Drawing.Color.Black;
            this.StartDetectionButton.Location = new System.Drawing.Point(10, 328);
            this.StartDetectionButton.Name = "StartDetectionButton";
            this.StartDetectionButton.Size = new System.Drawing.Size(141, 22);
            this.StartDetectionButton.TabIndex = 3;
            this.StartDetectionButton.Text = "Старт";
            this.StartDetectionButton.UseVisualStyleBackColor = true;
            this.StartDetectionButton.Click += new System.EventHandler(this.StartButton_Click);
            
            
            
            this.VideoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VideoPlayer.Location = new System.Drawing.Point(12, 12);
            this.VideoPlayer.Name = "VideoPlayer";
            this.VideoPlayer.Size = new System.Drawing.Size(360, 356);
            this.VideoPlayer.TabIndex = 0;
            this.VideoPlayer.Text = "VideoSourcePlayer";
            this.VideoPlayer.VideoSource = null;
            this.VideoPlayer.NewFrame += new MotionDetector.Controls.VideoSourcePlayer.NewFrameHandler(this.VideoPlayer_NewFrame);
            
            
            
            this.FPSTimer.Interval = 1000;
            this.FPSTimer.Tick += new System.EventHandler(this.FPSTimerTick);
            
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 401);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.VideoPlayer);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Детектор движения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MotionDetector.Controls.VideoSourcePlayer VideoPlayer;
        private System.Windows.Forms.Label ChangeVideoSourceLabel;
        private System.Windows.Forms.ComboBox VideoSourceChanger;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar StatusProgressBar;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Button StopDetectionButton;
        private System.Windows.Forms.Button StartDetectionButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton GridMotionProcessingRadioButton;
        private System.Windows.Forms.RadioButton BorderHighlightMotionProcessingRadioButton;
        private System.Windows.Forms.RadioButton AreaHighlightMotionProcessingRadioButton;
        private System.Windows.Forms.RadioButton NoMotionProcessingRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton BackgroundModelingMotionDetectorRadioButton;
        private System.Windows.Forms.RadioButton FrameDifferenceMotionDetectorRadioButton;
        private System.Windows.Forms.RadioButton NoMotionDetectorRadioButton;
        private System.Windows.Forms.CheckBox MotionHistoryCheckbox;
        private System.Windows.Forms.Timer FPSTimer;
        private System.Windows.Forms.ToolStripStatusLabel FPSLabel;
        private System.Windows.Forms.CheckBox SoundNotificationCheckBox;
    }
}

