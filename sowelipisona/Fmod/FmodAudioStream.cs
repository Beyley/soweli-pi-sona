using System.Runtime.InteropServices;
using ChaiFoxes.FMODAudio;
using FMOD;
using ManagedBass;
using Channel = FMOD.Channel;
using ChannelGroup = FMOD.ChannelGroup;
using Sound = FMOD.Sound;

namespace sowelipisona.Fmod;

internal class FmodAudioStream : AudioStream {
	private readonly float _initialFrequency;

	private  DSP     _pitchShift;
	internal Channel Channel;

	public   int   DspIndex;
	internal Sound Sound;

	public FmodAudioStream(byte[] data) {
		CREATESOUNDEXINFO info = new CREATESOUNDEXINFO();
		info.length = (uint)data.Length;
		info.cbsize = Marshal.SizeOf(info);
		RESULT result = CoreSystem.Native.createSound(data, MODE.OPENMEMORY | MODE.DEFAULT, ref info, out this.Sound);
		if (result != RESULT.OK)
			throw new Exception($"Failed to create sound! err:{result}");

		result = CoreSystem.Native.createDSPByType(DSP_TYPE.PITCHSHIFT, out this._pitchShift);
		if (result != RESULT.OK)
			throw new Exception($"Failed to create pitch DSP! err:{result}");

		result = CoreSystem.Native.playSound(this.Sound, default(ChannelGroup), true, out this.Channel);
		if (result != RESULT.OK)
			throw new Exception($"Failed to create `sound`! err:{result}");

		result = this.Channel.addDSP(0, this._pitchShift);
		if (result != RESULT.OK)
			throw new Exception($"Failed to add dsp! err:{result}");

		result = this.Channel.getFrequency(out this._initialFrequency);
		if (result != RESULT.OK)
			throw new Exception($"Failed to get frequency! err:{result}");
	}
	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get {
			RESULT result = this.Channel.getPosition(out uint position, TIMEUNIT.MS);
			if (result != RESULT.OK)
				throw new Exception($"Failed to get current position! err:{result}");

			return position;
		}
		set {
			RESULT result = this.Channel.setPosition((uint)value, TIMEUNIT.MS);
			if (result != RESULT.OK)
				throw new Exception($"Failed to set current position! err:{result}");
		}
	}

	public override double Length {
		get {
			RESULT result = this.Sound.getLength(out uint length, TIMEUNIT.MS);
			if (result != RESULT.OK)
				throw new Exception($"Failed to get length! err:{result}");

			return length;
		}
	}
	public override double Volume {
		get {
			this.Channel.getVolume(out float volume);
			return volume;
		}
		set => this.Channel.setVolume((float)value);
	}

	//TODO: fix this, the flags seem to not be getting set (100% this should be working, ill have to read up more on the Fmod docs)
	public override bool Loop {
		get {
			this.Channel.getMode(out MODE mode);
			return (mode & MODE.LOOP_NORMAL) != 0;
		}
		set {
			this.Channel.getMode(out MODE mode);
			if (value) {
				mode &= ~MODE.LOOP_OFF;
				mode |= MODE.LOOP_NORMAL;
			}
			else {
				mode |= MODE.LOOP_OFF;
				mode &= ~MODE.LOOP_NORMAL;
			}
			this.Channel.setMode(mode);
		}
	}

	public override PlaybackState PlaybackState {
		get {
			this.Channel.getPaused(out bool paused);
			return paused ? PlaybackState.Paused : PlaybackState.Playing;
		}
	}

	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}

	public override bool Play() {
		this.Channel.setPosition(0, TIMEUNIT.MS);
		this.Channel.setPaused(false);

		return true;
	}
	public override bool Resume() {
		this.Channel.setPaused(false);

		return true;
	}
	public override bool Pause() {
		this.Channel.setPaused(true);

		return true;
	}
	public override bool Stop() {
		this.Channel.setPosition(0, TIMEUNIT.MS);
		this.Channel.setPaused(true);

		return true;
	}
	public override bool SetSpeed(double speed, bool pitch = false) {
		this.Channel.setFrequency((float)(this._initialFrequency * speed));

		if (!pitch)
			this._pitchShift.setParameterFloat(0, (float)(1f / speed));

		return true;
	}
	public override double GetSpeed() {
		this.Channel.getFrequency(out float frequency);

		return frequency / this._initialFrequency;
	}

	internal override bool Dispose() {
		this.Sound.release();

		return true;
	}
}
