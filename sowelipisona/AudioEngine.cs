namespace sowelipisona;

public abstract class AudioEngine {
	public readonly List<AudioStream> Streams = new();

	/// <summary>
	/// The current audio device, this should be set to the default by the overriding initialize
	/// </summary>
	public AudioDevice AudioDeviceInUse { get; protected set; }
	public bool        Initialized      { get; protected set; }

	/// <summary>
	/// Initializes the audio engine to make it ready for audio playback
	/// </summary>
	/// <param name="windowId">The ID of the window, not providing one will produce different behavior depending on the backend</param>
	/// <returns>Whether initializing the audio engine was successful</returns>
	public abstract bool Initialize(IntPtr windowId = default);

	public abstract bool SetAudioDevice(AudioDevice device);

	public void DisposeStream(AudioStream stream) {
		if (stream.Dispose())
			this.Streams.Remove(stream);
		else
			throw new Exception("Unable to dispose stream! This should *never* happen!");
	}

	/// <summary>
	///     Open a sound file from the system and play its contents
	/// </summary>
	/// <param name="filename">The filename to load from</param>
	/// <returns>The created AudioStream</returns>
	public AudioStream CreateStream(string filename) {
		//Create the stream using the internal constructor
		AudioStream stream = this.EngineCreateStream(File.ReadAllBytes(filename));

		this.Streams.Add(stream);

		return stream;
	}

	/// <summary>
	///     Play a sound file stored in a byte array
	/// </summary>
	/// <param name="data">The audio file to create the stream with</param>
	/// <returns>The created AudioStream</returns>
	public AudioStream CreateStream(byte[] data) {
		AudioStream stream = this.EngineCreateStream(data);

		this.Streams.Add(stream);

		return stream;
	}

	/// <summary>
	/// Gets a list of all audio devices available
	/// </summary>
	/// <returns>The registered devices</returns>
	public abstract AudioDevice[] GetAudioDevices();

	/// <summary>
	///     Creates an audio stream from a byte array
	/// </summary>
	/// <param name="data">The data for the audio file</param>
	/// <returns></returns>
	protected abstract AudioStream EngineCreateStream(byte[] data);
}
