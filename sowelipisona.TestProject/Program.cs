using sowelipisona;
using sowelipisona.Fmod;
using sowelipisona.LibVLC;
using sowelipisona.ManagedBass;
using sowelipisona.SharpAudio;
AudioEngine bassEngine;
AudioEngine fmodEngine;
AudioEngine libvlcEngine;
AudioEngine sharpAudioEngine;

bassEngine = new ManagedBassAudioEngine();

Console.WriteLine("Initializing ManagedBass backend!");
bassEngine.Initialize();

TestEngine(bassEngine);

fmodEngine = new FmodAudioEngine();

Console.WriteLine("Initializing FMod backend!");
fmodEngine.Initialize();

TestEngine(fmodEngine);

libvlcEngine = new LibVLCAudioEngine();

Console.WriteLine("Initializing LibVLC backend!");
libvlcEngine.Initialize();

TestEngine(libvlcEngine);

sharpAudioEngine = new SharpAudioAudioEngine();

Console.WriteLine("Initializing SharpAudio backend!");
sharpAudioEngine.Initialize();

TestEngine(sharpAudioEngine);

void TestEngine(AudioEngine engine) {
	try {
		AudioDevice[] audioDevices = engine.GetAudioDevices();

		Console.WriteLine("-- Audio Devices --");
		foreach (AudioDevice audioDevice in audioDevices)
			Console.WriteLine($"Name: {audioDevice.Name} | Id: {audioDevice.Id} | Type: {audioDevice.Type}");
		Console.WriteLine("-------------------");
	}
	catch (NotImplementedException) {
		Console.WriteLine("AudioEngine.GetAudioDevices() not implemented!");
	}

	AudioStream stream = null;
	try {
		Console.WriteLine("Creating Stream");
		stream = engine.CreateStream("test.mp3");
	}
	catch (NotImplementedException) {
		Console.WriteLine("AudioEngine.CreateStream(filename) not implemented!");
	}
	if (stream != null) {
		try {
			Console.WriteLine($"Stream created (handle:{stream.Handle})!\nPlaying Stream!");
			stream.Start();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioEngine.CreateStream(filename) not implemented!");
		}
		Console.WriteLine($"Press enter to pause the stream | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.Pause();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Pause() not implemented!");
		}
		Console.WriteLine($"Press enter to play the stream | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.Resume();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Resume() not implemented!");
		}
		Console.WriteLine($"Press enter to change the speed with pitch | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.SetSpeed(2, true);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=2, pitch=true) not implemented!");
		}
		Console.WriteLine($"Press enter to change the speed with no pitch | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.SetSpeed(2);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=2, pitch=false) not implemented!");
		}
		Console.WriteLine($"Press enter to go back to 1x speed | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.SetSpeed(1);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=1) not implemented!");
		}
		Console.WriteLine($"Press enter to change to low volume | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.SetVolume(0.5);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetVolume(volume=0.5d) not implemented!");
		}
		Console.WriteLine($"Press enter to go to max volume again | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.SetVolume(1);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetVolume(volume=1d) not implemented!");
		}
		Console.WriteLine($"Press enter to run the next test | curpos: {stream.CurrentPosition}");
		Console.ReadLine();
		try {
			stream.Stop();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Stop() not implemented!");
		}
		try {
			engine.DisposeStream(stream);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioEngine.DisposeStream() or AudioStream.Dispose() not implemented!");
		}
	}
	else {
		Console.WriteLine("Creating stream failed! We are unable to continue.");
	}
}
