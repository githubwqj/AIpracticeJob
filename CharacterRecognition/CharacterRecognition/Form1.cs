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
using Baidu.Aip.Ocr;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CharacterRecognition
{
    public partial class Form1 : Form
    {
        static String AppID = "23073333";
        static String APIKEY = "9cMhROW8K5RiCzo156aXv5xh";
        static String SECRETKEY = "GDKCBn1XO3yvFcwDfk80oqid0svKUvP7";
        static String access_token = "";
        static Ocr client = null;

        public Form1()
        {
            InitializeComponent();
            client = new Baidu.Aip.Ocr.Ocr(APIKEY, SECRETKEY);
            client.Timeout = 60000;  // 修改超时时间
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
            
                textBox1.Text = file.FileName;  //获得文件的绝对路径
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
             if (textBox1.Text == null || textBox1.Text == "") {
                 MessageBox.Show("请选择图片");
                 return;
            }
            var image = File.ReadAllBytes(textBox1.Text);
            // 调用通用文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
             var options = new Dictionary<string, object>();
             JObject result = null;
            if (checkBox2.Checked) {
                options.Add("detect_direction", "true");
            }
          	        
	       
            if (radioButton2.Checked) //判断精度
            {
                
                if (checkBox1.Checked) //判断位置
                {
                    result = client.Accurate(image, options);  //高精度有位置
                }
                else {
                    result = client.AccurateBasic(image, options);//高精度没有位置
                }

            }
            else {
                if (checkBox1.Checked) //判断位置
                {
                    result = client.General(image, options);   //普通精度有位置
                }
                else {

                    result = client.GeneralBasic(image, options);   //普通精度没有位置
                }
                
            }
            String printresult = "";
            
            //输出结果
            JArray arr = (JArray)result["words_result"];
            if (result["direction"] != null)
            {
                printresult = printresult + "direction:" + result["direction"] + "\n";
            }

            foreach (var item in arr)  //循环获取值
            {
                var jo=(JObject)item;
                if (jo["location"] != null) {
                    printresult = printresult +"left:"+ Convert.ToString(jo["location"]["left"])
                        + "|top:" + Convert.ToString(jo["location"]["top"])
                        + "|width:" + Convert.ToString(jo["location"]["width"])
                        + "|height:" + Convert.ToString(jo["location"]["height"]) + "\n";
                }
                if (jo["words"] != null) { 
               
                     printresult = printresult+Convert.ToString(jo["words"])+"\n";
                }
                if (jo["chars"] != null) {
                    JArray chararr = (JArray)jo["chars"];
                     foreach (var chars in chararr)  //循环获取值
                     {
                         if (chars["location"] != null)
                         {
                             printresult = "\t\t" + printresult + "left:" + Convert.ToString(chars["location"]["left"])
                                 + "|top:" + Convert.ToString(chars["location"]["top"])
                                 + "|width:" + Convert.ToString(chars["location"]["width"])
                                 + "|height:" + Convert.ToString(chars["location"]["height"]) + "\n";
                         }
                         printresult = "\t\t"+printresult + Convert.ToString(chars["char"]) + "\n";

                     }

                }

            }
            richTextBox1.Text = printresult;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "") {
                MessageBox.Show("请选择图片");
                return;
            }
            var image = File.ReadAllBytes(textBox1.Text);
            // 调用网络图片文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获

            
            var options = new Dictionary<string, object>();
            if (checkBox2.Checked)
            {
                options.Add("detect_direction", "true");
            }
            var result = client.WebImage(image, options);
            String printresult = "";
            //输出结果
            JArray arr = (JArray)result["words_result"];

            foreach (var item in arr)  //循环获取值
            {
                var jo = (JObject)item;
                printresult = printresult + Convert.ToString(jo["words"]) + "\n";
            }
            richTextBox2.Text = printresult;


          	  
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "") {
                MessageBox.Show("请选择图片");
                return;
            }
            var image = File.ReadAllBytes(textBox1.Text);
            // 调用网络图片文字识别, 图片参数为本地图片，可能会抛出网络等异常，请使用try/catch捕获
             JObject result = null;
            String printresult = "";
             //身份证
             if (radioButton4.Checked || radioButton5.Checked)
             {
                 if (radioButton4.Checked)
                 {
                     result = client.Idcard(image, "front");
                 }
                 else {
                     result = client.Idcard(image, "back");
                 }
             
                 JObject jo = (JObject)result["words_result"];
                
                if (jo["姓名"] != null) {
                    printresult = printresult + ("姓名:" + Convert.ToString(jo["姓名"]["words"]) + "\n");
                }
                if (jo["性别"] != null) {
                    printresult = printresult + ("性别:" + Convert.ToString(jo["性别"]["words"]) + "\n");
                }
                if (jo["民族"] != null) {
                    printresult = printresult + ("民族:" + Convert.ToString(jo["民族"]["words"]) + "\n");
                }
                if (jo["出生"] != null) {
                    printresult = printresult + ("出生:" + Convert.ToString(jo["出生"]["words"]) + "\n");
                }
                if (jo["住址"] != null) {
                    printresult = printresult + ("地址:" + Convert.ToString(jo["住址"]["words"]) + "\n");
                }
                if (jo["公民身份号码"] != null) {
                    printresult = printresult + ("公民身份号码:" + Convert.ToString(jo["公民身份号码"]["words"]) + "\n");
            
                }
             }
            //银行卡
             if (radioButton6.Checked) {
                 result = client.Bankcard(image);
                 printresult = printresult + "银行名称:"+Convert.ToString(result["result"]["bank_name"])+"\n";
                 printresult = printresult + "银行卡号:" +  Convert.ToString(result["result"]["bank_card_number"]) + "\n";
             }
             //驾驶证识别
             if (radioButton8.Checked) {
                 result = client.DrivingLicense(image);
                 printresult = printresult + "证号:" + Convert.ToString(result["words_result"]["证号"]["words"]) + "\n";
                 printresult = printresult + "有效期限:" + Convert.ToString(result["words_result"]["有效期限"]["words"]) + "\n";
                 printresult = printresult + "准驾车型:" + Convert.ToString(result["words_result"]["准驾车型"]["words"]) + "\n";
                 printresult = printresult + "有效起始日期:" + Convert.ToString(result["words_result"]["有效起始日期"]["words"]) + "\n";
                 printresult = printresult + "姓名:" + Convert.ToString(result["words_result"]["姓名"]["words"]) + "\n";
                 printresult = printresult + "国籍:" + Convert.ToString(result["words_result"]["国籍"]["words"]) + "\n";
                 printresult = printresult + "出生日期:" + Convert.ToString(result["words_result"]["出生日期"]["words"]) + "\n";
                 printresult = printresult + "性别:" + Convert.ToString(result["words_result"]["性别"]["words"]) + "\n";
                 printresult = printresult + "初次领证日期:" + Convert.ToString(result["words_result"]["初次领证日期"]["words"]) + "\n";
             }
             //营业执照识别
             if (radioButton7.Checked) {
                 // 调用营业执照识别，可能会抛出网络等异常，请使用try/catch捕获
                 result = client.BusinessLicense(image);
                 printresult = printresult + "单位名称:" + Convert.ToString(result["words_result"]["单位名称"]["words"]) + "\n";
                 printresult = printresult + "法人:" + Convert.ToString(result["words_result"]["法人"]["words"]) + "\n";
                 printresult = printresult + "地址:" + Convert.ToString(result["words_result"]["地址"]["words"]) + "\n";
                 printresult = printresult + "有效期:" + Convert.ToString(result["words_result"]["有效期"]["words"]) + "\n";
                 printresult = printresult + "证件编号:" + Convert.ToString(result["words_result"]["证件编号"]["words"]) + "\n";
                 printresult = printresult + "社会信用代码:" + Convert.ToString(result["words_result"]["社会信用代码"]["words"]) + "\n";
             }
             //行驶证
             if (radioButton8.Checked)
             {
                 // 调用营业执照识别，可能会抛出网络等异常，请使用try/catch捕获
                 result = client.VehicleLicense(image);
                 printresult = printresult + "品牌型号:" + Convert.ToString(result["words_result"]["品牌型号"]["words"]) + "\n";
                 printresult = printresult + "发证日期:" + Convert.ToString(result["words_result"]["发证日期"]["words"]) + "\n";
                 printresult = printresult + "使用性质:" + Convert.ToString(result["words_result"]["使用性质"]["words"]) + "\n";
                 printresult = printresult + "发动机号码:" + Convert.ToString(result["words_result"]["发动机号码"]["words"]) + "\n";
                 printresult = printresult + "号牌号码:" + Convert.ToString(result["words_result"]["号牌号码"]["words"]) + "\n";
                 printresult = printresult + "所有人:" + Convert.ToString(result["words_result"]["所有人"]["words"]) + "\n";
                 printresult = printresult + "住址:" + Convert.ToString(result["words_result"]["住址"]["words"]) + "\n";
                 printresult = printresult + "注册日期:" + Convert.ToString(result["words_result"]["注册日期"]["words"]) + "\n";
                 printresult = printresult + "车辆识别代号:" + Convert.ToString(result["words_result"]["车辆识别代号"]["words"]) + "\n";
                 printresult = printresult + "车辆类型:" + Convert.ToString(result["words_result"]["车辆类型"]["words"]) + "\n";
             }
             //车牌
             if (radioButton9.Checked)
             {
                 // 调用营业执照识别，可能会抛出网络等异常，请使用try/catch捕获
                 result = client.LicensePlate(image);
                 printresult = printresult + "颜色:" + Convert.ToString(result["words_result"]["color"]) + "\n";
                 printresult = printresult + "车牌号:" + Convert.ToString(result["words_result"]["number"]) + "\n";

             }


            richTextBox3.Text = printresult;

        }

        //表格识别结果
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("请选择图片");
                return;
            }
            var image = File.ReadAllBytes(textBox1.Text);

            var result = client.TableRecognitionRequest(image);
            richTextBox4.Text = "11";
        }


        //通用票据识别结果
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("请选择图片");
                return;
            }
            var image = File.ReadAllBytes(textBox1.Text);
            var result = client.Receipt(image);
            JArray  arr= (JArray)result["words_result"];
            String printresult = "";
            foreach (var item in arr)  //循环获取值
            {
                var jo = (JObject)item;
                printresult = printresult + Convert.ToString(jo["words"]) + "\n";
            }

            richTextBox5.Text = printresult;

        }
    }
}
