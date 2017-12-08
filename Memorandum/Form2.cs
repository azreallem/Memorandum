using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Drawing.Drawing2D;

using System.Text.RegularExpressions;

namespace Memorandum
{
    public partial class Form2 : Form
    {
        List<string> listContent = new List<string>();
        List<string> listTime = new List<string>();
        List<string> listTitle = new List<string>();

        string path1 = System.Windows.Forms.Application.StartupPath + @"\text.dat";
        int index = 0;
        int t_delete = 0;

        Form loginform = null;
        string user = "";
        string password = "";


        public Form2(Form myform, string u, string p)
        {
            this.loginform = myform;
            this.user = u;
            this.password = p;

            InitializeComponent();
            this.KeyPreview = true;

            FileStream fs = File.Open(path1, FileMode.OpenOrCreate);
            fs.Close();
            byte[] byteArray = File.ReadAllBytes(path1);
            string tempstr = DataUnlock(byteArray, user, password);
            int length = Regex.Matches(tempstr, "\r\n").Count;  //获取tempstr中 "\r\n"的个数
            string[] data = new string[length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = tempstr.Substring(0, tempstr.IndexOf("\r\n") + 1);
                tempstr = tempstr.Substring(tempstr.IndexOf("\r\n") + "\r\n".Length - 1);
            }

            if (data != null)
            {
                int t1 = 0, t2 = 0, t3 = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Contains("修改时间"))
                    {
                        string str = data[i].Substring(data[i].IndexOf(":") + 1);
                        listTime.Add(str);
                        if (t1 == 0)
                        {
                            t1 = 1;
                            textBox2.Text = str;
                        }
                    }
                    if (data[i].Contains("内容"))
                    {
                        string str = data[i].Substring(data[i].IndexOf(":") + 1);
                        listContent.Add(str);
                        if (t2 == 0)
                        {
                            t2 = 1;
                            textBox1.Text = str;
                        }
                    }
                    if (data[i].Contains("主题"))
                    {
                        string str = data[i].Substring(data[i].IndexOf(":") + 1);
                        listTitle.Add(str);
                        listBox1.Items.Add(str);
                        if (t3 == 0)
                        {
                            t3 = 1;
                            textBox3.Text = str;
                            listBox1.Text = str;
                        }
                    }
                }
            }
            if (listTitle.Count == 0)
            {
                listTitle.Add("");
                listTime.Add("");
                listContent.Add("");
                listBox1.Items.Add("");
                updateData();
                listBox1.Text = "";
                saveData();
            }
        }

        //保存
        private void button1_Click(object sender, EventArgs e)
        {
            if (listTitle.Count != 0)
            {
                listTitle[index] = textBox3.Text;//标题

                //获取系统时间
                if (!listContent[index].Equals(textBox1.Text.Replace("\r\n", "@换行@")))
                {
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    listTime[index] = currentTime.ToString("yyyy/MM/dd");//时间
                }
                listContent[index] = textBox1.Text.Replace("\r\n", "@换行@");//写入

                updateData();
            }
            else
                MessageBox.Show("暂无主题");
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (t_delete == 0)
            {
                if (!textBox1.Text.Replace("\r\n", "@换行@").Equals(listContent[index]) || textBox2.Text != listTime[index] || textBox3.Text != listTitle[index])
                {
                    DialogResult result = MessageBox.Show("是否保存修改？", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        button1_Click(sender, e);
                    }
                }
            }
            else
                t_delete = 0;
            index = listBox1.SelectedIndex;
            if (index == -1)
                index = 0;
            if (listTitle.Count != 0)
            {
                textBox1.Text = listContent[index].Replace("@换行@", "\r\n");//读取
                textBox2.Text = listTime[index];
                textBox3.Text = listTitle[index];
            }
        }

        //新建
        private void button2_Click(object sender, EventArgs e)
        {

            if (listTitle.Count != 0)
            {
                if (!textBox1.Text.Replace("\r\n", "@换行@").Equals(listContent[index]) || textBox2.Text != listTime[index] || textBox3.Text != listTitle[index])
                {
                    button1_Click(sender, e);
                }

                index = listBox1.SelectedIndex;
                if (index == -1)
                    index = 0;
                if (listTitle.Count != 0)
                {
                    textBox1.Text = listContent[index].Replace("@换行@", "\r\n");//读取
                    textBox2.Text = listTime[index];
                    textBox3.Text = listTitle[index];
                }
                index = listTitle.Count;

            }
            listTitle.Insert(0, "");
            listTime.Insert(0, "");
            listContent.Insert(0, "");
            listBox1.Items.Insert(0, "");
            updateData();
            listBox1.Text = "";
            saveData();

        }
        //删除
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否删除？", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                deleteData();
            }
        }

        private void deleteData()
        {
            if (listTitle.Count != 0)
            {
                listContent.Remove(listContent[index]);
                listTime.Remove(listTime[index]);
                listTitle.Remove(listTitle[index]);
                if (index != 0)
                    index--;
                else
                    t_delete = 1;
                listBox1.Items.RemoveAt(index);
                saveData();
                updateData();
                if (listTitle.Count == 0)
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    listTitle.Add("");
                    listTime.Add("");
                    listContent.Add("");
                    listBox1.Items.Add("");
                    updateData();
                    listBox1.Text = "";
                    saveData();
                }
            }
            else
            {
                listTitle.Add("");
                listTime.Add("");
                listContent.Add("");
                listBox1.Items.Add("");
                updateData();
                listBox1.Text = "";
                saveData();
            }
        }

        private void updateData()
        {
            if (listTitle.Count != 0)
            {
                textBox1.Text = listContent[index].Replace("@换行@", "\r\n");//读取
                textBox2.Text = listTime[index];
                textBox3.Text = listTitle[index];
                listBox1.Items[index] = listTitle[index];
                saveData();
            }
        }

        private void saveData()
        {
            //保存内容
            string data = "";
            for (int i = 0; i < listTitle.Count; i++)
            {
                data += "修改时间:" + listTime[i] + "\r\n";
                data += "内容:" + listContent[i] + "\r\n";
                data += "主题:" + listTitle[i] + "\r\n";
            }
            byte[] byteArray = DataLock(data, user, password);
            //File.WriteAllText(path1, data);
            File.WriteAllBytes(path1, byteArray);
        }

        //文件上传
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("上传文件必须为txt文件。\r\n且文件数据格式如下:\r\n修改时间:\r\n" +
                "内容:(只允许占用一行，如需换行请用 @换行@ 代替换行操作)" +
                "\r\n主题:\r\n", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                OpenFileDialog file1 = new OpenFileDialog();
                file1.Filter = "文本文件|*.txt|Dat文件(*.dat)|*.dat";
                if (file1.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = File.OpenText(file1.FileName);
                    while (sr.EndOfStream != true)
                    {
                        string str = sr.ReadLine();
                        if (str.Contains("修改时间"))
                        {
                            str = str.Substring(str.IndexOf(":") + 1);
                            listTime.Add(str);
                        }
                        if (str.Contains("内容"))
                        {
                            str = str.Substring(str.IndexOf(":") + 1);
                            listContent.Add(str);
                        }
                        if (str.Contains("主题"))
                        {
                            str = str.Substring(str.IndexOf(":") + 1);
                            listTitle.Add(str);
                            listBox1.Items.Add(str);
                        }
                    }
                    sr.Close();

                }
                updateData();
                saveData();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            button1_Click(sender, e);
            loginform.Close();
        }

        static byte CharToByte(char c)
        {
            string str1 = c.ToString();
            byte[] array = new byte[1];   //定义一组数组array
            array = System.Text.Encoding.ASCII.GetBytes(str1); //string转换的字母
            return array[0];
        }

        public static byte[] DataLock(string str, string user, string password)
        {
            string str1 = user + password;
            int count = 0;
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            int count2 = 0;
            int s = 961616044;
            byte[] shi = System.BitConverter.GetBytes(s);
            for (int i = 0; i < byteArray.Length; i++)
            {
                shi[count2] += CharToByte(str1[count]);
                byteArray[i] += shi[count2];
                if (count >= str1.Length - 1)
                    count = 0;
                else
                    count++;
                if (count2 >= shi.Length - 1)
                    count2 = 0;
                else
                    count2++;
            }
            return byteArray;
        }

        public static string DataUnlock(byte[] byteArray, string user, string password)
        {
            string str1 = user + password;
            int count = 0;
            int count2 = 0;
            int s = 961616044;
            byte[] shi = System.BitConverter.GetBytes(s);
            for (int i = 0; i < byteArray.Length; i++)
            {
                shi[count2] += CharToByte(str1[count]);
                byteArray[i] -= shi[count2];
                if (count >= str1.Length - 1)
                    count = 0;
                else
                    count++;
                if (count2 >= shi.Length - 1)
                    count2 = 0;
                else
                    count2++;
            }
            string str = System.Text.Encoding.Default.GetString(byteArray);
            return str;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("是否删除全部内容？", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int deleteCount = listBox1.Items.Count;
                for (int i = 0; i < deleteCount; i++)
                    deleteData();
            }
        }


        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color FColor = Color.Green;
            Color TColor = Color.Yellow;


            Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.ForwardDiagonal);


            g.FillRectangle(b, this.ClientRectangle);
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                if (e.Alt == true)
                    switch (e.KeyCode)
                    {
                        case Keys.D:
                            button5_Click(sender, e);
                            break;
                    }
                else
                    switch (e.KeyCode)
                    {
                        case Keys.N:
                            button2_Click(sender, e);
                            break;
                        case Keys.S:
                            button1_Click(sender, e);
                            break;
                        case Keys.D:
                            button3_Click(sender, e);
                            break;
                    }
            }
        }
    }
}
