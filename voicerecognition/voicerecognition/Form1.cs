using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Baidu.Aip.Speech;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.Runtime.InteropServices;



namespace voicerecognition
{
    public partial class Form1 : Form
    {
        
        string APP_ID = "22850886";
        string API_KEY = "ELkjDrARSz6IeSWF8MIQOuti";
        string SECRET_KEY = "qV901iE2BfFZDCs1GjYnZu5Nm2syMfon";
        string openfilePath = "";
        [DllImport("winmm.dll", SetLastError = true)]
        static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择语音文件";
            dialog.Filter = "音频文件(*.pcm,*.wav,*.amr,*.m4a)|*.pcm;*.wav;*.amr;*.m4a";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                openfilePath = dialog.FileName;
                textBox.Text = dialog.FileName;  //获取文件路径
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            var client=new Baidu.Aip.Speech.Asr(API_KEY,SECRET_KEY);
            client.Timeout=6000;
            if (openfilePath == "" || comboBox1.Text == "")
            {
                richTextBox1.Text = "请选择语音文件或音频格式";
                return;
            }
            FileInfo fileinfo = new FileInfo(openfilePath);
            byte[] data = new byte[fileinfo.Length];
            FileStream fs = fileinfo.OpenRead();
            fs.Read(data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            string selectMode = comboBox1.Text.ToString();
            JObject obj = client.Recognize(data, selectMode, 16000);
            if (obj["err_msg"].ToString() != "success.")
            {
                richTextBox1.Text = obj["err_msg"].ToString();
                return;
            }
            Array output_ratio = obj["result"].ToArray();
            string ss = output_ratio.GetValue(0).ToString();
            richTextBox1.Text = ss;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "") {
                MessageBox.Show("请输入文件用于语音合成");
                return;
            }

            Tts tts = new Tts(API_KEY, SECRET_KEY);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("aue", "6");
            TtsResponse result = tts.Synthesis(richTextBox2.Text, dic);
            if (!result.Success)
            {
                MessageBox.Show("合成失败");
                return;
            }
            Byte[] data = result.Data;
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"result.wav");
            ToFile(data, filePath);
            // 播放音频文件
            mciSendString("open result.wav alias temp_alias", null, 0, IntPtr.Zero);
            mciSendString("play temp_alias", null, 0, IntPtr.Zero);
            // 等待播放结束
            StringBuilder strReturn = new StringBuilder(64);
            do
            {
                mciSendString("status temp_alias mode", strReturn, 64, IntPtr.Zero);
            } while (!strReturn.ToString().Contains("stopped"));
            // 关闭音频文件
            mciSendString("close temp_alias", null, 0, IntPtr.Zero);

        }

        //生成文件
        public static void ToFile(byte[] data, string savepath)
        {
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }
            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(data, 0, data.Length);
            bw.Close();
            fs.Close();
        }
    }
}
