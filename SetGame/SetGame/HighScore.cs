using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    public class HighScore
    {
        public int points;
        public string name;

        public int CompareTo(HighScore compareScore)
        {
            if (this.points > compareScore.points)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }


}
