namespace sowelipisona;

public abstract class SoundEffectPlayer {
	public double Volume = 1f;

	/// <summary>
	///     Plays a new instance of the sound effect, in parallel with the last one
	/// </summary>
	/// <returns>Success</returns>
	public abstract bool PlayNew();

	internal abstract bool Dispose();
}
