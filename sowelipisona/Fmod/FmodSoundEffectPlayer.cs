using System.Runtime.InteropServices;
using ChaiFoxes.FMODAudio;
using FMOD;
using Channel = FMOD.Channel;
using ChannelGroup = FMOD.ChannelGroup;
using Sound = FMOD.Sound;

namespace sowelipisona.Fmod;

internal class FmodSoundEffectPlayer : SoundEffectPlayer {
	private Sound _sound;

	internal FmodSoundEffectPlayer(byte[] data) {
		CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO {
			length = (uint)data.Length
		};
		exinfo.cbsize = Marshal.SizeOf(exinfo);

		CoreSystem.Native.createSound(data, MODE.CREATESAMPLE | MODE.OPENMEMORY, ref exinfo, out this._sound);
	}

	public override bool PlayNew() {
		CoreSystem.Native.playSound(this._sound, default(ChannelGroup), false, out Channel channel);

		channel.setVolume((float)this.Volume);

		return true;
	}
	internal override bool Dispose() {
		this._sound.release();
		return true;
	}
}
