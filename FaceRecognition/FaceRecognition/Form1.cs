using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FaceRecognition
{
    public partial class Form1 : Form
    {
        static String AppID = "22913473";
        static String APIKEY = "Cjbt9w9LX0IXoeG9VyrAapjn";
        static String SECRETKEY = "n24t6wZsbLI3bqBjjmODZPBBdhrTvG7o";
        static String access_token = "";
        public static String getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", APIKEY));
            paraList.Add(new KeyValuePair<string, string>("client_secret", SECRETKEY));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            JObject  result = (JObject)JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

            return Convert.ToString(result["access_token"]);
        }
        public Form1()
        {
            InitializeComponent();
            access_token = getAccessToken();
            
        }
     
        public string filepath1 = string.Empty;
        public string filepath2 = string.Empty;
        static Baidu.Aip.ImageClassify.ImageClassify client= null;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    filepath1 = file.FileName;   //获得文件的绝对路径
                    FileStream fs = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
                    BinaryReader br = new BinaryReader(fs);
                    byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
                    MemoryStream ms = new MemoryStream(imgBytesIn);
                    pictureBox1.Image = Image.FromStream(ms);

                    //进行图片检测
                    string host = "https://aip.baidubce.com/rest/2.0/face/v3/detect?access_token=" + access_token;
                    Encoding encoding = Encoding.Default;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                    request.Method = "post";
                    request.KeepAlive = true;
                    JObject jobj = new JObject { { "image_type", "BASE64" }, { "face_field", 
                                                                                 "age,beauty,expression,face_shape,gender" } };
                    jobj.Add("image", Convert.ToBase64String(imgBytesIn));
                    string jResult = JsonConvert.SerializeObject(jobj);
                    byte[] buffer = encoding.GetBytes(jResult);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    JObject result = (JObject)((JObject)JsonConvert.DeserializeObject(Convert.ToString(reader.ReadToEnd())))["result"];
                    JArray arr = (JArray)result["face_list"];
                    StringBuilder sb = new StringBuilder("");
                    sb.Append("估算年龄:" + Convert.ToString(arr[0]["age"]));
                    sb.Append("\n");
                    sb.Append("样貌评分:" + Convert.ToString(arr[0]["beauty"]));
                    sb.Append("\n");
                    sb.Append("表情:" + expressionDict[Convert.ToString(arr[0]["expression"]["type"])]);
                    sb.Append("\n");
                    sb.Append("脸型:" + shapeDict[Convert.ToString(arr[0]["face_shape"]["type"])]);
                    sb.Append("\n");
                    sb.Append("性别:" + genderDict[Convert.ToString(arr[0]["gender"]["type"])]);
                    sb.Append("\n");
                    richTextBox3.Text = sb.ToString();
                    
                    


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }  
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = ".";
            file.Filter = "所有文件(*.*)|*.*";
            file.ShowDialog();
            if (file.FileName != string.Empty)
            {
                try
                {
                    filepath2 = file.FileName;   //获得文件的绝对路径
             

                    FileStream fs = new FileStream(filepath2, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
                    BinaryReader br = new BinaryReader(fs);
                    byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中 二进制数组
                    MemoryStream ms = new MemoryStream(imgBytesIn);
                    pictureBox2.Image = Image.FromStream(ms);

                    //进行图片检测
                    string host = "https://aip.baidubce.com/rest/2.0/face/v3/detect?access_token=" + access_token;
                    Encoding encoding = Encoding.Default;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
                    request.Method = "post";
                    request.KeepAlive = true;
                    JObject jobj = new JObject { { "image_type", "BASE64" }, { "face_field", 
                                                                                 "age,beauty,expression,face_shape,gender" } };
                    jobj.Add("image", Convert.ToBase64String(imgBytesIn));
                    string jResult = JsonConvert.SerializeObject(jobj);
                    byte[] buffer = encoding.GetBytes(jResult);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                    JObject result = (JObject)((JObject)JsonConvert.DeserializeObject(Convert.ToString(reader.ReadToEnd())))["result"];
                    JArray arr = (JArray)result["face_list"];
                    StringBuilder sb = new StringBuilder("");
                    sb.Append("估算年龄:" + Convert.ToString(arr[0]["age"]));
                    sb.Append("\n");
                    sb.Append("样貌评分:" + Convert.ToString(arr[0]["beauty"]));
                    sb.Append("\n");
                    sb.Append("表情:" + expressionDict[Convert.ToString(arr[0]["expression"]["type"])]);
                    sb.Append("\n");
                    sb.Append("脸型:" + shapeDict[Convert.ToString(arr[0]["face_shape"]["type"])]);
                    sb.Append("\n");
                    sb.Append("性别:" + genderDict[Convert.ToString(arr[0]["gender"]["type"])]);
                    sb.Append("\n");
                    richTextBox4.Text = sb.ToString();
                    



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 

        }

        Dictionary<string, string> genderDict = new Dictionary<string, string> { { "male", "男性" }, { "female", "女性" } };
        Dictionary<string, string> expressionDict = new Dictionary<string, string> { { "none", "不笑" }, { "smile", "微笑" }, { "laugh", "大笑" } };
        Dictionary<string, string> shapeDict = new Dictionary<string, string> { { "square", "正方形 " }, { "triangle", "三角形 " }, { "oval", "椭圆 " }, { "heart", "心形" }, { "round", "圆形" } };

        private void button1_Click(object sender, EventArgs e)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/match?access_token=" + access_token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;

            //获取图片
            JObject jobj1 = new JObject { { "image_type", "BASE64" }, { "face_type", "LIVE" }, { "quality_control", "LOW" } };
            FileStream fs1 = new FileStream(filepath1, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br1 = new BinaryReader(fs1);
            byte[] imgBytesIn1 = br1.ReadBytes((int)fs1.Length); //将流读入到字节数组中 二进制数组
            jobj1.Add("image", Convert.ToBase64String(imgBytesIn1));



            JObject jobj2 = new JObject { { "image_type", "BASE64" }, { "face_type", "LIVE" }, { "quality_control", "LOW" } };
            FileStream fs2 = new FileStream(filepath2, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br2 = new BinaryReader(fs2);
            byte[] imgBytesIn2 = br2.ReadBytes((int)fs2.Length);
            jobj2.Add("image", Convert.ToBase64String(imgBytesIn2));




            JArray arr = new JArray();
            arr.Add(jobj1);
            arr.Add(jobj2);

            byte[] buffer = encoding.GetBytes(JsonConvert.SerializeObject(arr));
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            JObject face_comparison_result = (JObject)((JObject)JsonConvert.DeserializeObject(Convert.ToString(reader.ReadToEnd())))["result"];
            richTextBox1.Text = Convert.ToString(face_comparison_result["score"]);

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
