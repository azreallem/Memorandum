using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Memorandum
{
    public partial class mainForm : Form
    {
        string user1 = "gaoliang";
        string password1 = "chensgehaor";

        public mainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            string b = textBox2.Text;
            if (a == "" || b == "")
            {
                MessageBox.Show("账号或密码不能为空!", "提示");
            }
            else if (textBox1.Text.Equals(user1) && textBox2.Text.Equals(password1))
            {
                this.Hide();
                Form form2 = new Form2(this, user1, password1);   //调用带参的构造函数
                form2.Show();
            }
            else
                MessageBox.Show("用户名或密码不正确!", "提示");
        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//判断回车键
            {
                this.button1_Click(sender, e);//触发按钮事件
            }
        }

        private void mainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color FColor = Color.Green;
            Color TColor = Color.Yellow;


            Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.ForwardDiagonal);


            g.FillRectangle(b, this.ClientRectangle);
        }

    }
}
