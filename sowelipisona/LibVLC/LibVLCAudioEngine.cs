using LibVLCSharp.Shared;
using LibVLCSharp.Shared.Structures;

namespace sowelipisona.LibVLC; 

public class LibVLCAudioEngine : AudioEngine {
	private LibVLCSharp.Shared.LibVLC _libVlc;
	
	public override bool Initialize(IntPtr windowId = default) {
		Core.Initialize();
		this._libVlc = new();

		return true;
	}
	
	public override bool SetAudioDevice(AudioDevice device) {
		foreach (AudioStream stream in this.Streams)
			stream.SetAudioDevice(device);

		return true;
	}
	public override AudioDevice[] GetAudioDevices() {
		AudioOutputDescription[] libVlcAudioOutputs = this._libVlc.AudioOutputs;

		AudioDevice[] devices = new AudioDevice[libVlcAudioOutputs.Length];
		for (int i = 0; i < libVlcAudioOutputs.Length; i++) {
			devices[i] = new LibVLCAudioDevice(i, libVlcAudioOutputs[i]);
		}

		return devices;
	}
	
	protected override AudioStream EngineCreateStream(byte[] data) {
		AudioStream stream = new LibVLCAudioStream(this._libVlc, data);

		return stream;
	}
}
