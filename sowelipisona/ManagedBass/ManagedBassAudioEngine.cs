using System.Runtime.InteropServices;
using ManagedBass;
using sowelipisona.Native;

namespace sowelipisona.ManagedBass;

public class ManagedBassAudioEngine : AudioEngine {
	public override bool Initialize(IntPtr windowId = default) {
		if (windowId == default) windowId = IntPtr.Zero;

		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
			Linux.Load("libbass.so", Linux.LoadFlags.RTLD_LAZY | Linux.LoadFlags.RTLD_GLOBAL);
			Linux.Load("libbass_fx.so", Linux.LoadFlags.RTLD_LAZY | Linux.LoadFlags.RTLD_GLOBAL);
		}
		
		bool success = Bass.Init(
			-1,
			44100,
			DeviceInitFlags.Latency,
			windowId
		);

		this.Initialized = success;

		if (!success)
			return false;
		
		success = Bass.GetDeviceInfo(Bass.CurrentDevice, out DeviceInfo info);
		this.AudioDeviceInUse = new ManagedBassAudioDevice(Bass.CurrentDevice, info);

		return success;
	}

	public override bool SetAudioDevice(AudioDevice device) {
		//This would 100% cause errors, so its best we handle it, even if its unlikely to actually happen in practice
		if (device is not ManagedBassAudioDevice) return false;
		
		Bass.CurrentDevice = device.Id;

		this.AudioDeviceInUse = device;

		return true;
	}

	public override AudioDevice[] GetAudioDevices() {
		int count = Bass.DeviceCount;
		
		AudioDevice[] devices = new AudioDevice[count];
		for (int i = 0; i < count; i++) {
			devices[i] = new ManagedBassAudioDevice(i, Bass.GetDeviceInfo(i));
		}

		return devices;
	}
	protected override AudioStream EngineCreateStream(byte[] data) {
		ManagedBassAudioStream stream = new(data);

		return stream;
	}
}
