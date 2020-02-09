using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
///Algorithms Project
///Intelligent Scissors
///

namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
    }
    public struct RGBPixelD
    {
        public double red, green, blue;
    }
    
  
    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }
        
        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox, int check)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (check == 0)
                        {
                            p[2] = cLustring.getMatrix()[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].red;
                            p[1] = cLustring.getMatrix()[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].green;
                            p[0] = cLustring.getMatrix()[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].blue;

                        }
                        else
                        {
                            p[2] = ImageMatrix[i, j].red;
                            p[1] = ImageMatrix[i, j].green;
                            p[0] = ImageMatrix[i, j].blue;
                        }
                           
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


       /// <summary>
       /// Apply Gaussian smoothing filter to enhance the edge detection 
       /// </summary>
       /// <param name="ImageMatrix">Colored image matrix</param>
       /// <param name="filterSize">Gaussian mask size</param>
       /// <param name="sigma">Gaussian sigma</param>
       /// <returns>smoothed color image</returns>
        public static RGBPixel[,] GaussianFilter1D(RGBPixel[,] ImageMatrix, int filterSize, double sigma)
        {
            /////hereeeeeeeeeeeeeee
            List<RGBPixelD> listRGP = findDistinctColors(ImageMatrix); //O(N^2)
            MST mst = new MST(listRGP); 
            mst.prim(); //O(E log V)
            cLustring = new CLustring(mst.getEdgesHeap(),listRGP,mst.getMSTList());
            ///AutoClustring  autoClustring =new AutoClustring(listRGP);
         
           
            Console.WriteLine();
            cLustring.buildCustring(getK(mst));
            // CLustring cLustring = new CLustring(autoClustring.getListOfColors(), autoClustring.getMSTDictionary());
            // cLustring.buildAutoCustring();
            ///return replaceNewColors(autoClustring.autoClustring(filterSize, listRGP, listRGP.Count), ImageMatrix);
            //return replaceNewColors(cLustring.getMatrix(), ImageMatrix);

            return ImageMatrix;

        }
        static CLustring cLustring;
        static int getK(MST mst)
        {
            int clusters = 1;
            double size = mst.getEdgesHeap().getSize();
            int distictcolors = (int)size;
            double cost = mst.totalCost;
            double brevSTDev = 0;
            double curSTDev = mst.STDev;
            double Mean = mst.Mean;
            Edge[] edges = mst.getEdgesHeap().Sortedges();
            int j = edges.Length - 1;
            int i = 0;
            double curSTDevRed = 0, brevSTDevRed = 0;
            while (Math.Abs(curSTDev - brevSTDev) > 0.0001)
            {
                brevSTDev = curSTDev;
                if (Math.Abs(edges[i].weight - Mean) > Math.Abs(edges[j].weight - Mean))
                {
                    cost -= edges[i].weight;
                    i++;
                }
                else
                {
                    cost -= edges[j].weight;
                    j--;
                }
                size--;
                Mean = cost / size;
                double sum = 0;
                for (int k = i; k <= j; k++)
                {
                    sum += Math.Abs(edges[k].weight - Mean) * Math.Abs(edges[k].weight - Mean);
                }
                curSTDev = Math.Sqrt(sum / Convert.ToDouble(size - 1));
                // curSTDevRed = Math.Abs(curSTDev - brevSTDev);
                clusters++;
            }
            if (distictcolors > clusters)
            return clusters;
            return distictcolors;  
        }
        /// <summary>
        /// Find Disticnt Colors in a image matrix 
        /// </summary>
        /// <param name="ImageMatrix">Colored image matrix</param>
        /// <returns>list of disticnt colors</returns>
        public static List<RGBPixelD> findDistinctColors(RGBPixel[,] ImageMatrix) // ==> ⊖(N^2)
        {
            bool [,,]arr=new bool[256,256,256]; //O(1)
            List<RGBPixelD> colorsHolder = new List<RGBPixelD>();//Θ(1)
            RGBPixelD rGBPixelD; //Θ(1)
            for (int i =0;i<GetHeight(ImageMatrix);i++)// iteration * body ===> N * O(Inner Loop) ==> N*N
            {
                for (int j = 0; j < GetWidth(ImageMatrix); j++) // iteration * body ===> N * O(1)
                {
                    if (!arr[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue])
                    {
                        arr[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] = true; //Θ(1)
                        rGBPixelD.red = ImageMatrix[i, j].red; //Θ(1)
                        rGBPixelD.green = ImageMatrix[i, j].green; //Θ(1)
                        rGBPixelD.blue = ImageMatrix[i, j].blue; //Θ(1)
                        colorsHolder.Add(rGBPixelD); //Θ(1)                    
                    }
                }
            }
            return colorsHolder; //Θ(1)
        }
        /// <summary>
        /// Replace Colors in the original matrix with the new colors after clustring   
        /// </summary>
        /// <param name="rGBPixels">Distinct Colors Number</param>
        /// <param name="ImageMatrix"> original Matrix of the image  </param>
        /// <returns>New Image Mattrix After its Colors Replaced </returns>
        public static RGBPixel[,] replaceNewColors(RGBPixel[,,] rGBPixels, RGBPixel[,] ImageMatrix)
        {
            RGBPixel rGBPixel;
            for (int i = 0; i < GetHeight(ImageMatrix); i++)
            {
                for (int j = 0; j < GetWidth(ImageMatrix); j++)
                {
                    rGBPixel.red = rGBPixels[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].red;
                    rGBPixel.green = rGBPixels[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].green;
                    rGBPixel.blue = rGBPixels[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue].blue;
                    ImageMatrix[i, j] = rGBPixel;
                }
            }
            return ImageMatrix;
        }
  

      
    }
}