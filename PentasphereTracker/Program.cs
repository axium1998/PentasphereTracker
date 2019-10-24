using System;
using System.Text.RegularExpressions;
using PentasphereTracker.Helpers;

namespace PentasphereTracker {
    internal static class Program {
        private static PolarCoord _northSouth = new PolarCoord(Axis.NorthSouth);
        private static PolarCoord _eastWest = new PolarCoord(Axis.EastWest);
        private static PolarCoord _anaKata = new PolarCoord(Axis.AnaKata);
        private static PolarCoord _strangeCharm = new PolarCoord(Axis.StrangeCharm);
        
        static void Main(string[] args) {
            Console.WriteLine("{0,9:0.0000}\tN/S", _northSouth.Degree);
            Console.WriteLine("{0,9:0.0000}\tE/W", _eastWest.Degree);
            Console.WriteLine("{0,9:0.0000}\tA/K", _anaKata.Degree);
            Console.WriteLine("{0,9:0.0000}\tS/C", _strangeCharm.Degree);

            while (true) {
                var input = Console.ReadLine();

                Regex regex = new Regex(@"\d*");
                if (input != null) {
                    Match match = regex.Match(input);

                Object[] inputArray = {
                        double.TryParse(match.Value),
                        input.Trim().Replace(match.Value, "").ToCharArray()
                    };

                    foreach (var o in inputArray) {
                        if(o.GetType() == typeof(char[])) {
                            foreach (var direction in (char[]) o) {
                                Console.Write($"{direction} ");
                            }
                            Console.WriteLine();
                        } else {
                            Console.WriteLine($"{o}");
                        }
                    }
                }

                Console.ReadKey();
                break;
            }
        }
    }
}