using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Region_Serialize_Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Layout FourSquare = new Layout
            {
                regions = new List<Region>
                {
                    new Region
                    {
                        Left = 0f,
                        Top = 0f,
                        Width = 0.5f,
                        Height = 0.5f,
                        Color = 0xff0000
                    },

                    new Region
                    {
                        Left = 0f,
                        Top = 0.5f,
                        Width = 0.5f,
                        Height = 0.5f,
                        Color = 0x0000ff
                    },

                    new Region
                    {
                        Left = 0.5f,
                        Top = 0f,
                        Width = 0.5f,
                        Height = 0.5f,
                        Color = 0x00ff00
                    },

                    new Region
                    {
                        Left = 0.5f,
                        Top = 0.5f,
                        Width = 0.5f,
                        Height = 0.5f,
                        Color = 0xffff00
                    },
                }
            };
            string json = JsonConvert.SerializeObject(FourSquare, Formatting.Indented);

            System.IO.File.WriteAllText("FourSquare.json", json);
        }
    }
}
