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
using System.Net.Sockets;
using System.Threading;

namespace SetGame
{
    public partial class Form1 : Form
    {
        bool multiplayer;
        int clicked;
        int cardsLeft;
        int cardCounter;
        int score;
        int time;
        System.Windows.Forms.Timer theTimer;
        CardSpace[] Space = new CardSpace[12];
        Card[] Deck = new Card[81];
        List<HighScore> Highscores = new List<HighScore>();
        NameForm _NameForm;
        HighScoreForm _HighScoreForm;
        HelpForm _HelpForm;

        TcpClient client;
        Stream s;
        StreamReader sr;
        StreamWriter sw;
        Thread t;

        delegate void ThreadShowCallback(int sIndex, int cIndex);
        delegate void ThreadEmptyCallback(int sIndex);
        delegate void ThreadScoreCallback(string scores);
        delegate void ThreadEndPhaseCallback();

        public Form1(string ip)
        {
            InitializeComponent();

            if (ip == "") { multiplayer = false; Initialize(); }
            else
            {
                multiplayer = true;
                Initialize();
                try
                {
                    client = new TcpClient(ip, 7777);
                    s = client.GetStream();
                    sr = new StreamReader(s);
                    sw = new StreamWriter(s);
                    sw.AutoFlush = true;
                    multiplayer = true;
                    messageLabel.Text = sr.ReadLine();
                    t = new Thread(new ThreadStart(Networker));
                    t.Start();
                }
                catch(Exception e)
                {
                    messageLabel.Text = e.ToString();
                    multiplayer = false;
                }
            }
        }

        private void Initialize()
        {

            clicked = 0;
            cardsLeft = 81;
            cardCounter = 12;
            score = 100;
            time = 0;
            theTimer = new System.Windows.Forms.Timer();
            theTimer.Interval = 1000;
            theTimer.Tick += new EventHandler(TimerEventProcessor);
            textBox1.Text = cardsLeft.ToString();
            messageLabel.Text = "";
            scoreLabel.Text = "";

            var serializer = new XmlSerializer(Highscores.GetType(), "HighScores.Scores");
            object obj;
            using (var reader = new StreamReader("highscores.xml"))
            {
                obj = serializer.Deserialize(reader.BaseStream);
            }
            Highscores = (List<HighScore>)obj;

            //pictureBox2.Image = Image.FromFile("C:/Spel/Annat/SetGame/2.png");

            
            Space[0] = new CardSpace(cardPic1, pictureBox1, pictureBox2, pictureBox3);
            Space[1] = new CardSpace(cardPic2, pictureBox4, pictureBox5, pictureBox6);
            Space[2] = new CardSpace(cardPic3, pictureBox7, pictureBox8, pictureBox9);
            Space[3] = new CardSpace(cardPic4, pictureBox10, pictureBox11, pictureBox12);
            Space[4] = new CardSpace(cardPic5, pictureBox13, pictureBox14, pictureBox15);
            Space[5] = new CardSpace(cardPic6, pictureBox16, pictureBox17, pictureBox18);
            Space[6] = new CardSpace(cardPic7, pictureBox19, pictureBox20, pictureBox21);
            Space[7] = new CardSpace(cardPic8, pictureBox22, pictureBox23, pictureBox24);
            Space[8] = new CardSpace(cardPic9, pictureBox25, pictureBox26, pictureBox27);
            Space[9] = new CardSpace(cardPic10, pictureBox28, pictureBox29, pictureBox30);
            Space[10] = new CardSpace(cardPic11, pictureBox31, pictureBox32, pictureBox33);
            Space[11] = new CardSpace(cardPic12, pictureBox34, pictureBox35, pictureBox36);

            for (int i = 0; i < 81; i++)
            {
                Deck[i] = new Card(i);
            }
            
            if (!multiplayer) { shuffle(Deck); }
            theTimer.Start();
        }

        private void TimerEventProcessor(Object myObject, EventArgs myEventargs)
        {
            time++;
            textBoxTimer.Text = (time/60).ToString() + ":" + (time%60).ToString();
            score--;
            textBoxScore.Text = score.ToString();
        }

        private void cardPic1_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[0].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic2_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[1].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic3_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[2].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic4_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[3].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic5_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[4].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic6_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[5].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic7_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[6].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic8_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[7].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic9_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[8].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic10_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[9].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic11_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[10].click(clicked)) >= 3) { testSet(); }
        }

        private void cardPic12_Click(object sender, EventArgs e)
        {
            if ((clicked = Space[11].click(clicked)) >= 3) { testSet(); }
        }

        private void testSet()
        {
            int[] index = new int[3];
            int j = 0;
            for (int i = 0; i < 12; i++)
            {
                if (Space[i].clicked) { index[j] = i; j++; }
            }

            if(match(index))
            {
                score += ScoreCalculator(index); textBoxScore.Text = score.ToString();

                if (!multiplayer)
                {
                    Space[index[0]].currentCard.used = true;
                    Space[index[1]].currentCard.used = true;
                    Space[index[2]].currentCard.used = true;
                    if (cardsLeft > 12)
                    {
                        Space[index[0]].showCard(Deck[cardCounter]); cardCounter++;
                        Space[index[1]].showCard(Deck[cardCounter]); cardCounter++;
                        Space[index[2]].showCard(Deck[cardCounter]); cardCounter++;
                    }
                    else
                    {
                        Space[index[0]].showEmpty(); Space[index[0]].empty = true;
                        Space[index[1]].showEmpty(); Space[index[1]].empty = true;
                        Space[index[2]].showEmpty(); Space[index[2]].empty = true;
                    }
                    cardsLeft -= 3; textBox1.Text = cardsLeft.ToString();

                    if (cardsLeft <= 12)
                    {
                        shuffleButton.Enabled = false;
                        endButton.Enabled = true;
                    }
                }
                else
                {
                    string cards = "";
                    for(int i = 0; i < 3; i++)
                    {
                        cards += Space[index[i]].currentCard.identity + " ";
                    }
                    sw.WriteLine(cards);
                }
                
            }

            clicked = Space[index[0]].click(clicked);
            clicked = Space[index[1]].click(clicked);
            clicked = Space[index[2]].click(clicked);
        }

        private bool match(int[] index)
        {
            int temp;
            temp = Space[index[0]].currentCard.setColor + Space[index[1]].currentCard.setColor + Space[index[2]].currentCard.setColor;
            if (temp != 0 && temp != 3 && temp != 6) { return false; }
            temp = Space[index[0]].currentCard.setShape + Space[index[1]].currentCard.setShape + Space[index[2]].currentCard.setShape;
            if (temp != 0 && temp != 3 && temp != 6) { return false; }
            temp = Space[index[0]].currentCard.setQuantity + Space[index[1]].currentCard.setQuantity+ Space[index[2]].currentCard.setQuantity;
            if (temp != 0 && temp != 3 && temp != 6) { return false; }
            temp = Space[index[0]].currentCard.setShading + Space[index[1]].currentCard.setShading + Space[index[2]].currentCard.setShading;
            if (temp != 0 && temp != 3 && temp != 6) { return false; }
            return true;
        }

        private void shuffle(Card[] cards)
        {
            int size = Deck.Length;
            Random r = new Random();
            int r1; int r2; Card temp;
            for (int i = 0; i < size; i++)
            {
                r1 = r.Next(0, size);
                r2 = r.Next(0, size);
                temp = Deck[r1]; Deck[r1] = Deck[r2]; Deck[r2] = temp;
            }

            if(size > cardsLeft)
            {
                Card[] tempDeck = new Card[cardsLeft];
                int j = 0;
                for(int i = 0; i < size; i++)
                {
                    if(!Deck[i].used)
                    {
                        tempDeck[j] = Deck[i];
                        j++;
                    }
                }
                Deck = tempDeck;
            }

            for (int i = 0; i < 12; i++)
            {
                Space[i].showCard(Deck[i]);
            }
            cardCounter = 12;
        }

        private void shuffleButton_Click(object sender, EventArgs e)
        {
            score -= 20 * matchCheck(); textBoxScore.Text = score.ToString();
            if (!multiplayer) { shuffle(Deck); }
            else { sw.WriteLine("Shuffle"); }
        }

        private int matchCheck()
        {
            //int temp = 0;
            //int temp2 = 0;
            int count = 0;

            for(int i = 0; i < 10; i++)
            {
                if (!Space[i].empty)
                {
                    for (int j = i+1; j < 11; j++)
                    {
                        if (!Space[j].empty)
                        {
                            for (int k = j + 1; k < 12; k++)
                            {
                                if(!Space[k].empty)
                                {
                                    int[] temp = {i, j, k};
                                    if(match(temp))
                                    {
                                        count++;
                                    }
                                    /*temp = Space[i].currentCard.setColor + Space[j].currentCard.setColor + Space[k].currentCard.setColor;
                                    if (temp == 0 || temp == 3 || temp == 6) { temp2++; }
                                    temp = Space[i].currentCard.setShape + Space[j].currentCard.setShape + Space[k].currentCard.setShape;
                                    if (temp == 0 || temp == 3 || temp == 6) { temp2++; }
                                    temp = Space[i].currentCard.setQuantity + Space[j].currentCard.setQuantity + Space[k].currentCard.setQuantity;
                                    if (temp == 0 || temp == 3 || temp == 6) { temp2++; }
                                    temp = Space[i].currentCard.setShading + Space[j].currentCard.setShading + Space[k].currentCard.setShading;
                                    if (temp == 0 || temp == 3 || temp == 6) { temp2++; }
                                    if (temp2 == 4) { return false; }
                                    else { temp2 = 0; }*/
                                }
                            }
                        }
                    }
                }
            }
            return count;
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            if(matchCheck() == 0)
            {
                theTimer.Stop();
                for(int i = 0; i < 12; i++)
                {
                    Space[i].unable();
                }
                messageLabel.Text = "Congratulations! You scored " + score;
                endButton.Enabled = false;

                if (!multiplayer)
                {
                    _NameForm = new NameForm(Highscores, score);
                    _NameForm.Show();
                }
                else
                {
                    sw.WriteLine("End " + score.ToString());
                }
            }
            else
            {
                messageLabel.Text = "There are still sets available";
            }
        }

        private int ScoreCalculator(int[] index)
        {
            int count = 5;
            if (Space[index[0]].currentCard.setColor == Space[index[1]].currentCard.setColor) { count--; }
            if (Space[index[0]].currentCard.setShading == Space[index[1]].currentCard.setShading) { count--; }
            if (Space[index[0]].currentCard.setQuantity == Space[index[1]].currentCard.setQuantity) { count--; }
            if (Space[index[0]].currentCard.setShape == Space[index[1]].currentCard.setShape) { count--; }
            return count*10;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (pauseButton.Text == "Pause")
            {
                pauseButton.Text = "Unpause";
                theTimer.Stop();

                endButton.Visible = false;
                shuffleButton.Visible = false;
                highScoreButton.Visible = false;
                helpButton.Visible = false;
                for (int i = 0; i < 12; i++)
                {
                    Space[i].showEmpty();
                }
            }
            else
            {
                pauseButton.Text = "Pause";
                theTimer.Start();

                endButton.Visible = true;
                shuffleButton.Visible = true;
                highScoreButton.Visible = true;
                helpButton.Visible = true;
                for(int i = 0; i < 12; i++)
                {
                    Space[i].showAgain();
                }
                /*
                if(_HighScoreForm.Created)
                {
                    _HighScoreForm.Close();
                }
                if(_HelpForm.Created)
                {
                    _HelpForm.Close();
                }
                */
            }
        }

        private void highScoreButton_Click(object sender, EventArgs e)
        {
            _HighScoreForm = new HighScoreForm(Highscores);
            _HighScoreForm.Show();
            pauseButton_Click(sender, e);
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            _HelpForm = new HelpForm();
            _HelpForm.Show();
            pauseButton_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                t.Abort();
                sr.Close();
                s.Close();
                client.Close();
            }
        }

        private void Networker()
        {
            while(true)
            {
                string incoming = sr.ReadLine();
                string[] splitIn = incoming.Split(' ');
                int[] cards = new int[splitIn.Length];

                if(splitIn[0] == "Score")
                {
                    string scores = "";
                    for (int i = 1; i < splitIn.Length; i++) { scores += splitIn[i] + " "; }
                    ThreadScore(scores);
                    break;
                }

                if (incoming == "End")
                {
                    sw.WriteLine("Score " + score.ToString());
                }
                else
                {

                    for (int i = 0; i < splitIn.Length; i++)
                    {
                        cards[i] = Convert.ToInt32(splitIn[i]);
                    }

                    cardsLeft = cards[splitIn.Length - 1];

                    for (int i = 0; i < 12; i++)
                    {
                        if (i < splitIn.Length - 1)
                        {
                            ThreadShow(i, cards[i]);
                        }
                        else
                        {
                            ThreadEmpty(i);
                        }
                    }
                    if (cardsLeft <= 12) { ThreadEndPhase(); }
                }
            }
            s.Close();
            client.Close(); 
        }

        private void ThreadShow(int sIndex, int cIndex)
        {
            if(this.pictureBox1.InvokeRequired)
            {
                ThreadShowCallback d = new ThreadShowCallback(ThreadShow);
                this.Invoke(d, new object[] { sIndex, cIndex });
            }
            else
            {
                Space[sIndex].showCard(Deck[cIndex]);
                textBox1.Text = cardsLeft.ToString();
            }
        }

        private void ThreadEmpty(int sIndex)
        {
            if(this.pictureBox1.InvokeRequired)
            {
                ThreadEmptyCallback d = new ThreadEmptyCallback(ThreadEmpty);
                this.Invoke(d, new object[] { sIndex });
            }
            else
            {
                Space[sIndex].showEmpty(); Space[sIndex].empty = true;
                if (Space[sIndex].clicked) { clicked = Space[sIndex].click(clicked); }
            }
        }

        private void ThreadEndPhase()
        {
            if (this.pictureBox1.InvokeRequired)
            {
                ThreadEndPhaseCallback d = new ThreadEndPhaseCallback(ThreadEndPhase);
                this.Invoke(d);
            }
            else
            {
                shuffleButton.Enabled = false;
                endButton.Enabled = true;
            }
        }

        private void ThreadScore(string scores)
        {
            if(this.scoreLabel.InvokeRequired)
            {
                ThreadScoreCallback d = new ThreadScoreCallback(ThreadScore);
                this.Invoke(d, new object[] { scores });
            }
            else 
            { 
                scoreLabel.Text = "The scores: " + scores;
                messageLabel.Text = "Congratulations! You scored " + score.ToString();
                endButton.Enabled = false;
            }
        }
    }
}
