using System.Runtime.CompilerServices;
using ManagedBass;

namespace sowelipisona.ManagedBass; 

public class ManagedBassAudioDevice : AudioDevice {
	private int        _id;
	private DeviceInfo _info;
	
	public override string     Name => this._info.Name;
	public override int        Id   => this._id;
	public override DeviceType Type => this._info.Type;

	internal ManagedBassAudioDevice(int id, DeviceInfo info) {
		this._id   = id;
		this._info = info;
	}
}
