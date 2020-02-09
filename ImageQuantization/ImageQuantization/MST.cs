using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class MST
    {
        
        MaxHeap maxHeap;
        Dictionary<int , double>[] MSTDictionary;
        List<RGBPixelD> listRGB;
        public double totalCost = 0;
       public double Mean = 0;
       
        public MST(List<RGBPixelD> listRGB)
        {
            this.listRGB = listRGB; //Θ(1)
            
        }


        public void prim()//E Log(D)
        {
            int size = listRGB.Count; // Number of Distincit Colors Θ(1)
            bool[] visited = new bool[size]; //Θ(1)
            Edge[] key = new Edge[size]; //Θ(1)
            MSTDictionary = new Dictionary<int, double>[size];//⊖(1)
            maxHeap = new MaxHeap(listRGB.Count - 1);//⊖(1)
            for (int i = 0; i < size; i++)
            {
                key[i] = new Edge(0, double.MaxValue, 0);
            }
            Edge minEdge = new Edge(0, 0, 0);
            int edgeCounter = 0; //Θ(1)
            while (edgeCounter != size) //iteration * body ===> E * O(Body) ==>  E * Log V
            {
                //Edge edgeHolder = edgesHeap.getMinEdge(); //(log E)
                if (visited[minEdge.colorB]) //Θ(1)
                    continue; //Θ(1)

                visited[minEdge.colorB] = true; //Θ(1)
                edgeCounter++; //Θ(1)
                totalCost += minEdge.weight; //Θ(1)
                if (edgeCounter != 1) //⊖(1)
                {
                    if (MSTDictionary[minEdge.colorA] == null)//⊖(1)
                        MSTDictionary[minEdge.colorA] = new Dictionary<int, double>();//⊖(1)
                    if (MSTDictionary[minEdge.colorB] == null)//⊖(1)
                        MSTDictionary[minEdge.colorB] = new Dictionary<int, double>();//⊖(1)
                    MSTDictionary[minEdge.colorA][minEdge.colorB] = minEdge.weight;//⊖(1)
                    MSTDictionary[minEdge.colorB][minEdge.colorA] = minEdge.weight;//⊖(1)
                    maxHeap.insertEdge(minEdge);//⊖(log heap size)
                }
                double min = double.MaxValue;
                int minEdgeIndexHolder = -1;
                for (int i = 0; i < size; i++) // iteration * body ===> E * O(body) ===> E * log V
                {
                    double weight = calcEuclideanDistance(listRGB[minEdge.colorB], listRGB[i]); //O(1)
                    if (!visited[i] && min > key[i].weight)
                    {
                        min = key[i].weight;
                        minEdgeIndexHolder = i;
                    }
                    if (!visited[i] && key[i].weight > weight)
                    {
                        key[i].weight = weight;
                        key[i].colorA = minEdge.colorB;
                        key[i].colorB = i;
                        //        edgesHeap.insertEdge(new Edge(minEdge.colorB, weight, i)); //O(log E)
                        if (weight < min)
                        {
                            min = weight;
                            minEdgeIndexHolder = i;
                        }
                    }
                }
                if (minEdgeIndexHolder != -1)
                    minEdge = new Edge(key[minEdgeIndexHolder].colorA, key[minEdgeIndexHolder].weight, key[minEdgeIndexHolder].colorB);

            }
            Mean = totalCost / size;
            calcSTDev(maxHeap.getArr(),Mean);
         //   totalCost = Math.Round(Math.Round(sum, 2), 1); //Θ(1)
            Console.WriteLine("Num of Colors: " + size); //Θ(1)

            Console.WriteLine("Sum: " + totalCost); //Θ(1)
        }

       
        //public void prim()//E Log(D)
        //{
        //    int size = listRGB.Count; // Number of Distincit Colors Θ(1)
        //    Console.WriteLine(int.MaxValue - (size * (size - 1)) / 2);
        //    MSTDictionary = new Dictionary<int, double>[size];//⊖(1)
        //    maxHeap = new MaxHeap(listRGB.Count - 1);//⊖(1)
        //    bool[] visited = new bool[size]; //Θ(1)
        //    int sizeHolder = ((size * (size - 1)) / 2);
        //    if (sizeHolder > int.MaxValue)
        //    {
                           
        //        sizeHolder = int.MaxValue - 10000; 
        //    }
        //    MinHeap edgesHeap = new MinHeap(sizeHolder ); //Θ(1)
        //    edgesHeap.insertEdge(new Edge(0, 0, 0)); // Edge(From, weight, to) takes O(log E) 
        //    int edgeCounter = 0; //Θ(1)

        //    while (edgeCounter != size && !edgesHeap.isEmpty()) //iteration * body ===> E * O(Body) ==>  E * Log V
        //    {
        //        Edge edgeHolder = edgesHeap.getMinEdge(); //(log E)
        //        if (visited[edgeHolder.colorB]) //Θ(1)
        //            continue; //Θ(1)

        //        visited[edgeHolder.colorB] = true; //Θ(1)
        //        edgeCounter++; //Θ(1)
        //        sum += edgeHolder.weight; //Θ(1)
        //        if (edgeCounter != 1) //⊖(1)
        //        {
        //            if (MSTDictionary[edgeHolder.colorA] == null)//⊖(1)
        //                MSTDictionary[edgeHolder.colorA] = new Dictionary<int, double>();//⊖(1)
        //            if (MSTDictionary[edgeHolder.colorB] == null)//⊖(1)
        //                MSTDictionary[edgeHolder.colorB] = new Dictionary<int, double>();//⊖(1)
        //            MSTDictionary[edgeHolder.colorA][edgeHolder.colorB] = edgeHolder.weight;//⊖(1)
        //            MSTDictionary[edgeHolder.colorB][edgeHolder.colorA] = edgeHolder.weight;//⊖(1)
        //            maxHeap.insertEdge(edgeHolder);//⊖(log heap size)
        //        }
        //        for (int i = 0; i < size; i++) // iteration * body ===> E * O(body) ===> E * log V
        //        {
        //            if (!visited[i])
        //            {
        //                double weight = calcEuclideanDistance(listRGB[edgeHolder.colorB], listRGB[i]); //O(1)
        //                edgesHeap.insertEdge(new Edge(edgeHolder.colorB, weight, i)); //O(log E)
        //            }
        //        }
        //    }
        //    Console.WriteLine("Sum: " + sum); //Θ(1)
        //}



        /// <summary>
        /// Calculate distance between two colors 
        /// </summary>
        /// <param name="firstColor">Node Color</param>
        /// <param name="secondColor">Node Color</param>
        /// <returns>Euclidean Distance</returns>
        public double calcEuclideanDistance(RGBPixelD firstColor, RGBPixelD secondColor)
        {
            double red = firstColor.red - secondColor.red; //Θ(1)
            red *= red; //Θ(1)
            double green = firstColor.green - secondColor.green; //Θ(1)
            green *= green; //Θ(1)
            double blue = firstColor.blue - secondColor.blue; //Θ(1)
            blue *= blue; //Θ(1)
            return Math.Sqrt(red + blue + green); //Θ(1)
        }

        public Dictionary<int , double>[] getMSTList()
        {
            return MSTDictionary;
        }
        public double STDev = 0;
        public double calcSTDev(Edge[] edges, double Mean)
        {
            double sum = 0;
           
            for (int i = 0; i < edges.Length; i++)
            {
               // Console.WriteLine(edges[i].weight);
                sum += Math.Abs(edges[i].weight - Mean) * Math.Abs(edges[i].weight - Mean);
            }
            STDev = Math.Sqrt(sum / Convert.ToDouble(edges.Length - 1));
            return STDev;

        }

        public MaxHeap getEdgesHeap()
        {
            return maxHeap;
        }
    }
}
