using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class CLustring
    {

        Dictionary<int, double>[] adjDictArray;
        RGBPixel[,,] replacedMatrix;
        public CLustring(MaxHeap edgesHeap, List<RGBPixelD> listRGB, Dictionary<int, double>[] AdjListdict)
        {
            adjDictArray = AdjListdict;//⊖(1)
            replacedMatrix = new RGBPixel[256, 256, 256];//⊖(1)
            size = listRGB.Count;//⊖(1)
            red = green = blue = counter = 0;//⊖(1)
            this.edgesHeap = edgesHeap;//⊖(1)
            this.listRGB = listRGB;//⊖(1)
        }

        public CLustring( List<RGBPixelD> listRGB, Dictionary<int, double>[] AdjListdict)
        {
            adjDictArray = AdjListdict;//⊖(1)
            replacedMatrix = new RGBPixel[256, 256, 256];//⊖(1)
            size = listRGB.Count;//⊖(1)
            red = green = blue = counter = 0;//⊖(1)
          
            this.listRGB = listRGB;//⊖(1)
        }


        int size;
        double red, green, blue;
        double counter;
        List<RGBPixelD> listRGB;
        MaxHeap edgesHeap;
        public RGBPixel[,,] getMatrix()//==>⊖(1)
        {
            return replacedMatrix;//⊖(1)
        }
        /// <summary>
        /// Find K Clusters And Replace The Original Colors With Thier Representative Colors  
        /// </summary>
        /// <param name="K">Number of Desired Clusters</param>
        public void buildCustring(int k)
        {
            Extract_KClusters(k);//0(k log D)
            FindRepresentativeColors();//⊖(D+E)==>//⊖(D+D)==>//⊖(2D)==>//⊖(D)
        }
        
        public void buildAutoCustring()
        {
          
            FindRepresentativeColors();//⊖(D+E)==>//⊖(D+D)==>//⊖(2D)==>//⊖(D)
        }
        /// <summary>
        /// Delete Edges with Max Weight And Remove THis Edges from the mstdictionary  
        /// </summary>
        /// <param name="K">Number of Desired Clusters</param>    
        void Extract_KClusters(int k)
        {
            //⊖(k*log D)
            for (int j = 0; j < k - 1; j++)
            {
                Edge edgeHolder = edgesHeap.getMaxEdge();//⊖(log E)
                adjDictArray[edgeHolder.colorA].Remove(edgeHolder.colorB);//⊖(1)
                adjDictArray[edgeHolder.colorB].Remove(edgeHolder.colorA);//⊖(1)
            }
        }
        /// <summary>
        /// Get each cluster colors and represent it with only one color  
        /// </summary>
        void FindRepresentativeColors() //==>⊖(D+E) ==> O(D)
        {
            bool[] visited = new bool[size];//⊖(1)
            bool[] visitedcpd = new bool[size];//⊖(1)

            for (int v = 0; v < size; v++)//==>⊖(D)
            {
                if (!visited[v])//⊖(1)
                {
                    DFS_GetClusterColor(v, visited);//⊖(D + E)
                    red /= counter;//⊖(1)
                    green /= counter;//⊖(1)
                    blue /= counter;//⊖(1)

                    DfS_ColorReplace(v, visitedcpd);//⊖(D+E)
                    red = green = blue = counter = 0;//⊖(1)
                }
            }
        }
        /// <summary>
        /// Get Culster Colors  
        /// </summary>
        /// <param name="V">Distinct Colors Number</param>
        /// <param name="visited">To know if specific color is visted or no </param>
        void DFS_GetClusterColor(int v, bool[] visited)//⊖(egdes+nodes)
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)
            red += listRGB[v].red;//⊖(1)
            green += listRGB[v].green;//⊖(1)
            blue += listRGB[v].blue;//⊖(1)
            if (adjDictArray[v] == null)//⊖(1)
                adjDictArray[v] = new Dictionary<int, double>();//⊖(1)
            counter++;//⊖(1)
            foreach (KeyValuePair<int, double> color in adjDictArray[v])//⊖(E)  all non visited edges 
            {
                if (!visited[color.Key])//⊖(1)
                    DFS_GetClusterColor(color.Key, visited);
            }

        }
        /// <summary>
        /// Replace Colors of Cluster With New Color  
        /// </summary>
        /// <param name="V">Distinct Colors Number</param>
        /// <param name="visited">To know if specific color is visted or no </param>
        void DfS_ColorReplace(int v, bool[] visited)//⊖(egdes+nodes)
        {
            // Mark the current node as visited and print it 
            visited[v] = true;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].red = (byte)red;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].green = (byte)green;//⊖(1)
            replacedMatrix[(int)listRGB[v].red, (int)listRGB[v].green, (int)listRGB[v].blue].blue = (byte)blue;//⊖(1)

            foreach (KeyValuePair<int, double> color in adjDictArray[v])//⊖(E) all non visited edges 
            {
                if (!visited[color.Key])//⊖(1)
                    DfS_ColorReplace(color.Key, visited);
            }


        }
    }
}