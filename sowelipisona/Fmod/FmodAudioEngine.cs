using ChaiFoxes.FMODAudio;
using FMOD;
using sowelipisona.Effects;
using sowelipisona.Fmod.Effects;

namespace sowelipisona.Fmod;

public class FmodAudioEngine : AudioEngine {
	private Task? _task;
	private bool  _run = true;

	public override double MusicVolume {
		get {
			//TODO:
			return 1;
		}
		set {
			//TODO
		}
	}
	public override double SampleVolume {
		get {
			//TODO:
			return 1;
		}
		set {
			//TODO
		}
	}
	public override bool Initialize(IntPtr windowId = default) {
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
		if (result != RESULT.OK) {
			throw new Exception($"Failed to get number of drivers! err {result}");
		}

		AudioDevice[] devices = new AudioDevice[count];
		for (int i = 0; i < count; i++) {
			devices[i] = new FmodAudioDevice(i);
		}

		return devices;
	}
	protected override AudioStream EngineCreateStream(byte[] data) {
		return new FmodAudioStream(data);
	}
	protected override SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data) => new FmodSoundEffectPlayer(data);
	
	public override LowPassFilterAudioEffect CreateLowPassFilterEffect(AudioStream stream) => new FmodLowPassFilterAudioEffect(stream);
	public override HighPassFilterAudioEffect CreateHighPassFilterEffect(AudioStream stream) => new FmodHighPassFilterAudioEffect(stream);
	public override ReverbAudioEffect CreateReverbEffect(AudioStream stream) => new FmodReverbAudioEffect(stream);
}
