using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetGame
{
    public partial class ChoiceForm : Form
    {
        string[] adress;
        public ChoiceForm(string[] ip)
        {
            InitializeComponent();
            adress = ip;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adress[0] = "";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            button3.Visible = true;
            textBox1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            adress[0] = textBox1.Text;
            this.Close();
        }
    }
}
