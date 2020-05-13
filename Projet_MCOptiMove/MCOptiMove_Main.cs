using NPDicoBiomes;
using System;
using System.Collections.Generic;
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
            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Desert"));
            Color gravelColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Gravelly Mountains"));
            Color icebergColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Deep Frozen Ocean"));

            Color myPoints = Color.FromArgb(255, 0, 0);

            List<Color> colorList = new List<Color> { desertColor, gravelColor, icebergColor, myPoints };

            Bitmap bmp = MCPMT.CreateRandomMap(width: 13, height: 12);
            bmp.SetPixel(0, 0, desertColor);
            bmp.SetPixel(1, 0, desertColor);
            bmp.SetPixel(0, 1, desertColor);
            bmp.SetPixel(1, 1, desertColor);

            bmp.SetPixel(1, 4, gravelColor);
            for(int y = 5; y<8;++y)
            {
                for(int x = 0; x<2;++x)
                {
                    bmp.SetPixel(x, y, gravelColor);
                }
            }

            bmp.SetPixel(0, 9, icebergColor);
            bmp.SetPixel(1, 9, icebergColor);
            bmp.SetPixel(1, 10, icebergColor);
            bmp.SetPixel(1, 11, icebergColor);

            bmp.SetPixel(11, 6, desertColor);
            bmp.SetPixel(12, 6, desertColor);
            bmp.SetPixel(11, 7, desertColor);
            bmp.SetPixel(12, 7, desertColor);

            bmp.SetPixel(8, 2, icebergColor);
            bmp.SetPixel(9, 2, icebergColor);
            bmp.SetPixel(8, 3, icebergColor);
            bmp.SetPixel(9, 3, icebergColor);
            bmp.SetPixel(9, 4, icebergColor);

            bmp.SetPixel(6, 10, myPoints);

            string savePath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\RESULT_MAPS\";
            bmp.Save(savePath + "RandomPixel_bigmap.png");
            Bitmap bmp_allBiomes = MCPMT.IsolateBiomes(bmp, colorList);
            bmp_allBiomes.Save(savePath + "bigmap.png");
            Bitmap bmp_desert = MCPMT.IsolateBiome(bmp, desertColor);
            bmp_desert.Save(savePath + "bigmap_desert.png");


            Dictionary<Color, List<List<int[]>>> dicoCluster = new Dictionary<Color, List<List<int[]>>>();

            MCPMT.AddBiomesCluster(bmp, dicoCluster, desertColor);
            MCPMT.AddBiomesCluster(bmp, dicoCluster, icebergColor);

            foreach (KeyValuePair<Color, List<List<int[]>>> item in dicoCluster)
            {
                Console.WriteLine("Couleur : " + item.Key + ": ");
                foreach (var cluster in item.Value)
                {
                    Console.WriteLine("cluster :");
                    foreach( var coords in cluster)
                    {
                        Console.Write("[{0},{1}]", coords[0], coords[1]);
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n");

            //List<int[]> clusternew = dicoCluster[icebergColor][1];
            //List<int[]> temp = MCPMT.GetBottom(clusternew);

            foreach (KeyValuePair<Color, List<List<int[]>>> item in dicoCluster)
            {
                foreach( var cluster in item.Value)
                {
                    List<int[]> temp = MCPMT.GetBottom(cluster);
                    foreach(var coords in temp)
                        Console.Write("[{0},{1}]", coords[0], coords[1]);
                    Console.WriteLine();
                }
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
