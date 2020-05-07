using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Projet_MCOptiMove
{
    public static class McPixelMapTools
    {
        public static Bitmap CreateRandomMap(int width, int height)
        {
            // Random number
            Random rand = new Random();

            // bitmap
            Bitmap bmp = new Bitmap(width, height);

            // create random pixels
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Generate random ARGB values
                    int a = rand.Next(256);
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);

                    // Set ARGB value
                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return bmp;
        }
        public static Bitmap IsolateBiome(Bitmap bmpIn, Color researchedColor)
        {
            List<string> mvt = new List<string> { "B", "G", "D", "H" };
            Bitmap filtratedBitmap = new Bitmap(bmpIn);

            for (int i = 0; i < bmpIn.Width; ++i)
            {
                for (int y = 0; y < bmpIn.Height; ++y)
                {
                    if (bmpIn.GetPixel(i, y) == researchedColor)
                    {
                        continue;
                    }
                    else
                    {
                        filtratedBitmap.SetPixel(i, y, Color.FromArgb(alpha: 0, Color.White));
                    }
                }
            }
            return filtratedBitmap;
        }
        public static Color GetColor(List<int> RGB)
        {
            return Color.FromArgb(RGB[0], RGB[1], RGB[2]);
        }
        public static int[] GetFirstNoAlphaPixelCoordinate(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Width; ++i)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    if (bmp.GetPixel(i, y).A != 0)
                        return new int[] { i, y };
                }
            }
            return null;
        }
        public static List<int[]> GetBiomeBorderList(Bitmap bmp, List<int[]> listePixels, int[] pixel, ref bool firstColumnDone, char mvtLoop, List<char> listeStop, char[] mvtBas, char[] mvtHaut)
        {
            //
            Console.WriteLine("Je suis le Pixel ({0}, {1})", pixel[0], pixel[1]);
            //
            // 1ère condition de fin de la boucle récursive
            try
            {
                if (listePixels[0].SequenceEqual(pixel))
                {
                    //stopAllLoops = true;
                    return listePixels;
                }
            }
            // Levé à la première boucle car la liste est vide
            catch (Exception) { }

            listePixels.Add(pixel);

            List<char> mvtsPixel = new List<char>(mvtBas);
            if (!firstColumnDone)
                listeStop.Add('G');

            foreach (char mvt in listeStop)
            {
                mvtsPixel.Remove(mvt);
            }

            //
            foreach (char mvt in listeStop)
                Console.WriteLine("Mouvement supprimés : " + mvt);
            //

            if (mvtLoop == 'B')
            {
                foreach (char direction in mvtsPixel)
                {
                    //
                    Console.WriteLine("({0}, {1}) prend la direction {2}", pixel[0], pixel[1], direction);
                    //
                    switch (direction)
                    {
                        case ('B'):
                            if ((pixel[1] + 1 < bmp.Height) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] + 1)))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'H' }, mvtBas, mvtHaut);
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 > 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'D' }, mvtBas, mvtHaut);
                            break;

                        case ('D'):
                            // La contrainte 'G' doit être enlevée à partir du premier mouvement 'D'
                            if (!firstColumnDone)
                                firstColumnDone = true;
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'G' }, mvtBas, mvtHaut);
                            break;

                        case ('H'):
                            if ((pixel[1] - 1 < 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] - 1)))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'B' }, mvtBas, mvtHaut);
                            break;
                    }
                }
            }
            if (mvtLoop == 'H')
            {
                foreach (char direction in mvtsPixel)
                {
                    switch (direction)
                    {
                        case ('H'):
                            if ((pixel[1] - 1 < 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] - 1)))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'B' }, mvtBas, mvtHaut);
                            break;

                        case ('D'):
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'G' }, mvtBas, mvtHaut);
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 > 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'D' }, mvtBas, mvtHaut);
                            break;

                        case ('B'):
                            if ((pixel[1] + 1 < bmp.Height) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] + 1)))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'H' }, mvtBas, mvtHaut);
                            break;
                    }
                }
            }
            return listePixels;
        }
    }
}
