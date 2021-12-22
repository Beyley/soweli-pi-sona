using LibVLCSharp.Shared;
using ManagedBass;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace sowelipisona.LibVLC; 

public class LibVLCAudioStream : AudioStream {
	private readonly Media        _media;
	private readonly MediaPlayer  _mediaPlayer;
	private readonly MemoryStream _dataStream;
	
	public override int Handle {
		get;
		internal set;
	}
	
	public LibVLCAudioStream(LibVLCSharp.Shared.LibVLC libVlc, byte[] data) {
		this._dataStream = new MemoryStream(data);
		
		this._media       = new(libVlc, new StreamMediaInput(this._dataStream));
		this._mediaPlayer = new(this._media);

		this._mediaPlayer.Volume = 100;

		this._mediaPlayer.EnableHardwareDecoding = true;
	}
	
	public override double CurrentPosition {
		get => this._mediaPlayer.Position * this._mediaPlayer.Length;
		set => this._mediaPlayer.Position = (float)(value / this._mediaPlayer.Length);
	}

	public override double Length => this._mediaPlayer.Length;
	
	public override bool SetAudioDevice(AudioDevice device) {
		this._mediaPlayer.SetOutputDevice(device.Name);
		
		return true;
	}
	public override bool Start() {
		this._mediaPlayer.Play();
		return true;
	}
	public override bool Resume() {
		this._mediaPlayer.SetPause(false);
		return true;
	}
	public override bool Pause() {
		this._mediaPlayer.SetPause(true);
		return true;
	}
	public override bool Stop() {
		this._mediaPlayer.Stop();
		return true;
	}
	
	public override bool SetSpeed(double speed, bool pitch = false) {
		if (pitch)
			throw new NotImplementedException();

		this._mediaPlayer.SetRate((float)speed);
		return true;
	}
	public override double GetSpeed() => this._mediaPlayer.Rate;
	
	public override double Volume {
		get => this._mediaPlayer.Volume / 100d;
		set => this._mediaPlayer.Volume = (int)(value * 100);
	}
	
	public override bool Loop {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public override PlaybackState PlaybackState => this._mediaPlayer.IsPlaying ? PlaybackState.Playing : PlaybackState.Paused;
	internal override bool Dispose() {
		this._mediaPlayer.Stop();
		this._mediaPlayer.Dispose();
		this._media.Dispose();

		return true;
	}
}
