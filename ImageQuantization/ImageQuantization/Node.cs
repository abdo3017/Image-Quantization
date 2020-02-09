using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class Node
    {
       public double weigth;
       public int dest;
        public Node(int dest,double weigth)
        {
            this.dest = dest;
            this.weigth = weigth;
        }
    }
}
