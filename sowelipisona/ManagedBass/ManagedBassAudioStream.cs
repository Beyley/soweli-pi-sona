using System.Runtime.InteropServices;
using ManagedBass;
using ManagedBass.Fx;

namespace sowelipisona.ManagedBass;

internal sealed class ManagedBassAudioStream : AudioStream {
	private readonly double _initialAudioFrequency;

	private readonly GCHandle _memoryHandle;
	private          double   _lastSpeed = 1d;
	private readonly Stream?  _stream;

	internal ManagedBassAudioStream(IReadOnlyCollection<byte> data) {
		this._memoryHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

		int tempAudioHandle = Bass.CreateStream(this._memoryHandle.AddrOfPinnedObject(), 0, data.Count, BassFlags.Prescan | BassFlags.Decode);

		this.Handle = BassFx.TempoCreate(tempAudioHandle, BassFlags.Prescan);

		this._initialAudioFrequency = Bass.ChannelGetAttribute(this.Handle, ChannelAttribute.Frequency);
	}

	public ManagedBassAudioStream(Stream stream) {
		this._stream = stream;

		int tempAudioHandle = Bass.CreateStream(StreamSystem.NoBuffer, BassFlags.Prescan | BassFlags.Decode, new FileProcedures {
			Close  = this.FileProcClose,
			Length = this.FileProcLength,
			Read   = this.FileProcRead,
			Seek   = this.FileProcSeek
		});

		this._initialAudioFrequency = Bass.ChannelGetAttribute(this.Handle, ChannelAttribute.Frequency);
	}

	private void FileProcClose(IntPtr user) {
		//this does nothing...
	}

	private long FileProcLength(IntPtr user) {
		if (this._stream is not { CanSeek: true })
			return 0;

		try {
			return this._stream.Length;
		}
		catch {
			return 0;
		}
	}

	private bool FileProcSeek(long offset, IntPtr user) {
		if (this._stream is not { CanSeek: true })
			return false;

		try {
			return this._stream.Seek(offset, SeekOrigin.Begin) == offset;
		}
		catch {
			return false;
		}
	}

	private int FileProcRead(IntPtr buffer, int length, IntPtr user) {
		if (this._stream is not { CanRead: true })
			return 0;

		try {
			unsafe {
				//Create a new staging array to store the read data in
				byte[] arr = new byte[length];
				//Read the data into the array
				int read = this._stream.Read(arr, 0, length);

				//Copy the array into the memory space, the Math.Min call is to ensure that we dont overcopy, only copying the lowest amount of bytes
				fixed (byte* ptr = arr) {
					Buffer.MemoryCopy(ptr, (void*)buffer, Math.Min(length, read), Math.Min(length, read));
				}

				//Return the bytes read
				return read;
			}
		}
		catch {
			return 0;
		}
	}

	public override int Handle {
		get;
		internal set;
	}

	public override double CurrentPosition {
		get => Bass.ChannelBytes2Seconds(this.Handle, Bass.ChannelGetPosition(this.Handle)) * 1000d;
		set => Bass.ChannelSetPosition(this.Handle, Bass.ChannelSeconds2Bytes(this.Handle, value / 1000d));
	}

	public override double Volume {
		get => Bass.ChannelGetAttribute(this.Handle, ChannelAttribute.Volume);
		set => Bass.ChannelSetAttribute(this.Handle, ChannelAttribute.Volume, value);
	}

	public override bool Loop {
		get => (Bass.ChannelFlags(this.Handle, 0, 0) & BassFlags.Loop) > 0;
		set => this.SetFlag(value, BassFlags.Loop);
	}

	public override double Length => Bass.ChannelBytes2Seconds(this.Handle, Bass.ChannelGetLength(this.Handle)) * 1000d;

	public override PlaybackState PlaybackState => Bass.ChannelIsActive(this.Handle);

	public override bool SetAudioDevice(AudioDevice device) {
		return Bass.ChannelSetDevice(this.Handle, device.Id);
	}

	public override bool Play() {
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
	public override double GetSpeed() {
		return this._lastSpeed;
	}

	private void SetFlag(bool value, BassFlags flag) {
		Bass.ChannelFlags(this.Handle, value ? flag : BassFlags.Default, flag);
	}

	internal override bool Dispose() {
		return Bass.StreamFree(this.Handle);
	}
}
