using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    class Card
    {
        public int setQuantity{get; set;}
        public int setColor;
        public int setShading;
        public int setShape;
        public int identity;
        public bool used;
        //System.Windows.Forms.PictureBox picture;

        public Card(int temp)
        {
            identity = temp;
            used = false;
            setColor = temp % 3;    temp /= 3;
            setQuantity = temp % 3; temp /= 3;
            setShading = temp % 3;  temp /= 3;
            setShape = temp % 3;
        }
    }
}
