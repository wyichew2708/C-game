namespace Windows_HelpTools
{
    partial class MainFormInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormInterface));
            this.ExitButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.PrintScreenButton = new System.Windows.Forms.Button();
            this.CropImageButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.ExitButton, "ExitButton");
            this.ExitButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PrintScreenButton
            // 
            this.PrintScreenButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.PrintScreenButton, "PrintScreenButton");
            this.PrintScreenButton.ForeColor = System.Drawing.Color.Blue;
            this.PrintScreenButton.Name = "PrintScreenButton";
            this.PrintScreenButton.UseVisualStyleBackColor = false;
            this.PrintScreenButton.Click += new System.EventHandler(this.PrintScreenButton_Click);
            // 
            // CropImageButton
            // 
            this.CropImageButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.CropImageButton, "CropImageButton");
            this.CropImageButton.ForeColor = System.Drawing.Color.Red;
            this.CropImageButton.Name = "CropImageButton";
            this.CropImageButton.UseVisualStyleBackColor = false;
            this.CropImageButton.Click += new System.EventHandler(this.CropImageButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.CancelButton, "CancelButton");
            this.CancelButton.ForeColor = System.Drawing.Color.Indigo;
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.UseVisualStyleBackColor = false;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MainFormInterface
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CropImageButton);
            this.Controls.Add(this.PrintScreenButton);
            this.Controls.Add(this.ExitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainFormInterface";
            this.Load += new System.EventHandler(this.MainFormInterface_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFormInterface_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormInterface_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button PrintScreenButton;
        private System.Windows.Forms.Button CropImageButton;
        private System.Windows.Forms.Button CancelButton;
    }
}

