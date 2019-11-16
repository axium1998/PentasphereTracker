using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PentasphereTracker.Helpers {
	public static class ConfigLoaderNew {
		public static async Task LoadConfig() {
			var configFolder = new DirectoryInfo(Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
					Environment.SpecialFolderOption.DoNotVerify), "PentasphereTracker"));

			if (!configFolder.Exists)
				configFolder.Create();

			Console.WriteLine($"Config dir {configFolder.FullName} found, loading...");

			var configFiles = new List<FileInfo>(configFolder.EnumerateFiles());
			if (configFiles.Count == 0) {
				Console.WriteLine("No campaign files exist! Starting new campaign...");
				await CreateNewConfig(configFolder);
			} else {
				var x = 1;
				foreach (var config in configFiles) {
					if (config.Extension == ".dat")
						Console.WriteLine($"[{x}]: {config.Name}");
					//TODO log non-compliant config file
					x++;
				}

				Console.WriteLine("[C]: Create new campaign");
				Console.WriteLine("[Q]: Quit");
				Console.WriteLine("[D]: Delete campaign");
				while (true) {
					Console.WriteLine("Please select an option.");
					try {
						var choice = Console.ReadLine()?.Trim().ToUpper();

						if (choice == "") {
							throw new ArgumentException($"\"NULL\" is not a valid choice");
						}

						if (choice == "C") {
							await CreateNewConfig(configFolder);
							break;
						}
						if (choice == "Q")
							break;
						if (choice == "D")
							throw new NotImplementedException(
								$"Cannot delete yet, please manually delete from {configFolder.FullName}");

						if (!int.TryParse(choice, out var choiceInt)) {
							throw new ArgumentException($"{choice} is not a valid choice");
						}

						if (choiceInt > configFiles.Count + 1)
							throw new ArgumentOutOfRangeException();
						if (choiceInt == configFiles.Count + 1)
							await CreateNewConfig(configFolder);

						FinishLoad(configFiles[choiceInt-1]);
						break;
					} catch (Exception e) {
						Console.WriteLine($"{e.Message}");
					}
				}
			}
		}

		private static async Task CreateNewConfig(FileSystemInfo configFolder) {
			try {
				while (true) {
					Console.WriteLine("PLease name the campaign.");
					Console.WriteLine("NOTE: Spaces will be replaced with underscores (_) for filesystem compliance");

					var campaignName = Console.ReadLine()?.Trim();
					if (campaignName == "") throw new ArgumentException("Name cannot be empty");

					var newConfig = new FileInfo(Path.Combine(configFolder.FullName, campaignName + ".dat"));

					File.Create(newConfig.FullName);
					PentasphereTracker.ConfigName = newConfig;
					break;
				}
			} catch (ArgumentException e) { Console.WriteLine(e.Message); }
		}

		public static async Task SaveConfig(FileSystemInfo config) {
			File.Delete(config.FullName);
			string[] output = {
				$"{PentasphereTracker.Radius}",
				$"{(int)CoordGrid.XAxis}:{(int)CoordGrid.YAxis}",
				$"{(int)CoordGrid.XShiftedAxis}:{(int)CoordGrid.YShiftedAxis}" +
				$"{CoordGrid.XShift}:{CoordGrid.YShift}",
				$"{PentasphereTracker.NorthSouth.Degree}:{PentasphereTracker.EastWest.Degree}:" +
				$"{PentasphereTracker.AnaKata.Degree}:{PentasphereTracker.StrangeCharm.Degree}"};

			File.Create(config.FullName);
			using (var writer = new StreamWriter(new FileStream(config.FullName, FileMode.CreateNew))) {
				foreach (var s in output) {
					await writer.WriteLineAsync(s);
				}
			}
		}
		
		
		private static void FinishLoad(FileSystemInfo config) {
			PentasphereTracker.ConfigName = config;
			
			var conf = File.ReadAllLines(config.FullName);
			int.TryParse(conf[0], out var radius);
			var axesOn = conf[1].Trim().Split(":");
			var axesShift = conf[2].Trim().Split(":");
			var degreesShift = conf[3].Trim().Split(":");
			var position = conf[4].Trim().Split(":");

			Enum.TryParse(typeof(Axis), axesOn[0], out var xAxis);
			Enum.TryParse(typeof(Axis), axesOn[1], out var yAxis);
			Enum.TryParse(typeof(Axis), axesShift[0], out var xAxisShift);
			Enum.TryParse(typeof(Axis), axesShift[1], out var yAxisShift);
			double.TryParse(degreesShift[0], out var xShift);
			double.TryParse(degreesShift[1], out var yShift);
			double.TryParse(position[0], out var deg1);
			double.TryParse(position[1], out var deg2);
			double.TryParse(position[2], out var deg3);
			double.TryParse(position[3], out var deg4);

			PentasphereTracker.Radius = radius;
			CoordGrid.XAxis = (Axis) xAxis;
			CoordGrid.YAxis = (Axis) yAxis;
			CoordGrid.XShiftedAxis = (Axis) xAxisShift;
			CoordGrid.YShiftedAxis = (Axis) yAxisShift;
			CoordGrid.XShift = xShift;
			CoordGrid.YShift = yShift;
			PentasphereTracker.NorthSouth.SetDegree(deg1);
			PentasphereTracker.EastWest.SetDegree(deg2);
			PentasphereTracker.AnaKata.SetDegree(deg3);
			PentasphereTracker.StrangeCharm.SetDegree(deg4);
		}
	}
}