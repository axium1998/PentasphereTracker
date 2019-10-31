using System;
using System.Collections.Generic;
using System.IO;

namespace PentasphereTracker.Helpers {
	public static class ConfigLoader {
		public static void LoadConfig() {
			var configFolder = new DirectoryInfo(Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData,
					Environment.SpecialFolderOption.DoNotVerify), "PentasphereTracker"));
			
			if(!configFolder.Exists)
				configFolder.Create();
			
			Console.WriteLine($"Config dir {configFolder.FullName} found, loading...");

			var configFiles = new List<FileInfo>(configFolder.EnumerateFiles());

			if (configFiles.Count == 0) {
				Console.WriteLine("No campaign files exist! Continuing to create a new one...");
				CreateConfig(configFolder);
			} else {
				var x = 1;
				foreach (var config in configFiles) {
					Console.WriteLine($"[{x}]: {config.Name.Replace(".json",  "").Trim()}");
					x++;
				}
				Console.WriteLine($"[{x}]: Create new campaign");
				Console.WriteLine("[Q]: Quit");
				Console.WriteLine("[D]: Delete campaign");
				while (true) {
					Console.WriteLine("Please select an option.");
					try {
						var choice = Console.ReadLine()?.Trim();
						if(choice == "")
							throw new ArgumentException($"\"{choice}\" is not a valid choice");

						if (choice == "Q")
							break;
						if (choice == "D")
							throw new NotImplementedException($"Cannot delete yet, please manually delete from {configFolder.FullName}");
						/*			
						if(choice > configFiles.Count+1)
							throw new ArgumentOutOfRangeException();
						if(choice == configFiles.Count+1)
							CreateConfig(configFolder);
						else if (choice == -1)
							break;
						break;
						*/
					} catch(Exception e) { Console.WriteLine($"{e.Message}"); }
				}
			}
		}

		private static void CreateConfig(FileSystemInfo configFolder) {
			while (true) {
				try {
					Console.WriteLine("Please give this campaign a name (whitespace will be replaced with '_')");
					var newCampaign = Console.ReadLine()?.Replace(" ", "_");
					if (newCampaign == "") throw new ArgumentException("New campaign name cannot be empty");
					var fileInfoNew = new FileInfo(Path.Combine(configFolder.FullName, newCampaign + ".json"));
					fileInfoNew.Create();
					break;
				} catch(ArgumentException e) { Console.WriteLine($"{e.Message}"); }
			}
		}
	}
}