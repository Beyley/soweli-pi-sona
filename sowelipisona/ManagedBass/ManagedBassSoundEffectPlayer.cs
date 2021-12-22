using System.Runtime.InteropServices;
using ManagedBass;

namespace sowelipisona.ManagedBass; 

public class ManagedBassSoundEffectPlayer : SoundEffectPlayer {
	public static int MaxSimultaneousPlaybacks = 32; //This is chosen due to hardware limitations on mobile devices

	private readonly GCHandle _memoryHandle;
	private readonly int      _handle;

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
	
	internal override bool Dispose() => Bass.SampleFree(this._handle);
}
