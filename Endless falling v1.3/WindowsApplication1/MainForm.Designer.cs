namespace WindowsApplication1
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GamePanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.genTimer = new System.Windows.Forms.Timer(this.components);
            this.ScoreLabel = new System.Windows.Forms.Label();
            this.CurrentScore = new System.Windows.Forms.Label();
            this.HighScore = new System.Windows.Forms.Label();
            this.HistoryScoreLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentLife = new System.Windows.Forms.Label();
            this.LifeLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UltiSkillLeftLabel = new System.Windows.Forms.Label();
            this.AITimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.BackColor = System.Drawing.Color.Transparent;
            this.GamePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GamePanel.Location = new System.Drawing.Point(47, 93);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(877, 419);
            this.GamePanel.TabIndex = 7;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // genTimer
            // 
            this.genTimer.Interval = 500;
            this.genTimer.Tick += new System.EventHandler(this.genTimer_Tick);
            // 
            // ScoreLabel
            // 
            this.ScoreLabel.AutoSize = true;
            this.ScoreLabel.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScoreLabel.ForeColor = System.Drawing.Color.Red;
            this.ScoreLabel.Location = new System.Drawing.Point(42, 33);
            this.ScoreLabel.Name = "ScoreLabel";
            this.ScoreLabel.Size = new System.Drawing.Size(72, 27);
            this.ScoreLabel.TabIndex = 8;
            this.ScoreLabel.Text = "Score:";
            // 
            // CurrentScore
            // 
            this.CurrentScore.AutoSize = true;
            this.CurrentScore.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentScore.ForeColor = System.Drawing.Color.Red;
            this.CurrentScore.Location = new System.Drawing.Point(126, 33);
            this.CurrentScore.Name = "CurrentScore";
            this.CurrentScore.Size = new System.Drawing.Size(24, 27);
            this.CurrentScore.TabIndex = 9;
            this.CurrentScore.Text = "0";
            // 
            // HighScore
            // 
            this.HighScore.AutoSize = true;
            this.HighScore.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HighScore.ForeColor = System.Drawing.Color.Red;
            this.HighScore.Location = new System.Drawing.Point(779, 33);
            this.HighScore.Name = "HighScore";
            this.HighScore.Size = new System.Drawing.Size(24, 27);
            this.HighScore.TabIndex = 11;
            this.HighScore.Text = "0";
            // 
            // HistoryScoreLabel
            // 
            this.HistoryScoreLabel.AutoSize = true;
            this.HistoryScoreLabel.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HistoryScoreLabel.ForeColor = System.Drawing.Color.Red;
            this.HistoryScoreLabel.Location = new System.Drawing.Point(641, 33);
            this.HistoryScoreLabel.Name = "HistoryScoreLabel";
            this.HistoryScoreLabel.Size = new System.Drawing.Size(121, 27);
            this.HistoryScoreLabel.TabIndex = 10;
            this.HistoryScoreLabel.Text = "High Score:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Kristen ITC", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(53, 586);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 147);
            this.label1.TabIndex = 12;
            this.label1.Text = "Instruction:\r\n\r\nPress \"Z\" to Dash \r\nPress \"X\" to call AI Assistant\r\nPress \"C\" to " +
                "Shoot\r\nPress \"V\" for Ultimate Skill\r\n";
            // 
            // CurrentLife
            // 
            this.CurrentLife.AutoSize = true;
            this.CurrentLife.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentLife.ForeColor = System.Drawing.Color.Red;
            this.CurrentLife.Location = new System.Drawing.Point(446, 33);
            this.CurrentLife.Name = "CurrentLife";
            this.CurrentLife.Size = new System.Drawing.Size(22, 27);
            this.CurrentLife.TabIndex = 14;
            this.CurrentLife.Text = "1";
            // 
            // LifeLabel
            // 
            this.LifeLabel.AutoSize = true;
            this.LifeLabel.Font = new System.Drawing.Font("Kristen ITC", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LifeLabel.ForeColor = System.Drawing.Color.Red;
            this.LifeLabel.Location = new System.Drawing.Point(362, 33);
            this.LifeLabel.Name = "LifeLabel";
            this.LifeLabel.Size = new System.Drawing.Size(57, 27);
            this.LifeLabel.TabIndex = 13;
            this.LifeLabel.Text = "Life:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Kristen ITC", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(669, 539);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 22);
            this.label2.TabIndex = 15;
            this.label2.Text = "Ultimate Skill Left:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UltiSkillLeftLabel
            // 
            this.UltiSkillLeftLabel.AutoSize = true;
            this.UltiSkillLeftLabel.Font = new System.Drawing.Font("Kristen ITC", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UltiSkillLeftLabel.ForeColor = System.Drawing.Color.Red;
            this.UltiSkillLeftLabel.Location = new System.Drawing.Point(846, 539);
            this.UltiSkillLeftLabel.Name = "UltiSkillLeftLabel";
            this.UltiSkillLeftLabel.Size = new System.Drawing.Size(19, 22);
            this.UltiSkillLeftLabel.TabIndex = 16;
            this.UltiSkillLeftLabel.Text = "2";
            this.UltiSkillLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AITimer
            // 
            this.AITimer.Interval = 800;
            this.AITimer.Tick += new System.EventHandler(this.AITimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.UltiSkillLeftLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentLife);
            this.Controls.Add(this.LifeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HighScore);
            this.Controls.Add(this.HistoryScoreLabel);
            this.Controls.Add(this.CurrentScore);
            this.Controls.Add(this.ScoreLabel);
            this.Controls.Add(this.GamePanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Endless Falling 1.0";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel GamePanel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer genTimer;
        private System.Windows.Forms.Label ScoreLabel;
        private System.Windows.Forms.Label CurrentScore;
        private System.Windows.Forms.Label HighScore;
        private System.Windows.Forms.Label HistoryScoreLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label CurrentLife;
        private System.Windows.Forms.Label LifeLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label UltiSkillLeftLabel;
        private System.Windows.Forms.Timer AITimer;
    }
}

