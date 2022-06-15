using FmodAudio;
using sowelipisona.Effects;
using sowelipisona.Fmod.Effects;

namespace sowelipisona.Fmod;

public class FmodAudioEngine : AudioEngine {
	private FmodSystem _system;

	public override bool Initialize(IntPtr windowId = default) {
		this._system = FmodAudio.Fmod.CreateSystem();

		this._system.Init(16);

		this.Initialized = true;

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
	protected override SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data) => new FmodSoundEffectPlayer(this._system, data);
	
	public override LowPassFilterAudioEffect CreateLowPassFilterEffect(AudioStream stream) => new FmodLowPassFilterAudioEffect(stream);
	public override HighPassFilterAudioEffect CreateHighPassFilterEffect(AudioStream stream) => new FmodHighPassFilterAudioEffect(stream);
	public override ReverbAudioEffect CreateReverbEffect(AudioStream stream) => new FmodReverbAudioEffect(stream);
}
