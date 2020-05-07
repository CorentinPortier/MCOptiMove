using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NPDicoBiomes
{
    class Program
    {
        static void Main(string[] args)
        {
            //string pathToDataFolder = (Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent).ToString();
            //string fileName = "DicoBiomes.txt";
            //string path =pathToDataFolder+@"\"+fileName;
            string path = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\Sources\Data\DicoBiomes.txt";
            var dicoBiomes = GetBiomesDictionnary();

            SaveDicoBiomes(dicoBiomes, path);
            //DisplayDicoBiomes(dicoBiomes);
        }
        private static Dictionary<string, List<List<string>>> GetBiomesDictionnary()
        {
            // https://docs.scrapy.org/en/xpath-tutorial/topics/xpath-tutorial.html

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://fr-minecraft.net/52-biomes-minecraft.php");

            List<HtmlNode> Zones = doc.DocumentNode.SelectNodes("//h3[@class = 'h-green']").ToList();
            List<HtmlNode> Familles = doc.DocumentNode.SelectNodes("//h4[@class = 'h-white']").ToList();
            List<HtmlNode> ListeLignesTableaux = doc.DocumentNode.SelectNodes("//table//tr").ToList();

            // https://www.tutorialsteacher.com/csharp/csharp-dictionary
            Dictionary<string, List<List<string>>> dicoBiomes = new Dictionary<string, List<List<string>>>();
            int numeroFamille = -1;
            string idBiome = "";
            string nomBiome = "";
            string couleurBiome = "";
            List<string> biome = new List<string>();
            List<List<string>> biomeFamille = new List<List<string>>();

            foreach (var ligne in ListeLignesTableaux)
            {
                //Pour skip le premier "ID"
                if (numeroFamille == -1)
                {
                    numeroFamille += 1;
                    continue;
                }

                if (!ligne.InnerText.Contains("ID"))
                {
                    idBiome = ligne.SelectSingleNode("./td[@class = 'id']/text()").InnerText;
                    nomBiome = ligne.SelectSingleNode("./td/em").InnerText;
                    couleurBiome = ligne.SelectSingleNode("./td[@class = 'id']/@style").Attributes[1].Value.Substring(19, 6);

                    //Console.WriteLine(idBiome + nomBiome + couleurBiome);
                    biome.Add(idBiome);
                    biome.Add(nomBiome);
                    biome.Add(couleurBiome);
                    biomeFamille.Add(new List<string>(biome));
                    //Console.WriteLine(biomeFamille[0][0] +", "+ biomeFamille[0][1]);
                    biome.Clear();
                    continue;
                }
                //Console.WriteLine(Familles[numeroFamille].InnerText +"\n"+ biomeFamille.Count + "\n");
                dicoBiomes.Add(Familles[numeroFamille].InnerText, new List<List<string>>(biomeFamille));
                biomeFamille.Clear();
                numeroFamille += 1;
            }
            return dicoBiomes;
        }
        static void SaveDicoBiomes(Dictionary<string, List<List<string>>> dicoBiomes, string path)
        {
            //https://stackoverflow.com/questions/27025435/how-do-i-read-and-write-a-c-sharp-string-dictionary-to-a-file
            string json = JsonConvert.SerializeObject(dicoBiomes);
            File.WriteAllText(path, json);
        }

        static void DisplayDicoBiomes(Dictionary<string, List<List<string>>> dicoBiomes)
        {
            foreach (KeyValuePair<string, List<List<string>>> item in dicoBiomes)
            {
                Console.WriteLine("Famille de biomes: {0}\nDétail:", item.Key);
                foreach (var biome in item.Value)
                {
                    Console.WriteLine(biome[0] + " " + biome[1] + " " + biome[2]);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
