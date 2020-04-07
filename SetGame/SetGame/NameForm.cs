using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace SetGame
{
    public partial class NameForm : Form
    {
        HighScore thisScore;
        List<HighScore> HighScores;
        public NameForm(List<HighScore> HighScoreList, int theScore)
        {
            InitializeComponent();
            label2.Text = "Congratulations! You scored " + theScore;
            thisScore = new HighScore();
            thisScore.points = theScore;
            HighScores = HighScoreList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                thisScore.name = textBox1.Text;
            }

            HighScores.Add(thisScore);
            HighScoreSort();
            
            if(HighScores.Count > 50)
            {
                List<HighScore> temp = new List<HighScore>();
                for(int i = 0; i < 50; i++)
                {
                    temp[i] = HighScores[i];
                }
                HighScores = temp;
            }

            var serializer = new XmlSerializer(HighScores.GetType(), "HighScores.Scores");
            using (var writer = new StreamWriter("highscores.xml", false))
            {
                serializer.Serialize(writer.BaseStream, HighScores);
            }

            var _HighScoreForm = new HighScoreForm(HighScores);
            _HighScoreForm.Show();
            this.Close();
        }

        public void HighScoreSort()
        {
            HighScore temp;
            int size = HighScores.Count;

            for (int i = size - 1; i > 0; i--)
            {
                if (HighScores[i].points > HighScores[i - 1].points)
                {
                    temp = HighScores[i];
                    HighScores[i] = HighScores[i - 1];
                    HighScores[i - 1] = temp;
                }
            }
        }
    }
}
