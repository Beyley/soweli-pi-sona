using System.Runtime.InteropServices;
using ManagedBass;
using ManagedBass.Fx;

namespace sowelipisona.ManagedBass;

public class ManagedBassAudioStream : AudioStream {
	private readonly double _initialAudioFrequency;

	private readonly GCHandle _memoryHandle;

	internal ManagedBassAudioStream(IReadOnlyCollection<byte> data) {
		this._memoryHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

		int tempAudioHandle = Bass.CreateStream(this._memoryHandle.AddrOfPinnedObject(), 0, data.Count, BassFlags.Prescan | BassFlags.Decode);

		this.Handle = BassFx.TempoCreate(tempAudioHandle, BassFlags.Prescan);

		this._initialAudioFrequency = Bass.ChannelGetAttribute(this.Handle, ChannelAttribute.Frequency);
	}
	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get => Bass.ChannelBytes2Seconds(this.Handle, Bass.ChannelGetPosition(this.Handle)) * 1000d;
		set => Bass.ChannelSetPosition(this.Handle, Bass.ChannelSeconds2Bytes(this.Handle, value));
	}

	public override bool SetAudioDevice(AudioDevice device) {
		return Bass.ChannelSetDevice(this.Handle, device.Id);
	}

	public override bool Start() {
		return Bass.ChannelPlay(this.Handle, true);
	}
	public override bool Resume() {
		return Bass.ChannelPlay(this.Handle);
	}
	public override bool Pause() {
		return Bass.ChannelPause(this.Handle);
	}
	public override bool Stop() {
		return Bass.ChannelStop(this.Handle);
	}
	private double _lastSpeed = 1d;
	public override bool SetSpeed(double speed, bool pitch = false) {
		if (pitch) {
			bool successFrequency = Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Frequency, this._initialAudioFrequency * speed);
			bool successTempo     = Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Tempo, 0);

			if (!successFrequency || !successTempo)
				return false;
			
			this._lastSpeed = speed;
			return true;
		}
		else {
			bool successFrequency = Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Frequency, this._initialAudioFrequency);
			bool successTempo     = Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Tempo, speed * 100 - 100);

			if (!successFrequency || !successTempo)
				return false;
			
			this._lastSpeed = speed;
			return true;
		}
	}
	public override double GetSpeed() => this._lastSpeed;
	public override bool SetVolume(double volume) {
		return Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Volume, volume);
	}
	public override double GetVolume() => Bass.ChannelGetAttribute(this.Handle, ChannelAttribute.Volume);

	public override PlaybackState PlaybackState => Bass.ChannelIsActive(this.Handle);

	internal override bool Dispose() {
		return Bass.StreamFree(this.Handle);
	}
}
