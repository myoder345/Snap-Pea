using System;
using Newtonsoft.Json;

namespace Region_Deserialize_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Layout layout = JsonConvert.DeserializeObject<Layout>(System.IO.File.ReadAllText("FourSquare.json"));

            int width;
            int height;

            Console.Out.Write("Please enter screen width: ");
            while(!Int32.TryParse(Console.ReadLine(), out width))
            {
                Console.Out.Write("Width must be an integer\nPlease enter screen width: ");
            }

            Console.Out.Write("Please enter screen height: ");
            while (!Int32.TryParse(Console.ReadLine(), out height))
            {
                Console.Out.Write("Height must be an integer\nPlease enter screen width: ");
            }

            Console.Out.WriteLine("Resolution: " + width + "x" + height);

            int i = 0;
            foreach(Region r in layout.regions)
            {
                Console.Out.WriteLine("Region " + i + " spans from (" + r.Left * width + ", " + r.Top * height + ") to (" + ((r.Left + r.Width) * width - 1) + ", " + ((r.Top + r.Height) * height - 1) + ")");
                ++i;
            }

            Console.Out.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
