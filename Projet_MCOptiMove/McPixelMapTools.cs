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
        public static Bitmap AddPixelsToMap(Bitmap mapBase, List<int[]> listePixels, Color biomeColor)
        {
            foreach(var pixel in listePixels)
            {
                mapBase.SetPixel(pixel[0], pixel[1], biomeColor);
            }
            return mapBase;
        }
        public static Bitmap DelPixelsOfMap(Bitmap mapBase, List<int[]> listePixels)
        {
            foreach (var pixel in listePixels)
            {
                mapBase.SetPixel(pixel[0], pixel[1], Color.FromArgb(alpha: 0, Color.White));
            }
            return mapBase;
        }
        public static Bitmap IsolateBiome(Bitmap bmpIn, Color researchedColor)
        {
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
        public static Bitmap IsolateBiomes(Bitmap bmpIn, List<Color> researchedColors)
        {
            Bitmap filtratedBitmap = new Bitmap(bmpIn);

            for (int i = 0; i < bmpIn.Width; ++i)
            {
                for (int y = 0; y < bmpIn.Height; ++y)
                {
                    if (researchedColors.Contains(bmpIn.GetPixel(i, y)))
                    {
                        continue;
                    }
                    else
                    {
                        //On supprime les couleurs non voulues
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
        public static List<int[]> GetBiomeBorderList(Bitmap bmp, List<int[]> listePixels, int[] pixel, ref bool firstColumnDone, ref bool stopAllLoops,char mvtLoop, List<char> listeStop, char[] mvtBas, char[] mvtHaut)
        {
            //
            Console.WriteLine("Je suis le Pixel ({0}, {1})", pixel[0], pixel[1]);
            //
            // 1ère condition de fin de la boucle récursive
            try
            {
                if (listePixels[0].SequenceEqual(pixel))
                {
                    stopAllLoops = true;
                    return listePixels;
                }
            }
            // Exception levée à la première boucle car la liste est vide
            catch (Exception) { }

            listePixels.Add(pixel);

            //
            Console.WriteLine("mvtLoop =" + mvtLoop);
            //

            if (mvtLoop == 'B')
            {
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

                foreach (char direction in mvtsPixel)
                {
                    //
                    Console.WriteLine("({0}, {1}) prend la direction {2}", pixel[0], pixel[1], direction);
                    //
                    switch (direction)
                    {
                        case ('B'):
                            if ((pixel[1] + 1 < bmp.Height) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] + 1)))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, ref stopAllLoops, mvtLoop, new List<char> { 'H' }, mvtBas, mvtHaut);
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 >= 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, ref stopAllLoops, mvtLoop, new List<char> { 'D' }, mvtBas, mvtHaut);
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('D'):
                            // La contrainte 'G' doit être enlevée à partir du premier mouvement 'D'
                            if (!firstColumnDone)
                                firstColumnDone = true;
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, ref stopAllLoops, mvtLoop, new List<char> { 'G' }, mvtBas, mvtHaut);
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('H'):
                            if ((pixel[1] - 1 >= 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] - 1)))
                            {
                                // On change le sens de la boucle;
                                // mvtLoop = 'H';
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, ref stopAllLoops, mvtLoop: 'H', new List<char> { 'B' }, mvtBas, mvtHaut);
                            }
                            if (stopAllLoops)
                                return listePixels;
                            break;
                    }
                }
            }
            if (mvtLoop == 'H')
            {
                List<char> mvtsPixel = new List<char>(mvtHaut);

                foreach (char mvt in listeStop)
                {
                    mvtsPixel.Remove(mvt);
                }
                //
                foreach (char mvt in listeStop)
                    Console.WriteLine("Mouvement supprimés : " + mvt);
                //

                foreach (char direction in mvtsPixel)
                {
                    //
                    Console.WriteLine("({0}, {1}) prend la direction {2}", pixel[0], pixel[1], direction);
                    //
                    switch (direction)
                    {
                        case ('H'):
                            if ((pixel[1] - 1 >= 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] - 1)))
                            {
                                // On garde le sens de la boucle;
                                // mvtLoop = 'H';
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, ref stopAllLoops, mvtLoop: 'H', new List<char> { 'B' }, mvtBas, mvtHaut);
                            }
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('D'):
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                            {
                                // On garde le sens de la boucle;
                                // mvtLoop = 'H';
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, ref stopAllLoops, mvtLoop: 'H', new List<char> { 'G' }, mvtBas, mvtHaut);
                            }
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 >= 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                            {
                                // On garde le sens de la boucle;
                                // mvtLoop = 'H';
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, ref stopAllLoops, mvtLoop: 'H', new List<char> { 'D' }, mvtBas, mvtHaut);
                            }
                            if (stopAllLoops)
                                return listePixels;
                            break;

                        case ('B'):
                            if ((pixel[1] + 1 < bmp.Height) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] + 1)))
                            {
                                // On change le sens de la boucle;
                                // mvtLoop = 'B';
                                listePixels = GetBiomeBorderList(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, ref stopAllLoops, mvtLoop: 'B', new List<char> { 'H' }, mvtBas, mvtHaut);
                            }
                            if (stopAllLoops)
                                return listePixels;
                            break;
                    }
                }
            }
            return listePixels;
        }
        public static List<int[]> GetBottom(List<int[]> listePixels)
        {
            List<int> listeDesX = new List<int>();
            foreach(int[] coords in listePixels)
            {
                if(!listeDesX.Contains(coords[0]))
                {
                    listeDesX.Add(coords[0]);
                }
            }
            List<int[]> returnedList = Bottom(listeDesX, listePixels);
            return returnedList;
        }

        // Méthodes dépendantes des précédentes méthodes.
        static public Dictionary<Color, List<List<int[]>>> AddBiomesCluster(Bitmap myMap, Dictionary<Color, List<List<int[]>>> myDic, Color biomeColor)
        {
            bool stopThis = false;
            List<List<int[]>> ClustersList = new List<List<int[]>>();
            Bitmap bmp_biome = IsolateBiome(myMap, biomeColor);

            while (!stopThis)
            {
                try
                {
                    int[] startPixel = GetFirstNoAlphaPixelCoordinate(bmp_biome);

                    char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
                    char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
                    List<int[]> listeDesPixels = new List<int[]>();
                    bool firstColumnDone = false;
                    bool stopAllLoops = false;
                    char mvtLoop = 'B';
                    List<char> listeStop = new List<char>() { 'H' };

                    listeDesPixels = GetBiomeBorderList(bmp_biome, listeDesPixels, startPixel, ref firstColumnDone, ref stopAllLoops, mvtLoop, listeStop, mvtBas, mvtHaut);

                    ClustersList.Add(listeDesPixels);

                    DelPixelsOfMap(bmp_biome, listeDesPixels);
                }
                catch (NullReferenceException)
                {
                    stopThis = true;
                }
            }
            myDic.Add(biomeColor, ClustersList);
            return myDic;
        }

        // Méthodes outil
        private static List<int[]> Bottom(List<int> listeDesX, List<int[]> listePixels)
        {
            List<int[]> returnedList = new List<int[]>();
            foreach (int x in listeDesX)
            {
                int xSaved = x;
                int ySaved = 0;

                foreach (int[] coords in listePixels)
                {
                    if (coords[0] == xSaved)
                    {
                        if (coords[1] > ySaved)
                        {
                            ySaved = coords[1];
                        }
                    }
                }
                returnedList.Add(new int[2] { xSaved, ySaved });
            }
            return returnedList;
        }
    }
}
