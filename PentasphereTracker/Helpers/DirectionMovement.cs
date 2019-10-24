using System;
using System.Text.RegularExpressions;

namespace PentasphereTracker.Helpers {
	/// <summary>Class <c>DirectionMovement</c> 
	/// 
	/// </summary>
	public class DirectionMovement {
		public double Degree { get; }
		public char Angle1 { get; }
		public char Angle2 { get; }

		// Regex to look for numbers within strings
		private static readonly Regex Regex = new Regex(@"\d*");

		public DirectionMovement(string str) {
			if (!double.TryParse(Regex.Match(str).Value, out var tmp))
				throw new ArgumentException();

			var newstr = str.Replace(tmp.ToString(), "").ToCharArray();
			Degree = tmp;
			Angle1 = newstr[0];
			Angle2 = newstr[1];
		}
	}
}