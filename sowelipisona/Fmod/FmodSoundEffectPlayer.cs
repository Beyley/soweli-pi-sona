using FmodAudio;

namespace sowelipisona.Fmod; 

public class FmodSoundEffectPlayer : SoundEffectPlayer {
	private FmodSystem _system;

	private readonly Sound _sound;

	internal FmodSoundEffectPlayer(FmodSystem system, byte[] data) {
		this._system = system;

		this._sound = this._system.CreateSound((ReadOnlySpan<byte>)data, Mode.Default | Mode.OpenMemory, new CreateSoundInfo {
			Length = (uint)data.Length
		});
	}

	public override bool PlayNew() {
		Channel channel = this._system.PlaySound(this._sound);

		channel.Volume = (float)this.Volume;

		return true;
	}
	internal override bool Dispose() {
		this._sound.Dispose();
		return true;
	}
}
