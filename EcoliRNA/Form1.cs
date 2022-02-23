using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using RDotNet;

namespace EcoliRNA
{
    public partial class Form1 : Form
    {
        string Rroot;
        string Executeroot;
        string sSelectedFile;
        string fastqfile1;
        string fastqfile2;
        string sIndexFile;
        string Rarg;
        Dictionary<string, string> control_samples= new Dictionary<string, string>();
        Dictionary<string, string> case_samples = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
        }

         private void button1_Click(object sender, EventArgs e)
        {

                OpenFileDialog choofdlog = new OpenFileDialog();
                choofdlog.Filter = "All Files (*.*)|*.*";
                choofdlog.FilterIndex = 1;
                choofdlog.Multiselect = true;

                if (choofdlog.ShowDialog() == DialogResult.OK)
                    sSelectedFile = choofdlog.FileName;
                else
                    sSelectedFile = string.Empty;
                if(textBox2.Text == String.Empty)
                {
                    fastqfile1 = sSelectedFile;
                   textBox2.Text =  Path.GetFileName(sSelectedFile); 
                }
                if (textBox2.Text!=String.Empty)
                {
                    fastqfile2= sSelectedFile;
                    textBox3.Text = Path.GetFileName(sSelectedFile);
                }
         }

        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = true;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                sIndexFile = Path.GetFileName(choofdlog.FileName);
                textBox1.Text = sIndexFile;
            }
            else
            {
                sIndexFile = string.Empty;
                textBox1.Text = sIndexFile;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strCmdText="";
           if(textBox4.Text==string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Enter The Index Name");
            }
            else
            {
                strCmdText = "/C  "+Executeroot + "\\"+"kallisto.exe index  --index=" + textBox4.Text + "  "+textBox1.Text;
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            }
            textBox4.Text = strCmdText;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strCmdText;

            if (textBox2.Text == string.Empty & textBox3.Text == string.Empty)
            {
                System.Windows.Forms.MessageBox.Show("Please Enter the fastq file");
            }
            else
            {

                 strCmdText = "/C  " + Executeroot + "\\"  + "kallisto.exe quant  -i  " + textBox5.Text + " -o  "+ textBox6.Text + "  "+textBox2.Text+"   "+ textBox3.Text;
                 textBox5.Text = strCmdText;
                 System.Diagnostics.Process.Start("CMD.exe", strCmdText);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            pictureBox1.ImageLocation = Executeroot + "\\"  +"geneexp.png";
        }

        private void button6_Click(object sender, EventArgs e)
        {
// using (OpenFileDialog Dialog = new OpenFileDialog { Filter = "All Files|*.*", Title = "OpenFile Dialog", RestoreDirectory = true })
            FolderBrowserDialog Dialog = new System.Windows.Forms.FolderBrowserDialog();
            {
                
                FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {


                    listBox1.Items.Add(folderBrowserDialog1.SelectedPath);
                    //MessageBox.Show(folderBrowserDialog1.SelectedPath);
                }
                case_samples.Add(Path.GetFileName(folderBrowserDialog1.SelectedPath), folderBrowserDialog1.SelectedPath);
                
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dialog = new System.Windows.Forms.FolderBrowserDialog();
            {
                FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    listBox2.Items.Add(folderBrowserDialog1.SelectedPath);
                    //MessageBox.Show(folderBrowserDialog1.SelectedPath);
                }
                control_samples.Add(Path.GetFileName(folderBrowserDialog1.SelectedPath), folderBrowserDialog1.SelectedPath);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string file = Path.Combine(Executeroot + "\\" , "target.txt");
            string text ="Sample,Treatment\n";
            Rarg = " target.txt";
            foreach (var item in control_samples)
            {
                text += item.Key+","+"0\n";
                Rarg += "  " + item.Key + "\\\\abundance.tsv    ";
            }
            foreach (var item in case_samples)
            {
                text += item.Key +","+"1\n";
                Rarg += "  "+item.Key + "\\\\abundance.tsv   ";
            }
            File.WriteAllText(file, text);

           

        }

        private void button9_Click(object sender, EventArgs e)
        {
            control_samples.Clear();
            case_samples.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();

        }

        private void button10_Click(object sender, EventArgs e)
        {
           
            pictureBox2.ImageLocation = Executeroot + "\\" + "correlation.png";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = File.ReadAllText(Executeroot + "\\" + "limma+result.txt");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string strCmdText = Rroot + "Rscript.exe" + "  " + Executeroot + "Rscript.r";
            MessageBox.Show(Executeroot + "Rscript.r");
            MessageBox.Show(Rroot +"Rscript.exe");
            MessageBox.Show(strCmdText);
            Rarg = "";
           // RScriptRunner.RunFromCmd(Executeroot  + "Rscript.r", Rroot + "Rscript.exe", Rarg);
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Rroot = textBox7.Text.Replace("\\","\\\\");

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            Executeroot = textBox8.Text.Replace("\\", "\\\\");
        }
    }
    /// <summary>
    /// This class runs R code from a file using the console.
    /// </summary>
    public class RScriptRunner
    {

        /// <summary>
        /// Runs an R script from a file using Rscript.exe.
        /// Example:  
        ///   RScriptRunner.RunFromCmd(curDirectory + @"\ImageClustering.r", "rscript.exe", curDirectory.Replace('\\','/'));
        /// Getting args passed from C# using R:
        ///   args = commandArgs(trailingOnly = TRUE)
        ///   print(args[1]);
        /// </summary>
        /// <param name="rCodeFilePath">File where your R code is located.</param>
        /// <param name="rScriptExecutablePath">Usually only requires "rscript.exe"</param>
        /// <param name="args">Multiple R args can be seperated by spaces.</param>
        /// <returns>Returns a string with the R responses.</returns>
        public static string RunFromCmd(string rCodeFilePath, string rScriptExecutablePath, string args)
        {
            string file = rCodeFilePath;
            string result = string.Empty;

            try
            {

                var info = new ProcessStartInfo();
                info.FileName = rScriptExecutablePath;
                info.WorkingDirectory = Path.GetDirectoryName(rScriptExecutablePath);
                info.Arguments = rCodeFilePath + " " + args;

                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    result = proc.StandardOutput.ReadToEnd();
                    Console.Write(result);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("R Script failed: " + result, ex);
            }
        }
    }
}
