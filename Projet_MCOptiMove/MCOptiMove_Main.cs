using NPDicoBiomes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using MCPMT = Projet_MCOptiMove.McPixelMapTools;



/*string[] arrayBiomes = { "Ocean", "Frozen Ocean", "Deep Frozen Ocean", "Cold Ocean", "Deep Cold Ocean", "Deep Ocean", "Lukewarm Ocean",
                                     "Deep Lukewarm Ocean", "Warm Ocean", "Deep Warm Ocean", "River", "Frozen River", "Beach", "Snowy Beach", "Stone Shore",
                                     "Plains", "Sunflower Plains", "Forest", "Flower Forest", "Wooded Hills", "Birch Forest", "Tall Birch Forest",
                                     "Birch Forest Hills", "Tall Birch Hills", "Dark Forest", "Dark Forest Hills", "Taiga", "Taiga Mountains", "Taiga Hills",
                                     "Giant Tree Taiga", "Giant Spruce Taiga", "Giant Spruce Taiga Hills", "Giant Tree Taiga Hills", "Jungle", "Modified Jungle",
                                     "Jungle Hills", "Jungle Edge", "Modified Jungle Edge", "Bamboo Jungle", "Bamboo Jungle Hills", "Mountains",
                                     "Gravelly Mountains", "Mountain Edge", "Wooded Mountains", "Gravelly Mountains+", "Swamp", "Swamp Hills", "Desert",
                                     "Desert Lakes", "Desert Hills", "Savanna", "Shattered Savanna", "Savanna Plateau", "Shattered Savanna Plateau",
                                     "Badlands", "Eroded Badlands", "Badlands Plateau", "Modified Badlands Plateau", "Wooded Badlands Plateau",
                                     "Modified Wooded Badlands Plateau", "Snowy Tundra", "Snowy Mountains", "Ice Spikes", "Snowy Taiga", "Snowy Taiga Mountains",
                                     "Snowy Taiga Hills", "Mushroom Fields", "Mushroom Field Shore", "Nether Wastes", "Soul Sand Valley", "Crimson Forest",
                                     "Warped Forest", "Basalt Deltas", "The End", "Small End Islands", "End Midlands", "End Highlands", "End Barrens", "The Void"};*/
namespace Projet_MCOptiMove
{
    class MCOptiMove_Main
    {
        static void Main(string[] args)
        {

            var watch = new Stopwatch();

            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Desert"));

            //bitmap
            Bitmap bmp = MCPMT.CreateRandomMap(width: 7, height: 8);
            bmp.SetPixel(1, 2, desertColor);
            bmp.SetPixel(1, 3, desertColor);
            bmp.SetPixel(1, 4, desertColor);
            bmp.SetPixel(1, 5, desertColor);
            bmp.SetPixel(2, 2, desertColor);
            bmp.SetPixel(3, 2, desertColor);
            bmp.SetPixel(4, 2, desertColor);
            bmp.SetPixel(4, 3, desertColor);
            bmp.SetPixel(4, 4, desertColor);
            bmp.SetPixel(3, 4, desertColor);
            bmp.SetPixel(3, 5, desertColor);
            bmp.SetPixel(3, 6, desertColor);
            bmp.SetPixel(3, 7, desertColor);
            bmp.SetPixel(4, 7, desertColor);
            bmp.SetPixel(5, 7, desertColor);
            bmp.SetPixel(6, 7, desertColor);
            bmp.SetPixel(6, 6, desertColor);
            bmp.SetPixel(6, 5, desertColor);
            bmp.SetPixel(5, 5, desertColor);
            bmp.SetPixel(5, 4, desertColor);
            bmp.SetPixel(5, 3, desertColor);
            bmp.SetPixel(6, 3, desertColor);
            bmp.SetPixel(5, 2, desertColor);
            bmp.SetPixel(4, 1, desertColor);
            bmp.SetPixel(4, 0, desertColor);
            bmp.SetPixel(3, 1, desertColor);
            bmp.SetPixel(2, 1, desertColor);

            bmp.SetPixel(3, 0, desertColor);

            //bmp.SetPixel(1, 6, desertColor);
            //bmp.SetPixel(1, 7, desertColor);
            //bmp.SetPixel(2, 7, desertColor);


            string savePath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\RESULT_MAPS\";
            bmp.Save(savePath + "RandomPixel_nope.png");
            Bitmap bmp_desert = MCPMT.IsolateBiome(bmp, desertColor);
            bmp_desert.Save(savePath + "nope.png");


            char[] mvtBas = new char[4] { 'G', 'B', 'D', 'H' };
            char[] mvtHaut = new char[4] { 'D', 'H', 'G', 'B' };
            List<int[]>  listePixels = new List<int[]>();
            bool firstColumnDone = false;
            bool stopAllLoops = false;
            char mvtLoop = 'B';
            List<char> listeStop = new List<char>() { 'H' };

            int[] startPixel = MCPMT.GetFirstNoAlphaPixelCoordinate(bmp_desert);

            foreach(var pix in MCPMT.GetBiomeBorderList(bmp_desert, listePixels, startPixel, ref firstColumnDone, ref stopAllLoops, mvtLoop, listeStop, mvtBas, mvtHaut))
            {
                Console.WriteLine("[{0}, {1}]",pix[0], pix[1]);
            }


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
 
        static public Bitmap getMyMap(Bitmap bmpBase, List<List<int[]>> listeDesClusters)
        {
            Bitmap myMap = new Bitmap(bmpBase);

            return myMap;
        }
    }
}
