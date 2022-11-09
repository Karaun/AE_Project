using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace AE教程2
{
    public partial class RealMap : Form
    {
        public ChromiumWebBrowser baidubrowser;
        public ChromiumWebBrowser gaodebrowser;
        public ChromiumWebBrowser gugebrowser;
        public ChromiumWebBrowser Diybrowser;
        public RealMap()
        {
            InitializeComponent();
            InitbaiduBrowser();
            InitgaodeBrowser();
           
            //创建方法，初始化全局组件后启动浏览器
        }

        //百度地图模组
        private void InitbaiduBrowser()
        {
            try
            {
                CefSettings settings = new CefSettings();
                //使用提供的设置初始化cef
                Cef.Initialize(settings);
                //创建浏览器组件
                string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                baidubrowser = new ChromiumWebBrowser(path + @"百度地图.html");
                //将其添加到表单并填充到表单窗口。
                panelbaiduMap.Controls.Add(baidubrowser);
                //将browser加载到panel1中
                baidubrowser.Dock = DockStyle.Fill;
            }
            catch
            {

            }

        }

        //高德地图模组
        private void InitgaodeBrowser()
        {
            try
            {
                panelgoogleMap.Visible = true;
                gaodebrowser = new ChromiumWebBrowser(@"https://www.amap.com/");
                panelgoogleMap.Controls.Add(gaodebrowser);
                gaodebrowser.Dock = DockStyle.Fill;
            }
            catch
            {

            }

        }



        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void 重启页面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否刷新页面", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
                RealMap r1 = new RealMap();
                r1.Show();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            
        }

        //自定义载入模组
        private void button1_Click(object sender, EventArgs e)
        {
                Diybrowser = new ChromiumWebBrowser(textBox1.Text);
                panelDiyMap.Controls.Add(Diybrowser);
                Diybrowser.Dock = DockStyle.Fill;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //点击回车就可实现跳转
            if (e.KeyChar == '\r')
            {
                button1.Focus();
                button1_Click(this, new EventArgs());
            }

        }

        private void RealMap_Load(object sender, EventArgs e)
        {
            MessageBox.Show("实地地图载入受网速影响,自定义载入默认谷歌地图(需要使用VPN)", "提示");
        }

        private void 问题修复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            baidubrowser = new ChromiumWebBrowser(path + @"百度地图.html");
            //将其添加到表单并填充到表单窗口。
            panelbaiduMap.Controls.Add(baidubrowser);
            //将browser加载到panel1中
            baidubrowser.Dock = DockStyle.Fill;
        }
    }
}
