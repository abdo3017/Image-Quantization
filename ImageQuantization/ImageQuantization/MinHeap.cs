using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageQuantization
{ 
    class MinHeap
    {
        Edge[] edges;
        int elements;
        int index;
        int size;

        public MinHeap(int size)//Θ(1)
        {
            this.size = size; //Θ(1)
            //this.size /= 2;  
            edges = new Edge[size]; //Θ(1)
        
            index = 0; //Θ(1)
            elements = 0; //Θ(1)
        }

        /// <summary>
        /// Add new edge in the heap
        /// </summary>
        /// <param name="edge">Edge</param>
        public void insertEdge(Edge edge) // O(log E)
        {
            edges[index] = edge; //Θ(1)
            index++; //Θ(1)
            elements++; //Θ(1)
            heapifyUp(); // O(log E)
        }

        /// <summary>
        /// sort the heap so that the smallest value will be the root
        /// </summary>
        private void heapifyUp() //O(Log E)
        {
            int indexHolder = index - 1; //Θ(1)

            while (!isRoot(indexHolder) && edges[indexHolder].weight < edges[getParentIndex(indexHolder)].weight) //O(log E)
            {
                int parentIndex = getParentIndex(indexHolder); //Θ(1)
                swapEdges(indexHolder, parentIndex); //Θ(1)                                                                                           
                indexHolder = parentIndex; //Θ(1)
            }
        }

        /// <summary>
        /// Checks if element is root
        /// </summary>
        /// <param name="index">index of element</param>
        /// <returns>bool</returns>
        private bool isRoot(int index) //Θ(1)
        {
            return (index == 0); //Θ(1)
        }

        /// <summary>
        /// get parent index of element
        /// </summary>
        /// <param name="index">index of element</param>
        /// <returns>index of parent</returns>
        private int getParentIndex(int index) //Θ(1)
        {
            return ((index - 1) / 2); //Θ(1)
        }

        /// <summary>
        /// Swap two Elements in Heap
        /// </summary>
        /// <param name="firstEgdeIndex"></param>
        /// <param name="secondeEdgeIndex"></param>
        private void swapEdges(int firstEgdeIndex, int secondeEdgeIndex) //Θ(1)
        {
            Edge edgeHolder = edges[firstEgdeIndex]; //Θ(1)
            edges[firstEgdeIndex] = edges[secondeEdgeIndex]; //Θ(1)
            edges[secondeEdgeIndex] = edgeHolder; //Θ(1)
        }

        /// <summary>
        /// Extract smallest edge in heap
        /// </summary>
        /// <returns> smallest edge</returns>
        public Edge getMinEdge() // log E
        {
            Edge minEdge = edges[0]; //Θ(1)
            edges[0] = edges[elements - 1]; //Θ(1)
            elements--; //Θ(1)
            index--; //Θ(1)
            heapifyDown(); // log E
            return minEdge; //Θ(1)
        }
        public int getSize() //Θ(1)
        {
            return elements; //Θ(1)
        }
        /// <summary>
        ///  resort the heap so that the smallest value will be the root
        /// </summary>
        private void heapifyDown() // log E
        {
            int indexHolder = 0; //Θ(1)
            int leftEdge; //Θ(1)
            int rightEdge; //Θ(1)
            Edge minEdge; //Θ(1)
            int smallerIndex; //Θ(1)

            while (hasLeftEdge(indexHolder)) // iteration * body ===> Log E * Θ(1) 
            {
                leftEdge = getLeftEdgeIndex(indexHolder); //Θ(1)
                rightEdge = getRightEdgeIndex(indexHolder); //Θ(1)
                minEdge = edges[indexHolder]; //Θ(1)
                smallerIndex = leftEdge; //Θ(1)

                if (edges[leftEdge].weight < minEdge.weight) //Θ(1)
                {
                    minEdge = edges[leftEdge]; //Θ(1)
                }
                if (hasRightEdge(indexHolder) && edges[rightEdge].weight < minEdge.weight) //Θ(1)
                {
                    minEdge = edges[rightEdge]; //Θ(1)
                    smallerIndex = rightEdge; //Θ(1)
                }
                if (edges[smallerIndex].weight > minEdge.weight) //Θ(1)
                {
                    break; //Θ(1)
                }

                swapEdges(smallerIndex, indexHolder); //Θ(1)
                indexHolder = smallerIndex; //Θ(1)
            }

        }


        private int getLeftEdgeIndex(int index) //Θ(1)
        {
            return ((2 * index) + 1); //Θ(1)
        }

        private int getRightEdgeIndex(int index) //Θ(1)
        {
            return ((2 * index) + 2); //Θ(1)
        }

        private bool hasLeftEdge(int index) //Θ(1)
        {
            return getLeftEdgeIndex(index) < elements; //Θ(1)
        }

        private bool hasRightEdge(int index) //Θ(1)
        {
            return getRightEdgeIndex(index) < elements; //Θ(1)
        }

        public bool isEmpty() //Θ(1)
        {
            return (elements == 0); //Θ(1)
        }

        public void displayEdges()
        {
            for (int i = 0; i < elements; i++)
                Console.Write(edges[i].weight + " ");
            Console.WriteLine();
        }
    }
}
