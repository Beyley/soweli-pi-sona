using ManagedBass;
using SharpAudio.Codec;

namespace sowelipisona.SharpAudio; 

public class SharpAudioAudioStream : AudioStream {
	private SoundStream _stream;
	
	public override int Handle {
		get => 0;
		internal set {}
	}
	
	public SharpAudioAudioStream(Stream data, global::SharpAudio.AudioEngine engine) {
		this._stream = new(data, engine);
	}
	
	public override double CurrentPosition {
		get => this._stream.Position.TotalMilliseconds;
		set => this._stream.TrySeek(TimeSpan.FromMilliseconds(value));
	}
	
	public override double Length => this._stream.Duration.TotalMilliseconds;
	
	public override bool SetAudioDevice(AudioDevice device) {
		throw new NotImplementedException();
	}
	public override bool Play() {
		this._stream.Play();
		return true;
	}
	public override bool Resume() {
		this._stream.State = SoundStreamState.Playing;
		return true;
	}
	public override bool Pause() {
		this._stream.State = SoundStreamState.Paused;
		return true;
	}
	public override bool Stop() {
		this._stream.Stop();
		return true;
	}
	public override bool SetSpeed(double speed, bool pitch = false) {
		throw new NotImplementedException();
	}
	public override double GetSpeed() {
		throw new NotImplementedException();
	}
	
	public override double Volume {
		get => this._stream.Volume;
		set => this._stream.Volume = (float)value;
	}
	public override bool Loop {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public override PlaybackState PlaybackState {
		get {
			return this._stream.State switch {
				SoundStreamState.Playing       => PlaybackState.Playing,
				SoundStreamState.Paused        => PlaybackState.Paused,
				SoundStreamState.Stop          => PlaybackState.Stopped,
				SoundStreamState.TrackFinished => PlaybackState.Stopped,
				SoundStreamState.Idle          => PlaybackState.Stopped,
				SoundStreamState.PreparePlay   => PlaybackState.Stopped,
			};
		}
	}
	internal override bool Dispose() {
		this._stream.Dispose();
		return true;
	}
}
