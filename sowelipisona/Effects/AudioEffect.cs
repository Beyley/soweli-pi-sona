namespace sowelipisona.Effects;

public abstract class AudioEffect {
    public bool Applied {
        get;
        protected set;
    }
    
    public AudioStream Stream;
    
    public abstract void Apply();
    public abstract void Remove();
}