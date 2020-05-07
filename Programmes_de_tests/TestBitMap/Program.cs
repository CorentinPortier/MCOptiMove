using System;
using System.Drawing;

namespace TestBitMap
{
    class Program
    {
        static void Main(string[] args)
        {
            test();
            Console.WriteLine("Image créée !");
        }
        static void test()
        {
            int width = 640, height = 320;

            // bitmap
            Bitmap bmp = new Bitmap(width, height);

            // Random number

            Random rand = new Random();

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

            // Save (write) the image
            bmp.Save(@"C:\Users\portable\Documents\Travail\Moi\Programmation\C#\Projet_MCOptiMove\Programmes_de_tests\TestBitMap\RandomPixel.png");
        }
    }
}
