using NPDicoBiomes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using MCPMT = Projet_MCOptiMove.McPixelMapTools;

namespace Projet_MCOptiMove
{
    class MCOptiMove_Main
    {
        static void Main(string[] args)
        {
            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Desert"));

            //string savePath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\RESULT_MAPS\";          

            // TESTS MAPS
            //char[] mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
            //char[] mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
            //List<int[]> listePixels = new List<int[]>();
            //bool firstColumnDone = false;
            //char mvtLoop = 'B';
            //List<char> listeStop = new List<char>() { 'H' };

            // TESTS MAP 1

            //int[] startPixel_1 = MCPMT.GetFirstNoAlphaPixelCoordinate(bmp1_Desert);

            //listePixels = MCPMT.GetBiomeBorderList(bmp1_Desert, listePixels, startPixel_1, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);

            //foreach (var elt in listePixels)
            //{
            //    foreach (var coord in elt)
            //        Console.Write(coord);
            //    Console.WriteLine();
            //}
        }
    }
}
