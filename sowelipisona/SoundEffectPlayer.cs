namespace sowelipisona; 

public abstract class SoundEffectPlayer {
	public double Volume = 1f;
	
	public abstract bool PlayNew();

	internal abstract bool Dispose();
}
