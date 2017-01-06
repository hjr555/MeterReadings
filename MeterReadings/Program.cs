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

            var engineers = new Dictionary<string, int>();
            IEnumerable<Reading> data;

            using (var r = new StreamReader(datapath))
            {
                var json = r.ReadToEnd();

                data = JsonConvert.DeserializeObject<List<Reading>>(json)
                    .OrderByDescending(x => x.Date);
            }

            Console.WriteLine($"Total readings: { data.Count() }");
            Console.WriteLine();

            foreach(var reading in data)
            {
                Console.WriteLine($"Date: { reading.Date.ToShortDateString() }\t Day: { reading.Day }\t Night: { reading.Night }");
            }
        }
    }
}
