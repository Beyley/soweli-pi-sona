namespace sowelipisona;

public abstract class AudioStream {
	public abstract int Handle { get; internal set; }

	public abstract bool SetAudioDevice(AudioDevice device);

	public abstract bool Start();
	public abstract bool Resume();
	public abstract bool Pause();
	public abstract bool Stop();
	public abstract bool SetSpeed(double  speed, bool pitch = false);
	public abstract bool SetVolume(double volume);
	
	internal abstract bool Dispose();
}
