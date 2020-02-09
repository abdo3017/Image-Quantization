using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuantization
{
    class MaxHeap
    {
        Edge[] edges;
        int size;
        int index;

        public MaxHeap(int size)
        {
            this.size = size;
            edges = new Edge[size];
            index = 0;
        }


        public void insertEdge(Edge edge)
        {
            edges[index] = edge;
            index++;
            heapifyUp();
        }

        private void heapifyUp()
        {

            int indexHolder = index - 1;

            while (!isRoot(indexHolder) && edges[indexHolder].weight > edges[getParentIndex(indexHolder)].weight)
            {
                int parentIndex = getParentIndex(indexHolder);
                swapEdges(parentIndex, indexHolder);
                indexHolder = parentIndex;
            }
        }


        private bool isRoot(int index)
        {
            if (index == 0)
                return true;
            return false;
        }


        public int getSize()
        {
            return size;
        }
        private int getParentIndex(int index)
        {
            return ((index - 1) / 2);
        }

        private void swapEdges(int firstEgdeIndex, int secondeEdgeIndex)
        {
            Edge edgeHolder = edges[firstEgdeIndex];
            edges[firstEgdeIndex] = edges[secondeEdgeIndex];
            edges[secondeEdgeIndex] = edgeHolder;
        }

        public void displayEdges()
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                if (edges[i].weight != 0)
                {
                    Console.WriteLine(edges[i].weight + " ");
                    sum += 1;
                }
            }
            Console.WriteLine();
            Console.WriteLine("summationn " + sum);
        }

        public Edge getMaxEdge()
        {
            Edge minEdge = edges[0];
            edges[0] = edges[size - 1];
            size--;
            heapifyDown();
            return minEdge;
        }

        private void heapifyDown()
        {
            int indexHolder = 0;
            int leftEdge;
            int rightEdge;
            Edge minEdge;
            int BiggerIndex;

            while (hasLeftEdge(indexHolder))
            {
                leftEdge = getLeftEdgeIndex(indexHolder);
                rightEdge = getRightEdgeIndex(indexHolder);
                minEdge = edges[indexHolder];
                BiggerIndex = leftEdge;


                if (hasRightEdge(indexHolder) && edges[rightEdge].weight > edges[leftEdge].weight)
                {
                    minEdge = edges[rightEdge];
                    BiggerIndex = rightEdge;
                }
                if (edges[BiggerIndex].weight < minEdge.weight)
                {
                    break;
                }

                swapEdges(BiggerIndex, indexHolder);
                indexHolder = BiggerIndex;
            }

        }
        public Edge top()
        {
            return edges[0];
        }

        private int getLeftEdgeIndex(int index)
        {
            return ((2 * index) + 1);
        }

        private int getRightEdgeIndex(int index)
        {
            return ((2 * index) + 2);
        }

        private bool hasLeftEdge(int index)
        {
            return getLeftEdgeIndex(index) < size;
        }

        private bool hasRightEdge(int index)
        {
            return getRightEdgeIndex(index) < size;
        }

        public bool isEmpty()
        {
            if (size == 0)
                return true;
            return false;
        }
        public Edge[] getArr()
        {
            
            return edges;
        }



        public Edge[] Sortedges()
        {
            Edge[] sortedArray = new Edge[edges.Length];

            // find smallest and largest value
            double minVal = edges[0].weight;
            double maxVal = edges[0].weight;
            for (int i = 1; i < edges.Length; i++)
            {
                if (edges[i].weight < minVal) minVal = edges[i].weight;
                else if (edges[i].weight > maxVal) maxVal = edges[i].weight;
            }
            int s = (int)(maxVal - minVal + 1);
            // init array of frequencies
            int[] counts = new int[s];

            // init the frequencies
            for (int i = 0; i < edges.Length; i++)
            {
                counts[(int)(edges[i].weight - minVal)]++;
            }

            // recalculate
            counts[0]--;
            for (int i = 1; i < counts.Length; i++)
            {
                counts[i] = counts[i] + counts[i - 1];
            }

            // Sort the array
            for (int i = edges.Length - 1; i >= 0; i--)
            {
                sortedArray[counts[(int)(edges[i].weight - minVal)]--] = edges[i];
            }       
            return sortedArray;
        }
    }
}









