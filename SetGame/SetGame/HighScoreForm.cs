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
    public partial class HighScoreForm : Form
    {
        Label[] Labels = new Label[10];

        public HighScoreForm(List<HighScore> Scores)
        {
            InitializeComponent();

            Labels[0] = label2;
            Labels[1] = label3;
            Labels[2] = label4;
            Labels[3] = label5;
            Labels[4] = label6;
            Labels[5] = label7;
            Labels[6] = label8;
            Labels[7] = label9;
            Labels[8] = label10;
            Labels[9] = label11;

            int size = Scores.Count;

            for(int i = 0; i < size && i < 10; i++)
            {
                Labels[i].Text = (i + 1).ToString() + ". " + Scores[i].points.ToString() + "  " + Scores[i].name.ToString(); 
            }
        }
    }
}
