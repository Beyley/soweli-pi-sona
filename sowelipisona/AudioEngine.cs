namespace sowelipisona;

public abstract class AudioEngine {
	public readonly List<AudioStream>       Streams            = new();
	public readonly List<SoundEffectPlayer> SoundEffectPlayers = new();

	/// <summary>
	///     The current audio device, this should be set to the default by the overriding initialize
	/// </summary>
	public AudioDevice AudioDeviceInUse { get; protected set; }
	/// <summary>
	/// Whether the engine is in an initialized state
	/// </summary>
	public bool Initialized { get;             protected set; }

	/// <summary>
	///     Initializes the audio engine to make it ready for audio playback
	/// </summary>
	/// <param name="windowId">The ID of the window, not providing one will produce different behavior depending on the backend</param>
	/// <returns>Whether initializing the audio engine was successful</returns>
	public abstract bool Initialize(IntPtr windowId = default);

	/// <summary>
	/// The implementation of this is depending on the backend, but it will either set the default for new streams, or the global for all streams
	/// </summary>
	/// <param name="device"></param>
	/// <returns></returns>
	public abstract bool SetAudioDevice(AudioDevice device);

	/// <summary>
	/// Disposes an AudioStream, freeing all resources
	/// </summary>
	/// <param name="stream">The AudioStream to dispose of</param>
	public void DisposeStream(AudioStream stream) {
		if (stream.Dispose())
			this.Streams.Remove(stream);
		else
			throw new Exception("Unable to dispose stream! This should *never* happen!");
	}

	/// <summary>
	/// Disposes a SoundEffectPlayer, freeing all resources
	/// </summary>
	/// <param name="player">The SoundEffectPlayer to dispose</param>
	public void DisposeSoundEffectPlayer(SoundEffectPlayer player) {
		if (player.Dispose())
			this.SoundEffectPlayers.Remove(player);
		else
			throw new Exception("Unable to dispose sound effect player! This should *never* happen!");
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
	///     Gets a list of all audio devices available
	/// </summary>
	/// <returns>The registered devices</returns>
	public abstract AudioDevice[] GetAudioDevices();

	/// <summary>
	///     Creates an audio stream from a byte array
	/// </summary>
	/// <param name="data">The data for the audio file</param>
	/// <returns></returns>
	protected abstract AudioStream EngineCreateStream(byte[] data);

	public SoundEffectPlayer CreateSoundEffectPlayer(string filename) {
		return this.CreateSoundEffectPlayer(File.ReadAllBytes(filename));
	}
	
	public SoundEffectPlayer CreateSoundEffectPlayer(byte[] data) {
		SoundEffectPlayer player = this.EngineCreateSoundEffectPlayer(data);

		this.SoundEffectPlayers.Add(player);

		return player;
	}
	
	protected abstract SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data);
}
