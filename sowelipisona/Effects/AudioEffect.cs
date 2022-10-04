namespace sowelipisona.Effects;

public abstract class AudioEffect {
	public AudioStream Stream;
	public bool Applied {
		get;
		protected set;
	}

	public abstract void Apply();
	public abstract void Remove();
}
