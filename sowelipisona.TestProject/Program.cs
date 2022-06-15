using sowelipisona;
using sowelipisona.Effects;
using sowelipisona.Fmod;
using sowelipisona.ManagedBass;
AudioEngine bassEngine;
AudioEngine fmodEngine;

bassEngine = new ManagedBassAudioEngine();

Console.WriteLine("Initializing ManagedBass backend!");
bassEngine.Initialize();

TestEngine(bassEngine);

fmodEngine = new FmodAudioEngine();

Console.WriteLine("Initializing FMod backend!");
fmodEngine.Initialize();

TestEngine(fmodEngine);

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
			stream.Play();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioEngine.CreateStream(filename) not implemented!");
		}
		Console.WriteLine($"Press enter to pause the stream | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Pause();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Pause() not implemented!");
		}
		Console.WriteLine($"Press enter to play the stream | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Resume();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Resume() not implemented!");
		}
		Console.WriteLine($"Press enter to change the speed with pitch | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.SetSpeed(2, true);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=2, pitch=true) not implemented!");
		}
		Console.WriteLine($"Press enter to change the speed with no pitch | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.SetSpeed(2);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=2, pitch=false) not implemented!");
		}
		Console.WriteLine($"Press enter to go back to 1x speed | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.SetSpeed(1);
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetSpeed(speed=1) not implemented!");
		}
		Console.WriteLine($"Press enter to change to low volume | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Volume = 0.5;
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetVolume(volume=0.5d) not implemented!");
		}
		Console.WriteLine($"Press enter to go to max volume again | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Volume = 1;
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.SetVolume(volume=1d) not implemented!");
		}
		Console.WriteLine($"Press enter to turn looping on! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Loop = true;
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Loop_set(true) not implemented!");
		}
		Console.WriteLine($"Press enter to turn looping off! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Loop = false;
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Loop_set(false) not implemented!");
		}
		Console.WriteLine($"Press enter to turn low pass effect on! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		LowPassFilterAudioEffect lowPassFilterAudioEffect = null;
		try {
			lowPassFilterAudioEffect = engine.CreateLowPassFilterEffect(stream);
			lowPassFilterAudioEffect.Apply();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Creating low pass effect failed!");
		}
		Console.WriteLine($"Press enter to turn low pass to a cutoff of 1000hz! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			lowPassFilterAudioEffect.FrequencyCutoff = 1000;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing low pass frequency to 1000hz failed!");
		}
		Console.WriteLine($"Press enter to turn low pass effect off! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			lowPassFilterAudioEffect.Remove();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Removing low pass effect failed!");
		}
		Console.WriteLine($"Press enter to turn high pass effect on! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		HighPassFilterAudioEffect highPassFilterAudioEffect = null;
		try {
			highPassFilterAudioEffect = engine.CreateHighPassFilterEffect(stream);
			highPassFilterAudioEffect.Apply();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Creating high pass effect failed!");
		}
		Console.WriteLine($"Press enter to turn high pass to a cutoff of 400hz! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			highPassFilterAudioEffect.FrequencyCutoff = 400;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing high pass frequency to 1000hz failed!");
		}
		Console.WriteLine($"Press enter to turn high pass effect off! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			highPassFilterAudioEffect.Remove();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Removing high pass effect failed!");
		}

		#region Reverb

		Console.WriteLine($"Press enter to turn reverb effect on! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		ReverbAudioEffect reverbAudioEffect = null;
		try {
			reverbAudioEffect = engine.CreateReverbEffect(stream);
			reverbAudioEffect.Apply();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Creating reverb effect  failed!");
		}
		
		Console.WriteLine($"Press enter to turn reverb dropoff to -10! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			reverbAudioEffect.ReverbDropoff = -10;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing reverb intensity to -10 failed!");
		}
		
		Console.WriteLine($"Press enter to turn reverb dropoff to 0! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			reverbAudioEffect.ReverbDropoff = 0;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing reverb intensity to 0 failed!");
		}
		
		Console.WriteLine($"Press enter to turn reverb time to 300 miliseconds! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			reverbAudioEffect.ReverbTime = 300;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing reverb time to 300 failed!");
		}
		
		Console.WriteLine($"Press enter to turn reverb time to 0.1 seconds! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			reverbAudioEffect.ReverbTime = 100;
		}
		catch (NotImplementedException) {
			Console.WriteLine("Changing reverb time to 100 failed!");
		}
		
		Console.WriteLine($"Press enter to turn reverb effect off! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			reverbAudioEffect.Remove();
		}
		catch (NotImplementedException) {
			Console.WriteLine("Removing reverb effect failed!");
		}

		#endregion

		Console.WriteLine($"Press enter to stop the stream! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
		try {
			stream.Stop();
		}
		catch (NotImplementedException) {
			Console.WriteLine("AudioStream.Stop() not implemented!");
		}
		Console.WriteLine($"Press enter to test the next backend! | curpos: {stream.CurrentPosition}/{stream.Length}");
		Console.ReadLine();
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
