using ManagedBass;

namespace sowelipisona.ManagedBass;

public class ManagedBassAudioDevice : AudioDevice {
	private DeviceInfo _info;

	internal ManagedBassAudioDevice(int id, DeviceInfo info) {
		this.Id    = id;
		this._info = info;
	}

	public override string Name => this._info.Name;
	public override int Id {
		get;
	}
	public override DeviceType Type => this._info.Type;
}
