using ChattingPacket;
using MNetwork.Engine;
using MNetwork.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerHost.Winform
{
    internal class LoginForm : Form
    {
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Button button1;
        private TextBox textBox1;

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(119, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 21);
            this.textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(119, 98);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(158, 21);
            this.textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(13, 60);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 14);
            this.textBox3.TabIndex = 2;
            this.textBox3.Text = "ID";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.MenuBar;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(13, 98);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 14);
            this.textBox4.TabIndex = 3;
            this.textBox4.Text = "PASSWORD";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 207);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            LoginRequestPacket packet = new LoginRequestPacket()
            {
                ID = textBox1.Text,
                Password = textBox2.Text
            };

            MNetworkEntry.Instance.Send(new ProtobufPacket<LoginRequestPacket>(0, PacketEnum.ProcessType.Data,
                (int)MessageType.LoginRequest, packet));

            this.Close();
        }
    }
}
