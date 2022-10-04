using ManagedBass;

namespace sowelipisona;

public abstract class AudioStream {
	public abstract int Handle { get; internal set; }
	/// <summary>
	///     The current position of the stream in milliseconds
	/// </summary>
	public abstract double CurrentPosition { get; set; }
	/// <summary>
	///     The total length of the stream in milliseconds
	/// </summary>
	public abstract double Length { get; }
	/// <summary>
	///     Gets/Sets the volume of the stream
	/// </summary>
	public abstract double Volume { get; set; }

	/// <summary>
	///     Whether to loop after the stream ends
	/// </summary>
	public abstract bool Loop { get; set; }

	/// <summary>
	///     The current state of the stream
	/// </summary>
	public abstract PlaybackState PlaybackState { get; }

	/// <summary>
	///     Sets the audio device of the stream
	/// </summary>
	/// <param name="device"></param>
	/// <returns></returns>
	public abstract bool SetAudioDevice(AudioDevice device);

	/// <summary>
	///     Plays the stream from the beginning
	/// </summary>
	/// <returns>Success</returns>
	public abstract bool Play();
	/// <summary>
	///     Resumes from a paused state
	/// </summary>
	/// <returns>Success</returns>
	public abstract bool Resume();
	/// <summary>
	///     Pauses the stream
	/// </summary>
	/// <returns>Success</returns>
	public abstract bool Pause();
	/// <summary>
	///     Stops the stream (basically .Pause() but resets the position to 0 in most cases)
	/// </summary>
	/// <returns>Success</returns>
	public abstract bool Stop();
	/// <summary>
	///     Sets the playback speed
	/// </summary>
	/// <param name="speed">How fast to play at? (1d = 1x speed, 2d = 2x speed)</param>
	/// <param name="pitch">Whether the pitch should change with the stream speed</param>
	/// <returns>Success</returns>
	public abstract bool SetSpeed(double speed, bool pitch = false);
	/// <summary>
	///     Gets the speed of the stream
	/// </summary>
	/// <returns>The current speed</returns>
	public abstract double GetSpeed();

	/// <summary>
	///     Disposes the stream
	/// </summary>
	/// <returns>Success</returns>
	internal abstract bool Dispose();
}
