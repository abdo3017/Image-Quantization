using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class AutoClustring
    {
        public AutoClustring(List<RGBPixelD> listRGB)
        {
            this.listRGB = listRGB; //Θ(1)
            size = listRGB.Count;
            
        }
        int size;
        MaxHeap maxHeap;
        Dictionary<int, double>[] MSTDictionary;
        List<RGBPixelD> listRGB;
        List<RGBPixelD> newListRGB;
        double totalCost = 0;
        double Mean = 0;
        double STDev = 0;
        double red, green, blue;
        double counter;
        double min = double.MaxValue;
        RGBPixelD minRGBPixelD;
        RGBPixel[,,] replacedMatrix;
        RGBPixelD rGBPixelD;
        Dictionary<int, double>[] MSTDictionaryCur;
        double distance = 0;
        RGBPixel[,,] rGBPixels;
        public RGBPixel[,,] autoClustring(int k, List<RGBPixelD> listRGB,int size)
        {
            int q = 1;//⊖(1)
            MSTDictionaryCur = MSTDictionary;//⊖(1)
            MSTDictionary = prim(size, listRGB);//⊖(D log E)
            while (!maxHeap.isEmpty()) //⊖(E log E)
            {
                Edge edgeHolder = maxHeap.top(); //⊖(1) 
                if (edgeHolder.weight > (Mean + STDev)) //⊖(1)
                {
                    maxHeap.getMaxEdge(); //⊖(log E)
                    q++; //⊖(1)
                    MSTDictionary[edgeHolder.colorA].Remove(edgeHolder.colorB);//⊖(1)
                    MSTDictionary[edgeHolder.colorB].Remove(edgeHolder.colorA);//⊖(1)
                }
                else break; //⊖(1)
            } 

            if (q <= k) //⊖(1)=*body(⊖(K log E))
            {
                for (int j = q; j < k; j++)//⊖(k)* body(⊖(log E))
                {
                    Edge edgeHolder = maxHeap.getMaxEdge();//⊖(log E)
                    MSTDictionary[edgeHolder.colorA].Remove(edgeHolder.colorB);//⊖(1)
                    MSTDictionary[edgeHolder.colorB].Remove(edgeHolder.colorA);//⊖(1)
                }
                 FindRepresentativeColors(size); //⊖(D)
                return replacedMatrix; //⊖(1)
            }
            else // O(D * size of new list)
            {
                 FindRepresentativeColors(size);   //⊖(D)           
                rGBPixels = autoClustring(k,newListRGB,q); // O(D * size of new list)+//⊖(E log E)??????
                fillColors(listRGB.Count); // O(D * size of new list)
            }
            return replacedMatrix; //⊖(1)
        }
    
        void FindRepresentativeColors(int size) //==>⊖(D+E) ==> O(D)
        {
            replacedMatrix = new RGBPixel[256, 256, 256];//⊖(1)
            newListRGB = new List<RGBPixelD>();
            bool[] visited = new bool[size];//⊖(1)
            bool[] visitedcpd = new bool[size];//⊖(1)

            for (int v = 0; v < size; v++)//==>⊖(D)
            {
                if (!visited[v])//⊖(1)
                {
                    DFS_GetClusterColor(v, visited);//⊖(egdes+nodes)

                    red /= counter;//⊖(1)
                    green /= counter;//⊖(1)
                    blue /= counter;//⊖(1)

                    min = double.MaxValue;//⊖(1)
                    minRGBPixelD = listRGB[v];//⊖(1)
                    rGBPixelD.red = red;//⊖(1)
                    rGBPixelD.green = green;//⊖(1)
                    rGBPixelD.blue = blue;//⊖(1)

                    DfS_ColorReplace(v, visitedcpd);//⊖(egdes+nodes)
                    newListRGB.Add(minRGBPixelD);//⊖(size)

                    red = green = blue = counter = 0;//⊖(1)
                }
            }
            
        }
     
        void DFS_GetClusterColor(int v, bool[] visited)//⊖(egdes+nodes)
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)
            red += listRGB[v].red;//⊖(1)
            green += listRGB[v].green;//⊖(1)
            blue += listRGB[v].blue;//⊖(1)
            if (MSTDictionary[v] == null)//⊖(1)
                MSTDictionary[v] = new Dictionary<int, double>();//⊖(1)
            counter++;//⊖(1)
            foreach (KeyValuePair<int, double> color in MSTDictionary[v])//⊖(E)
            {
                if (!visited[color.Key])//⊖(1)
                    DFS_GetClusterColor(color.Key, visited);
            }

        }
       
        void DfS_ColorReplace(int v, bool[] visited)//⊖(egdes+nodes)
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)            
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].red = (byte)red;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].green = (byte)green;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].blue = (byte)blue;//⊖(1)

            distance = calcEuclideanDistance(listRGB[v],rGBPixelD);//⊖(1)
            if (distance < min)
            {
                min = distance;//⊖(1)
                minRGBPixelD = listRGB[v];//⊖(1)

            }
            foreach (KeyValuePair<int, double> color in MSTDictionary[v])//⊖(E)
            {
                if (!visited[color.Key])//⊖(1)
                    DfS_ColorReplace(color.Key, visited);
            }


        }

        public Dictionary<int, double>[] prim(int size, List<RGBPixelD> listRGB)//E Log(D)
        {
            //int size = listRGB.Count; // Number of Distincit Colors Θ(1)
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
            totalCost = 0;
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
            STDev = calcSTDev(maxHeap.getArr(), Mean);
            //   totalCost = Math.Round(Math.Round(sum, 2), 1); //Θ(1)
            Console.WriteLine("Num of Colors: " + size); //Θ(1)

            Console.WriteLine("Sum: " + totalCost); //Θ(1)
            return MSTDictionary;
        }

        public double calcSTDev(Edge[] edges, double Mean)//⊖(size of edges)
        {
            double sum = 0;//⊖(1)
            double STDevv = 0;//⊖(1)
            for (int i = 0; i < edges.Length; i++)//⊖(size of edges)
            {
                sum += Math.Abs(edges[i].weight - Mean) * Math.Abs(edges[i].weight - Mean);//⊖(1)
            }
            STDevv = Math.Sqrt(sum / Convert.ToDouble(edges.Length-1));//⊖(1)
            return STDevv;//⊖(1)

        }

        public double calcEuclideanDistance(RGBPixelD firstColor, RGBPixelD secondColor)
        {
            double red = firstColor.red - secondColor.red; //Θ(1)
            red *= red; //Θ(1)
            double green = firstColor.green - secondColor.green; //Θ(1)
            green *= green; //Θ(1)
            double blue = firstColor.blue - secondColor.blue; //Θ(1)
            blue *= blue; //Θ(1)
            return Math.Sqrt(red + blue + green); //Θ(1)
        }//⊖(1)
        public RGBPixel[,,] getMatrix()//==>⊖(1)
        {
            return replacedMatrix;//⊖(1)
        }
        public Dictionary<int, double>[] getMSTDictionary()
        {
            return MSTDictionary;
        }//⊖(1)
        public List<RGBPixelD> getListOfColors()
        {
            return listRGB;
        }//⊖(1)

        void fillColors(int size) //==>⊖(D+E) ==> O(D*size of new list)
        {
           
            bool[] visited = new bool[size];//⊖(1)
            bool[] visitedcpd = new bool[size];//⊖(1)

            for (int v = 0; v < size; v++)//==>⊖(D)
            {
                if (!visited[v])
                {
                    DFS_check(v, visited);//⊖(D+E) *size of new list
                    DfS_finalreplace(v, visitedcpd);   //⊖(D+E)                                  
                    red = green = blue;//⊖(1)
                }
            }

        }

        
        void DFS_check(int v, bool[] visited)//⊖(egdes+nodes)*size
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)
            for(int i = 0; i < newListRGB.Count; i++)//⊖(size of newlist)
            {
                if (listRGB[v].red == newListRGB[i].red
                    && listRGB[v].green == newListRGB[i].green
                    && listRGB[v].blue == newListRGB[i].blue)//⊖(1)
                {
                    blue = rGBPixels[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].blue;//⊖(1)
                    green = rGBPixels[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].green;//⊖(1)
                    red = rGBPixels[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].red;//⊖(1)
                    break;//⊖(1)
                }
            }

            if (MSTDictionaryCur[v] == null)//⊖(1)
                MSTDictionaryCur[v] = new Dictionary<int, double>();//⊖(1)


            foreach (KeyValuePair<int, double> color in MSTDictionaryCur[v])//⊖(E)
            {
                if (!visited[color.Key])//⊖(1)
                    DFS_check(color.Key, visited);
            }

        }


        void DfS_finalreplace(int v, bool[] visited)//⊖(egdes+nodes)
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].red = (byte)red;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].green = (byte)green;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].blue = (byte)blue;//⊖(1)

            foreach (KeyValuePair<int, double> color in MSTDictionaryCur[v])//⊖(E)
            {
                if (!visited[color.Key])//⊖(1)
                    DfS_finalreplace(color.Key, visited);
            }
        }

    }
}
