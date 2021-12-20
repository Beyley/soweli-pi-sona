using sowelipisona;
using sowelipisona.ManagedBass;

AudioEngine engine;

engine = new ManagedBassAudioEngine();

Console.WriteLine("Initializing ManagedBass audio engine!");
engine.Initialize();

Console.WriteLine("Creating Stream");
AudioStream stream = engine.CreateStream("test.mp3");
Console.WriteLine($"Stream created (handle:{stream.Handle})!\nPlaying Stream!");
stream.Start();
Console.WriteLine("Press enter to pause the stream");
Console.ReadLine();
stream.Pause();
Console.WriteLine("Press enter to play the stream");
Console.ReadLine();
stream.Resume();
Console.WriteLine("Press enter to change the speed with pitch");
Console.ReadLine();
stream.SetSpeed(2, true);
Console.WriteLine("Press enter to change the speed with no pitch");
Console.ReadLine();
stream.SetSpeed(2);
Console.WriteLine("Press enter to go back to 1x speed");
Console.ReadLine();
stream.SetSpeed(1);
Console.WriteLine("Press enter to change to low volume");
Console.ReadLine();
stream.SetVolume(0.1);
Console.WriteLine("Press enter to go to max volume again");
Console.ReadLine();
stream.SetVolume(1);
Console.WriteLine("Press enter to exit the app");
Console.ReadLine();