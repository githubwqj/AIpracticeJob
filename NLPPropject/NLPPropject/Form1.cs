using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NLPPropject
{
    public partial class Form1 : Form
    {
       static String APP_ID = "22887788";
       static String API_KEY = "8RAAQ5ujTSBEnDWBxmyafuEC";
       static String SECRET_KEY = "l4ilYyr48NYGZZ3VfjkQngO0x7QHAdTd";
       static Baidu.Aip.Nlp.Nlp client = null;
       private readonly Dictionary<string, string> POSTable =
          new Dictionary<string, string>() {
          {"",""},{"a", "形容词"},
                                               {"ad", "副形词"},
                                               {"ag", "形语素"},
                                               {"an", "名形词"},
                                               {"b", "区别词"},
                                               {"c", "连词"},
                                               {"d", "副词"},
                                               {"dg", "副语素"},
                                               {"e", "叹词"},
                                               {"f", "方位名词"},
                                               {"g", "语素"},
                                               {"h", "前接成分"},
                                               {"i", "成语"},
                                               {"j", "简称略语"},
                                               {"k", "后接成分"},
                                               {"l", "习惯语"},
                                               {"m", "数量词"},
                                               {"n", "普通名词"}, 
                                               {"ng", "名语素"},
                                               {"nr", "人名"},
                                               {"ns", "地名"},
                                               {"nt", "机构团体名"},
                                               {"nw", "作品名"},
                                               {"nz", "其他专名"},
                                               {"o", "拟声词"},
                                               {"p", "介词"},
                                               {"q", "量词"},
                                               {"r", "代词"},
                                               {"s", "处所名词"},
                                               {"t", "时间名词"},
                                               {"tg", "时语素"},
                                               {"u", "助词"},
                                               {"un","未知词"},
                                               {"v", "普通动词"},
                                               {"vd", "动副词"},
                                               {"vg", "动语素"},
                                               {"vn", "动名词"},
                                               {"w", "标点符号"},
                                               {"x", "非语素字"},
                                               {"xc", "其他虚词"},
                                               {"y", "语气词"},
                                               {"z", "状态词"}};

       // 专名缩写表
       private readonly Dictionary<string, string> NETable =
           new Dictionary<string, string>() { {"PER", "人名"}, 
                                               {"LOC", "地名"},
                                               {"ORG", "机构名"},
                                               {"TIME", "时间"},
                                               {"","/"}};

       // 依存关系缩写
       private readonly Dictionary<string, string> DEPRELTable =
           new Dictionary<string, string>() { {"ATT", "定中关系"}, 
                                               {"QUN", "数量关系"},
                                               {"COO", "并列关系"},
                                               {"APP", "同位关系"},
                                               {"ADJ", "附加关系"},
                                               {"VOB", "动宾关系"},
                                               {"POB", "介宾关系"},
                                               {"SBV", "主谓关系"},
                                               {"SIM", "比拟关系"},
                                               {"TMP", "时间关系"},
                                               {"LOC", "处所关系"},
                                               {"DE", "“的”字结构"},
                                               {"DI", "“地”字结构"},
                                               {"DEI", "“得”字结构"},
                                               {"SUO", "“所”字结构"},
                                               {"BA", "“把”字结构"},
                                               {"BEI", "“被”字结构"},
                                               {"ADV", "状中结构"},
                                               {"CMP", "动补结构"},
                                               {"DBL", "兼语结构"},
                                               {"CNJ", "关联词"},
                                               {"CS", "关联结构"},
                                               {"MT", "语态结构"},
                                               {"VV", "连谓结构"},
                                               {"HED", "核心"},
                                               {"FOB", "前置宾语"},
                                               {"DOB", "双宾语"},
                                               {"TOP", "主题"},
                                               {"IS", "独立结构"},
                                               {"IC", "独立分句"},
                                               {"DC", "依存分句"},
                                               {"VNV", "叠词关系"},
                                               {"YGC", "一个词"},
                                               {"WP", "标点"},
                                               {"",""}};

        public Form1()
        {
            InitializeComponent();
            //依存句法分析
             client = new Baidu.Aip.Nlp.Nlp(API_KEY, SECRET_KEY);
             client.Timeout = 60000;  // 修改超时时间
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //词法分析
         
          if (textBox1.Text == "") {
              MessageBox.Show("请输入内容");
              return;

          }
          JObject obj = client.Lexer(textBox1.Text);
          JArray arr = (JArray)obj["items"];
          var num = 0;
          foreach (var item in arr)  //查找某个字段与值
          {
              ListViewItem litem = new ListViewItem();
              litem.Text = Convert.ToString(num++);                             //序号
              litem.SubItems.Add(Convert.ToString(item["item"]));                      //分词
              var pos = Convert.ToString(item["pos"]);
              litem.SubItems.Add(Convert.ToString(POSTable[pos]));                      //词性
              litem.SubItems.Add(Convert.ToString(item["item"]));                      //基本词
              listView1.Items.Add(litem);        
          }

          Console.WriteLine(obj["items"]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入内容");
                return;

            }
	
            var mode = comboBox1.Text == 
                "web模型" ? new Dictionary<string, object>{{"mode", 1}} : new Dictionary<string, object>{{"mode", 0}};
            JObject depResult = client.DepParser(textBox1.Text, mode);
            JToken[] itemArr = depResult.GetValue("items").ToArray();
            int head = 0;
            List<TreeNode> treeNodeList = new List<TreeNode>();
            List<int> idList = new List<int>();
            int index = 0;
            foreach (JToken item in itemArr)
            {
                int headId = item.Value<int>("head");
                if (head == headId)
                {
                    int id = item.Value<int>("id");
                    idList.Add(id);
                    string word = item.Value<string>("word");
                    string deprel = item.Value<string>("deprel");
                    string content = word + "（" + DEPRELTable[deprel] + "）";
                    treeNodeList.Add(new TreeNode(content));
                }
            }
            while (index < treeNodeList.Count)
            {
                TreeNode pTreeNode = treeNodeList.ElementAt(index);
                head = idList.ElementAt(index++);
                foreach (JToken item in itemArr)
                {
                    int headId = item.Value<int>("head");
                    if (head == headId)
                    {
                        int id = item.Value<int>("id");
                        idList.Add(id);
                        string word = item.Value<string>("word");
                        string deprel = item.Value<string>("deprel");
                        string content = word + "（" + DEPRELTable[deprel] + "）";
                        TreeNode treeNode = new TreeNode(content);
                        pTreeNode.Nodes.Add(treeNode);
                        treeNodeList.Add(treeNode);
                    }
                }
            }
            treeView2.Nodes.Add(treeNodeList.ElementAt(0));
            treeView2.ExpandAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //dnn模型
            if(textBox1.Text==""){
                MessageBox.Show("请输入内容");
                return;
            }
            JObject obj = client.DnnlmCn(textBox1.Text);
            textBox2.Text = Convert.ToString(obj["ppl"]);
            JArray arr = (JArray)obj["items"];
            var num = 0;
            foreach (var item in arr)  //查找某个字段与值
            {
                ListViewItem litem = new ListViewItem();
                litem.Text = Convert.ToString(num++);                             //序号
                litem.SubItems.Add(Convert.ToString(item["word"]));                      //单词
                litem.SubItems.Add(Convert.ToString(item["prob"]));                      //概率
                listView2.Items.Add(litem);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //短文本相似度
            // 如果有可选参数
            var options = new Dictionary<string, object>{};
            options.Add("model", comboBox2.Text);
            // 带参数调用短文本相似度
            JObject obj = client.Simnet(richTextBox1.Text, richTextBox2.Text, options);
            textBox3.Text = Convert.ToString(obj["score"]);
            Console.WriteLine(obj);
        }

        private void button5_Click(object sender, EventArgs e)
        {
             //评论观点
            // 如果有可选参数
            var options = new Dictionary<string, object>{
	    {"type", 13}
	};
            // 带参数调用评论观点抽取
            JObject obj = client.CommentTag(richTextBox3.Text, options);
            richTextBox4.Text = Convert.ToString(obj);
            Console.WriteLine(obj);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //情感分析
            // 调用情感倾向分析，可能会抛出网络等异常，请使用try/catch捕获
            JObject obj = client.SentimentClassify(richTextBox3.Text);
            richTextBox4.Text = Convert.ToString(obj);
            Console.WriteLine(obj);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //词向量

            // 调用词向量表示，可能会抛出网络等异常，请使用try/catch捕获
            JObject obj = client.WordEmbedding(textBox4.Text);
            richTextBox5.Text = Convert.ToString(obj["vec"]);
            Console.WriteLine(obj);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //词意相似度
            // 调用词义相似度，可能会抛出网络等异常，请使用try/catch捕获
            JObject obj = client.WordSimEmbedding(textBox5.Text, textBox6.Text);
            textBox7.Text=Convert.ToString(obj["score"]);
            Console.WriteLine(obj);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //文章标签
            // 调用文章标签，可能会抛出网络等异常，请使用try/catch捕获
            var result = "";
            JObject obj = client.Keyword(textBox8.Text, richTextBox6.Text);
            JArray arr = (JArray)obj["items"];
            if (arr != null && arr.Count != 0)
            {
                foreach (var item in arr)  //查找某个字段与值
                {

                    result = result+Convert.ToString(item["tag"]) + ":" + Convert.ToString(item["score"])+"\n";                          //序号

                }
            }
            richTextBox7.Text = Convert.ToString(result);
            Console.WriteLine(obj);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //文章分类
            // 调用文章分类，可能会抛出网络等异常，请使用try/catch捕获
            var result = "";
            JObject obj = client.Topic(textBox8.Text, richTextBox6.Text);
            JObject items = (JObject)obj["items"];
            JArray l1_arr = (JArray)obj["lv1_tag_list"];
            JArray l2_arr = (JArray)obj["lv2_tag_list"];
            if (l1_arr!=null&&l1_arr.Count != 0)
            {
                result = result + "一级分类结果:\n";
            foreach (var item in l1_arr)  //查找某个字段与值
            {

                result = result + "\t\t " + Convert.ToString(item["tag"]) + ":" + Convert.ToString(item["score"]) ;                          //序号
                
            }
            }
            if (l2_arr != null && l2_arr.Count != 0)
            {
                result = result + "二级分类结果:\n";
                foreach (var item in l2_arr)  //查找某个字段与值
                {

                    result = result + "\t\t " + Convert.ToString(item["tag"]) + ":" + Convert.ToString(item["score"]);                          //序号

                }
            }
            richTextBox7.Text = Convert.ToString(result);
            Console.WriteLine(obj);
        }

       
    }
}
