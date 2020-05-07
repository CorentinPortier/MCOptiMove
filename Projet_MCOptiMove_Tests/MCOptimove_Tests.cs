using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPDicoBiomes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MCPMT = Projet_MCOptiMove.McPixelMapTools;

//      _   _    _           ___   ___ 
//     /_\ (_)__| |___ ___  / _ \ / __|
//    / _ \| / _` / -_|_-< | (_) | (__ 
//   /_/ \_\_\__,_\___/__/  \___(_)___|
//                                     
// https://openclassrooms.com/fr/courses/2818931-programmez-en-oriente-objet-avec-c/2819216-les-tests-unitaires
// https://openclassrooms.com/fr/courses/1730206-apprenez-asp-net-mvc/1828221-tp-completer-le-modele#/id/r-1937189

namespace Projet_MCOptiMove_Tests
{
    [TestClass]
    public class MCOptimove_Tests
    {
        private char[] mvtBas;
        private char[] mvtHaut;
        private List<int[]> listePixels;
        private bool firstColumnDone;
        private char mvtLoop;
        private List<char> listeStop;

        [TestInitialize]
        public void InitialisationDesTests()
        {
            mvtBas = new char[4] { 'B', 'G', 'D', 'H' };
            mvtHaut = new char[4] { 'H', 'D', 'G', 'B' };
            listePixels = new List<int[]>();
            firstColumnDone = false;
            mvtLoop = 'B';
            listeStop = new List<char>() { 'H' };
        }
        [TestCleanup]
        public void NettoyageDesTests()
        {
        }
        //    __  __   __ _   _               _                 _        _            _   
        //   |  \/  | /_/| | | |             | |               | |      | |          | |  
        //   | \  / | ___| |_| |__   ___   __| | ___  ___    __| | ___  | |_ ___  ___| |_ 
        //   | |\/| |/ _ \ __| '_ \ / _ \ / _` |/ _ \/ __|  / _` |/ _ \ | __/ _ \/ __| __|
        //   | |  | |  __/ |_| | | | (_) | (_| |  __/\__ \ | (_| |  __/ | ||  __/\__ \ |_ 
        //   |_|  |_|\___|\__|_| |_|\___/ \__,_|\___||___/  \__,_|\___|  \__\___||___/\__|
        //                                                                                
        // 
        [TestMethod]
        public void CreerDicoBiome_ObtientBienLesNomsDesBiomesDuSite()
        {
            //      _                              
            //     /_\  _ _ _ _ __ _ _ _  __ _ ___ 
            //    / _ \| '_| '_/ _` | ' \/ _` / -_)
            //   /_/ \_\_| |_| \__,_|_||_\__, \___|
            //                           |___/     
            string[] arrayBiomes = { "Ocean", "Frozen Ocean", "Deep Frozen Ocean", "Cold Ocean", "Deep Cold Ocean", "Deep Ocean", "Lukewarm Ocean",
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
                                     "Warped Forest", "Basalt Deltas", "The End", "Small End Islands", "End Midlands", "End Highlands", "End Barrens", "The Void"};
            HashSet<string> biomes = new HashSet<string>(arrayBiomes);
            //      _      _   
            //     /_\  __| |_ 
            //    / _ \/ _|  _|
            //   /_/ \_\__|\__|
            //
            DicoBiomes DicoBiomesTest = new DicoBiomes();
            //      _                   _   
            //     /_\   ______ ___ _ _| |_ 
            //    / _ \ (_-<_-</ -_) '_|  _|
            //   /_/ \_\/__/__/\___|_|  \__|
            // 
            foreach (KeyValuePair<string, List<List<string>>> item in DicoBiomesTest.dicoBiomes)
            {
                foreach (var biome in item.Value)
                {
                    Assert.IsTrue(biomes.Contains(biome[1]));
                }
            }
        }
        [TestMethod]
        public void CreerCartePersonnalisee_ObtientBienLabonneListeDePixel()
        {
            //      _                              
            //     /_\  _ _ _ _ __ _ _ _  __ _ ___ 
            //    / _ \| '_| '_/ _` | ' \/ _` / -_)
            //   /_/ \_\_| |_| \__,_|_||_\__, \___|
            // 
            DicoBiomes dicoBiomes = new DicoBiomes();
            Color desertColor = MCPMT.GetColor(dicoBiomes.GetColorOfBiome("Desert"));

            // bitmap
            Bitmap bmp1 = MCPMT.CreateRandomMap(width: 3, height: 3);
            bmp1.SetPixel(1, 0, desertColor);
            bmp1.SetPixel(1, 1, desertColor);
            bmp1.SetPixel(2, 1, desertColor);

            Bitmap bmp1_Desert = MCPMT.IsolateBiome(bmp1, desertColor);

            int[] startPixel_1 = MCPMT.GetFirstNoAlphaPixelCoordinate(bmp1_Desert);
            //      _      _   
            //     /_\  __| |_ 
            //    / _ \/ _|  _|
            //   /_/ \_\__|\__|
            //
            listePixels = MCPMT.GetBiomeBorderList(bmp1_Desert, listePixels, startPixel_1, ref firstColumnDone, mvtLoop, listeStop, mvtBas, mvtHaut);
            //      _                   _   
            //     /_\   ______ ___ _ _| |_ 
            //    / _ \ (_-<_-</ -_) '_|  _|
            //   /_/ \_\/__/__/\___|_|  \__|
            //
            List<int[]> testListe = new List<int[]> { new int[2] { 1, 0 }, new int[2] { 1, 1 }, new int[2] { 2, 1 } };
            int i = 0;
            foreach (var pixel in listePixels)
            {
                Assert.IsTrue(testListe[i].SequenceEqual(pixel));
                ++i;
            }
        }
        public void Model()
        {
            //      _                              
            //     /_\  _ _ _ _ __ _ _ _  __ _ ___ 
            //    / _ \| '_| '_/ _` | ' \/ _` / -_)
            //   /_/ \_\_| |_| \__,_|_||_\__, \___|
            //                           |___/     

            //      _      _   
            //     /_\  __| |_ 
            //    / _ \/ _|  _|
            //   /_/ \_\__|\__|
            //
                
            //      _                   _   
            //     /_\   ______ ___ _ _| |_ 
            //    / _ \ (_-<_-</ -_) '_|  _|
            //   /_/ \_\/__/__/\___|_|  \__|
            //                              
        }
        //    __  __   __ _   _               _             _ _                                 _      _ _ 
        //   |  \/  | /_/| | | |             | |           ( | )                               | |    ( | )
        //   | \  / | ___| |_| |__   ___   __| | ___  ___   V V___ _   _ _ __  _ __   ___  _ __| |_ ___V V 
        //   | |\/| |/ _ \ __| '_ \ / _ \ / _` |/ _ \/ __|    / __| | | | '_ \| '_ \ / _ \| '__| __/ __|   
        //   | |  | |  __/ |_| | | | (_) | (_| |  __/\__ \    \__ \ |_| | |_) | |_) | (_) | |  | |_\__ \   
        //   |_|  |_|\___|\__|_| |_|\___/ \__,_|\___||___/    |___/\__,_| .__/| .__/ \___/|_|   \__|___/   
        //                                                              | |   | |                          
        //                                                              |_|   |_|                          
        private Bitmap CreateRandomMap(int width, int height)
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
    }
}