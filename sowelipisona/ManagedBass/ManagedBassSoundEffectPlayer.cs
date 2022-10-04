using System.Runtime.InteropServices;
using ManagedBass;

namespace sowelipisona.ManagedBass;

internal class ManagedBassSoundEffectPlayer : SoundEffectPlayer {
	public static    int MaxSimultaneousPlaybacks = 32; //This is chosen due to hardware limitations on mobile devices
	private readonly int _handle;

	private readonly GCHandle _memoryHandle;

	internal ManagedBassSoundEffectPlayer(byte[] data) {
		this._memoryHandle = GCHandle.Alloc(data, GCHandleType.Pinned);

		this._handle = Bass.SampleLoad(this._memoryHandle.AddrOfPinnedObject(), 0, data.Length, MaxSimultaneousPlaybacks, BassFlags.SampleOverrideLongestPlaying);
	}

	public override bool PlayNew() {
		int channelHandle = Bass.SampleGetChannel(this._handle);

		if (channelHandle == 0)
			return false;

		return Bass.ChannelPlay(channelHandle) && Bass.ChannelSetAttribute(channelHandle, ChannelAttribute.Volume, this.Volume);
	}

	internal override bool Dispose() {
		return Bass.SampleFree(this._handle);
	}
}
