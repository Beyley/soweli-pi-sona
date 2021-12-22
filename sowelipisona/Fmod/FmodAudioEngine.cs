using FmodAudio;

namespace sowelipisona.Fmod;

public class FmodAudioEngine : AudioEngine {
	private FmodSystem _system;

	public override bool Initialize(IntPtr windowId = default) {
		this._system = FmodAudio.Fmod.CreateSystem();

		this._system.Init(8);

		return true;
	}

	public override bool SetAudioDevice(AudioDevice device) {
		this._system.CurrentDriver = device.Id;

		return true;
	}

	public override AudioDevice[] GetAudioDevices() {
		int count = this._system.DriverCount;

		AudioDevice[] devices = new AudioDevice[count];
		for (int i = 0; i < count; i++)
			devices[i] = new FmodAudioDevice(i, this._system.GetDriverInfo(i));

		return devices;
	}
	protected override AudioStream EngineCreateStream(byte[] data) {
		return new FmodAudioStream(this._system, data);
	}
	protected override SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data) {
		throw new NotImplementedException();
	}
}
