using FmodAudio;
using ManagedBass;

namespace sowelipisona.Fmod;

public class FmodAudioDevice : AudioDevice {
	private readonly DriverInfo _info;

	public FmodAudioDevice(int id, DriverInfo info) {
		this.Id    = id;
		this._info = info;
	}

	public override string Name => this._info.DriverName;
	public override int Id {
		get;
	}
	public override DeviceType Type => DeviceType.Headphones;
}
