using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using PentasphereTracker.Helpers;

namespace PentasphereTracker {
    internal static class Program {
        // We need variables to hold our positions, just a little fancy with an axis defined
        private static PolarCoord _northSouth = new PolarCoord(Axis.NorthSouth);
        private static PolarCoord _eastWest = new PolarCoord(Axis.EastWest);
        private static PolarCoord _anaKata = new PolarCoord(Axis.AnaKata);
        private static PolarCoord _strangeCharm = new PolarCoord(Axis.StrangeCharm);
        
        
        static void Main(string[] args) {
            // For as long as we need, loop my basic input handler
            while (true) {
                try {
                    // Everytime we are able to move, we need our current coords
                    Console.WriteLine("{0,9:0.0000}\tN/S", _northSouth.Degree);
                    Console.WriteLine("{0,9:0.0000}\tE/W", _eastWest.Degree);
                    Console.WriteLine("{0,9:0.0000}\tA/K", _anaKata.Degree);
                    Console.WriteLine("{0,9:0.0000}\tS/C", _strangeCharm.Degree);
                    /* Input is in this format:
                     * <Distance> <Angle to axis plane><Axis traveled upon> [<Angle for second axis of travel><Axis traveled upon>]
                     */
                    var input = Console.ReadLine()
                                       ?.Trim();
                    //REF: 56 30NE 64CA
                    //REF: 56 meters, 30 degrees North of East, 64 degrees Charm of Ana 
                    // If our input was null, try again
                    if (input == null) continue;

                    var inputArray = new List<object>{};
                    // Since each operator in the input is separated by a space, split on that to isolate variables
                    var inputs = input.Trim().Split(" ");

                    // If we can't parse the distance, the input was incorrect
                    if (!double.TryParse(inputs[0], out var distance))
                        throw new ArgumentException($"{distance} is not a vaild distance");
                    
                    // Since it checks out, add it to my list
                    inputArray.Add(distance);
                    
                    // Now we have it, remove it from the input
                    input = input.Replace(inputs[0], "").Trim();
                    
                    // Now all we're left with is 1 or 2 directional vectors, let's do a thing with them
                    inputArray.AddRange(input.Trim().Split(" ").Select(s => new DirectionMovement(s)));

#if DEBUG
                    // Because I myself placed the variables in each slot of the List, I can do this without checking
                    Console.WriteLine($"Distance: {(double)inputArray[0]}");
                    if (inputArray.Count == 2) {
                        var angle = (DirectionMovement) inputArray[1];
                        Console.WriteLine($"Direction of movement: {angle.Degree} degrees {angle.Angle1} of {angle.Angle2}");
                    } else {
                        var angle1 = (DirectionMovement) inputArray[1];
                        var angle2 = (DirectionMovement) inputArray[2];
                        Console.WriteLine($"First direction of movement: {angle1.Degree} degrees {angle1.Angle1} of {angle1.Angle2}");
                        Console.WriteLine($"Second direction of movement: {angle2.Degree} degrees {angle2.Angle1} of {angle2.Angle2}");
                    }
#endif

                    Console.ReadKey();
                    break;
                } catch(ArgumentException e) { Console.WriteLine($"{e.Message}");}
            }
        }
    }
}