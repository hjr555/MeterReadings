using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MeterReadings.Models;
using Newtonsoft.Json;

namespace MeterReadings
{
    class Program
    {
        static void Main(string[] args)
        {
            var datapath = @"Data\readings.json";

            var data = GetReadings(datapath);

            DisplayData(data);
        }

        private static void DisplayData(IEnumerable<Reading> data)
        {
            var sorted = data.OrderByDescending(x => x.Date);
            decimal standingCharge = 21.798M;
            decimal dayRate = 14.776M;
            decimal nightRate = 7.706M;

            Console.WriteLine($"Total readings: { data.Count() }");
            Console.WriteLine();

            foreach (var reading in sorted)
            {
                Reading lastReading = sorted.Where(x => x.Date < reading.Date)?.FirstOrDefault();

                Console.WriteLine();


                if (lastReading != null)
                {
                    TimeSpan difference = reading.Date - lastReading.Date;
                    int dayDiff = reading.Day - lastReading.Day;
                    int nightDiff = reading.Night - lastReading.Night;

                    decimal estimate = 
                            ((dayDiff * dayRate) + 
                            (nightDiff * nightRate) + 
                            (standingCharge * difference.Days)) / 100;

                    Console.WriteLine($"Date: { reading.Date.ToShortDateString() }\t Day: { reading.Day } (Avg { dayDiff / difference.Days })\t Night: { reading.Night }(Avg { nightDiff / difference.Days })");
                    Console.WriteLine($"Est. Charge: £{ string.Format("{0:0.00}", estimate) }");
                    Console.WriteLine($"Est. Daily : £{ string.Format("{0:0.00}", estimate / difference.Days) }");
                }
                else
                {
                    Console.WriteLine($"Date: { reading.Date.ToShortDateString() }\t Day: { reading.Day }\t Night: { reading.Night }");
                }
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
