namespace ServerHost
{
    partial class ServerGUIForm
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
            this.ChattingList = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChattingList
            // 
            this.ChattingList.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ChattingList.ForeColor = System.Drawing.SystemColors.Info;
            this.ChattingList.Location = new System.Drawing.Point(3, 30);
            this.ChattingList.Multiline = true;
            this.ChattingList.Name = "ChattingList";
            this.ChattingList.ReadOnly = true;
            this.ChattingList.Size = new System.Drawing.Size(637, 421);
            this.ChattingList.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "Server start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ServerStartButtonClick);
            // 
            // ServerGUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(641, 454);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ChattingList);
            this.Name = "ServerGUIForm";
            this.Text = "Server Host";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ChattingList;
        private System.Windows.Forms.Button button1;
    }
}

