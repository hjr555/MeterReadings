using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ConsoleApplication8
{
    public class Reading
    {
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public int Night { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var datapath = @"Data\readings.json";

            var data = GetReadings(datapath);

            Console.WriteLine($"Total readings: { data.Count() }");
            Console.WriteLine();

            foreach(var reading in data)
            {
                Console.WriteLine($"Date: { reading.Date.ToShortDateString() }\t Day: { reading.Day }\t Night: { reading.Night }");
            }
        }

        private static IEnumerable<Reading> GetReadings(string path)
        {
            IEnumerable<Reading> data = default(IEnumerable<Reading>);

            var json = LoadJSON(path);
            data = DeserializeJson(json);

            return data;
        }

        private static string LoadJSON(string path)
        {
            string json = string.Empty;

            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    json = reader.ReadToEnd();
                }
            }

            return json;
        }
        private static IEnumerable<Reading> DeserializeJson(string json) => JsonConvert.DeserializeObject<List<Reading>>(json);
    }
}
