using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace Socket02_CS_SSPHomework
{
    //C#은 클래스의 다중 상속을 허용하지 않는다.
    public partial class ServerView : Form
    {
        

        public ServerView()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

            //sb.AppendLine("Hello2");
            //sb.AppendLine();

            //textBox6.Text = sb.ToString();
            //SendData();
            //SendData();
            Application.DoEvents();
            this.textBox6.Text = Global.sendData;

            


        }



        public void start()
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }


        //클래스 호출 준비
        ServerClass serverClass = new ServerClass();
        //ServerClass @class = new ServerClass();
        //ServerClass.AsyncObject asyncObject = new AsyncObject();

        // 버튼 클릭 이벤트 구성 시작.
        //int PortNumber;

        private void button1_Click(object sender, EventArgs e)
        {
            string textBox1 = this.textBox1.Text;
            Console.WriteLine("Listen_Button");
            Console.WriteLine(textBox1);
            // 클래스 호출 테스트
            //ServerClass.ServerClassTest();
            //ServerClass.ServerClassStart();
            //ServerClass.Server();

            serverClass.Start(Convert.ToInt32(textBox1));

            //SendData();


        }


        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Close_Button");

            serverClass.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Console.WriteLine("SendFile_Button");

            // 테스트용
            for (int i = 0; i < 1000; i++)
            {
                Application.DoEvents();
                this.textBox6.Text = i.ToString();
                Thread.Sleep(100);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Console.WriteLine("SendTC_Button");

            //테스트용
            this.textBox6.Text = Global.sendData;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string textBox2 = this.textBox2.Text;
            Console.WriteLine(textBox2);
            Console.WriteLine("SendMsg_Button");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Send Msg Clear_Button");
            this.textBox5.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Console.WriteLine("종료(X)_Button");
            Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Receive Msg Clear_Button");
            this.textBox6.Clear();
            
        }


        // 버튼 클릭 이벤트 구성 종료.


        // 텍스트박스

        /*
        private delegate void DataDelegate(string sData);

        private void DelegateFunction(string sData)
        {
            sData = Global.sendData;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(sData);
            textBox6.Text = sb.ToString();
        }
        public void SendData()
        {
            //Console.WriteLine(Global.sendData);
            while (true)
            {
                
                this.Invoke(new DataDelegate(DelegateFunction), "Test!");
            }
        }
        */

        public void SendData()
        {
            

            while(true)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Global.sendData);
                textBox6.Text = sb.ToString();
                Thread.Sleep(10);
            }
            
            


        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            this.textBox6.Text = Global.sendData;
        }
    }
}

