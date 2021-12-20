using FmodAudio;
using FmodAudio.DigitalSignalProcessing;
using ManagedBass;

namespace sowelipisona.Fmod;

public class FmodAudioStream : AudioStream {
	private readonly Sound   _sound;
	private          Channel _channel;

	private float _initialFrequency;

	private Dsp _pitchShift;

	private FmodSystem _system;

	public FmodAudioStream(FmodSystem system, byte[] data) {
		this._system = system;

		this._sound = this._system.CreateSound((ReadOnlySpan<byte>)data, Mode.Default | Mode.OpenMemory, new CreateSoundInfo {
			Length = (uint)data.Length
		});

		this._pitchShift = this._system.CreateDSPByType(DSPType.PitchShift);
	}
	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get => this._channel.GetPosition(TimeUnit.MS);
		set => this._channel.SetPosition(TimeUnit.MS, (uint)value);
	}

	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}

	public override bool Start() {
		this._channel = this._system.PlaySound(this._sound);
		this._channel.AddDSP(0, this._pitchShift);

		this._initialFrequency = this._channel.Frequency;

		return true;
	}
	public override bool Resume() {
		this._channel.Paused = false;

		return true;
	}
	public override bool Pause() {
		this._channel.Paused = true;

		return true;
	}
	public override bool Stop() {
		this._channel.Stop();

		return true;
	}
	public override bool SetSpeed(double speed, bool pitch = false) {
		this._channel.Frequency = (float)(this._initialFrequency * speed);

		if (!pitch)
			this._pitchShift.SetParameterFloat(0, (float)(1f / speed));

		return true;
	}
	public override double GetSpeed() => this._channel.Frequency / this._initialFrequency;
	public override bool SetVolume(double volume) {
		this._channel.Volume = (float)volume;

		return true;
	}
	public override double        GetVolume()   => this._channel.Volume;
	
	public override PlaybackState PlaybackState => this._channel.IsPlaying ? PlaybackState.Playing : PlaybackState.Paused;

	internal override bool Dispose() {
		this._sound.Release();

		return true;
	}
}