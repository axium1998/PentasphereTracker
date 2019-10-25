using System;

namespace PentasphereTracker.Helpers {
	public static class Trig {
		private static double RadiansFromDegrees(double degree) {
			return degree * Math.PI / 180.0;
		}

		private static double DegreesFromRadians(double radian) {
			return radian * 180 / Math.PI;
		}

		public static double SinOfDegree(double degree) {
			return Math.Sin(RadiansFromDegrees(degree));
		}

		public static double CosOfDegree(double degree) {
			return Math.Cos(RadiansFromDegrees(degree));
		}
	}
}