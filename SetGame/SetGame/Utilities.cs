using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    class Card
    {
        int setQuantity;
        int setColor;
        int setShading;
        int setShape;
        System.Windows.Forms.PictureBox picture;

        public void Card(System.Windows.Forms.PictureBox tempPic, int temp)
        {
            this.picture = tempPic;

            setColor = temp % 3;    temp /= 3;
            setQuantity = temp % 3; temp /= 3;
            setShading = temp % 3;  temp /= 3;
            setShape = temp % 3;
        }
    }
}
