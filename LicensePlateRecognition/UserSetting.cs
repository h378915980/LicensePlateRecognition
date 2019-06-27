using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LicensePlateRecognition
{
    public partial class UserSetting : Form
    {


        public static string savePath;
        public static bool isAutoProcessPlate = false;
        public static bool isAutoProcessChar = false;
        public static bool isPlateFolderReady = false;
        public static bool isCharFolderReady = false;
        public enum ShowTypes
        {
            UNKNOW=0,
            PLATE=1,
            CHAR=2,
            All=3
        } 



        public UserSetting()
        {
            InitializeComponent();
            InitSetting();
        }
        //初始化
        private void InitSetting()
        {
            this.textBox1.Text = savePath;
            this.checkBox1.CheckedChanged -= new System.EventHandler(checkBox1_CheckedChanged);
            this.checkBox2.CheckedChanged -= new System.EventHandler(checkBox2_CheckedChanged);
            this.checkBox1.Checked = isAutoProcessPlate;
            this.checkBox2.Checked = isAutoProcessChar;
            this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
            this.checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);

        }


        //关闭
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //保存
        private void button1_Click(object sender, EventArgs e)
        {
            savePath = this.textBox1.Text;
            MessageBox.Show("保存成功");
        }
        //打开文件夹
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        //自动训练车牌
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (PlateCategorySVM.IsReady == false)
            {
                this.checkBox1.CheckedChanged -= new System.EventHandler(checkBox1_CheckedChanged);
                this.checkBox1.Checked = isAutoProcessPlate;
                this.checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);
                MessageBox.Show("车牌训练库没有准备好，请先训练一次");
                return;
            }

            isAutoProcessPlate = !isAutoProcessPlate;
        }
        //自动训练字符
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (CharCategorySVM.IsReady == false)
            {
                this.checkBox2.CheckedChanged -= new System.EventHandler(checkBox2_CheckedChanged);
                this.checkBox2.Checked = isAutoProcessChar;
                this.checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);
                MessageBox.Show("字符训练库没有准备好，请先训练一次");
                return;
            }

            isAutoProcessChar = !isAutoProcessChar;
        }
    }
}
