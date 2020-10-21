using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double number = 0,number2=0,result=0;
        int inputnumber;
        bool imode = false,isequal=false;
        enum Operate { none, plus, minus, mutiplication, division }

        Operate mode = Operate.none;
        public Form1()
        {
            InitializeComponent();
            viewbefore.Text = Convert.ToString("");
            viewmid.Text = Convert.ToString("");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            number = number * 10 + 1;
            viewbase.Text = Convert.ToString(number);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            number = number * 10 + 9;
            viewbase.Text = Convert.ToString(number);

        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (mode != Operate.none) {
            //等于===== /
            if (mode == Operate.plus)
            {
                result = number2 + number;
            }
            if (mode == Operate.minus) {
                result = number2 - number;
            }
            if (mode == Operate.mutiplication)
            {
                result = number2 * number;
            }
            if (mode == Operate.division)
            {
                result = number2 / number;
            }
            
            cleanall();
            number = result;
            mode = Operate.none;
            viewbase.Text = Convert.ToString(result);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            number = number * 10 + 2;
            viewbase.Text = Convert.ToString(number);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            number = number * 10 + 3;
            viewbase.Text = Convert.ToString(number);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            number = number * 10 + 4;
            viewbase.Text = Convert.ToString(number);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            number = number * 10 + 5;
            viewbase.Text = Convert.ToString(number);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            number = number * 10 + 6;
            viewbase.Text = Convert.ToString(number);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            number = number * 10 + 7;
            viewbase.Text = Convert.ToString(number);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            number = number * 10 + 8;
            viewbase.Text = Convert.ToString(number);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            number = number * 10;
            viewbase.Text = Convert.ToString(number);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //清除;/
            number = 0;
            viewbase.Text = Convert.ToString(0);
            number2 = 0;
            viewbefore.Text = Convert.ToString("");
            viewmid.Text = Convert.ToString("");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        public void call(int an) {
            if (imode == false)
            {
                number = number * 10 + an;
                viewbase.Text = Convert.ToString(number2);
            }
            else {
                number2= number2 * 10 + an;
                viewbase.Text = Convert.ToString(number2);
            }
        }
        public void cleanall(){
            if (!isequal) {
                number = 0;
                viewbase.Text = Convert.ToString(number);
            }

            viewbefore.Text = "";
            viewmid.Text = "";
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //+++/
            mode = Operate.plus;
            viewmid.Text = "+";
            number2 = number;
            viewbefore.Text = Convert.ToString(number);
            number = 0;
            viewbase.Text = Convert.ToString(number);
            imode = true;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            //+++/
            mode = Operate.minus;
            viewmid.Text = "-";
            number2 = number;
            viewbefore.Text = Convert.ToString(number);
            number = 0;
            viewbase.Text = Convert.ToString(number);
            imode = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //***/
            mode = Operate.mutiplication;
            viewmid.Text = "*";
            number2 = number;
            viewbefore.Text = Convert.ToString(number);
            number = 0;
            viewbase.Text = Convert.ToString(number);
            imode = true;
        }

        private void button13_Click(object sender, EventArgs e)
        { 
            //"///"/
            mode = Operate.division;
            viewmid.Text = "/";
            number2 = number;
            viewbefore.Text = Convert.ToString(number);
            number = 0;
            viewbase.Text = Convert.ToString(number);
            imode = true;

        }

        private void viewbefore_Click(object sender, EventArgs e)
        {

        }
    }
}
