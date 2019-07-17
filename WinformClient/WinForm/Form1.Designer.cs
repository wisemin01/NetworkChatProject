namespace ClientHost
{
    partial class ClientGUIForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Connect = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.portInputBox = new System.Windows.Forms.TextBox();
            this.SendContextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.ipInputBox = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(194, 93);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(75, 23);
            this.Connect.TabIndex = 1;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.ConnectButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 122);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(888, 252);
            this.textBox1.TabIndex = 0;
            // 
            // portInputBox
            // 
            this.portInputBox.Location = new System.Drawing.Point(132, 12);
            this.portInputBox.Name = "portInputBox";
            this.portInputBox.Size = new System.Drawing.Size(137, 21);
            this.portInputBox.TabIndex = 2;
            this.portInputBox.Text = "9199";
            // 
            // SendContextBox
            // 
            this.SendContextBox.Location = new System.Drawing.Point(14, 392);
            this.SendContextBox.Name = "SendContextBox";
            this.SendContextBox.Size = new System.Drawing.Size(805, 21);
            this.SendContextBox.TabIndex = 3;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(836, 391);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(68, 21);
            this.SendButton.TabIndex = 4;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButtonClick);
            // 
            // ipInputBox
            // 
            this.ipInputBox.Location = new System.Drawing.Point(132, 39);
            this.ipInputBox.Name = "ipInputBox";
            this.ipInputBox.Size = new System.Drawing.Size(137, 21);
            this.ipInputBox.TabIndex = 5;
            this.ipInputBox.Text = "127.0.0.1";
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox6.Location = new System.Drawing.Point(16, 12);
            this.textBox6.Name = "textBox6";
            this.textBox6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox6.Size = new System.Drawing.Size(101, 15);
            this.textBox6.TabIndex = 7;
            this.textBox6.Text = "Port";
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox7.Location = new System.Drawing.Point(14, 41);
            this.textBox7.Name = "textBox7";
            this.textBox7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox7.Size = new System.Drawing.Size(101, 15);
            this.textBox7.TabIndex = 8;
            this.textBox7.Text = "IP Adress";
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox9.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox9.Location = new System.Drawing.Point(16, 93);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(166, 18);
            this.textBox9.TabIndex = 10;
            // 
            // ClientGUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 425);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.ipInputBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.SendContextBox);
            this.Controls.Add(this.portInputBox);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.textBox1);
            this.Name = "ClientGUIForm";
            this.Text = "My Chat Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Connect;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox portInputBox;
        private System.Windows.Forms.TextBox SendContextBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.TextBox ipInputBox;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox9;
    }
}

