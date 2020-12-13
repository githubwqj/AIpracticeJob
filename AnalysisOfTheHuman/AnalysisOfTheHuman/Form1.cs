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
            FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
            MemoryStream ms = new MemoryStream(imgBytesIn);
            pictureBox1.Image = Image.FromStream(ms);
            var image = File.ReadAllBytes(filepath1);
            var result = client.BodyAnalysis(image);
            var person_pix=(JObject)((JArray)result["person_info"])[0];
            Graphics g = pictureBox1.CreateGraphics();//创建一个画板
            IEnumerable<JProperty> properties = person_pix.Properties();
            g.FillEllipse(Brushes.Red, 10, 10, 4, 4);
            var b = person_pix.PropertyValues();
            var c = person_pix.HasValues;
            foreach (JToken item in properties)
            {
                while (item.HasValues) { 
               var value = (JObject)item.Next;
               g.FillEllipse(Brushes.Red, Convert.ToInt32(value["x"]), Convert.ToInt32(value["y"]), 4, 4);
                }
            }
            
                
            Console.WriteLine(result); 

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
            pictureBox2.Image = Image.FromStream(ms);
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
    	    {"area", "0,0,100,100,200,200"},
    	    {"show", "false"}
    	};
            // 带参数调用人流量统计
            var result = client.BodyNum(image, options);
            Console.WriteLine(result); 
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
            Console.WriteLine(result); 

        }
    }
}
