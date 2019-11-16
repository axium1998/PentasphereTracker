using System;

namespace PentasphereTracker.Helpers {
	/// <summary>Class <c>PolarCoord</c>
	/// maps the 3D object's position by axis
	/// </summary>
	public struct PolarCoord {
		// Hold the axis of which this PolarCoord represents
		public Axis Axis { get; private set; }
		// Hold the actual polar coordinate for use
		public double Degree { get; private set; }
		// And hold the previous coordinate
		public double OldDegree { get; private set; }

		/// <summary>Constructor <c>PolarCoord</c>
		/// creates a holder for the polar coordinate for use elsewhere
		/// </summary>
		/// <param name="axis">The axis of which this polar coordinate exists</param>
		public PolarCoord(Axis axis) {
			Axis = axis;
			OldDegree = 0;

			var random = new Random();
			Degree = random.NextDouble();
			Degree *= random.Next(-180, 180);
		}

		public PolarCoord AddDegree(double d) {
			OldDegree = Degree;
			Degree += d;
			return this;
		}

		public PolarCoord SetDegree(double d) {
			Degree = d;
			return this;
		}

		public double GetDiff() {
			return Degree - OldDegree;
		}
	}
}