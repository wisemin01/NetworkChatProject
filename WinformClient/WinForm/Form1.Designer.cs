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
            this.SendContextBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Connect
            // 
            this.Connect.Location = new System.Drawing.Point(16, 12);
            this.Connect.Name = "Connect";
            this.Connect.Size = new System.Drawing.Size(888, 23);
            this.Connect.TabIndex = 1;
            this.Connect.Text = "Connect";
            this.Connect.UseVisualStyleBackColor = true;
            this.Connect.Click += new System.EventHandler(this.ConnectButtonClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 55);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(888, 319);
            this.textBox1.TabIndex = 0;
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
            // ClientGUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 425);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.SendContextBox);
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
        private System.Windows.Forms.TextBox SendContextBox;
        private System.Windows.Forms.Button SendButton;
    }
}

