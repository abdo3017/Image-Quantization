using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuantization
{
    class  Edge
    {
        public int colorA, colorB;
        public double weight;

        public Edge(int colorA, double weight, int colorB)
        {
            this.colorA = colorA;
            this.weight = weight;
            this.colorB = colorB;
        }
    }
}
