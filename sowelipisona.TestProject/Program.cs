using sowelipisona;
using sowelipisona.Fmod;
using sowelipisona.LibVLC;
using sowelipisona.ManagedBass;

AudioEngine bassEngine;
AudioEngine fmodEngine;
AudioEngine libvlcEngine;

bassEngine = new ManagedBassAudioEngine();

Console.WriteLine("Initializing ManagedBass audio engine!");
bassEngine.Initialize();

TestEngine(bassEngine);

fmodEngine = new FmodAudioEngine();

Console.WriteLine("Initializing FMod audio engine!");
fmodEngine.Initialize();

TestEngine(fmodEngine);

libvlcEngine = new LibVLCAudioEngine();

Console.WriteLine("Initializing LibVLC audio engine!");
libvlcEngine.Initialize();

TestEngine(libvlcEngine);

void TestEngine(AudioEngine engine) {
	AudioDevice[] audioDevices = engine.GetAudioDevices();

	Console.WriteLine("-- Audio Devices --");
	foreach (AudioDevice audioDevice in audioDevices)
		Console.WriteLine($"Name: {audioDevice.Name} | Id: {audioDevice.Id} | Type: {audioDevice.Type}");
	Console.WriteLine("-------------------");

	Console.WriteLine("Creating Stream");
	AudioStream stream = engine.CreateStream("test.mp3");
	Console.WriteLine($"Stream created (handle:{stream.Handle})!\nPlaying Stream!");
	stream.Start();
	Console.WriteLine($"Press enter to pause the stream | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.Pause();
	Console.WriteLine($"Press enter to play the stream | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.Resume();
	Console.WriteLine($"Press enter to change the speed with pitch | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.SetSpeed(2, true);
	Console.WriteLine($"Press enter to change the speed with no pitch | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.SetSpeed(2);
	Console.WriteLine($"Press enter to go back to 1x speed | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.SetSpeed(1);
	Console.WriteLine($"Press enter to change to low volume | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.SetVolume(0.5);
	Console.WriteLine($"Press enter to go to max volume again | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.SetVolume(1);
	Console.WriteLine($"Press enter to run the next test | curpos: {stream.CurrentPosition}");
	Console.ReadLine();
	stream.Stop();
	engine.DisposeStream(stream);
}
