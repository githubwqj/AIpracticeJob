using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pictureRecognition
{
    public partial class Form1 : Form
    {
        static String AppID = "23103727";
        static String APIKEY = "5LDFV8Y6Frfi4vtNUWTSdIlf";
        static String SECRETKEY = "pZWV9OAhQW4kzA3Gg6SEQ9HeK55Py7Hz";
        String filepath1 = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            filepath1 = file.FileName;   //获得文件的绝对路径
            FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
            MemoryStream ms = new MemoryStream(imgBytesIn);
            pictureBox1.Image = Image.FromStream(ms);
        }
    }
}
