using FmodAudio;
using FmodAudio.DigitalSignalProcessing;
using ManagedBass;

namespace sowelipisona.Fmod;

public class FmodAudioStream : AudioStream {
	private readonly Sound    _sound;
	private          Channel? _channel;

	private float _initialFrequency;

	private Dsp _pitchShift;

	private FmodSystem _system;

	public FmodAudioStream(FmodSystem system, byte[] data) {
		this._system = system;

		this._sound = this._system.CreateSound((ReadOnlySpan<byte>)data, Mode.Default | Mode.OpenMemory, new CreateSoundInfo {
			Length = (uint)data.Length
		});

		this._channel = null;

		this._pitchShift = this._system.CreateDSPByType(DSPType.PitchShift);
	}
	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get {
			if (this._channel != null)
				return this._channel.GetPosition(TimeUnit.MS);

			return 0d;
		}
		set {
			if (this._channel != null)
				this._channel.SetPosition(TimeUnit.MS, (uint)value);
		}
	}

	public override double Length => this._sound.GetLength(TimeUnit.MS);
	
	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}

	public override bool Play() {
		this._channel?.Stop();
		
		this._channel = this._system.PlaySound(this._sound);

		if (this._channel == null) return false;
		
		this._channel.AddDSP(0, this._pitchShift);

		this._initialFrequency = this._channel.Frequency;

		return true;
	}
	public override bool Resume() {
		if (this._channel == null) return false;
		
		this._channel.Paused = false;

		return true;
	}
	public override bool Pause() {
		if (this._channel == null) return false;
		
		this._channel.Paused = true;

		return true;
	}
	public override bool Stop() {
		if (this._channel == null) return false;
		
		this._channel.Stop();

		return true;
	}
	public override bool SetSpeed(double speed, bool pitch = false) {
		if (this._channel == null) return false;
		
		this._channel.Frequency = (float)(this._initialFrequency * speed);

		if (!pitch)
			this._pitchShift.SetParameterFloat(0, (float)(1f / speed));

		return true;
	}
	public override double GetSpeed() => this._channel.Frequency / this._initialFrequency;
	public override double Volume {
		get => this._channel != null ? this._channel.Volume : 1f;
		set {
			if (this._channel == null) return;
			
			this._channel.Volume = (float)value;
		}
	}

	//TODO: fix this, the flags seem to not be getting set (100% this should be working, ill have to read up more on the Fmod docs)
	public override bool Loop {
		get => this._channel == null ? throw new Exception("The stream is not initialized!") : (this._channel.Mode & Mode.Loop_Normal) != 0;
		set {
			if (this._channel == null) return;
			
			if (value) {
				this._channel.Mode &= ~Mode.Loop_Off;
				this._channel.Mode |= Mode.Loop_Normal;
			}
			else {
				this._channel.Mode |= Mode.Loop_Off;
				this._channel.Mode &= ~Mode.Loop_Normal;
			}
		}
	}

	public override PlaybackState PlaybackState {
		get {
			if (this._channel == null) throw new Exception("Stream not initialized");

			return this._channel.Paused ? PlaybackState.Paused :  PlaybackState.Playing;
		}
	}

	internal override bool Dispose() {
		this._sound.Release();

		return true;
	}
}
