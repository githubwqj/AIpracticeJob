using Baidu.Aip.Speech;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace speechRecognition
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("winmm.dll", SetLastError = true)]
        static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        string appId = "22836516";
        string apiKey = "XjN1hGG9pVcBtiOef4sGREAw";
        string secretKey = "aD7p9zx2ung6nZccvmtUXcEPYWzQ367v";
        string openfilePath = "";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        //选择文件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Filter = "files|*.wav;*.pcm;*.amr;",
                FilterIndex = 1,
                Multiselect = false
            };

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                openfilePath = dialog.FileName;
                mp3Link.Text = openfilePath;
            }
        }

        //音频识别
        private void startRec_Click(object sender, RoutedEventArgs e)
        {
            recognizeResult.Text = "";
            Asr asr = new Asr(appId, apiKey, secretKey);
            if (openfilePath == "")
            {
                recognizeResult.Text = "先选取文件";
                return;
            }
            if (!File.Exists(openfilePath))
            {
                return;
            }
            FileInfo fileinfo = new FileInfo(openfilePath);
            byte[] data = new byte[fileinfo.Length];
            FileStream fs = fileinfo.OpenRead();
            fs.Read(data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            string selectMode = comboBox.Text.ToString();
            JObject obj = asr.Recognize(data, selectMode, 16000);
            if (obj["err_msg"].ToString() != "success.")
            {
                recognizeResult.Text = obj["err_msg"].ToString();
                return;
            }
            Array output_ratio = obj["result"].ToArray();
            string ss = output_ratio.GetValue(0).ToString();
            recognizeResult.Text = ss;
        }

        //音频合成
        private void startCreate_Click(object sender, RoutedEventArgs e)
        {
            string text = textInput.Text;
            Tts tts = new Tts(apiKey, secretKey);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("aue", "6");
            TtsResponse result = tts.Synthesis(text, dic);
            if (!result.Success)
            {
                MessageBox.Show("合成失败");
                return;
            }
            Byte[] data = result.Data;
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"result.wav");
            Bytes2File(data, filePath);
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
        public static void Bytes2File(byte[] data, string savepath)
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