using NPDicoBiomes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Projet_MCOptiMove
{
    class MCOptiMove_Main
    {
        static void Main(string[] args)
        {
            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = GetColor(dicoBiomes.GetColorOfBiome("Desert"));

            string savePath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\RESULT_MAPS\";

            // bitmap
            Bitmap bmp1 = CreateRandomMap(width: 3, height: 3);
            bmp1.SetPixel(1, 0, desertColor);
            bmp1.SetPixel(1, 1, desertColor);
            bmp1.SetPixel(2, 1, desertColor);
            // Save (write) the images 
            bmp1.Save(savePath + "RandomPixel_1.png");
            Bitmap bmp1_Desert = IsolateBiome(bmp1, desertColor);
            bmp1_Desert.Save(savePath + "test_1.png");

            Bitmap bmp2 = CreateRandomMap(width: 6, height: 5);
            bmp2.SetPixel(1, 0, desertColor);
            bmp2.SetPixel(1, 1, desertColor);
            bmp2.SetPixel(2, 1, desertColor);
            bmp2.SetPixel(2, 2, desertColor);
            bmp2.SetPixel(2, 3, desertColor);
            bmp2.SetPixel(1, 3, desertColor);
            // Save (write) the images 
            bmp2.Save(savePath + "RandomPixel_2.png");
            Bitmap bmp2_Desert = IsolateBiome(bmp2, desertColor);
            bmp2_Desert.Save(savePath + "test_2.png");

            // TESTS MAPS
            char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
            char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
            List<int[]> listePixels = new List<int[]>();
            bool firstColumnDone = false;
            char mvtLoop = 'B';
            List<char> listeStop = new List<char>() { 'H' };

            // TESTS MAP 1

            //int[] startPixel_1 = GetFirstNoAlphaPixelCoordinate(bmp1_Desert);

            //listePixels = Rlooping(bmp1_Desert, listePixels, startPixel_1, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);

            //foreach (var elt in listePixels)
            //{
            //    foreach (var coord in elt)
            //        Console.Write(coord);
            //    Console.WriteLine();
            //}

            // TEST MAP 2

            int[] startPixel_2 = GetFirstNoAlphaPixelCoordinate(bmp2_Desert);

            listePixels = Rlooping(bmp2_Desert, listePixels, startPixel_2, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);

            foreach (var elt in listePixels)
            {
                foreach (var coord in elt)
                    Console.Write(coord);
                Console.WriteLine();
            }

        }
        // Méthodes de tests
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
        // Méthodes
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
        public static void GetBiomeBorderList(Bitmap bmp, int[] startPixelCoordinate)
        {
            char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
            char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
            List<char> liste = new List<char>(mvtBas);

        }

        // char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
        // char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };

        // Bitmap bmDesert = IsolateBiome(bmp, desertColor);

        // List<int[]> listePixels = new List<int[]>();

        // int[] startPixel = GetFirstNoAlphaPixelCoordinate(bmDesert);
        // bool firstColumnDone = false;
        // char mvtLoop = 'B';
        // List<char> listeStop = new List<char>(){ 'H' };

        public static List<int[]> Rlooping(Bitmap bmp, List<int[]> listePixels, int[] pixel, ref bool firstColumnDone, char mvtLoop, List<char> listeStop, char[] mvtBas, char[] mvtHaut)
        {
            //
            Console.WriteLine("Je suis le Pixel ({0}, {1})", pixel[0], pixel[1]);
            //
            // 1ère condition de fin de rloop
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
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'H' }, mvtBas, mvtHaut);
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 > 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'D' }, mvtBas, mvtHaut);
                            break;

                        case ('D'):
                            // La contrainte 'G' doit être enlevée à partir du premier mouvement 'D'
                            if (!firstColumnDone)
                                firstColumnDone = true;
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'G' }, mvtBas, mvtHaut);
                            break;

                        case ('H'):
                            if ((pixel[1] - 1 < 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] - 1)))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'B' }, mvtBas, mvtHaut);
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
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0], pixel[1] - 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'B' }, mvtBas, mvtHaut);
                            break;

                        case ('D'):
                            if ((pixel[0] + 1 < bmp.Width) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] + 1, pixel[1])))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0] + 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'G' }, mvtBas, mvtHaut);
                            break;

                        case ('G'):
                            if ((pixel[0] - 1 > 0) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0] - 1, pixel[1])))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0] - 1, pixel[1] }, ref firstColumnDone, mvtLoop, new List<char> { 'D' }, mvtBas, mvtHaut);
                            break;

                        case ('B'):
                            if ((pixel[1] + 1 < bmp.Height) && (bmp.GetPixel(pixel[0], pixel[1]) == bmp.GetPixel(pixel[0], pixel[1] + 1)))
                                listePixels = Rlooping(bmp, listePixels, new int[2] { pixel[0], pixel[1] + 1 }, ref firstColumnDone, mvtLoop, new List<char> { 'H' }, mvtBas, mvtHaut);
                            break;
                    }
                }
            }
            return listePixels;
        }
    }
}
