using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
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
            var traveling = true;
            while (traveling) {
                Console.Clear();
                try {
                    // Every time we are able to move, we need our current coords
                    Console.WriteLine("{0,9:0.0000}\tN/S", _northSouth.Degree);
                    Console.WriteLine("{0,9:0.0000}\tE/W", _eastWest.Degree);
                    Console.WriteLine("{0,9:0.0000}\tA/K", _anaKata.Degree);
                    Console.WriteLine("{0,9:0.0000}\tS/C", _strangeCharm.Degree);
                    /* Input is in this format:
                     * <Distance> <Angle in coordinate form> <Angle to axis plane><Axis traveled upon>
                     * <Angle for second axis of travel><Axis traveled upon>
                     *
                     * 2 vectors are ALWAYS required (even if one is 0 degrees on a coordinate plane
                     */

                    //REF: 10 45 21NA 60EC
                    //REF: 56 meters, 30 degrees North of East, 64 degrees Charm of Ana 
                    // If our input was null, try again
                    var input = Console.ReadLine()
                                       ?.Trim();
                    switch (input) {
                        case "":
                            break;
                        case "break":
                            Environment.ExitCode = 0;
                            traveling = false;
                            break;
                    }

                    var inputArray = new List<object> { };
                    // Since each operator in the input is separated by a space, split on that to isolate variables
                    var inputs = input.Trim().Split(" ");

                    // If we can't parse the distance, the input was incorrect
                    if (!double.TryParse(inputs[0], out var distance))
                        throw new ArgumentException($"{inputs[0]} is not a valid distance");

                    if (!double.TryParse(inputs[1], out var angle))
                        throw new ArgumentException($"{inputs[1]} is not a valid angle");
                    
                    

                    // Since it checks out, add it to my list
                    inputArray.Add(distance);
                    inputArray.Add(angle);

                    // Now we have it, remove it from the input
                    input = input.Replace(inputs[0], "").Trim();
                    input = input.Replace(inputs[1], "").Trim();

                    // Now all we're left with is 2 directional vectors, let's do a thing with them
                    inputArray.AddRange(input.Trim().Split(" ").Select(s => new DirectionMovement(s)));


                    // Alright, time to do some trig!!!
                    var directionMovements = new List<DirectionMovement> {
                        (DirectionMovement) inputArray[2],
                        (DirectionMovement) inputArray[3]
                    };

                    var initTrig = distance * Trig.SinOfDegree(angle);

                    foreach (var directionMovement in directionMovements) {
                        switch (directionMovement.Angle) {
                            case "NE":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "NW":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "NA":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                Console.WriteLine($"Delta of N/S: {_northSouth.OldDegree} to {_northSouth.Degree} [{_northSouth.GetDiff()}]");
                                Console.WriteLine($"Delta of A/K: {_anaKata.OldDegree} to {_anaKata.Degree} [{_anaKata.GetDiff()}]");
                                break;
                            case "NK":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "NT":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "NC":
                                _northSouth.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "SE":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "SW":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "SA":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "SK":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "ST":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "SC":
                                _northSouth.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "EN":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "ES":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "EA":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "EK":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "ET":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "EC":
                                _eastWest.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                Console.WriteLine($"Delta of E/W: {_eastWest.OldDegree} to {_eastWest.Degree} [{_eastWest.GetDiff()}]");
                                Console.WriteLine($"Delta of T/C: {_strangeCharm.OldDegree} to {_strangeCharm.Degree} [{_strangeCharm.GetDiff()}]");
                                break;

                            case "WN":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "WS":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "WA":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "WK":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "WT":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "WC":
                                _eastWest.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "AN":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "AS":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "AE":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "AW":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "AT":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "AC":
                                _anaKata.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "KN":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "KS":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "KE":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "KW":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "KT":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "KC":
                                _anaKata.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _strangeCharm.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "TN":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "TS":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "TE":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "TW":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "TA":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "TK":
                                _strangeCharm.AddDegree(initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;

                            case "CN":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "CS":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _northSouth.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "CE":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "CW":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _eastWest.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "CA":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                            case "CK":
                                _strangeCharm.AddDegree(-initTrig * Trig.CosOfDegree(directionMovement.Degree));
                                _anaKata.AddDegree(-initTrig * Trig.SinOfDegree(directionMovement.Degree));
                                break;
                        }
                        
                        
                    }

                    // Because I myself placed the variables in each slot of the List, I can do this without checking
                    Console.WriteLine($"Distance: {(double) inputArray[0]}");
                    Console.WriteLine($"Angle of movement: {(double) inputArray[1]}");
                    var angle1 = (DirectionMovement) inputArray[2];
                    var angle2 = (DirectionMovement) inputArray[3];
                    Console.WriteLine(
                        $"First direction of movement: {angle1.Degree} degrees {angle1.Angle}");
                    Console.WriteLine(
                        $"Second direction of movement: {angle2.Degree} degrees {angle2.Angle}");

                    Console.ReadKey();

                } catch (ArgumentException e) {
                    Console.WriteLine($"{e.Message}");
                }
            }
            
            Environment.Exit(Environment.ExitCode);
        }
    }
}