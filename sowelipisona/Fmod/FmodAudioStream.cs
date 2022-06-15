using FmodAudio;
using FmodAudio.DigitalSignalProcessing;
using ManagedBass;

namespace sowelipisona.Fmod;

public class FmodAudioStream : AudioStream {
	internal readonly Sound   Sound;
	internal readonly Channel Channel;
	internal readonly FmodSystem System;

	private float _initialFrequency;

	private Dsp _pitchShift;

	public int DspIndex;

	public FmodAudioStream(FmodSystem system, byte[] data) {
		this.System = system;

		this.Sound = this.System.CreateSound((ReadOnlySpan<byte>)data, Mode.Default | Mode.OpenMemory, new CreateSoundInfo {
			Length = (uint)data.Length
		});

		this._pitchShift = this.System.CreateDSPByType(DSPType.PitchShift);
		
		this.Channel = this.System.PlaySound(this.Sound, default, true);
		this.Channel.AddDSP(0, this._pitchShift);
		this.DspIndex++;

		this._initialFrequency = this.Channel.Frequency;
	}
	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get => this.Channel.GetPosition(TimeUnit.MS);
		set => this.Channel.SetPosition(TimeUnit.MS, (uint)value);
	}

	public override double Length => this.Sound.GetLength(TimeUnit.MS);
	
	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}

	public override bool Play() {
		this.Channel.SetPosition(TimeUnit.MS, 0);
		this.Channel.Paused = false;

		return true;
	}
	public override bool Resume() {
		this.Channel.Paused = false;

		return true;
	}
	public override bool Pause() {
		this.Channel.Paused = true;

		return true;
	}
	public override bool Stop() {
		this.Channel.SetPosition(TimeUnit.MS, 0);
		this.Channel.Paused = true;

		return true;
	}
	public override bool SetSpeed(double speed, bool pitch = false) {
		this.Channel.Frequency = (float)(this._initialFrequency * speed);

		if (!pitch)
			this._pitchShift.SetParameterFloat(0, (float)(1f / speed));

		return true;
	}
	public override double GetSpeed() => this.Channel.Frequency / this._initialFrequency;
	public override double Volume {
		get => this.Channel.Volume;
		set => this.Channel.Volume = (float)value;
	}
	
	//TODO: fix this, the flags seem to not be getting set (100% this should be working, ill have to read up more on the Fmod docs)
	public override bool Loop {
		get => (this.Channel.Mode & Mode.Loop_Normal) != 0;
		set {
			if (value) {
				this.Channel.Mode &= ~Mode.Loop_Off;
				this.Channel.Mode |= Mode.Loop_Normal;
			}
			else {
				this.Channel.Mode |= Mode.Loop_Off;
				this.Channel.Mode &= ~Mode.Loop_Normal;
			}
		}
	}

	public override PlaybackState PlaybackState => this.Channel.Paused ? PlaybackState.Paused : PlaybackState.Playing;

	internal override bool Dispose() {
		this.Sound.Release();

		return true;
	}
}
