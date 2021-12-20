using LibVLCSharp.Shared.Structures;
using ManagedBass;

namespace sowelipisona.LibVLC; 

public class LibVLCAudioDevice : AudioDevice {
	public override string Name {
		get;
	}
	public override int Id {
		get;
	}
	public override DeviceType Type {
		get;
	}

	public LibVLCAudioDevice(int id, AudioOutputDescription description) {
		this.Id   = id;
		this.Name = description.Name;
		this.Type = DeviceType.Headphones;
	}
}
