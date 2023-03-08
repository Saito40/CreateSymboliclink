using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateSymboliclink
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        //作成元フォルダの場所選択
        private void button1_Click(object sender, EventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                InitialDirectory = this.textBox1.Text,
                IsFolderPicker = true,
            })
            {
                if(cofd.ShowDialog() == CommonFileDialogResult.Ok)
                    this.textBox1.Text = cofd.FileName;
            }
        }

        //作成するフォルダの場所選択
        private void button2_Click(object sender, EventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                InitialDirectory = this.textBox2.Text,
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() == CommonFileDialogResult.Ok)
                    this.textBox2.Text = cofd.FileName;
            }
        }

        //実行
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.textBox1.Text))
                    throw new Exception("作成元フォルダの場所を入力してください。");
                if (string.IsNullOrEmpty(this.textBox2.Text))
                    throw new Exception("作成するフォルダの場所を入力してください。");
                if (!Directory.Exists(this.textBox1.Text))
                    throw new Exception("作成元フォルダの場所が見つかりませんでした。");
                if (!Directory.Exists(this.textBox2.Text))
                    throw new Exception("作成するフォルダの場所が見つかりませんでした。");
                var createSymboliclinkPath = Path.Combine(this.textBox2.Text, this.textBox3.Text);
                if (Directory.Exists(createSymboliclinkPath))
                    throw new Exception("作成するフォルダ名と同じフォルダ名があります。別の名前にしてください。");

                //Directory.CreateSymbolicLink(createSymboliclinkPath, this.textBox1.Text);
                Process proc = new Process();
                proc.StartInfo.FileName = "cmd.exe";
                // 管理者として実行
                proc.StartInfo.Verb = "RunAs";
                proc.StartInfo.Arguments = "/c mklink /D " + createSymboliclinkPath + " " + this.textBox1.Text;
                proc.StartInfo.CreateNoWindow = true;

                proc.Start();
                proc.WaitForExit();
                if(proc.ExitCode != 0)
                    throw new Exception("エラーで実行できませんでした。" + proc.ExitCode.ToString());
                proc.Close();

                MessageBox.Show("作成できました。" + Environment.NewLine + createSymboliclinkPath);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
