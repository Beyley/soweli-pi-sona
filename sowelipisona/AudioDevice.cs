using ManagedBass;

namespace sowelipisona;

public abstract class AudioDevice {
	public abstract string Name { get; }
	public abstract int    Id   { get; }
	public abstract DeviceType Type { get; }
}
