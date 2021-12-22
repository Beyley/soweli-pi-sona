using ManagedBass;

namespace sowelipisona;

public abstract class AudioStream {
	public abstract int Handle { get; internal set; }
	/// <summary>
	///     The current position of the stream in miliseconds
	/// </summary>
	public abstract double CurrentPosition { get; set; }
	public abstract double Length { get; }

	public abstract bool SetAudioDevice(AudioDevice device);

	public abstract bool   Start();
	public abstract bool   Resume();
	public abstract bool   Pause();
	public abstract bool   Stop();
	public abstract bool   SetSpeed(double speed, bool pitch = false);
	public abstract double GetSpeed();
	public abstract double Volume { get; set; }

	public abstract PlaybackState PlaybackState { get; }
	
	internal abstract bool Dispose();
}
