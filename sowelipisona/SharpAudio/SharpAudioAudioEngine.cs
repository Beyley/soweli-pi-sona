namespace sowelipisona.SharpAudio; 

public class SharpAudioAudioEngine : AudioEngine {
	private global::SharpAudio.AudioEngine _audioEngine;
	
	public override bool Initialize(IntPtr windowId = default) {
		this._audioEngine = global::SharpAudio.AudioEngine.CreateDefault();

		this.Initialized = this._audioEngine != null;
		return this._audioEngine != null;
	}
	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}
	public override AudioDevice[] GetAudioDevices() {
		throw new NotImplementedException();
	}
	protected override AudioStream EngineCreateStream(byte[] data) {
		MemoryStream dataStream = new(data);

		return new SharpAudioAudioStream(dataStream, this._audioEngine);
	}
	protected override SoundEffectPlayer EngineCreateSoundEffectPlayer(byte[] data) {
		throw new NotImplementedException();
	}
}
