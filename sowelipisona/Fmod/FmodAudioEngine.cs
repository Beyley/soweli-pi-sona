using ChaiFoxes.FMODAudio;
using FMOD;
using sowelipisona.Effects;
using sowelipisona.Fmod.Effects;

namespace sowelipisona.Fmod;

public class FmodAudioEngine : AudioEngine {
	private readonly bool  _run = true;
	private          Task? _task;

	public override double MusicVolume {
		get =>
			//TODO:
			1;
		set {
			//TODO
		}
	}
	public override double SampleVolume {
		get =>
			//TODO:
			1;
		set {
			//TODO
		}
	}
	public override bool Initialize(IntPtr windowId = default(IntPtr)) {
		FMODManager.Init(FMODMode.Core, "");

		this._task = Task.Factory.StartNew(async () => {
			while (this._run) {
				FMODManager.Update();
				await Task.Delay(100);
			}
		});

		this.Initialized = true;

		return true;
	}

	public override bool SetAudioDevice(AudioDevice device) {
		CoreSystem.Native.setDriver(device.Id);

		return true;
	}

	public override AudioDevice[] GetAudioDevices() {
		RESULT result = CoreSystem.Native.getNumDrivers(out int count);
		if (result != RESULT.OK)
			throw new Exception($"Failed to get number of drivers! err {result}");

		AudioDevice[] devices = new AudioDevice[count];
		for (int i = 0; i < count; i++)
			devices[i] = new FmodAudioDevice(i);

		return devices;
	}
	protected override AudioStream EngineCreateStream(byte[] data) {
		return new FmodAudioStream(data);
	}
	protected override AudioStream EngineCreateStream(Stream stream) {
		return new FmodAudioStream(stream);
	}
	protected override SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data) {
		return new FmodSoundEffectPlayer(data);
	}

	public override LowPassFilterAudioEffect CreateLowPassFilterEffect(AudioStream stream) {
		return new FmodLowPassFilterAudioEffect(stream);
	}
	public override HighPassFilterAudioEffect CreateHighPassFilterEffect(AudioStream stream) {
		return new FmodHighPassFilterAudioEffect(stream);
	}
	public override ReverbAudioEffect CreateReverbEffect(AudioStream stream) {
		return new FmodReverbAudioEffect(stream);
	}
}
