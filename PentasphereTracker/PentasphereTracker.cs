using System;
using System.IO;
using System.Threading.Tasks;
using PentasphereTracker.Helpers;

namespace PentasphereTracker {
	internal static class PentasphereTracker {
		// We need variables to hold things we need
		public static PolarCoord NorthSouth = new PolarCoord(Axis.NorthSouth);
		public static PolarCoord EastWest = new PolarCoord(Axis.EastWest);
		public static PolarCoord AnaKata = new PolarCoord(Axis.AnaKata);
		public static PolarCoord StrangeCharm = new PolarCoord(Axis.StrangeCharm);
		public static double Radius;
		public static FileSystemInfo ConfigName;
		
		private static async Task Main(string[] args) {
			await ConfigLoaderNew.LoadConfig();
			Console.WriteLine("Config loaded! Now entering world...\n\n");
			await Travel();
		} 
		private static async Task Travel() {
			var traveling = true;
			while (traveling) {
				Console.WriteLine(NorthSouth.Degree + ":" + EastWest.Degree + ":" + AnaKata.Degree + ":" + StrangeCharm.Degree);
				Console.WriteLine("[M]: Move");
				Console.WriteLine("[R[: Shift");
				Console.WriteLine("[Y]: Randomize Coords");
				Console.WriteLine("[W]: Save");
				Console.WriteLine("[WQ]: Save and Quit");
				Console.WriteLine("[Q!]: Quit without saving");

				var input = Console.ReadLine()
				                   ?.Trim();
				switch (input?.ToUpper()) {
					case "M":
						break;
					case "R":
						break;
					case "C":
						
					case "W":
						await ConfigLoaderNew.SaveConfig(ConfigName);
						break;
					case "WQ":
						await ConfigLoaderNew.SaveConfig(ConfigName);
						traveling = false;
						break;
					case "Q!":
						traveling = false;
						break;
					
					default:
						Environment.ExitCode = 1;
						throw new ArgumentException($"{input} is not a valid response.");
				}
			}
		}
	}
}