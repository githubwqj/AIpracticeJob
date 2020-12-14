using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AnalysisOfTheHuman
{
    public partial class Form1 : Form
    {
        static String AppID = "23142978";
        static String APIKEY = "eUb5ZP9oQfyrcbGD4iHVNARo";
        static String SECRETKEY = "3aZl2tCRFQCPfe0UqHOCojddrwRr3ACR";
        String filepath1 = "";
        static Baidu.Aip.BodyAnalysis.Body client = null;
        public Form1()
        {
            InitializeComponent();
            client = new Baidu.Aip.BodyAnalysis.Body(APIKEY, SECRETKEY);
            client.Timeout = 60000;  // 修改超时时间

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            
            filepath1 = file.FileName;   //获得文件的绝对路径
            if (filepath1 == null || filepath1 == "")
                return;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = Image.FromFile(filepath1);
            var image = File.ReadAllBytes(filepath1);
            var result = client.BodyAnalysis(image);
            var person_pix_arr=(JArray)result["person_info"];
            var bmp=new Bitmap(pictureBox1.Image);
            Graphics g = Graphics.FromImage(bmp);//创建一个画板
            foreach (JObject person_pix in person_pix_arr) {
                foreach (var item in (JObject)person_pix["body_parts"])
                {
                    var x = item.Value["x"];
                    var y = item.Value["y"];
                    g.DrawEllipse(new Pen(Color.Red, 5), Convert.ToInt32(x), Convert.ToInt32(y), 4, 4);
                }
            }
            pictureBox1.Image = bmp;
                
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            
            filepath1 = file.FileName;   //获得文件的绝对路径
            if (filepath1 == null || filepath1 == "")
                return;
            FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
            MemoryStream ms = new MemoryStream(imgBytesIn);
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = Image.FromStream(ms);
            var bmp = new Bitmap(pictureBox2.Image);
            //Graphics g = Graphics.FromImage(bmp);//创建一个画板
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
    	    
    	    {"show", "true"}
    	};
            // 带参数调用人流量统计
            var result = client.BodyNum(image, options);
            textBox1.Text = Convert.ToString(result["person_num"]);

           // Console.WriteLine(result); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            
            filepath1 = file.FileName;   //获得文件的绝对路径
            if (filepath1 == null || filepath1 == "")
                return;
            FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
            MemoryStream ms = new MemoryStream(imgBytesIn);
            pictureBox3.Image = Image.FromStream(ms);

            var image = File.ReadAllBytes(filepath1);
            // 调用人体检测与属性识别，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.Gesture(image);
            StringBuilder sb = new StringBuilder("");
            foreach (var chars in (JArray)result["result"])  //循环获取值
            {
                JObject jo = (JObject)chars;
                sb.Append("classname:" + Convert.ToString(jo["classname"]) + "\n");
                sb.Append("probability:" + Convert.ToString(jo["probability"]) + "\n\n\n");


            }
            richTextBox1.Text = sb.ToString();
           // Console.WriteLine(result); 

        }
    }
}
