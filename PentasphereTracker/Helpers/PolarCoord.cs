using System;

namespace PentasphereTracker.Helpers {
	public struct PolarCoord {
		public Axis GetAxis { get; }
		public double Degree { get; }
		
		public PolarCoord(Axis axis) {
			GetAxis = axis;

			var random = new Random();
			Degree = random.NextDouble();
			Degree *= random.Next(-180, 180);
		}
	}
}