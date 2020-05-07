using NPDicoBiomes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using MCPMT = Projet_MCOptiMove.McPixelMapTools;

namespace Projet_MCOptiMove
{
    class MCOptiMove_Main
    {
        static void Main(string[] args)
        {
            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Desert"));

            string savePath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\RESULT_MAPS\";

            // bitmap
            Bitmap bmp1 = MCPMT.CreateRandomMap(width: 3, height: 3);
            bmp1.SetPixel(1, 0, desertColor);
            bmp1.SetPixel(1, 1, desertColor);
            bmp1.SetPixel(2, 1, desertColor);
            // Save (write) the images 
            bmp1.Save(savePath + "RandomPixel_1.png");
            Bitmap bmp1_Desert = MCPMT.IsolateBiome(bmp1, desertColor);
            bmp1_Desert.Save(savePath + "test_1.png");

            Bitmap bmp2 = MCPMT.CreateRandomMap(width: 6, height: 5);
            bmp2.SetPixel(1, 0, desertColor);
            bmp2.SetPixel(1, 1, desertColor);
            bmp2.SetPixel(2, 1, desertColor);
            bmp2.SetPixel(2, 2, desertColor);
            bmp2.SetPixel(2, 3, desertColor);
            bmp2.SetPixel(1, 3, desertColor);
            // Save (write) the images 
            bmp2.Save(savePath + "RandomPixel_2.png");
            Bitmap bmp2_Desert = MCPMT.IsolateBiome(bmp2, desertColor);
            bmp2_Desert.Save(savePath + "test_2.png");

            // TESTS MAPS
            char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
            char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
            List<int[]> listePixels = new List<int[]>();
            bool firstColumnDone = false;
            char mvtLoop = 'B';
            List<char> listeStop = new List<char>() { 'H' };

            // TESTS MAP 1

            //int[] startPixel_1 = MCPMT.GetFirstNoAlphaPixelCoordinate(bmp1_Desert);

            //listePixels = MCPMT.GetBiomeBorderList(bmp1_Desert, listePixels, startPixel_1, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);

            //foreach (var elt in listePixels)
            //{
            //    foreach (var coord in elt)
            //        Console.Write(coord);
            //    Console.WriteLine();
            //}

            // TEST MAP 2

            int[] startPixel_2 = MCPMT.GetFirstNoAlphaPixelCoordinate(bmp2_Desert);

            listePixels = MCPMT.GetBiomeBorderList(bmp2_Desert, listePixels, startPixel_2, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);

            foreach (var elt in listePixels)
            {
                foreach (var coord in elt)
                    Console.Write(coord);
                Console.WriteLine();
            }
        }

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
