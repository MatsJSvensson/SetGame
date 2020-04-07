using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SetGame
{
    class CardSpace
    {
        PictureBox backDrop;
        PictureBox sign1;
        PictureBox sign2;
        PictureBox sign3;
        public Card currentCard;
        public bool clicked;
        public bool empty;

        public CardSpace(PictureBox b, PictureBox s1, PictureBox s2, PictureBox s3)
        {
            backDrop = b;
            sign1 = s1;
            sign2 = s2;
            sign3 = s3;
            clicked = false;
            empty = false;
        }

        public void showCard(Card tempCard)
        {
            currentCard = tempCard;
            int index = tempCard.setShading*9 + tempCard.setColor * 3 + tempCard.setShape + 1;
            //System.Drawing.Image temp = System.Drawing.Image.FromFile("C:/Spel/Annat/SetGame/" + index.ToString() + ".png");
            System.Drawing.Image temp = System.Drawing.Image.FromFile(index.ToString() + ".png");

            sign1.SizeMode = PictureBoxSizeMode.Zoom; sign1.BackColor = Color.White;
            sign2.SizeMode = PictureBoxSizeMode.Zoom; sign2.BackColor = Color.White;
            sign3.SizeMode = PictureBoxSizeMode.Zoom; sign3.BackColor = Color.White;

            switch (tempCard.setQuantity)
            {
                case 0:
                    sign1.Visible = false;
                    sign2.Visible = true;
                    sign3.Visible = false;

                    sign2.Image = temp;
                    break;
                case 1:
                    sign1.Visible = true;
                    sign2.Visible = false;
                    sign3.Visible = true;

                    sign1.Image = temp;
                    sign3.Image = temp;
                    break;
                case 2:
                    sign1.Visible = true;
                    sign2.Visible = true;
                    sign3.Visible = true;

                    sign1.Image = temp;
                    sign2.Image = temp;
                    sign3.Image = temp;
                    break;
            }
            backDrop.BackColor = Color.White;
        }

        public void showEmpty()
        {
            backDrop.BackColor = Color.AntiqueWhite;
            backDrop.Enabled = false;
            sign1.Visible = false;
            sign2.Visible = false;
            sign3.Visible = false;
        }

        public void showAgain()
        {
            backDrop.Enabled = true;

            switch (currentCard.setQuantity)
            {
                case 0:
                    sign1.Visible = false;
                    sign2.Visible = true;
                    sign3.Visible = false;
                    break;
                case 1:
                    sign1.Visible = true;
                    sign2.Visible = false;
                    sign3.Visible = true;
                    break;
                case 2:
                    sign1.Visible = true;
                    sign2.Visible = true;
                    sign3.Visible = true;
                    break;
            }
            backDrop.BackColor = Color.White;
        }

        public int click(int nClicked)
        {
            if(clicked)
            {
                clicked = false; nClicked--;
                backDrop.BackColor = Color.White;
                backDrop.BorderStyle = BorderStyle.None;
            }
            else
            {
                clicked = true; nClicked++;
                backDrop.BackColor = Color.Lavender;
                backDrop.BorderStyle = BorderStyle.Fixed3D;
            }
            return nClicked;
        }

        public void unable()
        {
            backDrop.Enabled = false;
        }


    }
}
