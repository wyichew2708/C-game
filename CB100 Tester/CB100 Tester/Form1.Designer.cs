namespace CB100_Tester
{
    partial class CB100
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
            this.ConnectCOM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.COMTextBox = new System.Windows.Forms.TextBox();
            this.ReadTemp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SVSet = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ALMSet = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectCOM
            // 
            this.ConnectCOM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectCOM.Location = new System.Drawing.Point(196, 21);
            this.ConnectCOM.Name = "ConnectCOM";
            this.ConnectCOM.Size = new System.Drawing.Size(75, 23);
            this.ConnectCOM.TabIndex = 0;
            this.ConnectCOM.Text = "Connect";
            this.ConnectCOM.UseVisualStyleBackColor = true;
            this.ConnectCOM.Click += new System.EventHandler(this.ConnectCOM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM:";
            // 
            // COMTextBox
            // 
            this.COMTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.COMTextBox.Location = new System.Drawing.Point(73, 24);
            this.COMTextBox.Name = "COMTextBox";
            this.COMTextBox.Size = new System.Drawing.Size(100, 22);
            this.COMTextBox.TabIndex = 2;
            // 
            // ReadTemp
            // 
            this.ReadTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadTemp.Location = new System.Drawing.Point(29, 69);
            this.ReadTemp.Name = "ReadTemp";
            this.ReadTemp.Size = new System.Drawing.Size(125, 45);
            this.ReadTemp.TabIndex = 3;
            this.ReadTemp.Text = "Read Temperature";
            this.ReadTemp.UseVisualStyleBackColor = true;
            this.ReadTemp.Click += new System.EventHandler(this.ReadTemp_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(178, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Temperature:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(296, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(288, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 45);
            this.button1.TabIndex = 6;
            this.button1.Text = "Set Temperature Parameter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SVSet
            // 
            this.SVSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SVSet.Location = new System.Drawing.Point(150, 152);
            this.SVSet.Name = "SVSet";
            this.SVSet.Size = new System.Drawing.Size(100, 22);
            this.SVSet.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(14, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "SV Parameter:";
            // 
            // ALMSet
            // 
            this.ALMSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ALMSet.Location = new System.Drawing.Point(150, 189);
            this.ALMSet.Name = "ALMSet";
            this.ALMSet.Size = new System.Drawing.Size(100, 22);
            this.ALMSet.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(14, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "ALM Parameter:";
            // 
            // CB100
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(459, 266);
            this.Controls.Add(this.ALMSet);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SVSet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReadTemp);
            this.Controls.Add(this.COMTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectCOM);
            this.Name = "CB100";
            this.Text = "CB100 Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectCOM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox COMTextBox;
        private System.Windows.Forms.Button ReadTemp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox SVSet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ALMSet;
        private System.Windows.Forms.Label label4;
    }
}

