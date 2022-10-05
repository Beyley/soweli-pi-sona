namespace sowelipisona;

public class Waveform {
	public int Channels;
	internal Waveform() {}

	public Point[]? Points {
		get;
		internal set;
	}

	public delegate int GetSamplePositionFromTimePosition(double time);

	public GetSamplePositionFromTimePosition GetPointFromTime {
		get;
		internal set;
	}
	
	public struct Point {
		/// <summary>
		///     The amplitude of the left channel.
		/// </summary>
		public float AmplitudeLeft;

		/// <summary>
		///     The amplitude of the right channel.
		/// </summary>
		public float AmplitudeRight;

		/// <summary>
		///     Unnormalised total intensity of the low-range (bass) frequencies.
		/// </summary>
		public float LowIntensity;

		/// <summary>
		///     Unnormalised total intensity of the mid-range frequencies.
		/// </summary>
		public float MidIntensity;

		/// <summary>
		///     Unnormalised total intensity of the high-range (treble) frequencies.
		/// </summary>
		public float HighIntensity;
	}
}
