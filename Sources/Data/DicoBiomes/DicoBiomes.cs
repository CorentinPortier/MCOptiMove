using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPDicoBiomes
{
    public class DicoBiomes
    {
        // Variable
        public Dictionary<string, List<List<string>>> dicoBiomes { get; set; }
        private string dicoPath = @"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\Sources\Data\DicoBiomes.txt";

        // Constructeur
        public DicoBiomes()
        {
            try
            {
                this.dicoBiomes = JsonConvert.DeserializeObject<Dictionary<string, List<List<string>>>>(File.ReadAllText(this.dicoPath));
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Le fichier .txt est non existant. Code d'erreur :\n" + ex.Message);
            }
        }

        // Méthodes
        public List<int> GetColorOfBiome(string researchedBiome)
        {
            string temp = "";
            List<int> rgb = new List<int>();

            foreach (KeyValuePair<string, List<List<string>>> item in this.dicoBiomes)
            {
                foreach (var biomeDetail in item.Value)
                {
                    if (biomeDetail.Contains(researchedBiome))
                    {
                        foreach (var chiffre16 in biomeDetail[2])
                        {
                            temp += chiffre16;
                            if (temp.Length == 2)
                            {
                                //Console.WriteLine(temp);
                                rgb.Add(Convert.ToInt32(new string(temp), 16));
                                temp = "";
                            }
                        }
                        return rgb;
                    }
                }
            }
            throw new Exception("Le nom du biome recherché n'existe pas.");
        }
        public void Display()
        {
            foreach (KeyValuePair<string, List<List<string>>> item in this.dicoBiomes)
            {
                Console.WriteLine("Famille de biomes: {0}\nDétail:", item.Key);
                foreach (var biome in item.Value)
                {
                    Console.WriteLine("\t- Id du biome : " + biome[0] + "\n" +
                                      "\t- Nom du biome : " + biome[1] + "\n" +
                                      "\t- Couleur du biome : " + biome[2]);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
