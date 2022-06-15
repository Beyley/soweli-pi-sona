namespace sowelipisona.Effects;

public abstract class LowPassFilterAudioEffect : AudioEffect {
    public abstract double FrequencyCutoff {
        get;
        set;
    }
    
    protected double _frequencyCutoff = 200;

    protected LowPassFilterAudioEffect(AudioStream stream) {
        this.Stream = stream;
    }
    
    public override void Apply() {
        this.InternalApply();

        this.Applied = true;
    }
    
    public override void Remove() {
        this.InternalRemove();

        this.Applied = false;
    }

    protected abstract void InternalApply();
    protected abstract void InternalRemove();
}   