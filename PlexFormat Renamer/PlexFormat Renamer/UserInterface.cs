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

namespace PlexFormat_Renamer
{
    public partial class UserInterface : Form
    {
        public UserInterface()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                    NameHandler.Process(directory);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private List<string> ReadFile(string fn)
        {
            List<string> l = new List<string>();
            using (StreamReader input = new StreamReader(fn))
            {
                while (!input.EndOfStream)
                {
                    string name = input.ReadLine();
                    l.Add(name);
                }
                return l;
            }
        }
    }
}
