using Baidu.Aip.ImageClassify;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace pictureRecognition
{
    public partial class Form1 : Form
    {
        //static String AppID = "23103727";
        static String APIKEY = "5LDFV8Y6Frfi4vtNUWTSdIlf";
        static String SECRETKEY = "pZWV9OAhQW4kzA3Gg6SEQ9HeK55Py7Hz";
        String filepath1 = "";
        static ImageClassify client = null;
         Boolean Status=true;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            client = new Baidu.Aip.ImageClassify.ImageClassify(APIKEY, SECRETKEY);
            client.Timeout = 60000;
    
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            filepath1 = file.FileName;   //获得文件的绝对路径
            if (filepath1 == null || filepath1 == "") {
                return;
            }
            viewlabel();
            FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
            MemoryStream ms = new MemoryStream(imgBytesIn);
            pictureBox1.Image = Image.FromStream(ms);
            this.DisplayBaike();
            hidelabel();
            

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filepath1 == null || filepath1 == "")
            {

                return;
            }
            viewlabel();
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
	                    {"top_num", 3},
	                {"filter_threshold", "0.7"},
	                {"baike_num", 5}
	                };
            // 带参数调用菜品识别
            JObject result = client.DishDetect(image, options);
            var resultlist = (JArray)(result["result"]);
            StringBuilder sb = new StringBuilder("");
            foreach (var chars in resultlist)  //循环获取值
            {
                JObject jo = (JObject)chars;
                sb.Append("名字:" + Convert.ToString(jo["name"]) + "\n");
                sb.Append("卡路里有无:" + Convert.ToString(jo["has_calorie"])+"\n");
                sb.Append("卡路里:" + Convert.ToString(jo["calorie"]) + "\n");
                sb.Append("置信度:" + Convert.ToString(jo["probability"]) + "\n\n\n");
                

            }
            richTextBox2.Text = sb.ToString();
            hidelabel();
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            if (filepath1 == null || filepath1 == "")
            {

                return;
            }
            viewlabel();
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
	    {"top_num", 3},
	         {"baike_num", 5}
	            };
            // 带参数调用车辆识别
            JObject result = client.CarDetect(image, options);
            var resultlist = (JArray)(result["result"]);
            StringBuilder sb = new StringBuilder("");
            foreach (var chars in resultlist)  //循环获取值
            {
                JObject jo = (JObject)chars;
                sb.Append("名字:" + Convert.ToString(jo["name"]) + "\n年限" + Convert.ToString(jo["year"]) + "\n");
                sb.Append("置信度:" + Convert.ToString(jo["score"]) + "\n\n\n");

            }
            richTextBox2.Text = sb.ToString();
            Status = false;
            hidelabel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filepath1 == null || filepath1 == "")
            {

                return;
            }
            viewlabel();
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
	    {"custom_lib", "true"}
	};
            // 带参数调用logo商标识别
          var  result = client.LogoSearch(image, options);
          var resultlist = (JArray)(result["result"]);
          StringBuilder sb = new StringBuilder("");
          foreach (var chars in resultlist)  //循环获取值
          {
              JObject jo = (JObject)chars;
              sb.Append("名字:" + Convert.ToString(jo["name"]) + "\nlocation:" + Convert.ToString(jo["location"]) + "\n");
              sb.Append("置信度:" + Convert.ToString(jo["probability"]) + "\n\n\n");

          }
          richTextBox2.Text = sb.ToString();
          Status = false;
          hidelabel();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (filepath1 == null || filepath1 == "")
            {

                return;
            }
            viewlabel();
            var image = File.ReadAllBytes(filepath1);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
	    {"top_num", 3},
	    {"baike_num", 5}
	};
            // 带参数调用动物识别
           var result = client.AnimalDetect(image, options);
           var resultlist = (JArray)(result["result"]);
           StringBuilder sb = new StringBuilder("");
           foreach (var chars in resultlist)  //循环获取值
           {
               JObject jo = (JObject)chars;
               sb.Append("名字:" + Convert.ToString(jo["name"]) + "\n");

               sb.Append("置信度:" + Convert.ToString(jo["score"]) + "\n\n\n");

           }
           richTextBox2.Text = sb.ToString();
           Status = false;
           hidelabel();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (filepath1 == null || filepath1 == "")
            {

                return;
            }
            viewlabel();
            var image = File.ReadAllBytes(filepath1);
            var options = new Dictionary<string, object>{
	    {"baike_num", 5}
	};
            // 带参数调用植物识别
           var result = client.PlantDetect(image, options);
           var resultlist = (JArray)(result["result"]);
           StringBuilder sb = new StringBuilder("");
           foreach (var chars in resultlist)  //循环获取值
           {
               JObject jo = (JObject)chars;
               sb.Append("名字:" + Convert.ToString(jo["name"]) + "\n");

               sb.Append("置信度:" + Convert.ToString(jo["score"]) + "\n\n\n");

           }
           richTextBox2.Text = sb.ToString();
           Status = false;
           hidelabel();
        }



        //新建一个线程去展示
        public void DisplayBaike()
        {
            //if (this.InvokeRequired)
           // {
            //    this.Invoke(new MethodInvoker(delegate { DisplayBaike(); }));
             //   return;
            //}
            var options = new Dictionary<string, object>{
	            {"baike_num", 5}
	                };
            var image = File.ReadAllBytes(filepath1);
            JObject returninfo = client.AdvancedGeneral(image, options);
            JObject result = (JObject)((JArray)(returninfo["result"]))[0];

            var resultlist = (JArray)(returninfo["result"]);
            StringBuilder sb = new StringBuilder("");
            foreach (var chars in resultlist)  //循环获取值
            {
                JObject jo = (JObject)chars;
                sb.Append(Convert.ToString(jo["root"]) + ":" + Convert.ToString(jo["keyword"]) + "\n");
                sb.Append("置信度:" + Convert.ToString(jo["score"]) + "\n\n\n");

            }
           // richTextBox2.Text = sb.ToString();
            var baike_info = (JObject)result["baike_info"];
            richTextBox1.Text = Convert.ToString(baike_info["description"]);
            pictureBox2.ImageLocation = Convert.ToString(baike_info["image_url"]);
            
            Status = false;
            

        }
        public void viewlabel() {

           /** if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { viewlabel(); }));
                return;
            }*/
                        toolStripProgressBar1.Visible = true;
                        toolStripStatusLabel1.Visible = false;
                        
                    }
        public void hidelabel()
        {
            /**  if (this.InvokeRequired)
             {
                 this.Invoke(new MethodInvoker(delegate { hidelabel(); }));
                 return;
             }*/

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = true;

        }
               
            
    
        

    }
}
