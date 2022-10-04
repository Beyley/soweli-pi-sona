using ChaiFoxes.FMODAudio;
using FMOD;
using ManagedBass;

namespace sowelipisona.Fmod;

internal class FmodAudioDevice : AudioDevice {
	public FmodAudioDevice(int id) {
		RESULT result = CoreSystem.Native.getDriverInfo(id, out string name, 64, out Guid guid, out int systemrate, out SPEAKERMODE speakermod, out int speakermodechannels);
		if (result != RESULT.OK)
			throw new Exception($"Failed to get driver info! {result}");

		this.Id   = id;
		this.Name = name;
	}

	public override string Name { get; }
	public override int    Id   { get; }

	public override DeviceType Type => DeviceType.Headphones;
}
